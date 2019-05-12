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
        public Dictionary<TcpClient, int> TeamListDictionary = new Dictionary<TcpClient, int>();
        public static MainWindow Instance;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;

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
                    ClientListBox.Dispatcher.BeginInvoke(del, new object[] { teamName, client });
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

        void AddToList(string name, TcpClient client)
        {
            var pos = ClientListBox.Items.Add(name);
            TeamListDictionary.Add(client, pos);
        }

        void RemoveFromList(TcpClient client)
        {
            if (TeamListDictionary.ContainsKey(client))
            {
                ClientListBox.Items.RemoveAt(TeamListDictionary[client]);
                TeamListDictionary.Remove(client);
            }
            else
            {
                MessageBox.Show("Team is not in list but trying to be removed");
            }
        }

        private void OnTeamListDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(ClientListBox.SelectedItem == null)
            {
                MessageBox.Show("No item selected");
                return;
            }

            if(WindowManager.IsWindowOpen((string)ClientListBox.SelectedItem))
            {
                MessageBox.Show("This teams window is already open");
                return;
            }
            
            var newTeamWindow = new Unity_Escape_Room_Server_WPF.Windows.TeamWindow(handler.TeamsList[(string)ClientListBox.SelectedItem]);
            WindowManager.SetWindowOpenState((string)ClientListBox.SelectedItem, true, newTeamWindow);
            newTeamWindow.Show();
        }

        private void OnLobbyClick(object sender, RoutedEventArgs e)
        {
            if (WindowManager.IsWindowOpen("lobby"))
            {
                MessageBox.Show("Lobby window is already open");
                return;
            }

            var newLobbyWindow = new LobbyScreen();
            WindowManager.SetWindowOpenState("lobby", true, newLobbyWindow);
            newLobbyWindow.Show();
        }
    }
}
