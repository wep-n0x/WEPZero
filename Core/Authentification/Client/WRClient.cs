namespace Authentification
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

        public WRClient(System.Net.Sockets.Socket clientSocket)
            : base(clientSocket)
        { }

        public override void OnReceiveData(byte[] _buffer) {
            try {
                List<byte> mBuffer = new List<byte>();
                for (int I = 0; I < _buffer.Length; I++)
                    mBuffer.Add((byte)(_buffer[I] ^ 0xC3));

                InPacket _packet = new InPacket();
                _packet.Set(mBuffer.ToArray());

                Networking.PacketHandler _handler = Globals.GetInstance().ServerInstance.GetPacket(_packet.GetOPC());
                _handler.Set(this, mBuffer.ToArray());

                if (_handler != null)
                    _handler.Handle();
            } catch { }
        }
    }
}
