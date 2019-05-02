using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class HintResponsePacket : Packet
{
    public string Hint;

    public HintResponsePacket(string teamName, string hint)
    {
        PacketId = "hintResponse";
        TeamName = teamName;
        Hint = hint;
    }
}
