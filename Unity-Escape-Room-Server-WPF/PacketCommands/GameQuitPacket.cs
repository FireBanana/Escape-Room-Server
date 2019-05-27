public class GameQuitPacket : Packet
{

    public GameQuitPacket(string teamName)
    {
        PacketId = "gameQuit";
        TeamName = teamName;
    }
}
