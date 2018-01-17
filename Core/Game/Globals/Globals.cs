using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Globals
    {
        public Networking.WRServer ServerInstance = null;
        public Core.IO.ConfigReader Config = null;
        public Core.DB.Instance AuthDatabase = null; 

        private static Globals m_pGlobals = null;
        public static Globals GetInstance()
        {
            if (m_pGlobals == null)
                m_pGlobals = new Globals();
            return m_pGlobals;
        }
    }
}
