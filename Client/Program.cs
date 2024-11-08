using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;

namespace ServiceCore
{
    class GameSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            
            //보낸다
            for(int i=0; i<5 ; i++)
            {
                byte[] sendBuff = Encoding.UTF8.GetBytes($"Hello, World! {i}");
                Send(sendBuff);
            }

        }

        public override void OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[from Server] {recvData}");
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
        static void Main(string[] args)
        {

             //DNS
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host); // dns의 호스트엔트리는 여러 개일 수 있음
            IPAddress ipAddr = ipHost.AddressList[0]; //ipAddr 은 식당 전체 
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777); //포트는 앞문인지 정문인지

            Connector connector = new Connector();
            connector.Connect(endPoint , () => {return new GameSession(); });

            while(true)
            {
          
            try
            {
                //문지기한테 입장 문의
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }
            
            Thread.Sleep(100);
            }
        }
            
    }
}