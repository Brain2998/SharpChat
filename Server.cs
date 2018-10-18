using System;
using System.Net;
using System.Net.Sockets;

namespace SharpChat
{
    public class Server
    {
		private IPAddress ipAddress;
	    //private TcpClient tcpClient;
	    private TcpListener tcpListener;

		public Server(IPAddress address)
		{
			ipAddress = address;
		}

		public void StartListening()
		{
			tcpListener=new TcpListener(ipAddress, 1986);
			tcpListener.Start();

        }
    }
}
