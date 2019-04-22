using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EscapeRoomServer
{
    class NetworkHandler
    {
        public const int PORT = 2000;
        TcpListener listener;

        public List<TcpClient> ClientList = new List<TcpClient>();
        public string IpAddress;

        //Events
        public delegate void ClientConnectionCallback(TcpClient client);
        public event ClientConnectionCallback OnClientConnected;
        public event ClientConnectionCallback OnClientDisconnected;

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
            OnClientConnected.Invoke(client);
        }

        void OnClientDisconnect(TcpClient client)
        {
            OnClientDisconnected.Invoke(client);
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
                                Debug.Print("Received Message: " + packet.PacketId);
                            }
                            catch
                            {
                                MessageBox.Show("Could not deserialize");
                                Debug.Print("Could not deserialize");

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
