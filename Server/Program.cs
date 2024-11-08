using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;

namespace Server
{
    class GameSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            System.Console.WriteLine($"Onconnected {endPoint}");
            byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to MMORPG Server");
            Send(sendBuff);
            Thread.Sleep(1000);
            Disconnect();

        }

        public override void OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[from client] {recvData}");
        }
        public override void OnSend(int numOfBytes)
        {
            System.Console.WriteLine($"TransferBytes {numOfBytes}");
        }
        public override void OnDisconnected(EndPoint endPoint)
        {
            System.Console.WriteLine($"OnDisconnected {endPoint}");
        }
    }

    class Program
    {
        static Listener _listener = new Listener();
      
        static void Main(string[] args)
        {
            // DNS
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host); // DNS의 호스트 엔트리는 여러 개일 수 있음
            IPAddress ipAddr = ipHost.AddressList[0]; // IP 주소는 식당 전체를 나타냄
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777); // 포트는 앞문인지 정문인지 결정


            _listener.Init(endPoint , () => { return new GameSession();});

                while (true)
                {
                   ;
                }
            }
        }
    }

