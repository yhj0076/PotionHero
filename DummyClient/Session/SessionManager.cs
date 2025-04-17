﻿namespace DummyClient.Session;

public class SessionManager
{
    #region Singleton
    static SessionManager _session = new SessionManager();
    public static SessionManager Instance { get { return _session; } }
    #endregion
    
    List<ServerSession> _sessions = new List<ServerSession>();
    object _lock = new object();

    public void SendForEach()
    {
        lock (_lock)
        {
            foreach (ServerSession session in _sessions)
            {
                // C_GainedDmg cGainedDmg = new C_GainedDmg();
                // cGainedDmg.gainedDmg = 10;
                // session.Send(cGainedDmg.Write());
            }
        }
    }

    public ServerSession Generate()
    {
        lock (_lock)
        {
            ServerSession session = new ServerSession();
            _sessions.Add(session);
            return session;
        }
    }
}