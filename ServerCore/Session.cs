using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerCore
{
    public class Session
    {
        Socket _socket = null!; 
        int _disconnected = 0;

        object _lock = new object();
        Queue<byte[]> _sendQueue = new Queue<byte[]>();
        bool _pending = false;

         SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();

        public void Start(Socket socket)
        {
            _socket = socket;
            SocketAsyncEventArgs recvArgs = new SocketAsyncEventArgs();
            recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnReceiveCompleted);
            recvArgs.SetBuffer(new byte[1024], 0, 1024);

            _sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);

            RegisterRecv(recvArgs);
        }

        public void Send(byte[] sendBuff)
        {
            lock (_lock)
            {
                _sendQueue.Enqueue(sendBuff);
                if(_pending == false)
                RegisterSend();
            }
        }

        public void Disconnect()
        {
            if(Interlocked.Exchange(ref _disconnected, 1) == 1)
                return;
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        #region 네트워크 통신

        void RegisterSend()
        {
            _pending = true;
            byte[] buff = _sendQueue.Dequeue();
            _sendArgs.SetBuffer(buff, 0, buff.Length);

            bool pending = _socket.SendAsync(_sendArgs);
            if(pending == false)
                OnSendCompleted(null, _sendArgs);
        }
        void OnSendCompleted(object sender , SocketAsyncEventArgs args)
        {
            lock(_lock)
            {
                if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
                {
                    try
                    {
                        if(_sendQueue.Count > 0)
                        {
                            RegisterSend();
                        }
                        _pending = false;
                    }
                        catch(Exception e){
                        System.Console.WriteLine($"OnSendCompleted Failed {e}");
                    }
                }
                else
                {
                    Disconnect();
                }
            }
            
        }

        void RegisterRecv(SocketAsyncEventArgs args)
        {
            bool pending = _socket.ReceiveAsync(args); 
            if (pending == false) //pending 값이 false라는 것은 비동기 작업이 즉시 완료를 의미
                OnReceiveCompleted(null, args);
        }

        void OnReceiveCompleted(object? sender, SocketAsyncEventArgs args)
        {
            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            {
                try
                {
                    string recvData = Encoding.UTF8.GetString(args.Buffer, args.Offset, args.BytesTransferred);
                    Console.WriteLine($"[from client] {recvData}");
                    RegisterRecv(args); // 다음 수신 대기
                }
                catch (Exception e)
                {
                    Console.WriteLine($"OnReceiveCompleted failed: {e}");
                }
            }
            else
            {
                Disconnect();
            }
        }

        #endregion
    }
}
