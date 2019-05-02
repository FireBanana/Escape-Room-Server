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

namespace Unity_Escape_Room_Server_WPF
{
    public class NetworkHandler
    {
        public const int PORT = 2000;
        TcpListener listener;

        public static NetworkHandler Instance;

        public string IpAddress;

        public Dictionary<string, Team> TeamsList = new Dictionary<string, Team>();
        public Dictionary<string, TcpClient> ClientList = new Dictionary<string, TcpClient>();

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
                            var buffer = new byte[client.ReceiveBufferSize];
                            client.GetStream().Read(buffer, 0, buffer.Length);
                            try
                            {
                                var packet = JsonConvert.DeserializeObject<Packet>(Encoding.ASCII.GetString(buffer));

                                switch (packet.PacketId)
                                {
                                    case "authentication":
                                        var authPacket = JsonConvert.DeserializeObject<AuthenticationPacket>(Encoding.ASCII.GetString(buffer));
                                        SendAuthenticationResponse(client, "null");
                                        OnClientConnected.Invoke(authPacket.TeamName, client);

                                        if (!TeamsList.ContainsKey(authPacket.TeamName))
                                        {
                                            TeamsList.Add(authPacket.TeamName, new Team(authPacket.TeamName));
                                        }

                                        if(!ClientList.ContainsKey(authPacket.TeamName))
                                        {
                                            ClientList.Add(authPacket.TeamName, client);
                                        }

                                        break;
                                    case "pointsUpdate":
                                        var pointsPacket = JsonConvert.DeserializeObject<PointsUpdatePacket>(Encoding.ASCII.GetString(buffer));
                                        if (TeamsList.ContainsKey(pointsPacket.TeamName))
                                        {
                                            TeamsList[pointsPacket.TeamName].UpdatePoints(pointsPacket.NewPoints);
                                        }
                                        else
                                            MessageBox.Show("Trying to update points of non-existant team");


                                        break;
                                }
                                
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message);
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.Print("Transfer Error: " + e.ToString());
                            // TODO: Remove clients
                            OnClientDisconnect(client);
                            break;
                        }
                    }
                }
            }
            );
        }
    }
}
