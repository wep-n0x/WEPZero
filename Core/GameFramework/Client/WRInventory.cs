using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.Client
{
    public class WRInventory
    {
        public List<Elements.EItem> itemTable = new List<Elements.EItem>();

        public string[] Engineer;
        public string[] Medic;
        public string[]  Sniper;
        public string[] Assault;
        public string[] Heavy;
 
        public string BuildItemList()
        {
            StringBuilder sb = new StringBuilder();
            foreach(Elements.EItem mItem in itemTable.ToArray()) {
                sb.Append(mItem.Code + "-1-2-"+mItem.ExpireDate+"-0-0-0-0-0,");
            }

            if(itemTable.Count < 32) {
                for(int I = 0; I < (32 - itemTable.Count); I++) {
                    sb.Append("^,");
                }
            }

            return sb.Remove(sb.Length - 1, 1).ToString(); 
        }
    }
}
