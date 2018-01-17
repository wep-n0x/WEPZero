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
        public GameFramework.Client.WRInventory Inventory = new GameFramework.Client.WRInventory();
        public GameFramework.Client.WRPlayer Player = new GameFramework.Client.WRPlayer();
        
        public long RemoteIP = 0;
        public int RemotePort = 0;
        public long LocalIP = 0;
        public int LocalPort = 0;

        public void SendSystemMessage(string Message)
        {
            Handlers.CP_CHAT.MakeChatPacket(this, 0, "SYSTEM", Handlers.CP_CHAT.ChatType.Room_ToAll, 999, "NULL", "SYSTEM   >> " + Message);
        }

        public string GetOpenSlots()
        { 
            StringBuilder sb = new StringBuilder();
            if (this.Inventory.HasItem("CA01") || this.Account.Premium >= 3)
                sb.Append("T,");
            else
                sb.Append("F,");

            if ((this.Inventory.HasItem("DS05") || this.Inventory.HasItem("DV01")) || this.Inventory.HasItem("DS01") || this.Inventory.HasItem("DU01") || this.Inventory.HasItem("DU02") || this.Inventory.HasItem("DS03"))
                sb.Append("T,");
            else
                sb.Append("F,");

            if (this.Inventory.HasItem("CA03"))
                sb.Append("T,");
            else
                sb.Append("F,");

            sb.Append("F"); //(TADO): Add Retail system
            return sb.ToString();

        }

        #region SQL Functions

        public void SaveInventory()
        {
            MySql.Data.MySqlClient.MySqlConnection mConnection = Globals.GetInstance().GameDatabase.CreateConnection();
            MySql.Data.MySqlClient.MySqlCommand mCommand = new MySql.Data.MySqlClient.MySqlCommand("DELETE FROM inventory WHERE owner='" + this.Account.Idx + "'", mConnection);
            mCommand.ExecuteNonQuery();

            foreach(GameFramework.Elements.EItem mItem in this.Inventory.itemTable.ToArray())
            {
                mCommand = new MySql.Data.MySqlClient.MySqlCommand("INSERT INTO inventory(owner,code,expiredate) VALUES ('"+this.Account.Idx+"','"+mItem.Code+"','"+mItem.ExpireDate+"')", mConnection);
                mCommand.ExecuteNonQuery();
            }

            mCommand = new MySql.Data.MySqlClient.MySqlCommand("UPDATE equipment SET class1='" + string.Join(",", this.Inventory.Engineer) + "' WHERE owner='" + this.Account.Idx + "'", mConnection);
            mCommand.ExecuteNonQuery();

            mCommand = new MySql.Data.MySqlClient.MySqlCommand("UPDATE equipment SET class2='" + string.Join(",", this.Inventory.Medic) + "' WHERE owner='" + this.Account.Idx + "'", mConnection);
            mCommand.ExecuteNonQuery();

            mCommand = new MySql.Data.MySqlClient.MySqlCommand("UPDATE equipment SET class3='" + string.Join(",", this.Inventory.Sniper) + "' WHERE owner='" + this.Account.Idx + "'", mConnection);
            mCommand.ExecuteNonQuery();

            mCommand = new MySql.Data.MySqlClient.MySqlCommand("UPDATE equipment SET class4='" + string.Join(",", this.Inventory.Assault) + "' WHERE owner='" + this.Account.Idx + "'", mConnection);
            mCommand.ExecuteNonQuery();

            mCommand = new MySql.Data.MySqlClient.MySqlCommand("UPDATE equipment SET class5='" + string.Join(",", this.Inventory.Heavy) + "' WHERE owner='" + this.Account.Idx + "'", mConnection);
            mCommand.ExecuteNonQuery();

            mConnection.Close();
        }
        public void LoadInventory()
        {
            try {
                MySql.Data.MySqlClient.MySqlConnection mConnection = Globals.GetInstance().GameDatabase.CreateConnection();
                MySql.Data.MySqlClient.MySqlCommand mCommand = new MySql.Data.MySqlClient.MySqlCommand("SELECT code, expiredate FROM inventory WHERE owner='" + this.Account.Idx + "'", mConnection);
                MySql.Data.MySqlClient.MySqlDataReader mReader = mCommand.ExecuteReader();
                if (mReader.HasRows) {
                    while(mReader.Read()) {
                        GameFramework.Elements.EItem mItem = new GameFramework.Elements.EItem();
                        mItem.Code = mReader.GetString(0);
                        mItem.ExpireDate = mReader.GetString(1);
                        this.Inventory.itemTable.Add(mItem);
                    }
                }
                mReader.Close();
                mConnection.Close();

                mConnection = Globals.GetInstance().GameDatabase.CreateConnection();
                mCommand = new MySql.Data.MySqlClient.MySqlCommand("SELECT class1, class2, class3, class4, class5 FROM equipment WHERE owner='" + this.Account.Idx + "'", mConnection);
                mReader = mCommand.ExecuteReader();
                if (mReader.HasRows)
                {
                    while (mReader.Read())
                    {
                        this.Inventory.Engineer = mReader.GetString(0).Split(',');
                        this.Inventory.Medic = mReader.GetString(1).Split(',');
                        this.Inventory.Sniper = mReader.GetString(2).Split(',');
                        this.Inventory.Assault = mReader.GetString(3).Split(',');
                        this.Inventory.Heavy = mReader.GetString(4).Split(',');
                    }
                } else {
                    this.Disconnect();
                }
                mReader.Close();
                mConnection.Close();
            } catch { }
        }
        #endregion

        public WRClient(System.Net.Sockets.Socket clientSocket)
            : base(clientSocket)
        { }

        #region Networking
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
        #endregion
    }
}
