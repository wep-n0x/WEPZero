using System;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;
using System.Collections;
namespace Game
{
    public class UdpServer
    { 
        IPEndPoint GroupEP5350;
        IPEndPoint GroupEP5351;

        public Hashtable usersUDP = new Hashtable(255, 1);

        public void StartUDPServer()
        {
            Thread RecvThread1 = new Thread(new ThreadStart(RecvUDP1));
            RecvThread1.Start();
            Thread RecvThread2 = new Thread(new ThreadStart(RecvUDP2));
            RecvThread2.Start();
            Core.Log.WritePlain("UDP", "Done!");
        }

        private void RecvUDP1() {
            byte[] btReceiveData;
            IPAddress GroupIP;
            GroupIP = IPAddress.Parse(Globals.GetInstance().Config.GetValue("IP"));
            GroupEP5350 = new IPEndPoint(GroupIP, 5350);
            UdpClient UDPSocket_1 = new UdpClient(5350);
            try {
                while (true) { 
                    try {
                        btReceiveData = UDPSocket_1.Receive(ref GroupEP5350);
                        byte[] Response = AnalyzePacket(UDPSocket_1, btReceiveData, GroupEP5350, 5350);
                        if (Response != null) { 
                            UDPSocket_1.BeginSend(Response, Response.Length, GroupEP5350, SendProc, UDPSocket_1);
                        }
                    } catch { }
                }
            } catch { } 
        }

        private void RecvUDP2() {
            byte[] btReceiveData;
            IPAddress GroupIP;
            GroupIP = IPAddress.Parse(Globals.GetInstance().Config.GetValue("IP"));
            GroupEP5351 = new IPEndPoint(GroupIP, 5351);
            UdpClient UDPSocket_2 = new UdpClient(5351);
            try {
                while (true) {
                    try {
                        btReceiveData = UDPSocket_2.Receive(ref GroupEP5351);
                        byte[] Response = AnalyzePacket(UDPSocket_2, btReceiveData, GroupEP5351, 5351); 
                        UDPSocket_2.BeginSend(Response, Response.Length, GroupEP5351, SendProc, UDPSocket_2);
                    } catch { }
                } 
            } catch { }
        }

        void SendProc(IAsyncResult t) {
            try {
                UdpClient a = (UdpClient)t.AsyncState;
                a.EndSend(t);
            } catch (Exception) { } 
        }

        private byte[] intToByteArray(int value) {
            return System.BitConverter.GetBytes(value);
        }

        private byte[] AnalyzePacket(UdpClient cl, byte[] RecvPacket, IPEndPoint IPeo, int port) {
            try {
                byte[] Response = new Byte[1] { 0x00 };
                if (RecvPacket.Length > 5) {
                    if (RecvPacket[0] == 0x10 && RecvPacket[1] == 0x01 && RecvPacket[2] == 0x01) { /* MAP REQUEST */
                        int id = (int)((RecvPacket[4]) << 8) | (RecvPacket[5]);

                        //need to check it later
                        WRClient c = Globals.GetInstance().ServerInstance.GetClient((ushort)id);
                        if (c is WRClient)
                        {
                            usersUDP[id] = IPeo;
                            c.RemoteIP = IPeo.Address.Address;
                            c.RemotePort = (int)BitConverter.ToUInt16(new byte[] { BitConverter.GetBytes(IPeo.Port)[1], BitConverter.GetBytes(IPeo.Port)[0] },0);
                        } 

                        Byte b;
                        if (port == 5350)  
                            b = 0xe6; 
                        else 
                            b = 0xe7; 

                        Response = new Byte[14] { 0x10, 0x01, 0x01, 0x00, 0x14, b, 0x00, 0x00, 0x00, 0x00,
                        RecvPacket[RecvPacket.Length - 4],
                        RecvPacket[RecvPacket.Length - 3],
                        RecvPacket[RecvPacket.Length - 2],
                        RecvPacket[RecvPacket.Length - 1] }; 
                    } else if (RecvPacket[0] == 0x10 && RecvPacket[1] == 0x10 && RecvPacket[2] == 0x00 && RecvPacket[3] == 0x00 && RecvPacket[14] == 0x21) {
                        String[] exc = IPeo.Address.ToString().Split('.');// remote ip
                        int b1 = ((byte)Int32.Parse(exc[0])) ^ Core.BuildConfig.GameKey_Server;
                        int b2 = ((byte)Int32.Parse(exc[1])) ^ Core.BuildConfig.GameKey_Server;
                        int b3 = ((byte)Int32.Parse(exc[2])) ^ Core.BuildConfig.GameKey_Server;
                        int b4 = ((byte)Int32.Parse(exc[3])) ^ Core.BuildConfig.GameKey_Server;

                        Response = new Byte[65]
                        {
                            0x10, 0x10, 0, 0, RecvPacket[4], RecvPacket[5],
                            0xff, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x21, 0x00, 0x00, 0x41,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x11, 0x13, 0x11,
                            (byte)(RecvPacket[32]^Core.BuildConfig.GameKey_Client), (byte)(RecvPacket[33]^Core.BuildConfig.GameKey_Client), (byte)b1, (byte)b2, (byte)b3, (byte)b4,
                            0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x01, 0x11, 0x13, 0x11,
                            (byte)(RecvPacket[32]^Core.BuildConfig.GameKey_Client), (byte)(RecvPacket[33]^Core.BuildConfig.GameKey_Client), (byte)(RecvPacket[34]^Core.BuildConfig.GameKey_Client), (byte)(RecvPacket[35]^Core.BuildConfig.GameKey_Client), (byte)(RecvPacket[36]^Core.BuildConfig.GameKey_Client), (byte)(RecvPacket[37]^Core.BuildConfig.GameKey_Client),
                            0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x11
                        };

                        String lip = (int)(RecvPacket[34] ^ Core.BuildConfig.GameKey_Client) + "." + (int)(RecvPacket[35] ^ Core.BuildConfig.GameKey_Client) + "." + (int)(RecvPacket[36] ^ Core.BuildConfig.GameKey_Client) + "." + (int)(RecvPacket[37] ^ Core.BuildConfig.GameKey_Client); 
                        long lport = ((RecvPacket[33] ^ Core.BuildConfig.GameKey_Client) << 8) | (RecvPacket[32] ^ Core.BuildConfig.GameKey_Client);

                        int id = (int)((RecvPacket[4]) << 8) | (RecvPacket[5]);
                        WRClient c = Globals.GetInstance().ServerInstance.GetClient((ushort)id);
                        if (c is WRClient)
                        {
                            usersUDP[id] = IPeo;
                            c.RemoteIP = IPeo.Address.Address;
                            c.RemotePort = (int)BitConverter.ToUInt16(new byte[] { BitConverter.GetBytes(IPeo.Port)[1], BitConverter.GetBytes(IPeo.Port)[0] }, 0);
                            c.LocalIP = IPAddress.Parse(lip).Address;
                            c.LocalPort = (int)BitConverter.ToUInt16(new byte[] { BitConverter.GetBytes(lport)[1], BitConverter.GetBytes(lport)[0] }, 0);
                        } 
                    } else if (RecvPacket[0] == 0x10 && RecvPacket[1] == 0x10 && RecvPacket[2] == 0x00 && RecvPacket[3] == 0x00 && RecvPacket[14] == 0x31) { 
                        int id = (int)((RecvPacket[22]) << 8) | (RecvPacket[23]); 
                        cl.BeginSend(RecvPacket, RecvPacket.Length, (IPEndPoint)usersUDP[id], SendProc, (object)cl);
                        return null;
                    }
                }
                return Response;
            }
            catch (Exception ex)
            { 
                return new Byte[1] { 0x00 };
            }
        }
    }
}
