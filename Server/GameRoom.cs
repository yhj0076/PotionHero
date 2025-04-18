using Server.Session;
using ServerCore;

namespace Server;

public class GameRoom : IJobQueue
{
    ClientSession? _hostSession = null;
    ClientSession? _guestSession = null;
    JobQueue _jobQueue = new JobQueue();
    List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
    
    public void Push(Action job)
    {
        _jobQueue.Push(job);
    }

    public void Flush()
    {
        if (_hostSession != null)
            _hostSession.Send(_pendingList);
        if (_guestSession != null)
            _guestSession.Send(_pendingList);

        _pendingList.Clear();
    }

    public void Broadcast(ArraySegment<byte> segment)
    {
        _pendingList.Add(segment);
    }

    public void Enter(ClientSession session)
    {
        session.hp = 50;
        bool enemyIsExist = false;
        // 플레이어 추가
        if (_hostSession == null)
        {
            _hostSession = session;
            if (_guestSession != null)
                enemyIsExist = true;
        }
        else if (_guestSession == null)
        {
            _guestSession = session;
            if (_hostSession != null)
                enemyIsExist = true;
        }
        session.Room = this;
        
        S_JoinGameRoom joinGameRoom = new S_JoinGameRoom();
        joinGameRoom.EnemyIsExist = enemyIsExist;
        Broadcast(joinGameRoom.Write());
    }

    public void Leave(ClientSession session)
    {
        // 플레이어 제거
        if (_hostSession == session)
        {
            _hostSession = null;
        }
        else if (_guestSession == session)
        {
            _guestSession = null;
        }
        
        S_BroadcastLeaveGame leaveGame = new S_BroadcastLeaveGame();
        Broadcast(leaveGame.Write());
    }

    public void GainDmg(ClientSession clientSession, C_GainedDmg cGainedDmg)
    {
        clientSession.gainedDmg += cGainedDmg.gainedDmg;
        
        S_BroadCastGainedDmg gainedDmg = new S_BroadCastGainedDmg();
        gainedDmg.HostGainedDmg = _hostSession.gainedDmg;
        gainedDmg.GuestGainedDmg = _guestSession.gainedDmg;
        Broadcast(gainedDmg.Write());
    }

    
}