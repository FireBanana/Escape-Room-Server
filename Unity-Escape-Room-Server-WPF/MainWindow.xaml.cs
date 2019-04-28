using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

namespace Unity_Escape_Room_Server_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NetworkHandler handler;
        Dictionary<TcpClient, int> TeamListDictionary = new Dictionary<TcpClient, int>();

        void AddToList(string name, TcpClient client)
        {
            var pos = ClientListBox.Items.Add(name);
            TeamListDictionary.Add(client, pos);
        }

        void RemoveFromList(TcpClient client)
        {
            if(TeamListDictionary.ContainsKey(client))
            {
                ClientListBox.Items.RemoveAt(TeamListDictionary[client]);
                TeamListDictionary.Remove(client);
            }
            else
            {
                MessageBox.Show("Team is not in list but trying to be removed");
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            handler = new NetworkHandler("localhost");
            IpAddressText.Content = handler.IpAddress + ":" + NetworkHandler.PORT;

            handler.OnClientConnected += (teamName, client) =>
            {
                if (ClientListBox.Dispatcher.Thread != Thread.CurrentThread)
                {
                    var del = new NetworkHandler.ClientConnectionCallback((newTeamName, newClient) =>
                    {
                        AddToList(newTeamName, newClient);
                    });
                    ClientListBox.Dispatcher.BeginInvoke(del, new object[] { teamName });
                }
                else
                {
                    AddToList(teamName, client);
                }
            };
            handler.OnClientDisconnected += (client) =>
            {
                if (ClientListBox.Dispatcher.Thread != Thread.CurrentThread)
                {
                    var del = new NetworkHandler.ClientDisconnectionCallback(newClient =>
                    {
                        RemoveFromList(newClient);
                    });
                    ClientListBox.Dispatcher.BeginInvoke(del, new object[] { client });
                }
                else
                {
                    RemoveFromList(client);
                }
            };
        }
    }
}
