using EscapeRoomServer.PacketCommands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using System.Net.NetworkInformation;
using Unity_Escape_Room_Server_WPF.Windows;
using System.IO;

namespace Unity_Escape_Room_Server_WPF
{
    public class NetworkHandler
    {
        public const int PORT = 2000;
        TcpListener listener;

        public static NetworkHandler Instance;

        public string IpAddress;

        public Dictionary<string, Team> TeamsList = new Dictionary<string, Team>();
        //public Dictionary<string, Team> LobbyTeamsList = new Dictionary<string, Team>();
        public Dictionary<string, TcpClient> ClientList = new Dictionary<string, TcpClient>();
        public Dictionary<string, Team> CompletedTeamList = new Dictionary<string, Team>();

        //Events
        public delegate void ClientConnectionCallback(string teamName, TcpClient client);
        public delegate void ClientDisconnectionCallback(TcpClient client);
        public event ClientConnectionCallback OnClientConnected;
        public event ClientDisconnectionCallback OnClientDisconnected;

        public NetworkHandler(string ipAddress)
        {
            foreach (var address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    listener = new TcpListener(address, PORT);
                    IpAddress = address.ToString();
                }
            }

            Instance = this;
            listener.Start();
            BeginExecutingClientConnectListener();
        }

        public void BeginExecutingClientConnectListener()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var client = listener.AcceptTcpClient();
                    OnClientConnect(client);
                }
            });
        }

        void OnClientConnect(TcpClient client)
        {
            BeginClientListener(client);            
        }

        void OnClientDisconnect(TcpClient client)
        {
            OnClientDisconnected.Invoke(client);
        }

        void SendAuthenticationResponse(TcpClient client, string teamName)
        {
            var packet = new AuthenticationResponsePacket(teamName);

            var serializedPacket = JsonConvert.SerializeObject(packet);
            var buff = Encoding.ASCII.GetBytes(serializedPacket);
            client.GetStream().Write(buff, 0, buff.Length);
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
                ClientList[teamName].GetStream().Write(buff, 0, buff.Length);
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
                ClientList[teamName].GetStream().Write(buff, 0, buff.Length);
            }
            else
            {
                MessageBox.Show("Trying to send pause to a team that does not exist");
            }
        }

        void BeginClientListener(TcpClient client)
        {
            Task.Run(() =>
            {
                while (true)
                {

                    if (client.Connected)
                    {
                        try
                        {
                            if (!client.GetStream().DataAvailable)
                                continue;

                            var buffer = new byte[client.ReceiveBufferSize];
                            client.GetStream().Read(buffer, 0, buffer.Length);

                            var reader = new JsonTextReader(new StringReader(Encoding.ASCII.GetString(buffer)));
                            reader.SupportMultipleContent = true;

                            var packetList = new List<Packet>();
                            try
                            {
                                while (true)
                                {
                                    if (!reader.Read())
                                    {
                                        break;
                                    }

                                    JsonSerializer serializer = new JsonSerializer();
                                    Packet _packet = serializer.Deserialize<Packet>(reader);

                                    packetList.Add(_packet);
                                }

                                foreach (var packet in packetList)
                                {
                                    //var packet = JsonConvert.DeserializeObject<Packet>(Encoding.ASCII.GetString(buffer));

                                    switch (packet.PacketId)
                                    {
                                        case "authentication":
                                            var authPacket = JsonConvert.DeserializeObject<AuthenticationPacket>(Encoding.ASCII.GetString(buffer));
                                            SendAuthenticationResponse(client, "null");
                                            OnClientConnected.Invoke(authPacket.TeamName, client);

                                            if (!TeamsList.ContainsKey(authPacket.TeamName))
                                            {
                                                var newTeam = new Team(authPacket.TeamName);
                                                TeamsList.Add(authPacket.TeamName, newTeam);
                                                //LobbyTeamsList.Add(authPacket.TeamName, newTeam);
                                            }

                                            if (!ClientList.ContainsKey(authPacket.TeamName))
                                            {
                                                ClientList.Add(authPacket.TeamName, client);
                                            }

                                            break;
                                        case "gameQuit":
                                            var quitPacket = JsonConvert.DeserializeObject<GameQuitPacket>(Encoding.ASCII.GetString(buffer));
                                            if (TeamsList.ContainsKey(quitPacket.TeamName))
                                                TeamsList[quitPacket.TeamName].Stop(null, 0);
                                            else
                                                Task.Run(() => { MessageBox.Show("Disconnecting Team not found"); });
                                            Task.Run(() => { MessageBox.Show("Client disconnected through packet"); });
                                            OnClientDisconnect(client);
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
                                            TeamsList[clientTimePacket.TeamName].TimedEvent.Invoke(null, null);
                                            break;

                                        case "gameEnd":
                                            var gameEndPacket = JsonConvert.DeserializeObject<GameEndPacket>(Encoding.ASCII.GetString(buffer));
                                            TeamsList[gameEndPacket.TeamName].FinalChoice = gameEndPacket.FinalChoice;
                                            TeamsList[gameEndPacket.TeamName].Stop(gameEndPacket.FinalTime, gameEndPacket.FinalScore);
                                            //TeamsList[gameEndPacket.TeamName].FinalTime = gameEndPacket.FinalTime;

                                            if (CompletedTeamList.ContainsKey(gameEndPacket.TeamName))
                                            {
                                                Task.Run(() => { MessageBox.Show("A team whose name already exists has completed the game."); });
                                            }
                                            else
                                            {
                                                CompletedTeamList.Add(gameEndPacket.TeamName, TeamsList[gameEndPacket.TeamName]);
                                                MainWindow.Instance.RemoveFromList(client);
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
                            catch (Exception e)
                            {
                                Task.Run(() => {
                                    //MessageBox.Show("First Error in NetworkHandler: " + e.Message + "\n\n" + e.StackTrace + "\n\n" + e.InnerException);
                                    //var builder = new StringBuilder();

                                    //foreach (var item in e.Data)
                                    //{
                                    //    builder.Append(item.ToString() + "\n");
                                    //}

                                    //File.WriteAllText(@"C:\Users\Owais\Desktop\log.txt", builder.ToString());
                                });
                            }
                        }
                        catch (Exception e)
                        {
                            Task.Run(() =>
                            {
                                //MessageBox.Show("Transfer Error: " + e.ToString());
                                OnClientDisconnect(client);

                            });
                            break;
                        }
                    }
                }
            }
            );
        }
    }
}
