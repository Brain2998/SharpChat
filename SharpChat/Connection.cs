﻿using System;
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
				//clientName = clientName;
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
				Server.ChatForm.Log = "AcceptClient: " + e.Message;
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
					if (clientMessage.StartsWith("0|"))
					{
						Server.RemoveUser(clientName);
						CloseConnection();
					}
					if (clientMessage.StartsWith("1|"))
					{
						Server.SendMessages(clientName, clientMessage.Substring(2));
					}
				}
			}
			catch (Exception e)
			{
				Server.ChatForm.Log = "RecieveMessage: " + e.Message;
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
				Server.ChatForm.Log = "SendMessage: " + e.Message;
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
                Server.ChatForm.Log = "CloseConnectionReason: " + e.Message;
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
                Server.ChatForm.Log = "CloseConnection: " + e.Message;
            }
        }
    }
}