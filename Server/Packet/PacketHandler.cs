using Server.Session;
using ServerCore;

namespace Server.Packet;

public class PacketHandler
{
    public static void C_GainedDmgHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = (ClientSession)session;
        C_GainedDmg gainedDmg = (C_GainedDmg)packet;
        
        if (clientSession.Room == null)
            return;
        
        GameRoom room = clientSession.Room;
        room.Push(
            () => room.CalcDmg(clientSession, gainedDmg)
            );
    }
}