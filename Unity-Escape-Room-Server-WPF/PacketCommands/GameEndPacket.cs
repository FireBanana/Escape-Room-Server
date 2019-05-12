using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameEndPacket : Packet
{
    public string FinalChoice;
    public string FinalTime;

    public GameEndPacket(string teamName, string finalChoice, string finalTime)
    {
        PacketId = "gameEnd";
        FinalChoice = finalChoice;
        FinalTime = finalTime;
        TeamName = teamName;
    }
}

