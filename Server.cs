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
				Connection connection = new Connection(tcpClient);
			}
		}

		public void StopListening()
		{
			foreach (Connection client in Users.Values)
			{
				client.CloseConnection("0|201");
			}
			thrListener.Abort();
			tcpListener.Stop();
			Users.Clear();
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
