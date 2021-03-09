using EscapeRoomServer.PacketCommands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unity_Escape_Room_Server_WPF.Windows;
using System.IO;
using LiteNetLib;

namespace Unity_Escape_Room_Server_WPF
{
    public class NetworkHandler
    {
        public const int PORT = 2000;

        public static NetworkHandler Instance;

        public string IpAddress;

        EventBasedNetListener listener;
        NetManager server;

        NetPeer peer;

        public Dictionary<string, Team> TeamsList = new Dictionary<string, Team>();
        public Dictionary<string, NetPeer> ClientList = new Dictionary<string, NetPeer>();
        public Dictionary<string, Team> CompletedTeamList = new Dictionary<string, Team>();

        //Events
        public delegate void ClientConnectionCallback(string teamName, NetPeer client);
        public delegate void ClientDisconnectionCallback(NetPeer client);
        public event ClientConnectionCallback OnClientConnected;
        public event ClientDisconnectionCallback OnClientDisconnected;

        public NetworkHandler(string ipAddress)
        {
            foreach (var address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    IpAddress = address.ToString();
                }
            }

            listener = new EventBasedNetListener();
            server = new NetManager(listener);
            server.Start(PORT);

            listener.ConnectionRequestEvent += request =>
            {
                request.Accept();
            };

            listener.PeerConnectedEvent += _peer =>
            {

                Task.Run(() => { MessageBox.Show("Device connected", "Alert"); });
                peer = _peer;
            };

            listener.PeerDisconnectedEvent += (_peer, info) =>
            {
                OnClientDisconnect();
            };

            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
            {
                var buffer = new byte[dataReader.AvailableBytes];
                dataReader.GetBytes(buffer, dataReader.AvailableBytes);

                var packet = JsonConvert.DeserializeObject<Packet>(Encoding.ASCII.GetString(buffer));

                ParsePacket(packet, buffer);
                dataReader.Recycle();
            };

            Instance = this;
            BeginClientListener();
        }

        void OnClientDisconnect()
        {
            OnClientDisconnected.Invoke(peer);
        }

        void SendAuthenticationResponse(string teamName)
        {
            var packet = new AuthenticationResponsePacket(teamName);

            var serializedPacket = JsonConvert.SerializeObject(packet);
            var buff = Encoding.ASCII.GetBytes(serializedPacket);
            peer.Send(buff, DeliveryMethod.ReliableUnordered);
        }

        public void SendGameEnd(string teamName, int score)
        {
            var packet = new GameEndRequestPacket(teamName, score);

            var client = ClientList[teamName];

            var serializedPacket = JsonConvert.SerializeObject(packet);
            var buff = Encoding.ASCII.GetBytes(serializedPacket);
            peer.Send(buff, DeliveryMethod.ReliableUnordered);
        }

        public void SendHintResponse(string teamName, string hintDescription)
        {
            if(WindowManager.IsWindowOpen("scoreboard"))
            {
                var window = (RoomScoreboard)WindowManager.GetWindow("scoreboard");
                window.DisplayMessage(hintDescription);
            }

            if(ClientList.ContainsKey(teamName))
            {
                var packet = new HintResponsePacket(teamName, hintDescription);
                var serializedPacket = JsonConvert.SerializeObject(packet);
                var buff = Encoding.ASCII.GetBytes(serializedPacket);
                ClientList[teamName].Send(buff, DeliveryMethod.ReliableUnordered);
            }
            else
            {
                MessageBox.Show("Trying to send a hint to a team that does not exist");
            }
        }

        public void SendPauseCommand(string teamName, bool isPaused)
        {
            if (ClientList.ContainsKey(teamName))
            {
                var packet = new PauseGamePacket(teamName, isPaused);
                var serializedPacket = JsonConvert.SerializeObject(packet);
                var buff = Encoding.ASCII.GetBytes(serializedPacket);
                ClientList[teamName].Send(buff, DeliveryMethod.ReliableUnordered);
            }
            else
            {
                MessageBox.Show("Trying to send pause to a team that does not exist");
            }
        }

        void BeginClientListener()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    server.PollEvents();            
                }
            }
            );
        }

        void ParsePacket(Packet packet, byte[] buffer)
        {
            switch (packet.PacketId)
            {
                case "authentication":
                    var authPacket = JsonConvert.DeserializeObject<AuthenticationPacket>(Encoding.ASCII.GetString(buffer));
                    SendAuthenticationResponse("null");
                    OnClientConnected.Invoke(authPacket.TeamName, peer);
                   
                    if (!TeamsList.ContainsKey(authPacket.TeamName))
                    {
                        var newTeam = new Team(authPacket.TeamName);
                        TeamsList.Add(authPacket.TeamName, newTeam);
                        //LobbyTeamsList.Add(authPacket.TeamName, newTeam);
                    }

                    if (!ClientList.ContainsKey(authPacket.TeamName))
                    {
                        ClientList.Add(authPacket.TeamName, peer);
                    }

                    break;

                case "gameQuit":
                    var quitPacket = JsonConvert.DeserializeObject<GameQuitPacket>(Encoding.ASCII.GetString(buffer));
                    if (TeamsList.ContainsKey(quitPacket.TeamName))
                    {
                        TeamsList[quitPacket.TeamName].Stop();
                        TeamsList[quitPacket.TeamName].Timer.Stop();
                    }
                    else
                        Task.Run(() => { MessageBox.Show("Disconnecting team not found"); });

                    Task.Run(() => { MessageBox.Show("Device Disconnected"); });
                    OnClientDisconnect();
                    return;

                case "pointsUpdate":
                    var pointsPacket = JsonConvert.DeserializeObject<PointsUpdatePacket>(Encoding.ASCII.GetString(buffer));
                    if (TeamsList.ContainsKey(pointsPacket.TeamName))
                    {
                        TeamsList[pointsPacket.TeamName].UpdatePoints(pointsPacket.NewPoints, pointsPacket.IsHidden);
                    }
                    else
                        Task.Run(() => { MessageBox.Show("Trying to update points of non-existant team"); });

                    break;

                case "hintRequest":
                    var hintRequestPacket = JsonConvert.DeserializeObject<HintRequestPacket>(Encoding.ASCII.GetString(buffer));
                    Task.Run(() => { MessageBox.Show("Hint Request Received"); });
                    break;

                case "helpRequest":
                    var helpRequestPacket = JsonConvert.DeserializeObject<HelpRequestPacket>(Encoding.ASCII.GetString(buffer));
                    Task.Run(() => { MessageBox.Show("Help Request Received"); });
                    break;

                case "clientTime":
                    var clientTimePacket = JsonConvert.DeserializeObject<ClientTimePacket>(Encoding.ASCII.GetString(buffer));
                    //TeamsList[clientTimePacket.TeamName].TimedEvent.Invoke(clientTimePacket.ElaspsedSeconds);                                
                    break;

                case "gameEnd":
                    var gameEndPacket = JsonConvert.DeserializeObject<GameEndPacket>(Encoding.ASCII.GetString(buffer));
                    TeamsList[gameEndPacket.TeamName].FinalChoice = gameEndPacket.FinalChoice;
                    TeamsList[gameEndPacket.TeamName].Stop();
                    //TeamsList[gameEndPacket.TeamName].FinalTime = gameEndPacket.FinalTime;

                    if (CompletedTeamList.ContainsKey(gameEndPacket.TeamName))
                    {
                        Task.Run(() => { MessageBox.Show("A team whose name already exists has completed the game."); });
                    }
                    else
                    {
                        CompletedTeamList.Add(gameEndPacket.TeamName, TeamsList[gameEndPacket.TeamName]);
                        MainWindow.Instance.RemoveFromList(peer);
                        if (WindowManager.IsWindowOpen("lobby"))
                        {
                            ((LobbyScreen)WindowManager.GetWindow("lobby")).LoadItemsToTable();
                        }

                        TeamsList.Remove(gameEndPacket.TeamName);
                    }
                    break;
            }
        }
    }
}