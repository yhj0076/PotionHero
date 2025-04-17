using Server.Utility;
using ServerCore;

public enum PacketType
{
    S_Ping = 1,
	C_Pong = 2,
	
}

public interface IPacket
{
    ushort Protocol { get; }
    void Read(ArraySegment<byte> segment);
    ArraySegment<byte> Write();
}

public class S_Ping : ByteControlHelper, IPacket
{
    public int ping;
    
    public ushort Protocol { get { return (ushort)PacketType.S_Ping; } }
    
    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        ReadOnlySpan<byte> buffer = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);    // PacketType만큼 건너뛰기
        this.ping = ReadBytes(buffer, ref count, this.ping);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(8192);

        ushort count = 0;
        bool success = true;

        Span<byte> buffer = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        
        count += sizeof(ushort);
        success &= WriteBytes(ref buffer, ref count, (ushort)PacketType.S_Ping);
        success &= WriteBytes(ref buffer, ref count, ping);
        
        success &= BitConverter.TryWriteBytes(buffer, count);
        
        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }
}
public class C_Pong : ByteControlHelper, IPacket
{
    public int pong;
    
    public ushort Protocol { get { return (ushort)PacketType.C_Pong; } }
    
    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        ReadOnlySpan<byte> buffer = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);    // PacketType만큼 건너뛰기
        this.pong = ReadBytes(buffer, ref count, this.pong);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(8192);

        ushort count = 0;
        bool success = true;

        Span<byte> buffer = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        
        count += sizeof(ushort);
        success &= WriteBytes(ref buffer, ref count, (ushort)PacketType.C_Pong);
        success &= WriteBytes(ref buffer, ref count, pong);
        
        success &= BitConverter.TryWriteBytes(buffer, count);
        
        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }
}

