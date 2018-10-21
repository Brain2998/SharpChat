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
			//clientThread = new Thread(AcceptClient);
			//clientThread.Start();
        }

        public void CloseConnection(string reason)
		{
			try
			{
				clientWriter.WriteLine(reason);
				clientWriter.Flush();
				tcpClient.Close();
				clientThread.Abort();
				//clientReader.Close();
				//clientWriter.Close();
			}
			catch (ThreadAbortException)
			{
				clientReader.Close();
                clientWriter.Close();
			}
			catch (Exception e)
            {
				Server.ChatForm.Log = "CloseConnectionReason: " + e.Message+ "\n";
            }
		}

		public void CloseConnection()
		{
			try
			{
				tcpClient.Close();
				clientThread.Abort();
				//clientReader.Close();
				//clientWriter.Close();
			}
			catch (ThreadAbortException)
			{
				clientReader.Close();
                clientWriter.Close();
			}
			catch (Exception e)
            {
				Server.ChatForm.Log = "CloseConnection: " + e.Message+ "\n";
            }
		}

		public void AcceptClient()
		{
			clientReader = new StreamReader(tcpClient.GetStream());
			clientWriter = new StreamWriter(tcpClient.GetStream());
			try
			{
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
						clientThread = new Thread(RecieveMessages);
                        clientThread.Start();
					}
				}
				else
				{
					CloseConnection("0|102");
				}
			}
			catch (ThreadAbortException)
            {

            }
			catch (Exception e)
			{
				Server.ChatForm.Log = "AcceptClient: " + e.Message+ "\n";
				CloseConnection("0|101");
			}
		}

		private void RecieveMessages()
		{
			try
			{
				//while (tcpClient.GetStream().DataAvailable)
				while ((clientMessage = clientReader.ReadLine()) != "")
				{
					//clientMessage = clientReader.ReadLine();
					if (clientMessage.StartsWith("0|"))
					{
						Server.RemoveUser(clientName);
						CloseConnection();
					}
				}
			}
			catch (ThreadAbortException)
			{
				
			}
			catch (Exception e)
			{
				Server.ChatForm.Log = "RecieveMessage: " + e.Message+ "\n";
				CloseConnection("0|200");
			}
		}
    }
}