namespace Authentification
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

        }

        static void Main(string[] args)
        {
            Threads.AFThread.StartTime = DateTime.Now;
            Console.Title = "Warrock emulation project - auth server";
            Console.WriteLine(Core.BuildConfig.Rev + " [auth-server]");
            Console.WriteLine("Hit <Ctrl+C> to exit");
            Console.WriteLine();

            Globals.GetInstance().Config = new Core.IO.ConfigReader();
            Core.Log.WritePlain("CONFIG", "Reading config file... (auth.ini)");
            Globals.GetInstance().Config.ReadFile("auth.ini");
            Core.Log.WritePlain("CONFIG", "Done!");
            Console.WriteLine();
             
            load_db();

            /* Creating Listener Instance */
            Globals.GetInstance().ServerInstance = new Networking.WRServer(Globals.GetInstance().Config.GetValue("IP"), Convert.ToUInt16(Globals.GetInstance().Config.GetValue("Port")));
            Globals.GetInstance().ServerInstance.LoadPacketTable();
            Globals.GetInstance().ServerInstance.Initialize();
            Globals.GetInstance().ServerInstance.BeginListening();
            Console.WriteLine();
            Core.Log.WritePlain("WEP", "WEP started in " + ((TimeSpan)(DateTime.Now - Threads.AFThread.StartTime)).TotalMilliseconds + "ms");
            Console.WriteLine();

            /* Prevent application from closing */
            Thread AntiFreezeThread = new Thread(new ThreadStart(Threads.AFThread.MainThread));
            AntiFreezeThread.Start();
        }
    }
}
