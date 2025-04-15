namespace PacketGenerator;

public class PacketFormat
{
    // {0} 패킷 이름/ 번호 목록
    // {1} 패킷 목록
    public static string fileFormat =
        @"using PotionHeroServer.Tools;
using ServerCore;

namespace PotionHeroServer.Packet;

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