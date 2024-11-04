using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets; 

namespace ServerCore
{
    class Listener
    {
        private Socket _listenSocket;

        public void Init(IPEndPoint endPoint)
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // 주소 체계 + 통신 방법
            
            // 문지기 교육
            _listenSocket.Bind(endPoint); // IP 주소와 포트 번호 바인드

            // 영업 시작
            // backlog: 최대 대기 수
            _listenSocket.Listen(10);

        }

        public Socket Accept()
        {
            return _listenSocket.Accept();
        }
    }
}