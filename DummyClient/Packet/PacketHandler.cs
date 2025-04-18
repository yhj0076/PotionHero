using System.Threading.Channels;
using DummyClient.Session;
using ServerCore;

namespace DummyClient.Packet;

public class PacketHandler
{
    public static void S_JoinGameRoomHandler(PacketSession session, IPacket packet)
    {
        S_JoinGameRoom joinGameRoom = packet as S_JoinGameRoom;
        ServerSession serverSession = session as ServerSession;
    }

    public static void S_BroadcastGameStartHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastGameStart broadcastGameStart = packet as S_BroadcastGameStart;
        ServerSession serverSession = session as ServerSession;
    }

    public static void S_BroadcastEndGameHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastEndGame endGame = packet as S_BroadcastEndGame;
        ServerSession serverSession = session as ServerSession;
        Console.WriteLine($"Winner: {endGame.WinnerId}");
        serverSession.DisConnect();
    }

    public static void S_BroadcastLeaveGameHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastLeaveGame leaveGame = packet as S_BroadcastLeaveGame;
        ServerSession serverSession = session as ServerSession;
    }

    public static void S_AttackResultHandler(PacketSession session, IPacket packet)
    {
        S_AttackResult attackResult = packet as S_AttackResult;
        ServerSession serverSession = session as ServerSession;
        Console.WriteLine($"result\n" +
                          $"host HP : {attackResult.HostHp}\n" +
                          $"guest HP : {attackResult.GuestHp}");
    }

    public static void S_BroadCastGainedDmgHandler(PacketSession session, IPacket packet)
    {
        S_BroadCastGainedDmg gainedDmg = packet as S_BroadCastGainedDmg;
        ServerSession serverSession = session as ServerSession;

        Console.WriteLine($"\tPlayer1 gainedDmg : {gainedDmg.HostGainedDmg}\n" +
                          $"\tPlayer2 gainedDmg : {gainedDmg.GuestGainedDmg}\n");
    }
}