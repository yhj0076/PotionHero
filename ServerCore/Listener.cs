﻿using System.Net;
using System.Net.Sockets;

namespace ServerCore;

public class Listener
{
    Socket _listenSocket;
    private Func<Session> _sessionFactory;

    public void init(IPEndPoint endpoint, Func<Session> sessionFactory, int register = 10, int backlog = 100)
    {
        _listenSocket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _sessionFactory += sessionFactory;
        
        _listenSocket.Bind(endpoint);
        
        _listenSocket.Listen(backlog);

        for (int i = 0; i < register; i++)
        {
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            RegisterAccept(args);
        }
    }

    private void RegisterAccept(SocketAsyncEventArgs args)
    {
        args.AcceptSocket = null;
        
        bool _pending = _listenSocket.AcceptAsync(args);
        if(_pending == false)
            OnAcceptCompleted(null, args);
    }

    private void OnAcceptCompleted(object? sender, SocketAsyncEventArgs args)
    {
        if (args.SocketError == SocketError.Success)
        {
            Session session = _sessionFactory.Invoke();
            session.Start(args.AcceptSocket);
            session.OnConnected(args.AcceptSocket.RemoteEndPoint);
        }
        else
        {
            Console.WriteLine(args.SocketError.ToString());
        }
        
        RegisterAccept(args);
    }
}