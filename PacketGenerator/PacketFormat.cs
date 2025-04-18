namespace PacketGenerator;

public class PacketFormat
{
    // {0} 패킷 등록
    // {1} DummyClient or Server
    public static string managerFormat = 
@"using {1}.Packet;
using ServerCore;

public class PacketManager
{{
    #region Singleton
    private static PacketManager _instance = new PacketManager();
    public static PacketManager Instance {{ get {{ return _instance; }} }}
    #endregion

    PacketManager()
    {{
        Register();
    }}
    
    Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makeFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
    Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();

    public void Register()
    {{
{0}
    }}

    public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IPacket> onRecvCallback = null)
    {{
        ushort count = 0;
        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += sizeof(ushort);
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += sizeof(ushort);
        
        Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
        if (_makeFunc.TryGetValue(id, out func))
        {{
            IPacket packet = func.Invoke(session, buffer);
            if(onRecvCallback != null)
                onRecvCallback.Invoke(session, packet);
            else
                HandlePacket(session, packet);
        }}
    }}
    
    T MakePacket<T>(PacketSession packetSession, ArraySegment<byte> buffer) where T : IPacket, new()
    {{
        T packet = new T();
        packet.Read(buffer);
        return packet;
    }}

    public void HandlePacket(PacketSession session, IPacket packet)
    {{
        Action<PacketSession, IPacket> action = null;
        if(_handler.TryGetValue(packet.Protocol, out action))
            action.Invoke(session, packet);
    }}
}}";

    // {0} 패킷 이름
    public static string managerRegisterFormat = 
@"        _makeFunc.Add((ushort)PacketType.{0}, MakePacket<{0}>);
        _handler.Add((ushort)PacketType.{0}, PacketHandler.{0}Handler);";
    
    // {0} 패킷 이름/ 번호 목록
    // {1} 패킷 목록
    public static string fileFormat =
        @"using ServerCore;
using ServerCore.Utility;

public enum PacketType
{{
    {0}
}}

public interface IPacket
{{
    ushort Protocol {{ get; }}
    void Read(ArraySegment<byte> segment);
    ArraySegment<byte> Write();
}}

{1}
";

    // {0} 패킷 이름
    // {1} 패킷 번호
    public static string packetEnumFormat =
        @"{0} = {1},";
    
    
    // {0} 패킷 이름
    // {1} 멤버 변수들
    // {2} 멤버 변수 Read
    // {3} 멤버 변수 Write
    public static string packetFormat = 
        @"public class {0} : ByteControlHelper, IPacket
{{
    {1}
    
    public ushort Protocol {{ get {{ return (ushort)PacketType.{0}; }} }}
    
    public void Read(ArraySegment<byte> segment)
    {{
        ushort count = 0;
        ReadOnlySpan<byte> buffer = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);    // PacketType만큼 건너뛰기
        {2}
    }}

    public ArraySegment<byte> Write()
    {{
        ArraySegment<byte> segment = SendBufferHelper.Open(8192);

        ushort count = 0;
        bool success = true;

        Span<byte> buffer = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        
        count += sizeof(ushort);
        success &= WriteBytes(ref buffer, ref count, (ushort)PacketType.{0});
        {3}
        
        success &= BitConverter.TryWriteBytes(buffer, count);
        
        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }}
}}
";

    // {0} 변수 형식
    // {1} 변수 이름
    public static string memberFormat = 
        @"public {0} {1};";

    // {0} 변수 이름
    public static string readFormat =
        @"this.{0} = ReadBytes(buffer, ref count, this.{0});";

    // {0} 변수 이름
    public static string writeFormat =
        @"success &= WriteBytes(ref buffer, ref count, {0});";
}