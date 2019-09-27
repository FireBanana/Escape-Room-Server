using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GameEndRequestPacket : Packet
{
    public int Score;

    public GameEndRequestPacket(string teamName, int score)
    {
        PacketId = "gameEndRequest";
        TeamName = teamName;
        Score = score;
    }
}
