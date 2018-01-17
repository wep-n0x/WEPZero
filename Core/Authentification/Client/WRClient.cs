﻿namespace Authentification
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

        public WRClient(System.Net.Sockets.Socket clientSocket)
            : base(clientSocket)
        { MakeHandShake(); }

        #region Packets
        private void MakeHandShake()
        {
            OutPacket mPacket = new OutPacket(4608);
            mPacket.AddBlock(new Random().Next(1000000, 9900000));
            mPacket.AddBlock(0);
            byte[] mBuffer = mPacket.GetOutput();
            for (byte I = 0; I < mBuffer.Length; I++)
                mBuffer[I] ^= Core.BuildConfig.AuthKey_Server;
            this.ClientSocket.Send(mBuffer);
        }
        #endregion

        public override void OnDisconnect() {
            Globals.GetInstance().ServerInstance.RemoveClient(this);
        }

        public override void OnReceiveData(byte[] _buffer) {
            try {
                List<byte> mBuffer = new List<byte>();
                for (int I = 0; I < _buffer.Length; I++)
                    mBuffer.Add((byte)(_buffer[I] ^ Core.BuildConfig.AuthKey_Client));

                InPacket _packet = new InPacket();
                _packet.Set(mBuffer.ToArray());

                Networking.PacketHandler _handler = Globals.GetInstance().ServerInstance.GetPacket(_packet.GetOPC());
                if (_handler != null && _handler is Networking.PacketHandler)
                {
                    _handler.Set(this, mBuffer.ToArray());

                    if (_handler != null)
                        _handler.Handle();
                } else {
                    Core.Log.WriteError("Received unhandled packet! [" + _packet.GetOPC() + "]");
                }
            } catch { }
        }
    }
}
