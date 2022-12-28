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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WpfAppServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Listner listner;
        ObservableCollection<Client> lstClientConnected;
        public MainWindow()
        {
            InitializeComponent();
            listner = new Listner(4000);
            listner.SocketAccepted += Listner_SocketAccepted;
            lstClientConnected = new ObservableCollection<Client>();
            Loaded += MainWindow_Loaded; 
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            listner.Start();
        }

        private void Listner_SocketAccepted(Socket e)
        {
            Client client = new Client(e);
            
            client.Received += Client_Received;
            client.Disconnected += Client_Disconnected;
            client.Message = "X";
            client.TimeConnected = Convert.ToDateTime(null);
            Application.Current.Dispatcher.Invoke(() =>
            {
                lstClientConnected.Add(client);
                ListViewItems.ItemsSource =  lstClientConnected; 
            });
        }

        private void Client_Disconnected(Client sender)
        {
            Application.Current.Dispatcher.Invoke(() => {
                for (int i = 0; i < ListViewItems.Items.Count; i++)
                {
                    Client client = ListViewItems.Items[i] as Client;

                    if (client.ID == sender.ID)
                    {
                        lstClientConnected.RemoveAt(i);
                        ListViewItems.ItemsSource = lstClientConnected;
                    }
                }
            });
            
        }

        private void Client_Received(Client sender, byte[] data)
        {
            Application.Current.Dispatcher.Invoke(() => {
                for (int i = 0; i < ListViewItems.Items.Count; i++)
                {
                    Client client = ListViewItems.Items[i] as Client;

                    if (client.ID == sender.ID)
                    {
                        lstClientConnected[i].Message = Encoding.ASCII.GetString(data);
                        lstClientConnected[i].TimeConnected = DateTime.Now;

                        
                        break;
                    }
                }
            });
        }
    }
}
