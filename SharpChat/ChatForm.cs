using System;
using System.Net;
using Gtk;
using SharpChat;

public partial class MainWindow : Gtk.Window
{
	private bool Running = false;
	private Server server;
	private IPAddress address;
	public ListStore usersList = new ListStore(typeof(string));

	public string Log
	{
		get
		{
			return textLog.Buffer.Text;
		}
		set
		{
			textLog.Buffer.Text += value+"\n";
		}
	}

	public string ipAddress
	{
		get
		{
			return serverIp.Text;
		}
		set
		{
			serverIp.Text = value;
		}
	}

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {	      
		Build();
		TreeViewColumn usersColumn = new TreeViewColumn();
        usersColumn.Title = "List of connected users";
        connectedUses.AppendColumn(usersColumn);
        connectedUses.Model = usersList;      
        CellRendererText usersCell = new CellRendererText();
        usersColumn.PackStart(usersCell, true);      
        usersColumn.AddAttribute(usersCell, "text", 0);
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
		if (server.isRunning)
		{
			server.StopListening();
		}
        Application.Quit();
        a.RetVal = true;
    }

    private void StartFormChange(bool running)
	{
		Running = running;
		serverIp.IsEditable = !running;
		startServer.Label = running ? "Stop server" : "Start Server";
	}
    
	protected void OnStartServerClicked(object sender, EventArgs e)
	{
		try
		{
			if (Running)
			{
				server.StopListening();
				Log = "Server is terminated.";
				StartFormChange(false);
			}
			else
			{
				if (IPAddress.TryParse(ipAddress, out address))
				{
					server = new Server(address, this);
					server.StartListening();
					Log = "Waiting for connections...";
					StartFormChange(true);
				}
				else
				{
					Log = "Not valid IP address.";
				}
			}
		}
		catch (Exception err)
        {
           Log = "StartClicked: " + err.Message;
        }
    }
}