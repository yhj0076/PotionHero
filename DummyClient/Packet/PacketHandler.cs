using DummyClient.Session;
using ServerCore;

namespace DummyClient.Packet;

public class PacketHandler
{
    public static void S_BroadcastGainedDmgHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastGainedDmg packetData = packet as S_BroadcastGainedDmg;
        ServerSession serverSession = session as ServerSession;
    }
}