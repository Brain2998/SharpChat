
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.Fixed fixed1;

	private global::Gtk.Label serverIpLabel;

	private global::Gtk.Entry serverIp;

	private global::Gtk.Button startServer;

	private global::Gtk.ScrolledWindow GtkScrolledWindow;

	private global::Gtk.TextView textLog;

	protected virtual void Build()
	{
		global::Stetic.Gui.Initialize(this);
		// Widget MainWindow
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString("SharpChat");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		this.Resizable = false;
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.fixed1 = new global::Gtk.Fixed();
		this.fixed1.Name = "fixed1";
		this.fixed1.HasWindow = false;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.serverIpLabel = new global::Gtk.Label();
		this.serverIpLabel.Name = "serverIpLabel";
		this.serverIpLabel.LabelProp = global::Mono.Unix.Catalog.GetString("IP-address");
		this.fixed1.Add(this.serverIpLabel);
		global::Gtk.Fixed.FixedChild w1 = ((global::Gtk.Fixed.FixedChild)(this.fixed1[this.serverIpLabel]));
		w1.X = 16;
		w1.Y = 14;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.serverIp = new global::Gtk.Entry();
		this.serverIp.CanFocus = true;
		this.serverIp.Name = "serverIp";
		this.serverIp.IsEditable = true;
		this.serverIp.InvisibleChar = '•';
		this.fixed1.Add(this.serverIp);
		global::Gtk.Fixed.FixedChild w2 = ((global::Gtk.Fixed.FixedChild)(this.fixed1[this.serverIp]));
		w2.X = 95;
		w2.Y = 7;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.startServer = new global::Gtk.Button();
		this.startServer.CanFocus = true;
		this.startServer.Name = "startServer";
		this.startServer.UseUnderline = true;
		this.startServer.Label = global::Mono.Unix.Catalog.GetString("Start server");
		this.fixed1.Add(this.startServer);
		global::Gtk.Fixed.FixedChild w3 = ((global::Gtk.Fixed.FixedChild)(this.fixed1[this.startServer]));
		w3.X = 265;
		w3.Y = 5;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
		this.GtkScrolledWindow.WidthRequest = 340;
		this.GtkScrolledWindow.HeightRequest = 300;
		this.GtkScrolledWindow.Name = "GtkScrolledWindow";
		this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
		this.textLog = new global::Gtk.TextView();
		this.textLog.CanFocus = true;
		this.textLog.Name = "textLog";
		this.textLog.Editable = false;
		this.GtkScrolledWindow.Add(this.textLog);
		this.fixed1.Add(this.GtkScrolledWindow);
		global::Gtk.Fixed.FixedChild w5 = ((global::Gtk.Fixed.FixedChild)(this.fixed1[this.GtkScrolledWindow]));
		w5.X = 17;
		w5.Y = 57;
		this.Add(this.fixed1);
		if ((this.Child != null))
		{
			this.Child.ShowAll();
		}
		this.DefaultWidth = 376;
		this.DefaultHeight = 445;
		this.Show();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler(this.OnDeleteEvent);
		this.startServer.Clicked += new global::System.EventHandler(this.OnStartServerClicked);
	}
}
