namespace Core.TcpServer
{
    using System;
    using System.Text;
    using System.Net;
    using System.Net.Sockets;


    public class Listener
    {
        private Socket ServerSocket;

        public ushort Port = 0;
        public ulong IP = 0; //=> Converting IP to Integer

        public Listener(IPAddress _IP, ushort _Port)
        {
            this.IP = (ulong)_IP.Address;
            this.Port = _Port;
        }

        public void Initialize()
        {
            this.ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.ServerSocket.Bind(new IPEndPoint(new IPAddress((long)this.IP), this.Port));
        }

        public bool BeginListening()
        {
            try {
                this.ServerSocket.Listen(0);
                this.ServerSocket.BeginAccept(new AsyncCallback(this.OnAcceptConnection), null);
                Log.WriteNetwork("TcpListener is now listening on port: " + this.Port);
                return true;
            } catch { }
            return false;
        }

        private void OnAcceptConnection(IAsyncResult iAr) {
            try  {
                Socket _clientSocket = this.ServerSocket.EndAccept(iAr);
                if(_clientSocket.Connected) {
                    Log.WriteNetwork("New connection ! [" + ((IPEndPoint)_clientSocket.RemoteEndPoint).ToString() + "]"); 
                    this.HandleConnection(_clientSocket);
                }
            } catch { }

            if (this.ServerSocket != null)
                this.ServerSocket.BeginAccept(new AsyncCallback(this.OnAcceptConnection), null);
        }

        public virtual void HandleConnection(Socket _tcpSocket) { }
    }
}
