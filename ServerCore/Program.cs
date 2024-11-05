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
                    Session session = new Session();
                    session.Start(clientSocket);
                    byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to MMORPG Server");
                    session.Send(sendBuff);

                    Thread.Sleep(1000);
                    session.Disconnect();

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

