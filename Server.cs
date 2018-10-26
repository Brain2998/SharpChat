using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;

namespace SharpChat
{
    public class Server
    {
		private IPAddress ipAddress;
	    private TcpClient tcpClient;
	    private TcpListener tcpListener;
		private static Hashtable Users = new Hashtable();
		public static MainWindow ChatForm;
		private Thread thrListener;
		public bool isRunning = false;

		public static Hashtable UserTable
		{
			get 
			{
				return Users;
			}
			set
			{
				Users = value;
			}
		}

		public Server(IPAddress address, MainWindow form)
		{
			ipAddress = address;
			ChatForm = form;
		}

		public void StartListening()
		{
			try
			{
				tcpListener = new TcpListener(ipAddress, 1986);
				tcpListener.Start();
				isRunning = true;
				thrListener = new Thread(KeepListening);
				thrListener.Start();
			}
			catch (Exception e)
            {
				ChatForm.Log = "StartListening: " + e.Message;
            }
        }

		private void KeepListening()
		{
			try
			{
				while (isRunning)
                {
					if (!tcpListener.Pending())
					{
						Thread.Sleep(200);
						continue;
					}
					tcpClient = tcpListener.AcceptTcpClient();
					Connection connection = new Connection(tcpClient);
					connection.AcceptClient();
				}
			}
			catch (Exception e)
			{
				ChatForm.Log= "KeepListening: " + e.Message+" "+e.TargetSite;
			}
		}

		public void StopListening()
		{
			try
			{
				foreach (Connection client in Users.Values)
				{
					client.CloseConnection("0|201");
				}
				isRunning = false;
				tcpListener.Stop();
				thrListener.Join();

				Users.Clear();
			}
			catch (Exception e)
			{
				ChatForm.Log = "StopListening: " + e.Message;
			}
		}

		public static void SendMessages(string From, string Message)
		{
			foreach (Connection client in Users.Values)
			{
				client.SendMessage("1|"+From + ": " + Message);
			}
		}

		public static void AddUser(Connection User, string Username)
		{
			Users.Add(Username, User);
			ChatForm.Log = Username + " join";
		}

		public static void RemoveUser(string Username)
		{
			Users.Remove(Username);
			ChatForm.Log = Username + " left";
		}
    }
}
