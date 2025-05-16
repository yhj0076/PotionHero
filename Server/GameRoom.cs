using Server.Session;
using ServerCore;
using Timer = System.Timers.Timer;

namespace Server;

public class GameRoom : IJobQueue
{
    ClientSession? _hostSession = null;
    ClientSession? _guestSession = null;
    JobQueue _jobQueue = new JobQueue();
    List<ArraySegment<byte>> _pendingListH = new List<ArraySegment<byte>>();
    List<ArraySegment<byte>> _pendingListG = new List<ArraySegment<byte>>();

    private bool GameStart = false;
    private int gameTime = 15;
    private System.Timers.Timer timer;
    object _lock = new object();
    
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
        Console.WriteLine($"host : {_hostSession?.SessionId}, guest : {_guestSession?.SessionId}");
    }

    public void Start()
    {
        if (_hostSession != null && _guestSession != null)
        {
            _hostSession.gainedDmg = 0;
            _guestSession.gainedDmg = 0;
            _hostSession.hp = 100;
            _guestSession.hp = 100;
            gameTime = 15;
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
    
    // 공격 메서드
    public void Attack()
    {
        if (_hostSession != null && _guestSession != null)
        {
            int dmg = _hostSession.gainedDmg - _guestSession.gainedDmg;
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
            _hostSession.Send(attackResultH.Write());
            _guestSession.Send(attackResultG.Write());
            GameEnd();
        }
    }

    // 게임 종료 메서드
    public void GameEnd()
    {
        if (_hostSession.hp <= 0 || _guestSession.hp <= 0)
        {
            timer.Stop();
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

    public void TickTock()
    {
        if (_hostSession != null && _guestSession != null)
        {
            lock (_lock)
            {
                if (timer != null)
                {
                    timer.Stop();
                    timer.Dispose();
                }

                var startDelayTimer = new Timer(3000);
                startDelayTimer.AutoReset = false;
                startDelayTimer.Elapsed += (sender, e) =>
                {
                    timer = new Timer(1000);
                    timer.Elapsed += (sender, e) => TimeTick();
                    timer.AutoReset = true;
                    timer.Start();

                    startDelayTimer.Dispose();
                };
                startDelayTimer.Start();
            }
        }
    }

    // TODO : 1. timer는 Nullable 안되게
    // 2. 하나의 스레드가 하나의 GameRoom을 담당하게(중요★★★★★)
    // 3. Manager로 Class 분리(중요★★★★★)
    
    public void TimeTick()
    {
        if (_hostSession != null && _guestSession != null)
        {
            if (gameTime > 0)
            {
                gameTime--;
                S_Timer ticktock = new S_Timer();
                ticktock.second = gameTime;
                Broadcast(ticktock.Write());
            }
            else
            {
                Attack();
                gameTime = 15;
            }
        }
    }
}