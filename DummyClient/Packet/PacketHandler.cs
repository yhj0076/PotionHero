using System.Threading.Channels;
using DummyClient.Session;
using ServerCore;

namespace DummyClient.Packet;

public class PacketHandler
{
    public static void S_PingHandler(PacketSession session, IPacket packet)
    {
        ServerSession serverSession = session as ServerSession;
        S_Ping packetData = packet as S_Ping;
        Console.WriteLine($"Ping : {packetData.ping}");
        serverSession.pong = packetData.ping + 1;
        packetData.ping = serverSession.pong;
    }
}