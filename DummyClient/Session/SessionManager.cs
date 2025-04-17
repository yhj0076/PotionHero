namespace DummyClient.Session;

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
                C_Pong packet = new C_Pong();
                packet.pong = 1;
                session.Send(packet.Write());
            }
        }
    }

    public void Send()
    {
        lock (_lock)
        {
            if (_sessions.Count > 0)
            {
                C_Pong pong = new C_Pong();
                pong.pong = 1;
                _sessions[0].Send(pong.Write());
            }
            else
            {
                Console.WriteLine("No sessions left");
            }
        }
    }

    public ServerSession Generate()
    {
        lock (_lock)
        {
            ServerSession session = new ServerSession();
            _sessions.Add(session);
            Console.WriteLine("Session generated");
            return session;
        }
    }
}