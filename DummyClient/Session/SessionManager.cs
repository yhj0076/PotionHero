using System.Net;

namespace DummyClient.Session;

public class SessionManager
{
    #region Singleton
    static SessionManager _session = new SessionManager();
    public static SessionManager Instance { get { return _session; } }
    #endregion
    
    List<ServerSession> _sessions = new List<ServerSession>();
    ServerSession _singleSession = new ServerSession();
    object _lock = new object();
    Random _random = new Random();
    
    public void SendForEach()
    {
        lock (_lock)
        {
            // C_GainedDmg cGainedDmg = new C_GainedDmg();
            // cGainedDmg.gainedDmg = _random.Next(3,10);
            // _sessions[0].Send(cGainedDmg.Write());
            
            // C_GainedDmg cGainedDmg1 = new C_GainedDmg();
            // cGainedDmg1.gainedDmg = _random.Next(3,10);
            // _sessions[1].Send(cGainedDmg1.Write());
            C_GainedDmg cGainedDmg = new C_GainedDmg();
            cGainedDmg.gainedDmg = _random.Next(3,4);
            _singleSession.Send(cGainedDmg.Write());
        }
    }

    public ServerSession GenerateSingleSession()
    {
        lock (_lock)
        {
            ServerSession singleSession = new ServerSession();
            _singleSession = singleSession;
            Console.WriteLine("Single session generated");
            return singleSession;
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