using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameEndPacket : Packet
{
    public string FinalChoice;
    public int FinalTime;
    public int FinalScore;
    //public int ElapsedTime;

    public GameEndPacket(string teamName, string finalChoice, int finalTime, int finalScore /*int elapsedTime*/)
    {
        PacketId = "gameEnd";
        FinalChoice = finalChoice;
        FinalTime = finalTime;
        TeamName = teamName;
        FinalScore = finalScore;
        //ElapsedTime = elapsedTime;
    }
}

