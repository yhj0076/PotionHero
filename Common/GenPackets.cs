using System;
using ServerCore;
using ServerCore.Utility;

public enum PacketType
{
    S_JoinGameRoom = 1,
	S_BroadcastGameStart = 2,
	S_BroadcastEndGame = 3,
	C_LeaveGame = 4,
	S_BroadcastLeaveGame = 5,
	C_GainedDmg = 6,
	S_AttackResult = 7,
	S_BroadCastGainedDmg = 8,
	C_TimeUp = 9,
	C_JoinedGame = 10,
	
}

public interface IPacket
{
    ushort Protocol { get; }
    void Read(ArraySegment<byte> segment);
    ArraySegment<byte> Write();
}

public class S_JoinGameRoom : ByteControlHelper, IPacket
{
    public bool EnemyIsExist;
    
    public ushort Protocol { get { return (ushort)PacketType.S_JoinGameRoom; } }
    
    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        ReadOnlySpan<byte> buffer = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);    // PacketType만큼 건너뛰기
        this.EnemyIsExist = ReadBytes(buffer, ref count, this.EnemyIsExist);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(8192);

        ushort count = 0;
        bool success = true;

        Span<byte> buffer = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        
        count += sizeof(ushort);
        success &= WriteBytes(ref buffer, ref count, (ushort)PacketType.S_JoinGameRoom);
        success &= WriteBytes(ref buffer, ref count, EnemyIsExist);
        
        success &= BitConverter.TryWriteBytes(buffer, count);
        
        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }
}
public class S_BroadcastGameStart : ByteControlHelper, IPacket
{
    
    
    public ushort Protocol { get { return (ushort)PacketType.S_BroadcastGameStart; } }
    
    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        ReadOnlySpan<byte> buffer = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);    // PacketType만큼 건너뛰기
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(8192);

        ushort count = 0;
        bool success = true;

        Span<byte> buffer = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        
        count += sizeof(ushort);
        success &= WriteBytes(ref buffer, ref count, (ushort)PacketType.S_BroadcastGameStart);
        
        
        success &= BitConverter.TryWriteBytes(buffer, count);
        
        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }
}
public class S_BroadcastEndGame : ByteControlHelper, IPacket
{
    public int WinnerId;
    
    public ushort Protocol { get { return (ushort)PacketType.S_BroadcastEndGame; } }
    
    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        ReadOnlySpan<byte> buffer = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);    // PacketType만큼 건너뛰기
        this.WinnerId = ReadBytes(buffer, ref count, this.WinnerId);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(8192);

        ushort count = 0;
        bool success = true;

        Span<byte> buffer = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        
        count += sizeof(ushort);
        success &= WriteBytes(ref buffer, ref count, (ushort)PacketType.S_BroadcastEndGame);
        success &= WriteBytes(ref buffer, ref count, WinnerId);
        
        success &= BitConverter.TryWriteBytes(buffer, count);
        
        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }
}
public class C_LeaveGame : ByteControlHelper, IPacket
{
    
    
    public ushort Protocol { get { return (ushort)PacketType.C_LeaveGame; } }
    
    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        ReadOnlySpan<byte> buffer = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);    // PacketType만큼 건너뛰기
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(8192);

        ushort count = 0;
        bool success = true;

        Span<byte> buffer = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        
        count += sizeof(ushort);
        success &= WriteBytes(ref buffer, ref count, (ushort)PacketType.C_LeaveGame);
        
        
        success &= BitConverter.TryWriteBytes(buffer, count);
        
        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }
}
public class S_BroadcastLeaveGame : ByteControlHelper, IPacket
{
    
    
    public ushort Protocol { get { return (ushort)PacketType.S_BroadcastLeaveGame; } }
    
    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        ReadOnlySpan<byte> buffer = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);    // PacketType만큼 건너뛰기
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(8192);

        ushort count = 0;
        bool success = true;

        Span<byte> buffer = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        
        count += sizeof(ushort);
        success &= WriteBytes(ref buffer, ref count, (ushort)PacketType.S_BroadcastLeaveGame);
        
        
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
public class S_AttackResult : ByteControlHelper, IPacket
{
    public int dmg;
    
    public ushort Protocol { get { return (ushort)PacketType.S_AttackResult; } }
    
    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        ReadOnlySpan<byte> buffer = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);    // PacketType만큼 건너뛰기
        this.dmg = ReadBytes(buffer, ref count, this.dmg);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(8192);

        ushort count = 0;
        bool success = true;

        Span<byte> buffer = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        
        count += sizeof(ushort);
        success &= WriteBytes(ref buffer, ref count, (ushort)PacketType.S_AttackResult);
        success &= WriteBytes(ref buffer, ref count, dmg);
        
        success &= BitConverter.TryWriteBytes(buffer, count);
        
        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }
}
public class S_BroadCastGainedDmg : ByteControlHelper, IPacket
{
    public int HostGainedDmg;
	public int GuestGainedDmg;
    
    public ushort Protocol { get { return (ushort)PacketType.S_BroadCastGainedDmg; } }
    
    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        ReadOnlySpan<byte> buffer = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);    // PacketType만큼 건너뛰기
        this.HostGainedDmg = ReadBytes(buffer, ref count, this.HostGainedDmg);
		this.GuestGainedDmg = ReadBytes(buffer, ref count, this.GuestGainedDmg);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(8192);

        ushort count = 0;
        bool success = true;

        Span<byte> buffer = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        
        count += sizeof(ushort);
        success &= WriteBytes(ref buffer, ref count, (ushort)PacketType.S_BroadCastGainedDmg);
        success &= WriteBytes(ref buffer, ref count, HostGainedDmg);
		success &= WriteBytes(ref buffer, ref count, GuestGainedDmg);
        
        success &= BitConverter.TryWriteBytes(buffer, count);
        
        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }
}
public class C_TimeUp : ByteControlHelper, IPacket
{
    
    
    public ushort Protocol { get { return (ushort)PacketType.C_TimeUp; } }
    
    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        ReadOnlySpan<byte> buffer = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);    // PacketType만큼 건너뛰기
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(8192);

        ushort count = 0;
        bool success = true;

        Span<byte> buffer = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        
        count += sizeof(ushort);
        success &= WriteBytes(ref buffer, ref count, (ushort)PacketType.C_TimeUp);
        
        
        success &= BitConverter.TryWriteBytes(buffer, count);
        
        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }
}
public class C_JoinedGame : ByteControlHelper, IPacket
{
    
    
    public ushort Protocol { get { return (ushort)PacketType.C_JoinedGame; } }
    
    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        ReadOnlySpan<byte> buffer = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);    // PacketType만큼 건너뛰기
        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(8192);

        ushort count = 0;
        bool success = true;

        Span<byte> buffer = new Span<byte>(segment.Array, segment.Offset, segment.Count);
        
        count += sizeof(ushort);
        success &= WriteBytes(ref buffer, ref count, (ushort)PacketType.C_JoinedGame);
        
        
        success &= BitConverter.TryWriteBytes(buffer, count);
        
        if (success == false)
            return null;

        return SendBufferHelper.Close(count);
    }
}

