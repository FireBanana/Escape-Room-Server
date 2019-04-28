using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoomServer.PacketCommands
{
    public class PointsUpdatePacket : Packet
    {
        public PointsUpdatePacket()
        {
            PacketId = "pointsUpdate";
        }
    }
}
