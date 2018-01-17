using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Handlers
{
    public class CP_LOGIN : Networking.PacketHandler
    {
        public enum ErrorCodes : uint
        {
            NormalProcedure = 73030,    // Please log in using the normal procedure!
            InvalidPacket = 90100,      // Invalid Packet.
            UnregisteredUser = 90101,   // Unregistered User.
            AtLeast6Chars = 90102,      // You must type at least 6 characters .
            NicknameToShort = 90103,    // Nickname should be at least 6 charaters.
            IdInUseOtherServer = 90104, // Same ID is being used on the server.
            NotAccessible = 90105,      // Server is not accessible.
            TrainingServer = 90106,     // Trainee server is accesible until the rank of a private..
            ClanWarError = 90112,       // You cannot participate in Clan War
            LackOfResponse = 91010,     // Connection terminated because of lack of response for a while.
            ServerIsFull = 91020,       // You cannot connect. Server is full.
            InfoReqInTrafic = 91030,    // Info request are in traffic.
            AccountUpdateFailed = 91040,// Account update has failed.
            BadSynchronization = 91050, // User Info synchronization has failed.
            IdInUse = 92040,            // That ID is currently being used.
            PremiumOnly = 98010         // Available to Premium users only.
        }

        public static void MakeLoginError(WRClient wc, ErrorCodes err)
        {
            Core.OutPacket mPacket = new Core.OutPacket(25088);
            mPacket.AddBlock((int)err);
            byte[] mBuffer = mPacket.GetOutput();
            for (int I = 0; I < mBuffer.Length; I++)
                mBuffer[I] ^= Core.BuildConfig.GameKey_Server;
            wc.ClientSocket.Send(mBuffer);
        }

        public static void MakeCharacterInfo(WRClient wc)
        { 
            Core.OutPacket mPacket = new Core.OutPacket(25088);
            mPacket.AddBlock(1);
            mPacket.AddBlock("GameServer");
            mPacket.AddBlock(wc.SessionIdx);
            mPacket.AddBlock(wc.Account.Idx);
            mPacket.AddBlock(wc.SessionIdx);
            mPacket.AddBlock(wc.Account.Nickname);
            mPacket.AddBlock(-1); // CLAN
            mPacket.AddBlock(-1); // CLAN 
            mPacket.AddBlock(-1); // CLAN
            mPacket.AddBlock(-1); // CLAN
            mPacket.AddBlock(wc.Account.Premium);
            mPacket.AddBlock(0); // Unknown
            mPacket.AddBlock(0); // Unknown
            mPacket.AddBlock(Globals.GetInstance().LevelManager.GetLevelForExp(wc.Account.Exp)); // Level
            mPacket.AddBlock(wc.Account.Exp); 
            mPacket.AddBlock(0); // Unknown
            mPacket.AddBlock(0); // Unknown
            mPacket.AddBlock(wc.Account.Dinar);
            mPacket.AddBlock(wc.Account.Kills);
            mPacket.AddBlock(wc.Account.Deaths);
            mPacket.AddBlock(0); // Unknown
            mPacket.AddBlock(0); // Unknown
            mPacket.AddBlock(0); // Unknown
            mPacket.AddBlock(0); // Unknown
            mPacket.AddBlock(0); // Unknown 
            mPacket.AddBlock(wc.GetOpenSlots()); // Slots opened
            mPacket.AddBlock(string.Join(",", wc.Inventory.Engineer));
            mPacket.AddBlock(string.Join(",", wc.Inventory.Medic));
            mPacket.AddBlock(string.Join(",", wc.Inventory.Sniper));
            mPacket.AddBlock(string.Join(",", wc.Inventory.Assault));
            mPacket.AddBlock(string.Join(",", wc.Inventory.Heavy));
            mPacket.AddBlock(wc.Inventory.BuildItemList());
            mPacket.AddBlock(0); // Unknown
            mPacket.AddBlock(0); // Unknown 
            byte[] mBuffer = mPacket.GetOutput();
            for (int I = 0; I < mBuffer.Length; I++)
                mBuffer[I] ^= Core.BuildConfig.GameKey_Server;
            wc.ClientSocket.Send(mBuffer);
        }

        public override void Handle()
        {
            try {
                int userId = this.packet.GetInt(0);

                MySql.Data.MySqlClient.MySqlConnection mConnection = Globals.GetInstance().GameDatabase.CreateConnection();
                MySql.Data.MySqlClient.MySqlCommand mCommannd = new MySql.Data.MySqlClient.MySqlCommand("SELECT id, username, nickname, accesslevel, exp, dinar, kills, deaths, premium, premiumexpire FROM accounts WHERE id='" + userId + "'", mConnection);
                MySql.Data.MySqlClient.MySqlDataReader mReader = mCommannd.ExecuteReader();
                if (mReader.HasRows) {
                    while (mReader.Read()) {
                        this.client.Account.Idx = mReader.GetInt32(0);
                        this.client.Account.Username = mReader.GetString(1);
                        this.client.Account.Nickname = mReader.GetString(2);
                        this.client.Account.Accesslevel = mReader.GetInt32(3);
                        this.client.Account.Exp = mReader.GetInt32(4);
                        this.client.Account.Dinar = mReader.GetInt32(5);
                        this.client.Account.Kills = mReader.GetInt32(6);
                        this.client.Account.Deaths = mReader.GetInt32(7);
                        this.client.Account.Premium = mReader.GetInt32(8);
                        this.client.Account.PremiumExpire = mReader.GetInt32(9);

                        this.client.Inventory = new GameFramework.Client.WRInventory();
                        this.client.LoadInventory();

                        /* Check for expired items */
                        foreach(GameFramework.Elements.EItem mItem in this.client.Inventory.itemTable.ToArray()) {
                            DateTime itemTime = DateTime.ParseExact(mItem.ExpireDate, "yyMMddHH", null);
                            TimeSpan itemSpan = (TimeSpan)(itemTime - DateTime.Now);
                            if(itemSpan.TotalMilliseconds <= 0) { // Item is expired
                                this.client.Inventory.itemTable.Remove(mItem);
                                this.client.SaveInventory();
                            }
                        }

                        MakeCharacterInfo(this.client);

                        this.client.SendSystemMessage("MOTD: " + Globals.GetInstance().Config.GetValue("MOTD"));
                        this.client.SendSystemMessage("Welcome to WEP v" + Core.BuildConfig.Rev + "!");
                        this.client.SendSystemMessage("This emulator is still in development.");
                    }
                } else {
                    MakeLoginError(this.client, ErrorCodes.NormalProcedure);
                }
                mReader.Close();
                mConnection.Close();
            } catch { }
        }
    }
}
