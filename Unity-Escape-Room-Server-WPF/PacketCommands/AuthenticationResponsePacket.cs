using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class AuthenticationResponsePacket : Packet
    {
        public string TeamName;

        public AuthenticationResponsePacket(string teamName)
        {
            PacketId = "authenticationResponse";
            TeamName = teamName;
        }
    }

