using Server.Utility;
using ServerCore;

public enum PacketType
{
    S_BroadcastGainedDmg = 1,
	C_GainedDmg = 2,
	
}

public interface IPacket
{
    ushort Protocol { get; }
    void Read(ArraySegment<byte> segment);
    ArraySegment<byte> Write();
}

public class S_BroadcastGainedDmg : ByteControlHelper, IPacket
{
    public int gainedDmg;
    
    public ushort Protocol { get { return (ushort)PacketType.S_BroadcastGainedDmg; } }
    
    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        ReadOnlySpan<byte> buffer = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);    // PacketType만큼 건너뛰기
        this.gainedDmg = ReadBytes(buffer, ref count, this.gainedDmg);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(8192);

        ushort count = 0;
        bool success = true;

        Span<byte> buffer = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        
        count += sizeof(ushort);
        success &= WriteBytes(ref buffer, ref count, (ushort)PacketType.S_BroadcastGainedDmg);
        success &= WriteBytes(ref buffer, ref count, gainedDmg);
        
        success &= BitConverter.TryWriteBytes(buffer, count);
        
        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }
}
public class C_GainedDmg : ByteControlHelper, IPacket
{
    public int gainedDmg;
    
    public ushort Protocol { get { return (ushort)PacketType.C_GainedDmg; } }
    
    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        ReadOnlySpan<byte> buffer = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);    // PacketType만큼 건너뛰기
        this.gainedDmg = ReadBytes(buffer, ref count, this.gainedDmg);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(8192);

        ushort count = 0;
        bool success = true;

        Span<byte> buffer = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        
        count += sizeof(ushort);
        success &= WriteBytes(ref buffer, ref count, (ushort)PacketType.C_GainedDmg);
        success &= WriteBytes(ref buffer, ref count, gainedDmg);
        
        success &= BitConverter.TryWriteBytes(buffer, count);
        
        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }
}

