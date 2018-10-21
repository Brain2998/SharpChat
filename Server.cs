using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.Collections.Generic;

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
				thrListener = new Thread(KeepListening);
				thrListener.Start();
			}
			catch (ThreadAbortException)
            {

            }
			catch (Exception e)
            {
				ChatForm.Log = "StartListening: " + e.Message +"\n";
            }
        }

		private void KeepListening()
		{
			try
			{
				while (true)
                {
					tcpClient = tcpListener.AcceptTcpClient();
					Connection connection = new Connection(tcpClient);
					connection.AcceptClient();
				}
			}
			catch (ThreadAbortException)
			{
				
			}
			catch (Exception e)
			{
				ChatForm.Log= "KeepListening: " + e.Message+ "\n";
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
				thrListener.Abort();
				//tcpListener.Stop();
				//Users.Clear();
			}
			catch (ThreadAbortException)
			{
				tcpListener.Stop();
                Users.Clear();
			}
			catch (Exception e)
			{
				ChatForm.Log = "StopListening: " + e.Message+ "\n";
			}
		}

		public static void AddUser(Connection User, string Username)
		{
			Users.Add(Username, User);
			ChatForm.Log = Username + " join\n";
		}

		public static void RemoveUser(string Username)
		{
			Users.Remove(Username);
			ChatForm.Log = Username + " left\n";
		}
    }
}
