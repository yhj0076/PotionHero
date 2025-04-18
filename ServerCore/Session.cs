﻿using System.Net;
using System.Net.Sockets;

namespace ServerCore;

public abstract class PacketSession : Session
{
    public static readonly int HeaderSize = 2;

    public sealed override int OnRecv(ArraySegment<byte> buffer)
    {
        int processLen = 0;
        int packetCount = 0;

        while (true)
        {
            if (buffer.Count < HeaderSize)  // 헤더 파싱 가능한지 확인
                break;
            
            ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            if(buffer.Count < dataSize)     // 패킷이 전부 도착했는지 확인
                break;
            
            OnRecvPacket(new ArraySegment<byte>(buffer.Array, buffer.Offset, dataSize));
            packetCount++;
            processLen += dataSize;
            buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + dataSize, buffer.Count - dataSize);
        }
        
        return processLen;
    }

    public abstract void OnRecvPacket(ArraySegment<byte> buffer);
}

public abstract class Session
{
    Socket _socket;
    int _disconnected = 0;

    private RecvBuffer _recvBuffer = new RecvBuffer(65535);
    
    object _lock = new object();
    Queue<ArraySegment<byte>> _sendQueue = new Queue<ArraySegment<byte>>();
    List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
    SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
    SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();
    
    public abstract void OnConnected(EndPoint endPoint);
    public abstract int OnRecv(ArraySegment<byte> buffer);
    public abstract void OnSend(int numOfBytes);
    public abstract void OnDisconnected(EndPoint endPoint);

    void Clear()
    {
        lock (_lock)
        {
            _sendQueue.Clear();
            _pendingList.Clear();
        }
    }

    public void Start(Socket socket)
    {
        _socket = socket;

        _recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);
        _sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);

        RegisterRecv();
    }

    public void Send(ArraySegment<byte> sendBuff)
    {
        lock (_lock)
        {
            _sendQueue.Enqueue(sendBuff);
            if (_pendingList.Count == 0)
            {
                RegisterSend();
            }
        }
    }
    
    public void Send(List<ArraySegment<byte>> sendBuffList)
    {
        if (sendBuffList.Count == 0)
            return;

        lock (_lock)
        {
            foreach (var sendBuff in sendBuffList)
            {
                _sendQueue.Enqueue(sendBuff);
            }

            if (_pendingList.Count == 0)
            {
                RegisterSend();
            }
        }
    }

    public void DisConnect()
    {
        if (Interlocked.Exchange(ref _disconnected, 1) == 1)
            return;
        
        OnDisconnected(_socket.RemoteEndPoint);
        _socket.Shutdown(SocketShutdown.Both);
        _socket.Close();
        Clear();
    }

    #region 네트워크 통신
    
    private void RegisterSend()
    {
        if (_disconnected == 1)
            return;

        while (_sendQueue.Count > 0)
        {
            ArraySegment<byte> sendBuff = _sendQueue.Dequeue();
            _pendingList.Add(sendBuff);
        }
        
        _sendArgs.BufferList = _pendingList;

        try
        {
            bool pending = _socket.SendAsync(_sendArgs);
            if(!pending)
                OnSendCompleted(null, _sendArgs);
        }
        catch (Exception e)
        {
            Console.WriteLine($"RegisterSend Failed : {e}");
        }
    }
    
    private void OnSendCompleted(object? sender, SocketAsyncEventArgs args)
    {
        // Console.WriteLine($"OnSendCompleted : {args.SocketError}");
        lock (_lock)
        {
            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            {
                try
                {
                    _sendArgs.BufferList = null;
                    _pendingList.Clear();
                    
                    OnSend(_sendArgs.BytesTransferred);
                    
                    if(_sendQueue.Count > 0)
                        RegisterSend();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"OnSendCompleted Failed : {e}");
                }
            }
            else
            {
                // Console.WriteLine($"OnSendCompleted Failed\n" +
                //                   $"args.SocketError = {args.SocketError}\n" +
                //                   $"args.BytesTransferred = {args.BytesTransferred}\n");
                DisConnect();
            }
        }
    }

    private void RegisterRecv()
    {
        // Console.WriteLine($"RegisterRecv : {_socket.RemoteEndPoint}");
        if (_disconnected == 1)
            return;
        
        _recvBuffer.Clean();
        ArraySegment<byte> segment = _recvBuffer.RecvSegment;
        _recvArgs.SetBuffer(segment.Array, segment.Offset, segment.Count);

        try
        {
            bool pending = _socket.ReceiveAsync(_recvArgs);
            if(!pending)
                OnRecvCompleted(null, _recvArgs);
        }
        catch (Exception e)
        {
            Console.WriteLine($"RegisterRecv Failed : {e}");
        }
    }
    
    private void OnRecvCompleted(object? sender, SocketAsyncEventArgs args)
    {
        // Console.WriteLine($"OnRecvCompleted : {args.SocketError}");
        if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
        {
            try
            {
                // Write 커서 이동
                if (_recvBuffer.OnWrite(args.BytesTransferred) == false)
                {
                    // Console.WriteLine($"OnRecvCompleted Failed : Cannot Move WriteCursor");
                    DisConnect();
                    return;
                }

                // 컨텐츠 쪽으로 데이터를 넘겨주고 얼마나 처리했는지 받는다
                int processLen = OnRecv(_recvBuffer.DataSegment);
                if (processLen < 0 || _recvBuffer.DataSize < processLen)
                {
                    // Console.WriteLine($"OnRecvCompleted Failed : {_recvBuffer.DataSize} < processLen");
                    DisConnect();
                    return;
                }
                
                // Read 커서 이동
                if (_recvBuffer.OnRead(processLen) == false)
                {
                    // Console.WriteLine($"OnRecvCompleted Failed : Cannot Move ReadCursor");
                    DisConnect();
                    return;
                }
                
                RegisterRecv();
            }
            catch (Exception e)
            {
                Console.WriteLine($"OnRecvCompleted Failed : {e}");
            }
        }
        else
        {
            // Console.WriteLine($"OnRecvCompleted Failed\n" +
            //                   $"args.SocketError = {args.SocketError}\n" +
            //                   $"args.BytesTransferred = {args.BytesTransferred}\n");
            DisConnect();
        }
    }
    #endregion
}