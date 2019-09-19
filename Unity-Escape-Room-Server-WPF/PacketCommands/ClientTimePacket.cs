using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ClientTimePacket : Packet
{
    public int ElaspsedSeconds { get; set; } //elapsed

    public ClientTimePacket(string teamName, int time)
    {
        PacketId = "clientTime";
        TeamName = teamName;
        ElaspsedSeconds = time;
    }
}
