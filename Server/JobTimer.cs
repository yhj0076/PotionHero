namespace Server;

struct JobTimerElem : IComparable<JobTimerElem>
{
    public int execTick;    // 실행 시간
    public Action action;
    
    public int CompareTo(JobTimerElem other)
    {
        return other.execTick - execTick;
    }
}

public class JobTimer
{
    PriorityQueue<JobTimerElem, int> _priorityQueue = new PriorityQueue<JobTimerElem, int>();
    object _lock = new object();
    
    public static JobTimer Instance { get; } = new JobTimer();

    public void Push(Action action, int tickAfter = 0)
    {
        JobTimerElem job;
        job.execTick = System.Environment.TickCount + tickAfter;
        job.action = action;

        lock (_lock)
        {
            _priorityQueue.Enqueue(job, job.execTick);
        }
    }

    public void Flush()
    {
        while (true)
        {
            int now = System.Environment.TickCount;
            
            JobTimerElem job;

            lock (_lock)
            {
                if (_priorityQueue.Count == 0)
                    break;

                job = _priorityQueue.Peek();
                if (job.execTick > now)
                    break;
                
                _priorityQueue.Dequeue();
            }
            
            job.action.Invoke();
        }
    }
}