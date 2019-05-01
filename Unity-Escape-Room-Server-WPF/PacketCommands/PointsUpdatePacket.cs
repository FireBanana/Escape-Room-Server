using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class PointsUpdatePacket : Packet
    {
        public int NewPoints;

        public PointsUpdatePacket(string teamName, int points)
        {
            PacketId = "pointsUpdate";
            NewPoints = points;
            TeamName = teamName;
        }
    }

