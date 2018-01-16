namespace Authentification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    class RunTime
    {
        private static void load_db()
        {

        }

        static void Main(string[] args)
        {
            Console.Title = "Warrock emulation project - auth server";
            Console.WriteLine("0.0.1 [auth-server]");
            Console.WriteLine("Hit <Ctrl+C> to exit");
            Console.WriteLine();

            load_db();

            /* Creating Listener Instance */
            Globals.GetInstance().ServerInstance = new Networking.WRServer("0.0.0.0", 5330);
            Globals.GetInstance().ServerInstance.LoadPacketTable();
            Globals.GetInstance().ServerInstance.Initialize();
            Globals.GetInstance().ServerInstance.BeginListening();

            Console.ReadKey();
        }
    }
}
