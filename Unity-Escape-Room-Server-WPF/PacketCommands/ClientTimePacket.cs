using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ClientTimePacket : Packet
{
    public string Time { get; set; } 

    public ClientTimePacket(string teamName, string time)
    {
        PacketId = "clientTime";
        TeamName = teamName;
        Time = time;
    }
}
