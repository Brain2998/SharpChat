using System;
using System.Net;
using Gtk;
using SharpChat;
using System.Threading.Tasks;

public partial class MainWindow : Gtk.Window
{
	private bool Running = false;
	private Server server;
	private IPAddress address;
	public ListStore usersList = new ListStore(typeof(string));

    public async void LogMessage(string message)
    {
        Label logMessage = new Label();
        logMessage.Xalign = 0;
        logMessage.Yalign = 0;
        logMessage.LabelProp = message;
        logBox.PackStart(logMessage, false, false, 0);
        logBox.ShowAll();
        await PutTaskDelay();
        Adjustment logAdjustment = logWindow.Vadjustment;
        logWindow.Vadjustment.Value = logAdjustment.Upper - logAdjustment.PageSize;
    }

    async Task PutTaskDelay()
    {
        await Task.Delay(100);
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
        usersColumn.Title = "List of connected clients";
        connectedUses.AppendColumn(usersColumn);
        connectedUses.Model = usersList;      
        CellRendererText usersCell = new CellRendererText();
        usersColumn.PackStart(usersCell, true);      
        usersColumn.AddAttribute(usersCell, "text", 0);
        GLib.ExceptionManager.UnhandledException += logException;
    }

    void logException(GLib.UnhandledExceptionArgs args)
    {
        args.ExitApplication = false;
        LogMessage("GlobalException: " + args.ExceptionObject.ToString());
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
		if (server.isRunning)
		{
			server.StopListening();
		}
        Environment.Exit(0);
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
                LogMessage("Server is terminated.");
				StartFormChange(false);
			}
			else
			{
				if (IPAddress.TryParse(ipAddress, out address))
				{
					server = new Server(address, this);
					server.StartListening();
                    LogMessage("Waiting for connections...");
					StartFormChange(true);
				}
				else
				{
                    LogMessage("Not valid IP address.");
				}
			}
		}
		catch (Exception err)
        {
            LogMessage("StartClicked: " + err.Message);
        }
    }

    protected void OnKickClientClicked(object sender, EventArgs e)
    {
        TreeIter iter;
        TreePath[] treePath = connectedUses.Selection.GetSelectedRows();
        for (int i = 0; i < treePath.Length; ++i)
        {
            usersList.GetIter(out iter, treePath[i]);
            server.KickUser(usersList.GetValue(iter, 0).ToString());
            usersList.Remove(ref iter);
        }
    }
}