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
        },2);

        while (true)
        {
            try
            {
                SessionManager.Instance.SendForEach();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
            Thread.Sleep(250);
        }
    }
}