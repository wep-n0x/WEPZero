namespace Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;


    class RunTime
    {
        private static void load_db()
        {
            Core.Log.WritePlain("MySQL", "Connecting to database... (" + Globals.GetInstance().Config.GetValue("SQL_HOSTNAME") + ":3306)");
            Globals.GetInstance().GameDatabase = new Core.DB.Instance();
            Globals.GetInstance().GameDatabase.SetCredentials(Globals.GetInstance().Config.GetValue("SQL_HOSTNAME"), Globals.GetInstance().Config.GetValue("SQL_USERNAME"), Globals.GetInstance().Config.GetValue("SQL_PASSWORD"), Globals.GetInstance().Config.GetValue("SQL_DATABASE"));

            if (!Globals.GetInstance().GameDatabase.TestConnection())
            {
                Core.Log.WriteError("Couldn't connect to database!");
                Console.ReadKey();
                Environment.Exit(0);
            }

            Core.Log.WritePlain("MySQL", "Done!");
            Console.WriteLine();

            MySql.Data.MySqlClient.MySqlConnection mConnection = Globals.GetInstance().GameDatabase.CreateConnection();
            MySql.Data.MySqlClient.MySqlCommand mCommand = new MySql.Data.MySqlClient.MySqlCommand("UPDATE accounts SET online=0 WHERE 1;", mConnection);
            mCommand.ExecuteNonQuery();
            mConnection.Close(); 
        }

        static void Main(string[] args)
        {
            Threads.AFThread.StartTime = DateTime.Now;
            Console.Title = "Warrock emulation project - game server";
            Console.WriteLine(Core.BuildConfig.Rev + " [game-server]");
            Console.WriteLine("Hit <Ctrl+C> to exit");
            Console.WriteLine();

            Globals.GetInstance().Config = new Core.IO.ConfigReader();
            Core.Log.WritePlain("CONFIG", "Reading config file... (" + Core.BuildConfig.Game_ConfigFile + ")");
            Globals.GetInstance().Config.ReadFile(Core.BuildConfig.Game_ConfigFile);
            Core.Log.WritePlain("CONFIG", "Done!");
            Console.WriteLine();

            load_db();

            Core.Log.WritePlain("ITEMSHOP", "Reading items file... (data/items.txt)");
            Globals.GetInstance().ShopInstance = new ItemShop();
            Globals.GetInstance().ShopInstance.ReadItemsFile();
            Console.WriteLine();
             
            Core.Log.WritePlain("UDP", "Starting udp sockets...");
            Globals.GetInstance().UdpInstance = new UdpServer();
            Globals.GetInstance().UdpInstance.StartUDPServer();
            Console.WriteLine(); 

            /* Creating Listener Instance */
            Globals.GetInstance().ServerInstance = new Networking.WRServer(Globals.GetInstance().Config.GetValue("IP"), Convert.ToUInt16(Globals.GetInstance().Config.GetValue("Port")));
            Globals.GetInstance().ServerInstance.LoadPacketTable();
            Globals.GetInstance().ServerInstance.Initialize();
            Globals.GetInstance().ServerInstance.BeginListening();
            Console.WriteLine();
             

            Core.Log.WritePlain("CORE", "Emulator started in " + ((TimeSpan)(DateTime.Now - Threads.AFThread.StartTime)).TotalMilliseconds + "ms");
            Console.WriteLine();

            /* Create room update thread */
            Thread RoomUpdateThread = new Thread(new ThreadStart(Threads.RUThread.MainThread));
            RoomUpdateThread.SetApartmentState(ApartmentState.STA);
            RoomUpdateThread.Priority = ThreadPriority.AboveNormal;
            RoomUpdateThread.Start();

            /* Prevent application from closing */
            Thread AntiFreezeThread = new Thread(new ThreadStart(Threads.AFThread.MainThread));
            AntiFreezeThread.Start();
        }
    }
}
