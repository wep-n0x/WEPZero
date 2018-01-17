using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class ItemShop
    {
        public Dictionary<string, GameFramework.Elements.EShopItem> itemTable = new Dictionary<string, GameFramework.Elements.EShopItem>();

        public void ReadItemsFile() {
            try {
                int Idx = 0;
                string[] _content = System.IO.File.ReadAllLines(Environment.CurrentDirectory + "\\data\\items.txt");
                if (_content.Length > 0) {
                    for(int I = 0; I < _content.Length; I++) {
                        if(_content[I].Contains("<BASIC_INFO>")) { // Item found
                            string lineName = _content[I + 2];
                            string lineCode = _content[I + 3];
                            string lineBuyable = _content[I + 11];
                            string lineCost = _content[I + 14];

                            string itemName = lineName.Replace(" ", "").Trim().Split('=')[1];
                            string itemCode = lineCode.Replace(" ", "").Trim().Split('=')[1];
                            string itemBuyable = lineBuyable.Replace(" ", "").Trim().Split('=')[1];
                            string itemCost = lineCost.Replace(" ", "").Trim().Split('=')[1];


                            bool valBuyable = itemBuyable.ToLower().Contains("true");
                            string[] valCost = itemCost.Split(',');

                            GameFramework.Elements.EShopItem mShopItem = new GameFramework.Elements.EShopItem();
                            mShopItem.ID = Idx;
                            mShopItem.Name = itemName;
                            mShopItem.Code = itemCode;
                            mShopItem.Buyable = valBuyable;
                            mShopItem.Cost = valCost;
                            Idx++;

                            if (itemCode.StartsWith("DA"))
                            {
                                string linePower = _content[I + 27];
                                string itemPower = linePower.Replace(" ", "").Trim().Split('=')[1];
                                int valPower = Convert.ToInt32(itemPower);
                                mShopItem.WeaponPower = valPower;
                            }

                            itemTable.Add(itemCode, mShopItem);
                        }
                    }
                }
                Core.Log.WritePlain("ITEMSHOP", "Loaded " + itemTable.Count + " items into memory.");
                return;
            }
            catch { }
            Core.Log.WriteError("Couldn't read \\data\\items.bin!");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
