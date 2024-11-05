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
        Action<Socket> _onAcceptHandler; // 비동기로 손님이 입장하면 그 다음걸 액션취하기 위함.

        public void Init(IPEndPoint endPoint , Action<Socket> onAcceptHandler)
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // 주소 체계 + 통신 방법
            _onAcceptHandler += onAcceptHandler;
            
            // 문지기 교육
            _listenSocket.Bind(endPoint); // IP 주소와 포트 번호 바인드

            // 영업 시작
            // backlog: 최대 대기 수
            _listenSocket.Listen(10);


            SocketAsyncEventArgs args = new SocketAsyncEventArgs(); //나중에 재사용가능
            args.Completed += new EventHandler<SocketAsyncEventArgs>(onAcceptCompleted); //콜백함수로 onAccpetCompleted
            RegisterAccept(args);
        }

        void RegisterAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;  // 재사용하므로 다시 비워야함
            bool pending = _listenSocket.AcceptAsync(args); 
            if(pending == false) //바로 완료가 되었을 때
                onAcceptCompleted(null, args);
        }
        void onAcceptCompleted(object? sender, SocketAsyncEventArgs args)
        {
            if(args.SocketError == SocketError.Success)
            {
                _onAcceptHandler?.Invoke(args.AcceptSocket);
            }
            else
                System.Console.WriteLine(args.SocketError.ToString());

            RegisterAccept(args); //다시 등록하기
        }
        public Socket Accept()
        {
            _listenSocket.AcceptAsync();
            return _listenSocket.Accept();
        }
    }
}