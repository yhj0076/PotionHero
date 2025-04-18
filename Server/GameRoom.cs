using Server.Session;
using ServerCore;

namespace Server;

public class GameRoom : IJobQueue
{
    ClientSession? _hostSession = null;
    ClientSession? _guestSession = null;
    JobQueue _jobQueue = new JobQueue();
    List<ArraySegment<byte>> _pendingListH = new List<ArraySegment<byte>>();
    List<ArraySegment<byte>> _pendingListG = new List<ArraySegment<byte>>();

    private bool GameStart = false;
    private int hostGainedDmg = 0;
    private bool hostStopGain = false;
    private int guestGainedDmg = 0;
    private bool guestStopGain = false;
    
    public void Push(Action job)
    {
        _jobQueue.Push(job);
    }

    public void Clear()
    {
        _jobQueue.Clear();
    }
    
    public void Flush()
    {
        if (_hostSession != null)
            _hostSession.Send(_pendingListH);
        if (_guestSession != null)
            _guestSession.Send(_pendingListG);

        _pendingListH.Clear();
        _pendingListG.Clear();
    }

    public void Broadcast(ArraySegment<byte> segment)
    {
        _pendingListH.Add(segment);
        _pendingListG.Add(segment);
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

    public void Start()
    {
        if (_hostSession != null && _guestSession != null && !GameStart)
        {
            GameStart = true;
            S_BroadcastGameStart broadcastGameStart = new S_BroadcastGameStart();
            Broadcast(broadcastGameStart.Write());
        }
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
        if (_hostSession != null && _guestSession != null)
        {
            clientSession.gainedDmg += cGainedDmg.gainedDmg;

            S_BroadCastGainedDmg gainedDmgH = new S_BroadCastGainedDmg();
            gainedDmgH.HostGainedDmg = _hostSession.gainedDmg;
            gainedDmgH.GuestGainedDmg = _guestSession.gainedDmg;
            S_BroadCastGainedDmg gainedDmgG = new S_BroadCastGainedDmg();
            gainedDmgG.HostGainedDmg = _guestSession.gainedDmg;
            gainedDmgG.GuestGainedDmg = _hostSession.gainedDmg;
            // Broadcast(gainedDmg.Write());
            _pendingListH.Add(gainedDmgH.Write());
            _pendingListG.Add(gainedDmgG.Write());
        }
    }

    
    //둘 다한테서 정지신호를 받았을 때 공걱 계산하기
    public void Stop(ClientSession session)
    {
        if (_hostSession == session)
        {
            hostStopGain = true;
            hostGainedDmg = session.gainedDmg;
            session.gainedDmg = 0;
        }
        else if(_guestSession == session)
        {
            guestStopGain = true;
            guestGainedDmg = session.gainedDmg;
            session.gainedDmg = 0;
        }
    }
    
    public void Attack()
    {
        if (_hostSession != null && _guestSession != null)
        {
            if (hostStopGain && guestStopGain)
            {
                int hostDmg = hostGainedDmg;
                int guestDmg = guestGainedDmg;

                int dmg = hostDmg - guestDmg;

                if (dmg > 0)
                {
                    _guestSession.hp -= dmg;
                }
                else if (dmg < 0)
                {
                    _hostSession.hp -= -dmg;
                }

                S_AttackResult attackResultH = new S_AttackResult();
                attackResultH.dmg = dmg;
                S_AttackResult attackResultG = new S_AttackResult();
                attackResultG.dmg = -dmg;
                _hostSession.gainedDmg = 0;
                _guestSession.gainedDmg = 0;
                // _hostSession.Send(attackResultH.Write());
                // _guestSession.Send(attackResultG.Write());
                _pendingListH.Add(attackResultH.Write());
                _pendingListG.Add(attackResultG.Write());
                hostStopGain = false;
                guestStopGain = false;
            }

            GameEnd();
        }
    }

    public void GameEnd()
    {
        if (_hostSession.hp <= 0 || _guestSession.hp <= 0)
        {
            S_BroadcastEndGame endGame = new S_BroadcastEndGame();
            if (_hostSession.hp < _guestSession.hp)
            {
                endGame.WinnerId = _guestSession.SessionId;
            }
            else
            {
                endGame.WinnerId = _hostSession.SessionId;
            }
            Broadcast(endGame.Write());
        }
    }
}