using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace GameFramework.Tools
{
    public class LevelManager
    {
        private string[] expTable = new string[] { };

        public void LoadLevelFile() {
            try {
                expTable = File.ReadAllLines(Environment.CurrentDirectory + "\\data\\level.txt"); 
            } catch { }
        }

        public int GetExpForLevel(int Level) { return Convert.ToInt32(expTable[Level]); }

        public int GetLevelForExp(int Exp) {
            for(int I = 0; I < expTable.Length; I++) {
                if (this.GetExpForLevel(I + 1) > Exp && GetExpForLevel(I) <= Exp)
                    return I;
            }
            return 0;
        }
    }
}
