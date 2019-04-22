using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace EscapeRoomServer
{
    public partial class Form1 : Form
    {

        NetworkHandler handler;

        public Form1()
        {
            InitializeComponent();
            handler = new NetworkHandler("localhost");
            IpAddressText.Text = handler.IpAddress + ":" + NetworkHandler.PORT;

            handler.OnClientConnected += (client) => 
            {
                if (ClientListBox.InvokeRequired)
                {
                    var del = new NetworkHandler.ClientConnectionCallback(newClient => 
                    {
                        ClientListBox.Items.Add(newClient.Client.RemoteEndPoint.ToString());
                    });
                    Invoke(del, new object[] { client });
                }
                else
                {
                    ClientListBox.Items.Add(client.Client.RemoteEndPoint.ToString());
                }
            };
            handler.OnClientDisconnected += (client) =>
            {
                if (ClientListBox.InvokeRequired)
                {
                    var del = new NetworkHandler.ClientConnectionCallback(newClient =>
                    {
                        ClientListBox.Items.Remove(newClient.Client.RemoteEndPoint.ToString());
                    });
                    Invoke(del, new object[] { client });
                }
                else
                {
                    ClientListBox.Items.Remove(client.Client.RemoteEndPoint.ToString());
                }
            };
        }
    }
}
