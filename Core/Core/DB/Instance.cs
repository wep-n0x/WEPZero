using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DB
{
    public class Instance
    {
        private string mysqlConnectionInfo = string.Empty;

        public void SetCredentials(string hostname, string username, string password, string database)
        {
            this.mysqlConnectionInfo = string.Format("server={0};port={1} ;user id={2}; password={3}; database={4}; pooling=false", hostname, "3306", username, password, database);
        }

        public bool TestConnection() {
            try {
                MySql.Data.MySqlClient.MySqlConnection mConnection = new MySql.Data.MySqlClient.MySqlConnection(this.mysqlConnectionInfo);
                mConnection.Open();
                mConnection.Close();
                return true;
            } catch { }
            return false;
        }

        public MySql.Data.MySqlClient.MySqlConnection CreateConnection() {
            MySql.Data.MySqlClient.MySqlConnection mConnection = new MySql.Data.MySqlClient.MySqlConnection(mysqlConnectionInfo);
            try {
                mConnection.Open();
                return mConnection;
            } catch { }
            return null;
        }
    }
}
