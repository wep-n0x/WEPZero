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

        public bool HasItem(string _code)
        {
            foreach (Elements.EItem mItem in itemTable.ToArray())
                if (mItem.Code.ToLower().Equals(_code.ToLower()))
                    return true;
            return false;
        }

        public void AddItem(int _Idx, string _Code, int _LeasePeriod)
        {
            if (_LeasePeriod == -1)
                _LeasePeriod = 1333337; //Retail Weapon

            if(!HasItem(_Code)) {
                DateTime dt = DateTime.Now;
                dt = dt.AddDays(_LeasePeriod).AddHours(-1);
                string expireDate = String.Format("{0:yyMMddHH}", dt);

                Elements.EItem mNewItem = new Elements.EItem();
                mNewItem.Code = _Code;
                mNewItem.ExpireDate = expireDate;

                this.itemTable.Add(mNewItem); 
            } else {
                foreach (Elements.EItem mItem in this.itemTable.ToArray()) {
                    if(mItem.Code.ToLower().Equals(_Code.ToLower())) { 
                        DateTime itemTime = DateTime.ParseExact(mItem.ExpireDate, "yyMMddHH", null);
                        itemTime = itemTime.AddDays(_LeasePeriod).AddHours(-1);

                        string expireDate = String.Format("{0:yyMMddHH}", itemTime);
                        mItem.ExpireDate = expireDate;
                        break;
                    }
                }
            }
        }
    }
}
