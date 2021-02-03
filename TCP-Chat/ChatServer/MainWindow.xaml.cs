using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace ChatServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static IPHostEntry iphostinfo = Dns.GetHostEntry(Dns.GetHostName());
        static IPAddress ip = iphostinfo.AddressList[1]; //may have to be changed depending on the device and network on which the server runs 
        Server server = new Server(ip, 11000);
        public MainWindow()
        {
            InitializeComponent();
            Closing += StopServer;
        }
        public void StartServer(object senderObj, System.EventArgs e)
        {

            server.OpenServer();
            
            Start.IsEnabled = false;
            Stop.IsEnabled = true;

        }
        public void StopServer(object sender, EventArgs e)
        {
            Task.Run(() => server.StopServer());

            Start.IsEnabled = true;
            Stop.IsEnabled = false;
        }
    }
}
