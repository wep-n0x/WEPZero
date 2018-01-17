using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DB
{
    public class Tools
    {
        public static string EscapeString(string query)
        { 
            try { return query.Replace(@"\", "\\").Replace("'", @"`"); }
            catch { return ""; }
        }
    }
}
