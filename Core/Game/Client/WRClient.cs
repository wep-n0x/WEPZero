namespace Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Core;

    public class WRClient : Core.TcpServer.Client
    {
        public ushort SessionIdx = 0;
        public Client.WRAccount Account = new Client.WRAccount();

        public long RemoteIP = 0;
        public int RemotePort = 0;
        public long LocalIP = 0;
        public int LocalPort = 0;

        public WRClient(System.Net.Sockets.Socket clientSocket)
            : base(clientSocket)
        { }

        #region Packets 
        #endregion

        public override void OnDisconnect()
        {
            Globals.GetInstance().ServerInstance.RemoveClient(this);
            Globals.GetInstance().UdpInstance.usersUDP[this.SessionIdx] = null;
        }

        public override void OnReceiveData(byte[] _buffer)
        {
            try
            {
                List<byte> mBuffer = new List<byte>();
                for (int I = 0; I < _buffer.Length; I++)
                    mBuffer.Add((byte)(_buffer[I] ^ Core.BuildConfig.GameKey_Client));

                InPacket _packet = new InPacket();
                _packet.Set(mBuffer.ToArray());

                Networking.PacketHandler _handler = Globals.GetInstance().ServerInstance.GetPacket(_packet.GetOPC());
                if (_handler != null && _handler is Networking.PacketHandler)
                {
                    _handler.Set(this, mBuffer.ToArray());

                    if (_handler != null)
                        _handler.Handle();
                }
                else
                {
                    Core.Log.WriteError("Received unhandled packet! [" + _packet.GetOPC() + "]");
                }
            }
            catch { }
        }
    }
}
