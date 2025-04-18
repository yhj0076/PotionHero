using DummyClient.Packet;
using ServerCore;

public class PacketManager
{
    #region Singleton
    private static PacketManager _instance = new PacketManager();
    public static PacketManager Instance { get { return _instance; } }
    #endregion

    PacketManager()
    {
        Register();
    }
    
    Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makeFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
    Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();

    public void Register()
    {
        _makeFunc.Add((ushort)PacketType.S_JoinGameRoom, MakePacket<S_JoinGameRoom>);
        _handler.Add((ushort)PacketType.S_JoinGameRoom, PacketHandler.S_JoinGameRoomHandler);
        _makeFunc.Add((ushort)PacketType.S_BroadcastGameStart, MakePacket<S_BroadcastGameStart>);
        _handler.Add((ushort)PacketType.S_BroadcastGameStart, PacketHandler.S_BroadcastGameStartHandler);
        _makeFunc.Add((ushort)PacketType.S_BroadcastEndGame, MakePacket<S_BroadcastEndGame>);
        _handler.Add((ushort)PacketType.S_BroadcastEndGame, PacketHandler.S_BroadcastEndGameHandler);
        _makeFunc.Add((ushort)PacketType.S_BroadcastLeaveGame, MakePacket<S_BroadcastLeaveGame>);
        _handler.Add((ushort)PacketType.S_BroadcastLeaveGame, PacketHandler.S_BroadcastLeaveGameHandler);
        _makeFunc.Add((ushort)PacketType.S_AttackResult, MakePacket<S_AttackResult>);
        _handler.Add((ushort)PacketType.S_AttackResult, PacketHandler.S_AttackResultHandler);
        _makeFunc.Add((ushort)PacketType.S_BroadCastGainedDmg, MakePacket<S_BroadCastGainedDmg>);
        _handler.Add((ushort)PacketType.S_BroadCastGainedDmg, PacketHandler.S_BroadCastGainedDmgHandler);

    }

    public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IPacket> onRecvCallback = null)
    {
        ushort count = 0;
        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += sizeof(ushort);
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += sizeof(ushort);
        
        Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
        if (_makeFunc.TryGetValue(id, out func))
        {
            IPacket packet = func.Invoke(session, buffer);
            if(onRecvCallback != null)
                onRecvCallback.Invoke(session, packet);
            else
                HandlePacket(session, packet);
        }
    }
    
    T MakePacket<T>(PacketSession packetSession, ArraySegment<byte> buffer) where T : IPacket, new()
    {
        T packet = new T();
        packet.Read(buffer);
        return packet;
    }

    public void HandlePacket(PacketSession session, IPacket packet)
    {
        Action<PacketSession, IPacket> action = null;
        if(_handler.TryGetValue(packet.Protocol, out action))
            action.Invoke(session, packet);
    }
}