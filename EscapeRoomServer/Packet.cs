using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoomServer
{
    [Serializable]
    public class Packet
    {
        public string PacketId;
        public string Message;
        public enum Type
        {
            Authentication, HintRequest, PointsUpdate, HelpRequest, PauseGame
        }

        public Type ResolvePacketType(string id)
        {
            switch(id)
            {
                case "authentication":
                    return Type.Authentication;
                case "hintRequest":
                    return Type.HintRequest;
                case "pointsUpdate":
                    return Type.PointsUpdate;
                case "helpRequest":
                    return Type.HelpRequest;
                case "pauseGame":
                    return Type.PauseGame;
                default:
                    return Type.Authentication;
            }
        }
    }
}
