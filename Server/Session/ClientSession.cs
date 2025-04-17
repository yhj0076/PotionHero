using System.Net;
using Server.Packet;
using ServerCore;

namespace Server.Session;

public class ClientSession : PacketSession
{
    public int SessionId { get; set; }
    public GameRoom Room { get; set; }
    public int pong { get; set; }
    
    public override void OnConnected(EndPoint endPoint)
    {
        Console.WriteLine($"Session : Connected to {endPoint}");
        
        Program._room.Push(() =>
        {
            Program._room.Enter(this);
        });
    }

    public override void OnSend(int numOfBytes)
    {
        // throw new NotImplementedException();
        //Room.StartHeartbeat(this);
    }

    public override void OnDisconnected(EndPoint endPoint)
    {
        Console.WriteLine($"Disconnecting from {endPoint}");
        SessionManager.Instance.Remove(this);
        if (Room != null)
        {
            GameRoom gameRoom = Room;
            gameRoom.Push(() => gameRoom.Leave(this));
            Room = null;
        }

        Console.WriteLine($"Disconnected from {endPoint}");
    }

    public override void OnRecvPacket(ArraySegment<byte> buffer)
    {
        Console.WriteLine("OnRecvPacket");
        PacketManager.Instance.OnRecvPacket(this, buffer);
    }
}