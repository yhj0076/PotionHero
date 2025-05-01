using Server.Session;
using ServerCore;

namespace Server.Packet;

public class PacketHandler
{
    public static void C_LeaveGameHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;
        
        GameRoom gameRoom = clientSession.Room;
        gameRoom.Push(() =>
        {
            gameRoom.Leave(clientSession);
        });
    }

    public static void C_GainedDmgHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_GainedDmg cGainedDmg = packet as C_GainedDmg;
        
        if(clientSession.Room == null)
            return;
        
        GameRoom gameRoom = clientSession.Room;
        gameRoom.Push(() =>
        {
            gameRoom.GainDmg(clientSession, cGainedDmg);
        });
    }

    public static void C_TimeUpHandler(PacketSession session, IPacket packet)
    {
        // ClientSession clientSession = session as ClientSession;
        //
        // GameRoom gameRoom = clientSession.Room;
        // gameRoom.Push(() =>
        // {
        //     gameRoom.Stop(clientSession);
        // });
    }

    public static void C_JoinedGameHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        
        GameRoom gameRoom = clientSession.Room;
        gameRoom.Push(() =>
        {
            gameRoom.Start();
        });
    }
}