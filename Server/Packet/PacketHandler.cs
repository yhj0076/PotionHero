using Server.Session;
using ServerCore;

namespace Server.Packet;

public class PacketHandler
{
    public static void C_GainedDmgHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = (ClientSession)session;

        if (clientSession.Room == null)
            return;
        
        GameRoom room = clientSession.Room;
        room.Push(
            () => room.Leave(clientSession)
            );
    }
}