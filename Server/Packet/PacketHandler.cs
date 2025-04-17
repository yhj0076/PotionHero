using Server.Session;
using ServerCore;

namespace Server.Packet;

public class PacketHandler
{
    public static void C_PongHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_Pong c_Pong = packet as C_Pong;
        
        if (clientSession.Room == null)
        {
            return;
        }
        
        GameRoom gameRoom = clientSession.Room;
        gameRoom.Push(() =>
        {
            gameRoom.Ping(clientSession,c_Pong.pong);
        });
    }
}