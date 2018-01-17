using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class BuildConfig
    {
        public static string Rev = "0.0.1";
        public static string LastUpdate = "16.01.2018";

        public static string Auth_ConfigFile = "auth.ini";
        public static string Game_ConfigFile = "game.ini";

        public static byte AuthKey_Client = 0xC3;
        public static byte AuthKey_Server = 0x96;

        public static byte GameKey_Client = 0xC3;
        public static byte GameKey_Server = 0x96;
    }
}
