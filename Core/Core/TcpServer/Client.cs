namespace Core.TcpServer
{
    using System;
    using System.Text;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public class Client
    {
        public Socket ClientSocket;

        public Client(Socket _ClientSocket) {
            this.ClientSocket = _ClientSocket;
            this.Initialize();
        }

        public void Disconnect(string _reason = "") {
            if(_reason.Length > 0) {
                //=> (TADO): Add Reason to database
            }

            try { this.ClientSocket.Close(); }
            catch { }

            this.OnDisconnect(); 
        }

        private void Initialize() {
            try {
                /*=> Begin to receive data <=*/
                Thread T = new Thread(new ThreadStart(this.RecvDataThread));
                T.SetApartmentState(ApartmentState.STA);
                T.Priority = ThreadPriority.Highest;
                T.Start();
            } catch { this.Disconnect(); }
        }

        private void RecvDataThread() {
            int mLength = 0;
            byte[] mBuffer = new byte[8192];

            while(this.ClientSocket.Connected) {
                mLength = this.ClientSocket.Receive(mBuffer);
                if (mLength > 0) {
                    byte[] _packetBuffer = new byte[mLength];
                    Array.Copy(mBuffer, _packetBuffer, mLength);

                    try {
                        string[] sPackets = ASCIIEncoding.GetEncoding("Windows-1250").GetString(_packetBuffer).Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string mPacket in sPackets) {
                            if (mPacket.Length > 10){
                                byte[] mPacketBuffer = ASCIIEncoding.GetEncoding("Windows-1250").GetBytes(mPacket);
                                this.OnReceiveData(mPacketBuffer);
                            }
                        }
                    } catch { }
                } else
                    this.Disconnect();
            }

            this.Disconnect();
        }

        public virtual void OnDisconnect() { }
        public virtual void OnReceiveData(byte[] _buffer) { }
    }
}
