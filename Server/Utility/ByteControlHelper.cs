using Server.Exception;

namespace Server.Utility;

public class ByteControlHelper
{
    // 아래는 현재 적용 가능 타입들
    /*
     * bool
     * char
     * double
     * float
     * Half
     * int
     * Int128
     * long
     * short
     * uint
     * UInt128
     * ulong
     * ushort
     */
    #region TryWriteBytes 가능 타입별 정의
    // BitConverter.TryWriteBytes에서 가능한 타입을 전부 추가하였다. 버전에 따라 다르겠지만, 최대한 원형을 유지할 것(2025.04.14)
    protected bool WriteBytes(ref Span<byte> buffer, ref ushort count, bool value)
    {
        bool success = BitConverter.TryWriteBytes(buffer.Slice(count, buffer.Length - count), value);
        if (success == false)
            throw new WriteBytesException();
        count += sizeof(bool);
        return success;
    }
    protected bool WriteBytes(ref Span<byte> buffer, ref ushort count, char value)
    {
        bool success = BitConverter.TryWriteBytes(buffer.Slice(count, buffer.Length - count), value);
        if (success == false)
            throw new WriteBytesException();
        count += sizeof(bool);
        return success;
    }
    protected bool WriteBytes(ref Span<byte> buffer, ref ushort count, double value)
    {
        bool success = BitConverter.TryWriteBytes(buffer.Slice(count, buffer.Length - count), value);
        if (success == false)
            throw new WriteBytesException();
        count += sizeof(bool);
        return success;
    }
    protected bool WriteBytes(ref Span<byte> buffer, ref ushort count, float value)
    {
        bool success = BitConverter.TryWriteBytes(buffer.Slice(count, buffer.Length - count), value);
        if (success == false)
            throw new WriteBytesException();
        count += sizeof(bool);
        return success;
    }
    protected bool WriteBytes(ref Span<byte> buffer, ref ushort count, Half value)
    {
        bool success = BitConverter.TryWriteBytes(buffer.Slice(count, buffer.Length - count), value);
        if (success == false)
            throw new WriteBytesException();
        count += sizeof(bool);
        return success;
    }
    protected bool WriteBytes(ref Span<byte> buffer, ref ushort count, int value)
    {
        bool success = BitConverter.TryWriteBytes(buffer.Slice(count, buffer.Length - count), value);
        if (success == false)
            throw new WriteBytesException();
        count += sizeof(bool);
        return success;
    }
    protected bool WriteBytes(ref Span<byte> buffer, ref ushort count, Int128 value)
    {
        bool success = BitConverter.TryWriteBytes(buffer.Slice(count, buffer.Length - count), value);
        if (success == false)
            throw new WriteBytesException();
        count += sizeof(bool);
        return success;
    }
    protected bool WriteBytes(ref Span<byte> buffer, ref ushort count, long value)
    {
        bool success = BitConverter.TryWriteBytes(buffer.Slice(count, buffer.Length - count), value);
        if (success == false)
            throw new WriteBytesException();
        count += sizeof(bool);
        return success;
    }
    protected bool WriteBytes(ref Span<byte> buffer, ref ushort count, short value)
    {
        bool success = BitConverter.TryWriteBytes(buffer.Slice(count, buffer.Length - count), value);
        if (success == false)
            throw new WriteBytesException();
        count += sizeof(bool);
        return success;
    }
    protected bool WriteBytes(ref Span<byte> buffer, ref ushort count, uint value)
    {
        bool success = BitConverter.TryWriteBytes(buffer.Slice(count, buffer.Length - count), value);
        if (success == false)
            throw new WriteBytesException();
        count += sizeof(bool);
        return success;
    }
    protected bool WriteBytes(ref Span<byte> buffer, ref ushort count, UInt128 value)
    {
        bool success = BitConverter.TryWriteBytes(buffer.Slice(count, buffer.Length - count), value);
        if (success == false)
            throw new WriteBytesException();
        count += sizeof(bool);
        return success;
    }
    protected bool WriteBytes(ref Span<byte> buffer, ref ushort count, ulong value)
    {
        bool success = BitConverter.TryWriteBytes(buffer.Slice(count, buffer.Length - count), value);
        if (success == false)
            throw new WriteBytesException();
        count += sizeof(bool);
        return success;
    }
    protected bool WriteBytes(ref Span<byte> buffer, ref ushort count, ushort value)
    {
        bool success = BitConverter.TryWriteBytes(buffer.Slice(count, buffer.Length - count), value);
        if (success == false)
            throw new WriteBytesException();
        count += sizeof(bool);
        return success;
    }
    #endregion

    #region Byte 읽기 기능 타입별 정의
    // BitConverter.To[Type]에서 가능한 타입을 전부 추가하였다. 버전에 따라 다르겠지만, 최대한 원형을 유지할 것(2025.04.14)
    protected bool ReadBytes(ReadOnlySpan<byte> buffer, ref ushort count, bool value)
    {
        bool readByte = BitConverter.ToBoolean(buffer.Slice(count, buffer.Length - count));
        count += sizeof(bool);
        return readByte;
    }
    
    protected char ReadBytes(ReadOnlySpan<byte> buffer, ref ushort count, char value)
    {
        char readByte = BitConverter.ToChar(buffer.Slice(count, buffer.Length - count));
        count += sizeof(bool);
        return readByte;
    }
    
    protected double ReadBytes(ReadOnlySpan<byte> buffer, ref ushort count, double value)
    {
        double readByte = BitConverter.ToDouble(buffer.Slice(count, buffer.Length - count));
        count += sizeof(bool);
        return readByte;
    }
    
    protected float ReadBytes(ReadOnlySpan<byte> buffer, ref ushort count, float value)
    {
        float readByte = BitConverter.ToSingle(buffer.Slice(count, buffer.Length - count));
        count += sizeof(bool);
        return readByte;
    }
    
    protected Half ReadBytes(ReadOnlySpan<byte> buffer, ref ushort count, Half value)
    {
        Half readByte = BitConverter.ToHalf(buffer.Slice(count, buffer.Length - count));
        count += sizeof(bool);
        return readByte;
    }
    
    protected int ReadBytes(ReadOnlySpan<byte> buffer, ref ushort count, int value)
    {
        int readByte = BitConverter.ToInt32(buffer.Slice(count, buffer.Length - count));
        count += sizeof(bool);
        return readByte;
    }
    
    protected Int128 ReadBytes(ReadOnlySpan<byte> buffer, ref ushort count, Int128 value)
    {
        Int128 readByte = BitConverter.ToInt128(buffer.Slice(count, buffer.Length - count));
        count += sizeof(bool);
        return readByte;
    }
    
    protected long ReadBytes(ReadOnlySpan<byte> buffer, ref ushort count, long value)
    {
        long readByte = BitConverter.ToInt64(buffer.Slice(count, buffer.Length - count));
        count += sizeof(bool);
        return readByte;
    }
    
    protected short ReadBytes(ReadOnlySpan<byte> buffer, ref ushort count, short value)
    {
        short readByte = BitConverter.ToInt16(buffer.Slice(count, buffer.Length - count));
        count += sizeof(bool);
        return readByte;
    }

    protected uint ReadBytes(ReadOnlySpan<byte> buffer, ref ushort count, uint value)
    {
        uint readByte = BitConverter.ToUInt32(buffer.Slice(count, buffer.Length - count));
        count += sizeof(bool);
        return readByte;
    }
    
    protected UInt128 ReadBytes(ReadOnlySpan<byte> buffer, ref ushort count, UInt128 value)
    {
        UInt128 readByte = BitConverter.ToUInt128(buffer.Slice(count, buffer.Length - count));
        count += sizeof(bool);
        return readByte;
    }
    
    protected ulong ReadBytes(ReadOnlySpan<byte> buffer, ref ushort count, ulong value)
    {
        ulong readByte = BitConverter.ToUInt64(buffer.Slice(count, buffer.Length - count));
        count += sizeof(bool);
        return readByte;
    }
    
    protected ushort ReadBytes(ReadOnlySpan<byte> buffer, ref ushort count, ushort value)
    {
        ushort readByte = BitConverter.ToUInt16(buffer.Slice(count, buffer.Length - count));
        count += sizeof(bool);
        return readByte;
    }
    
    #endregion
}