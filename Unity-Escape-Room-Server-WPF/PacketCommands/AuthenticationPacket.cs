using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    class AuthenticationPacket : Packet
    {
        public string TeamName;

        public AuthenticationPacket(string teamName)
        {
            PacketId = "authentication";
            TeamName = teamName;
        }
    }
