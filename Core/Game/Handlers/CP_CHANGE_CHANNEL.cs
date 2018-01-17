using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Handlers
{
    public class CP_CHANGE_CHANNEL : Networking.PacketHandler
    {
        public static void MakeChannelChange(WRClient wc)
        {
            Core.OutPacket mPacket = new Core.OutPacket(28673);
            mPacket.AddBlock(1);
            mPacket.AddBlock((int)wc.Player.ChannelID);
            byte[] mBuffer = mPacket.GetOutput();
            for (int I = 0; I < mBuffer.Length; I++)
                mBuffer[I] ^= Core.BuildConfig.GameKey_Server;
            wc.ClientSocket.Send(mBuffer);
        }

        public override void Handle()
        { 
            int desiredChannel = this.packet.GetInt(0);
            if(Core.BuildConfig.ChannelEnabled.Split(',')[desiredChannel] == "C") {
                desiredChannel = (int)GameFramework.Channels.CQC;
                //TADO: Chat that channel is closed
            }

            this.client.Player.ChannelID = (GameFramework.Channels)desiredChannel;
            MakeChannelChange(this.client);
        }
    }
}
