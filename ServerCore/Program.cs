using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Program
    {
        static Listener _listener = new Listener();
        static void OnAcceptHandler(Socket clientSocket)
        {
            try{
                    byte[] recvBuff = new byte[1024];
                    int recvBytes = clientSocket.Receive(recvBuff);
                    string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                    System.Console.WriteLine($"[From Client] {recvData}");

                    // 보낸다
                    byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to MMORPG Server");
                    clientSocket.Send(sendBuff);

                    // 연결 종료
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
            }catch (Exception e){
                System.Console.WriteLine(e);
            }
        }
        static void Main(string[] args)
        {
            // DNS
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host); // DNS의 호스트 엔트리는 여러 개일 수 있음
            IPAddress ipAddr = ipHost.AddressList[0]; // IP 주소는 식당 전체를 나타냄
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777); // 포트는 앞문인지 정문인지 결정


            _listener.Init(endPoint , OnAcceptHandler);

                while (true)
                {
                   ;
                }
            }
        }
    }

