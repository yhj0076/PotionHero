using Server.Exception;
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

public class S_Ping : IPacket
{
    public int ping;
    
    public ushort Protocol { get { return (ushort)PacketType.S_Ping; } }
    
    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        ReadOnlySpan<byte> buffer = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);    // PacketType만큼 건너뛰기
        this.ping = BitConverter.ToInt32(buffer.Slice(count, buffer.Length - count));
        count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(8192);

        ushort count = 0;
        bool success = true;

        Span<byte> buffer = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(buffer.Slice(count, buffer.Length - count), (ushort)PacketType.S_Ping);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(buffer.Slice(count, buffer.Length - count), ping);
        count += sizeof(int);

        success &= BitConverter.TryWriteBytes(buffer, count);
        
        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }
}
public class C_Pong : IPacket
{
    public int pong;
    
    public ushort Protocol { get { return (ushort)PacketType.C_Pong; } }
    
    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        ReadOnlySpan<byte> buffer = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);    // PacketType만큼 건너뛰기
        this.pong = BitConverter.ToInt32(buffer.Slice(count, buffer.Length - count));
        count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(8192);

        ushort count = 0;
        bool success = true;

        Span<byte> buffer = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(buffer.Slice(count, buffer.Length - count), (ushort)PacketType.C_Pong);
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(buffer.Slice(count, buffer.Length - count), pong);
        count += sizeof(int);

        success &= BitConverter.TryWriteBytes(buffer, count);
        
        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }
}