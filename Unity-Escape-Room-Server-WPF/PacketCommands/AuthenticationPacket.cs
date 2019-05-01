using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class AuthenticationPacket : Packet
    {
        public AuthenticationPacket(string teamName)
        {
            PacketId = "authentication";
            TeamName = teamName;
        }
    }
