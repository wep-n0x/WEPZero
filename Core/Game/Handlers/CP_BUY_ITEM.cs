using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Handlers
{
    public class CP_BUY_ITEM : Networking.PacketHandler
    {
        public enum ErrorCodes : uint
        {
            PremiumOnly = 98010,        // Available to Premium users only.
            GoldPremiumOnly = 98020,    // Available to Gold users only.
            Slot5IsFree = 98030,        // 5th Slot is free for Gold user.
            InvalidItem = 97010,        // Item is no longer valid
            Slot5Required = 97012,      // You must purchase 5th slot first.
            Slot5RequiredTime = 97015,  // Insufficient slot time.
            CannotBeBougth = 97020,     // Item cannot be bought
            NotEnoughDinar = 97040,     // Insufficient balance
            LevelUnsuitable = 97050,    // Your level is unsuitable
            LevelRequirement = 97060,   // You do not meet the level requirements\n to purchase this weapon.
            InventoryFull = 97070,      // Your inventory is full
            ExceededLeasePeriod = 97080,// Cannot purchase. You have exceeded maximum lease period.
            CannotPurchaseTwice = 97090 // You cannot purchase the item twice.
        }

        public static void MakeShopError(WRClient wc, ErrorCodes err)
        {
            Core.OutPacket mPacket = new Core.OutPacket(30208);
            mPacket.AddBlock((int)err);
            byte[] mBuffer = mPacket.GetOutput();
            for (int I = 0; I < mBuffer.Length; I++)
                mBuffer[I] ^= Core.BuildConfig.GameKey_Server;
            wc.ClientSocket.Send(mBuffer);
        }

        public static void MakeShopPurchase(WRClient wc)
        {
            Core.OutPacket mPacket = new Core.OutPacket(30208);
            mPacket.AddBlock(1); //Error_OK
            mPacket.AddBlock(1110); //Action_Code
            mPacket.AddBlock(-1);
            mPacket.AddBlock(3);
            mPacket.AddBlock(wc.Inventory.itemTable.Count);
            mPacket.AddBlock(wc.Inventory.BuildItemList());
            mPacket.AddBlock(wc.Account.Dinar);
            mPacket.AddBlock("F,F,F,F"); 
            byte[] mBuffer = mPacket.GetOutput();
            for (int I = 0; I < mBuffer.Length; I++)
                mBuffer[I] ^= Core.BuildConfig.GameKey_Server;
            wc.ClientSocket.Send(mBuffer);
        }

        private int[] days = { 3, 7, 15, 30 };
        public override void Handle() {
            int actionType = this.packet.GetInt(0);
            if (actionType == 1110)  { //Buy item
                string itemCode = this.packet.GetString(1).ToUpper();

                if (itemCode.Length == 4) {
                    if (Globals.GetInstance().ShopInstance.itemTable.ContainsKey(itemCode)) {
                        GameFramework.Elements.EShopItem mShopItem = Globals.GetInstance().ShopInstance.itemTable[itemCode];
                        if (mShopItem != null) {
                            int length = this.packet.GetInt(4);
                            if (length < 4) {
                                if(mShopItem.Buyable == true) {
                                    if(this.client.Inventory.itemTable.Count < 32) {
                                        int dinarResult = this.client.Account.Dinar - int.Parse(mShopItem.Cost[length]);
                                        if(dinarResult >= 0) {
                                            this.client.Account.Dinar = dinarResult;

                                            MySql.Data.MySqlClient.MySqlConnection mConnection = Globals.GetInstance().GameDatabase.CreateConnection();
                                            MySql.Data.MySqlClient.MySqlCommand mCommand = new MySql.Data.MySqlClient.MySqlCommand("UPDATE accounts SET dinar='" +dinarResult+ "' WHERE id='" + this.client.Account.Idx + "'", mConnection);
                                            mCommand.ExecuteNonQuery();
                                            mConnection.Close();

                                            this.client.Inventory.AddItem(this.client.Account.Idx, itemCode, days[length]);
                                            this.client.SaveInventory();

                                            MakeShopPurchase(this.client);
                                        } else { 
                                            MakeShopError(this.client, ErrorCodes.NotEnoughDinar);
                                        }
                                    } else {
                                        MakeShopError(this.client, ErrorCodes.InventoryFull);
                                    }
                                } else {
                                    MakeShopError(this.client, ErrorCodes.CannotBeBougth);
                                }
                            } else {
                                MakeShopError(this.client, ErrorCodes.ExceededLeasePeriod);
                            }
                        } else {
                            MakeShopError(this.client, ErrorCodes.CannotBeBougth);
                        }
                    } else { 
                        MakeShopError(this.client, ErrorCodes.CannotBeBougth);
                    }
                } else {
                    MakeShopError(this.client, ErrorCodes.InvalidItem);
                }
            }
        }
    }
}
