using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace SharpChat
{
    public class Connection
    {
		private TcpClient tcpClient;
		private Thread clientThread;
		private StreamReader clientReader;
		private StreamWriter clientWriter;
		private string clientResponse;
		private string clientName;

		public Connection(TcpClient Client)
        {
			tcpClient = Client;
			clientThread = new Thread(AcceptClient);
			clientThread.Start();
        }

        public void CloseConnection(string reason)
		{
			if (reason != "3")
			{
				clientWriter.WriteLine(reason);
				clientWriter.Flush();
			}
			tcpClient.Close();
			clientThread.Abort();
			clientReader.Close();
			clientWriter.Close();
		}

        private void AcceptClient()
		{
			clientReader = new StreamReader(tcpClient.GetStream());
			clientWriter = new StreamWriter(tcpClient.GetStream());
			clientName = clientReader.ReadLine();
			if (clientName != "")
			{
				if (Server.UserTable.Contains(clientName))
				{
					CloseConnection("2");
				}
				else
				{
					clientWriter.WriteLine("0");
					clientWriter.Flush();
					Server.AddUser(tcpClient, clientName);
				}
			}
			else
			{
				CloseConnection("1");
			}
			try
			{
				while ((clientResponse=clientReader.ReadLine())!="")
				{
					if (clientResponse=="3")
					{
						Server.RemoveUser(tcpClient, clientName);
						CloseConnection("3");
					}
				}
			}
			catch
			{
			//	CloseConnection("1");
			}
		}      
    }
}
