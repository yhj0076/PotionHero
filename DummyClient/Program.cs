using System.Net;
using DummyClient.Session;
using ServerCore;

namespace DummyClient;

class Program
{
    static void Main(string[] args)
    {
        // DNS(Domain Name System)
        string host = Dns.GetHostName();
        IPHostEntry ipEntry = Dns.GetHostEntry(host);
        IPAddress ipAddress = ipEntry.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ipAddress, 8080);
        
        Connector connector = new Connector();
        connector.Connect(endPoint, () =>
        {
            return SessionManager.Instance.Generate();
        },1);

        float timeCount = 0;
        while (true)
        {
            try
            {
                SessionManager.Instance.SendForEach();
                if (timeCount > 15)
                {
                    SessionManager.Instance.SendTimeStop();
                    timeCount = 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            timeCount += 1f;
            Thread.Sleep(1000);
        }
    }
}