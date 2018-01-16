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

        public override void OnReceiveData(byte[] _buffer)
        {
            base.OnReceiveData(_buffer);
        }
    }
}
