using Server.Packet;
using Server.Session;
using ServerCore;

namespace Server;

public class GameRoom : IJobQueue
{
    List<ClientSession> _clientSessions = new List<ClientSession>();
    JobQueue _jobQueue = new JobQueue();
    List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
    
    public void Push(Action job)
    {
        _jobQueue.Push(job);
    }

    public void Flush()
    {
        foreach (var session in _clientSessions)
        {
            session.Send(_pendingList);
        }
        
        _pendingList.Clear();
    }

    public void Broadcast(ArraySegment<byte> segment)
    {
        _pendingList.Add(segment);
    }

    public void Enter(ClientSession session)
    {
        // 플레이어 추가
        _clientSessions.Add(session);
        session.Room = this;
    }

    public void Leave(ClientSession session)
    {
        // 플레이어 제거
        _clientSessions.Remove(session);
    }

    public void CalcDmg(ClientSession session, C_GainedDmg gainedDmg)
    {
        
    }
}