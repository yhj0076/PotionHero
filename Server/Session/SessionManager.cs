﻿namespace Server.Session;

public class SessionManager
{
    static SessionManager _session = new SessionManager();
    public static SessionManager Instance { get { return _session; } }

    private int _sessionId = 0;
    Dictionary<int, ClientSession> _sessions = new Dictionary<int, ClientSession>();
    object _lock = new object();

    public ClientSession Generate()
    {
        lock (_lock)
        {
            int sessionId = ++_sessionId;
            
            ClientSession session = new ClientSession();
            session.SessionId = sessionId;
            _sessions.Add(sessionId, session);

            Console.WriteLine($"Connected to session {session.SessionId}");
            
            return session;
        }
    }

    public ClientSession Find(int id)
    {
        lock (_lock)
        {
            ClientSession session = null;
            _sessions.TryGetValue(id, out session);
            return session;
        }
    }

    public void Remove(ClientSession session)
    {
        lock (_lock)
        {
            _sessions.Remove(session.SessionId);
        }
    }
}