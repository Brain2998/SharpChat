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
		private string clientMessage;
		private string clientName;

		public Connection(TcpClient Client)
        {
			tcpClient = Client;
			clientThread = new Thread(AcceptClient);
			clientThread.Start();
        }

        public void CloseConnection(string reason)
		{
			clientWriter.WriteLine(reason);
			clientWriter.Flush();
			tcpClient.Close();
			clientThread.Abort();
			clientReader.Close();
			clientWriter.Close();
		}

		public void CloseConnection()
		{
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
					CloseConnection("0|103");
				}
				else
				{
					clientWriter.WriteLine("0|100");
					clientWriter.Flush();
					Server.AddUser(this, clientName);
					RecieveMessages();               
				}
			}
			else
			{
				CloseConnection("0|102");
			}
		}

		private void RecieveMessages()
		{
			//while (tcpClient.GetStream().DataAvailable)
			while ((clientMessage=clientReader.ReadLine())!="")
            //while (true)
            {
				//clientMessage = clientReader.ReadLine();
                if (clientMessage.StartsWith("0|"))
                {
                    if (clientMessage == "0|202")
                    {
                        Server.RemoveUser(clientName);
                        CloseConnection();
                    }
                }
            }
		}
    }
}