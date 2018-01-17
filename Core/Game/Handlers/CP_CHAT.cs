using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Handlers
{
    public class CP_CHAT : Networking.PacketHandler
    {
        public enum ChatType : int
        {
            Notice1 = 1,
            Notice2,
            Lobby_ToChannel,
            Room_ToAll,
            Room_ToTeam,
            Whisper,
            Lobby_ToAll = 8,
            Clan
        }

        public enum ErrorCodes : int
        {
            ErrorUser = 95040
        }

        public static void MakeChatError(WRClient wc, ErrorCodes err)
        {
            Core.OutPacket mPacket = new Core.OutPacket(29696);
            mPacket.AddBlock((int)err);
            byte[] mBuffer = mPacket.GetOutput();
            for (int I = 0; I < mBuffer.Length; I++)
                mBuffer[I] ^= Core.BuildConfig.GameKey_Server;
            wc.ClientSocket.Send(mBuffer);
        }

        public static void MakeChatPacket(WRClient wc, int userId, string name, ChatType type, int targetId, string targetName, string message)
        {
            Core.OutPacket mPacket = new Core.OutPacket(29696);
            mPacket.AddBlock(1); //Error_OK
            mPacket.AddBlock(userId);
            mPacket.AddBlock(name);
            mPacket.AddBlock((int)type);
            mPacket.AddBlock(targetId);
            mPacket.AddBlock(targetName);
            mPacket.AddBlock(message);
            byte[] mBuffer = mPacket.GetOutput();
            for (int I = 0; I < mBuffer.Length; I++)
                mBuffer[I] ^= Core.BuildConfig.GameKey_Server;
            wc.ClientSocket.Send(mBuffer);
        }


        public static void MakeChatPacket(WRClient wc, ChatType type, int targetId, string targetName, string message)
        {
            MakeChatPacket(wc, wc.Account.Idx, wc.Account.Nickname, type, targetId, targetName, message);
        }

        public override void Handle()
        {
            base.Handle();
        }
    }
}
