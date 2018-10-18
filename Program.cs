using System;
using Gtk;

namespace SharpChat
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Application.Init();
            MainWindow win = new MainWindow();
            win.Show();
//testcommit123
			Application.Run();
        }
    }
}
