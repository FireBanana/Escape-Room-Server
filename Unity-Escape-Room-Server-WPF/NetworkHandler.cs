using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Unity_Escape_Room_Server_WPF
{
    class NetworkHandler
    {
        public const int PORT = 2000;
        TcpListener listener;

        public List<TcpClient> ClientList = new List<TcpClient>();
        public string IpAddress;

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
            ClientList.Add(client);
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
                                //MessageBox.Show("Received Message: " + packet.PacketId);

                                switch (packet.PacketId)
                                {
                                    case "authentication":
                                        var authPacket = (AuthenticationPacket)packet;
                                        SendAuthenticationResponse(client, "null");
                                        OnClientConnected.Invoke(authPacket.TeamName, client);
                                        break;
                                }
                                
                            }
                            catch
                            {
                                //MessageBox.Show("Could not deserialize");
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.Print("Transfer Error: " + e.ToString());
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
