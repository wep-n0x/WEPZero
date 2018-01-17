namespace Game.Networking
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    public class WRServer : Core.TcpServer.Listener
    {
        #region PacketTable 
        private Dictionary<ushort, PacketHandler> _packetTable;

        public void LoadPacketTable()
        {
            _packetTable = new Dictionary<ushort, PacketHandler>();
            AddPacket(24832, new Handlers.CP_GET_SERVER_TIME());
            AddPacket(25088, new Handlers.CP_LOGIN());
            AddPacket(24576, new Handlers.CP_EXIT());
        }

        private void AddPacket(ushort _opc, PacketHandler _handler)
        {
            if (_packetTable.ContainsKey(_opc) == false)
                _packetTable.Add(_opc, _handler);
            else
                Core.Log.WriteError("Duplicate handler! [" + _opc + "]");
        }

        public PacketHandler GetPacket(ushort _opc)
        {
            if (_packetTable.ContainsKey(_opc))
                return _packetTable[_opc];
            else
                return (PacketHandler)null;
        }

        #endregion

        #region SessionManagement
        private Dictionary<ushort, WRClient> _tempSessions;
        public int overallSessions = 0;

        public WRClient GetClient(ushort SessionIdx)
        {
            if (_tempSessions.ContainsKey(SessionIdx))
                return _tempSessions[SessionIdx];
            else
                return (WRClient)null;
        }

        public void RemoveClient(WRClient client)
        {
            if (client.SessionIdx == 0)
                return;

            if (_tempSessions.ContainsKey(client.SessionIdx))
            {
                _tempSessions.Remove(client.SessionIdx);
                Core.Log.WriteNetwork("Removed session [" + client.SessionIdx + "]");
            }
        }

        private ushort GetFreeID()
        {
            for (ushort I = 1; I < ushort.MaxValue; I++)
            {
                if (_tempSessions.ContainsKey(I) == false)
                    return I;
            }
            return 0;
        }
        #endregion

        public WRServer(string _ip, ushort _port)
            : base(System.Net.IPAddress.Parse(_ip), _port)
        { _tempSessions = new Dictionary<ushort, WRClient>(); }

        #region Networking
        public override void HandleConnection(Socket _tcpSocket)
        {
            try
            {
                WRClient _wc = new WRClient(_tcpSocket);

                overallSessions++;

                ushort _tempSessionID = GetFreeID();
                this._tempSessions.Add(_tempSessionID, _wc);
                _wc.SessionIdx = _tempSessionID;

                Core.Log.WriteDebug("New WRClient (SIdx: " + _wc.SessionIdx + ")");
            }
            catch { }
        }
        #endregion
    }
}
