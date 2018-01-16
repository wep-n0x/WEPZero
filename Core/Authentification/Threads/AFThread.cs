using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentification.Threads
{
    /*====> Anti Freeze Thread <====*/
    public class AFThread {
        public static DateTime StartTime;

        public static void MainThread() {
            while(true) {
                string sInput = Console.ReadLine();
                string[] sArgs = sInput.Split(' ');
                switch (sArgs[0])
                {
                    case "exit":
                        Environment.Exit(0);
                        break;
                    case "quit":
                        Environment.Exit(0);
                        break;
                    case "close":
                        Environment.Exit(0);
                        break;

                    case "info":
                        Core.Log.WriteDebug("Core running since "+ ((TimeSpan)(DateTime.Now - StartTime)).TotalMilliseconds+ "ms");
                        Core.Log.WriteDebug("Sessions overall: " + Globals.GetInstance().ServerInstance.overallSessions);
                        Core.Log.WriteDebug("Server Version: " + Core.BuildConfig.Rev + "(" + Core.BuildConfig.LastUpdate + ")");
                        break;
                }
            }
        }
    }
}
