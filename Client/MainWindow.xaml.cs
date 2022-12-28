using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;

namespace ServerSideWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Socket socket;
        public MainWindow()
        {
            InitializeComponent();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            socket.Connect(IPAddress.Parse("127.0.0.1"), 4000);
            MessageBox.Show("Connected");
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            int s= socket.Send(Encoding.ASCII.GetBytes(txtSend.Text));

            if (s>0)

            {
                MessageBox.Show("Data Sent");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            socket.Shutdown(0);
            socket.Close();
            socket.Dispose();

            Application.Current.Shutdown(0);
        }
    }
}
