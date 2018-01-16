namespace Authentification.Networking
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
     
    public class WRServer : Core.TcpServer.Listener
    {
        private Dictionary<ushort, WRClient> _tempSessions;

        private ushort GetFreeID() {
            for(ushort I = 0; I < ushort.MaxValue; I++) {
                if (_tempSessions.ContainsKey(I) == false)
                    return I;
            }
            return 0;
        }

        public WRServer(string _ip, ushort _port)
            : base(System.Net.IPAddress.Parse(_ip), _port)
        { }

        public override void HandleConnection(Socket _tcpSocket) {
            try {
                WRClient _wc = new WRClient(_tcpSocket);

                ushort _tempSessionID = GetFreeID();
                this._tempSessions.Add(_tempSessionID, _wc);
                _wc.SessionIdx = _tempSessionID;

                Core.Log.WriteDebug("New WRClient (SIdx: " + _wc.SessionIdx + ")");
            } catch { }
        }
    }
}
