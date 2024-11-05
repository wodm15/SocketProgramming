using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServiceCore
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
             //DNS
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host); // dns의 호스트엔트리는 여러 개일 수 있음
            IPAddress ipAddr = ipHost.AddressList[0]; //ipAddr 은 식당 전체 
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777); //포트는 앞문인지 정문인지

            //휴대폰 설정
            Socket socket= new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try{
                //문지기한테 입장 문의
            socket.Connect(endPoint);
            System.Console.WriteLine($"Connect To {socket.RemoteEndPoint.ToString()}");

            //보낸다
            byte[] sendBuff = Encoding.UTF8.GetBytes("Hello, World!");
            int sendBytes = socket.Send(sendBuff);

            //받는다
            byte[] recvBuff = new byte[1024];
            int recvBytes = socket.Receive(recvBuff);
            string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
            System.Console.WriteLine($"[From Server] {recvData}");

            //나간다
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
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