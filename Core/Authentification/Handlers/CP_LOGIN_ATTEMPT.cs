using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentification.Handlers
{
    public class CP_LOGIN_ATTEMPT : Networking.PacketHandler
    {
        public enum ErrorCodes : uint
        {
            Exception = 70101,
            NewNickname = 72000,
            WrongUser = 72010,
            WrongPW = 72020,
            AlreadyLoggedIn = 72030,
            ClientVerNotMatch = 70301,
            Banned = 73050,
            BannedTime = 73020,
            NotActive = 73040,
            EnterIDError = 74010,
            EnterPasswordError = 74020,
            ErrorNickname = 74030,
            NicknameTaken = 74070,
            NicknameToLong = 74100, // Longer then 10 characters.
            IlligalNickname = 74110
        }

        public static void MakeLoginError(WRClient wc, ErrorCodes err)
        {
            Core.OutPacket mPacket = new Core.OutPacket(4352);
            mPacket.AddBlock((int)err);
            byte[] mBuffer = mPacket.GetOutput();
            for (byte I = 0; I < mBuffer.Length; I++)
                mBuffer[I] ^= Core.BuildConfig.AuthKey_Server;
            wc.ClientSocket.Send(mBuffer);
        }

        public static void MakeServerListPacket(WRClient wc)
        {
            Core.OutPacket mPacket = new Core.OutPacket(4352);
            mPacket.AddBlock(1); // ErrorCode (Error_OK)
            mPacket.AddBlock(wc.Account.Idx); // ID
            mPacket.AddBlock(0);
            mPacket.AddBlock(wc.Account.Username); // Username
            mPacket.AddBlock(wc.Account.Password); // Password
            mPacket.AddBlock(wc.Account.Nickname); // Nickname
            mPacket.AddBlock(wc.SessionIdx);
            mPacket.AddBlock(1); // Unknown
            mPacket.AddBlock(0); // Unknown
            mPacket.AddBlock(wc.Account.Accesslevel); // Accesslevel

            mPacket.AddBlock(Globals.GetInstance().ServerListHandler.serverListTable.Count); // Server Count
            foreach(ServerInfo mInfo in Globals.GetInstance().ServerListHandler.serverListTable.Values)
            {
                mPacket.AddBlock(mInfo.Idx);
                mPacket.AddBlock(mInfo.Name);
                mPacket.AddBlock(mInfo.IP);
                mPacket.AddBlock(5340);
                mPacket.AddBlock(0); // PlayerCount
                mPacket.AddBlock(mInfo.Flag);
            }

            mPacket.AddBlock(-1); // Clan ID
            mPacket.AddBlock(-1); // Clan Name
            mPacket.AddBlock(-1); // Clan Master
            mPacket.AddBlock(-1); // Unknown 
            mPacket.AddBlock(0); // Unknown
            mPacket.AddBlock(0); // Unknown
            byte[] mBuffer = mPacket.GetOutput();
            for (byte I = 0; I < mBuffer.Length; I++)
                mBuffer[I] ^= Core.BuildConfig.AuthKey_Server;
            wc.ClientSocket.Send(mBuffer);
        }

        public override void Handle() {
            string mUsername = Core.DB.Tools.EscapeString(this.packet.GetString(2));
            string mPassword = Core.DB.Tools.EscapeString(this.packet.GetString(3));

            try {
                MySql.Data.MySqlClient.MySqlConnection mConnection = Globals.GetInstance().AuthDatabase.CreateConnection();
                MySql.Data.MySqlClient.MySqlCommand mCommand = new MySql.Data.MySqlClient.MySqlCommand("SELECT id, username, password, nickname, accesslevel, online FROM accounts WHERE username='" + mUsername + "'",mConnection);
                MySql.Data.MySqlClient.MySqlDataReader mReader = mCommand.ExecuteReader();
                if (!mReader.HasRows) {
                    MakeLoginError(this.client, ErrorCodes.WrongUser);
                } else {
                    while (mReader.Read()) { 
                        if (mPassword != mReader.GetString(2)) {
                            MakeLoginError(this.client, ErrorCodes.WrongPW); 
                        } else if(mReader.GetInt32(5) == 1) { 
                            MakeLoginError(this.client, ErrorCodes.AlreadyLoggedIn);
                        } else if(mReader.GetInt32(4) == 0) { 
                            MakeLoginError(this.client, ErrorCodes.Banned);
                        } else {
                            this.client.Account.Idx = mReader.GetInt32(0);
                            this.client.Account.Username = mUsername;
                            this.client.Account.Password = "#CENSORED#";
                            this.client.Account.Nickname = mReader.GetString(3);
                            this.client.Account.Accesslevel = mReader.GetInt32(4);
                            MakeServerListPacket(this.client);
                        }
                    }
                }
                mReader.Close();
                mConnection.Close();
            } catch(Exception ex) {
                MakeLoginError(this.client, ErrorCodes.Exception);
            }
        }
    }
}
