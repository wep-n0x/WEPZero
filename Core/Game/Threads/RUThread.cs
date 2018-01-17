using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Game.Threads
{
    /*====> Room Update Thread <====*/
    public class RUThread
    { 
        public static void MainThread()
        {
            while (true)
            {
                // Sleep
                Thread.Sleep(1000);
            }
        }
    }
}
