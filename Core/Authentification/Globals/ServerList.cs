using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentification {
    public class ServerInfo {
        public int Idx;
        public string Name;
        public string IP;
        public int Flag;

        public ServerInfo(int _idx, string _name, string _ip, int _flag)
        {
            this.Idx = _idx;
            this.Name = _name;
            this.IP = _ip;
            this.Flag = _flag;
        }
    }

    public class ServerList {
        public Dictionary<int, ServerInfo> serverListTable = new Dictionary<int, ServerInfo>();

        public void LoadServerList() {
            Core.Log.WritePlain("SERVERLIST", "Loading server list...");
            try {
                MySql.Data.MySqlClient.MySqlConnection mConnection = Globals.GetInstance().AuthDatabase.CreateConnection();
                MySql.Data.MySqlClient.MySqlCommand mCommand = new MySql.Data.MySqlClient.MySqlCommand("SELECT id, name, ip, flag FROM serverlist WHERE 1", mConnection);
                MySql.Data.MySqlClient.MySqlDataReader mReader = mCommand.ExecuteReader();
                while(mReader.Read()) {
                    ServerInfo mInfo = new ServerInfo(mReader.GetInt32(0), mReader.GetString(1), mReader.GetString(2), mReader.GetInt32(3));
                    serverListTable.Add(mInfo.Idx, mInfo);
                }
                mReader.Close();
                mConnection.Close();
                Core.Log.WritePlain("SERVERLIST", "Done!");
                Console.WriteLine();
                return;
            } catch { }
            Core.Log.WriteError("Couldn't load serverlist!");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
