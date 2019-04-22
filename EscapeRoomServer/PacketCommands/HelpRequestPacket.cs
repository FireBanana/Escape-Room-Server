using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoomServer.PacketCommands
{
    public class HelpRequestPacket : Packet
    {
        public HelpRequestPacket()
        {
            PacketId = "helpRequest";
        }
    }
}
