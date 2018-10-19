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
		private static MainWindow ChatForm;
		private Thread thrListener;
		private List<Connection> Connections;

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
			tcpListener=new TcpListener(ipAddress, 1986);
			tcpListener.Start();
			thrListener = new Thread(KeepListening);
			thrListener.Start();
        }

		private void KeepListening()
		{
			while (true)
            {
                tcpClient = tcpListener.AcceptTcpClient();
				if (tcpClient.Connected)
				{
					Connection connection = new Connection(tcpClient);
					//Connections.Add(connection);
				}
            }
		}

		public void StopListening()
		{
			tcpListener.Stop();
			thrListener.Abort();
			/*foreach (TcpClient client in Users.Values)
			{
				client.Close();
			}*/
			for (int i = 0; i < Connections.Count; ++i)
			{
				Connections[i].CloseConnection("4");
			}
		}

		public static void AddUser(TcpClient User, string Username)
		{
			Users.Add(Username, User);
			ChatForm.Log = Username + " join\n";
		}

		public static void RemoveUser(TcpClient user, string Username)
		{
			Users.Remove(Username);
			ChatForm.Log = Username + " left\n";
		}
    }
}
