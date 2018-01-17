using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentification.Handlers
{
    public class CP_GET_CLIENT_VER : Networking.PacketHandler
    {
        private static void MakeClientVerPacket(WRClient wc)
        {
            Core.OutPacket mPacket = new Core.OutPacket(4112);
            mPacket.AddBlock(Globals.GetInstance().Config.GetValue("Format")); // Format
            mPacket.AddBlock(Globals.GetInstance().Config.GetValue("Launcher")); // Launcher Version
            mPacket.AddBlock(Globals.GetInstance().Config.GetValue("Updater")); // Updater Version
            mPacket.AddBlock(Globals.GetInstance().Config.GetValue("Client")); // Client Version
            mPacket.AddBlock(Globals.GetInstance().Config.GetValue("Sub")); // Sub Version
            mPacket.AddBlock(Globals.GetInstance().Config.GetValue("Option")); // Option
            mPacket.AddBlock(Globals.GetInstance().Config.GetValue("URL")); // Updater URL
            byte[] mBuffer = mPacket.GetOutput();
            for (byte I = 0; I < mBuffer.Length; I++)
                mBuffer[I] ^= Core.BuildConfig.AuthKey_Server;
            wc.ClientSocket.Send(mBuffer);
        }

        public override void Handle()
        {
            MakeClientVerPacket(this.client);
        }
    }
}
