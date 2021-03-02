using LiteNetLib;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Unity_Escape_Room_Server_WPF.Windows;

namespace Unity_Escape_Room_Server_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NetworkHandler handler;
        public Dictionary<NetPeer, int> TeamListDictionary = new Dictionary<NetPeer, int>();
        public static MainWindow Instance;

        delegate void UICallback();

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

        void AddToList(string name, NetPeer client)
        {
            var pos = ClientListBox.Items.Add(name);
            if (!TeamListDictionary.ContainsKey(client))
                TeamListDictionary.Add(client, pos);
        }

        public void RemoveFromList(NetPeer client)
        {
            if (TeamListDictionary.ContainsKey(client))
            {
                if (ClientListBox.Dispatcher.Thread != Thread.CurrentThread)
                {
                    var del = new UICallback(() =>
                    {
                        ClientListBox.Items.RemoveAt(TeamListDictionary[client]);
                        if(handler.ClientList.ContainsValue(client))
                        {
                            foreach(var item in handler.ClientList)
                            {
                                if(item.Value == client)
                                {
                                    handler.TeamsList.Remove(item.Key);
                                }
                            }
                        }                        
                        TeamListDictionary.Remove(client);
                    });
                    ClientListBox.Dispatcher.BeginInvoke(del, null);
                }
                else
                {
                    ClientListBox.Items.RemoveAt(TeamListDictionary[client]);
                    if (handler.ClientList.ContainsValue(client))
                    {
                        foreach (var item in handler.ClientList)
                        {
                            if (item.Value == client)
                            {
                                handler.TeamsList.Remove(item.Key);
                            }
                        }
                    }
                    TeamListDictionary.Remove(client);
                }

            }
            else
            {
                MessageBox.Show("Team is not in list but trying to be removed");
            }
        }

        private void OnTeamListDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ClientListBox.SelectedItem == null)
            {
                MessageBox.Show("No item selected");
                return;
            }

            if (WindowManager.IsWindowOpen((string)ClientListBox.SelectedItem))
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

        private void OnRoomScoreboard(object sender, RoutedEventArgs e)
        {
            if(WindowManager.IsWindowOpen("scoreboard"))
            {
                MessageBox.Show("Room Scoreboard is already open");
                return;
            }

            var newScoreboard = new RoomScoreboard(null);
            WindowManager.SetWindowOpenState("scoreboard", true, newScoreboard);
            newScoreboard.Show();
        }
    }
}
