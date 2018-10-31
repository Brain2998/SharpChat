using System;
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
		private bool isConnected = false;

		public Connection(TcpClient Client)
        {
			tcpClient = Client;
        }

		public void AcceptClient()
		{
			clientReader = new StreamReader(tcpClient.GetStream());
			clientWriter = new StreamWriter(tcpClient.GetStream());
			try
			{
				clientName = clientReader.ReadLine().Substring(2);
				if (clientName != "")
				{
					if (Server.UserTable.Contains(clientName))
					{
						CloseConnection("0|103");
					}
					else
					{
						SendMessage("0|100");
						Server.AddUser(this, clientName);
						isConnected = true;
						clientThread = new Thread(RecieveMessages);
                        clientThread.Start();
					}
				}
				else
				{
					CloseConnection("0|102");
				}
			}
			catch (Exception e)
			{
                Server.ChatForm.LogMessage("AcceptClient: " + e.Message);
				if (isConnected)
				{
					CloseConnection("0|101");
				}
			}
		}

		private void RecieveMessages()
		{
			try
			{            
				while (isConnected)
				{
					if (!tcpClient.GetStream().DataAvailable)
					{
						Thread.Sleep(200);
						continue;
					}
					clientMessage = clientReader.ReadLine();
                    switch (clientMessage.Substring(0,2))
                    {
                        case "0|":
                            Server.RemoveUser(clientName);
                            CloseConnection();
                            break;
                        case "1|":
                        case "2|":
                            Server.SendMessages(clientName, clientMessage);
                            break;
                    }
				}
			}
			catch (Exception e)
			{
                Server.ChatForm.LogMessage("RecieveMessage: " + e.Message);
				if (isConnected)
				{
					CloseConnection("0|200");
				}
			}
		}

		public void SendMessage(string message)
		{
			try
			{
				clientWriter.WriteLine(message);
				clientWriter.Flush();
			}
			catch (Exception e)
			{
                Server.ChatForm.LogMessage("SendMessage: " + e.Message);
                if (isConnected)
                {
                    CloseConnection("0|200");
                }
			}
		}

		public void CloseConnection(string reason)
        {
            try
            {
				SendMessage(reason);
                tcpClient.Close();
                isConnected = false;
                clientThread.Join();
                clientReader.Close();
                clientWriter.Close();
            }
            catch (Exception e)
            {
                Server.ChatForm.LogMessage("CloseConnectionReason: " + e.Message);
            }
        }

        public void CloseConnection()
        {
            try
            {
                tcpClient.Close();
                isConnected = false;
                clientThread.Join();
                clientReader.Close();
                clientWriter.Close();
            }
            catch (Exception e)
            {
                Server.ChatForm.LogMessage("CloseConnection: " + e.Message);
            }
        }
    }
}