using System.Net;
using Server.Session;
using ServerCore;

namespace Server;

class Program
{
    private static Listener _listener = new Listener();
    public static GameRoom _room = new GameRoom();
    private static int portNum = 8080;
    static void FlushRoom()
    {
        _room.Push(() => _room.Flush());
        JobTimer.Instance.Push(FlushRoom, 250);
    }
    
    static void Main(string[] args)
    {
        // DNS(Domain Name System)
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAddress = ipHost.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ipAddress, portNum);
        
        _listener.init(endPoint, () =>
        {
            return SessionManager.Instance.Generate();
        });
        Console.WriteLine($"Listening on port {portNum}");

        JobTimer.Instance.Push(FlushRoom);
        while (true)
        {
            JobTimer.Instance.Flush();
        }
    }
}