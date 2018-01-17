using System;
using System.Globalization;

namespace Game.Handlers
{
    public class CP_GET_SERVER_TIME : Networking.PacketHandler
    {
        public static void MakeServerTime(WRClient wc)
        {
            Core.OutPacket mPacket = new Core.OutPacket(24832);
            mPacket.AddBlock(1); //Error_OK

            DateTime now = DateTime.Now.ToUniversalTime();
            int month = now.Month - 1;
            int year = now.Year - 1900;

            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            mPacket.AddBlock(now.ToString(@"ss\/mm\/HH\/dd") + "/" + month + "/" + year + "/" + weekNum + "/" + now.DayOfYear + "/0");
            byte[] mBuffer = mPacket.GetOutput();
            for(int I = 0; I < mBuffer.Length; I++) {
                mBuffer[I] ^= Core.BuildConfig.GameKey_Server;
            }
            wc.ClientSocket.Send(mBuffer);
        }
        public override void Handle()
        {
            int mVersion = this.packet.GetInt(1);
            string mMacAddress = Core.DB.Tools.EscapeString(this.packet.GetString(2));

            if(mVersion == 3) {
                if(mMacAddress.Length == 12) {
                    MakeServerTime(this.client);
                }
            } 
        }
    }
}
