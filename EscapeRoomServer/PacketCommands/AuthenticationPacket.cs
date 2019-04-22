using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoomServer
{
    class AuthenticationPacket : Packet
    {
        public string AuthenticationData;

        public AuthenticationPacket(string mssg, string data)
        {
            PacketId = "authentication";
            AuthenticationData = data;
            Message = mssg;
        }
    }
}
