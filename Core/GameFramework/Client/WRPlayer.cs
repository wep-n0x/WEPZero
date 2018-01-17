using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.Client
{
    public class WRPlayer
    {
        public int Kills = 0, Deaths = 0, Flags = 0, Point = 0;
        public int Health = 0;
        public bool IsLiving = false;
        public bool IsReady = false;

        public int WeaponID = 0;
        public Classes Class = Classes.Engineer;
        public Team TeamID = Team.None;
        
        public void Reset()
        {
            Health = 1000;
            IsLiving = IsReady = false;

            this.Kills = this.Deaths = this.Flags = this.Point = 0;

            WeaponID = -1;
            Class = Classes.Engineer;
            TeamID = Team.None;
        }

        #region Game Functions
        public void TakeDamage(int _amount) {
            if (IsLiving == false)
                return;

            int HPResult = this.Health - _amount;
            if (HPResult <= 0)
                HPResult = 0;
            this.Health = HPResult;
        }

        public void Heal(int _amount) {
            if (IsLiving == false)
                return;

            int HPResult = this.Health + _amount;
            if (HPResult >= 1000)
                HPResult = 1000;
            this.Health = HPResult;
        }
        
        public void OnDeath() {
            if (IsLiving == false)
                return;

            this.Health = 0;
            this.IsLiving = false;
            this.Deaths++;
            this.Point += 2; //Double Point Rate
        }

        public void OnKill() {
            if(IsLiving) {
                this.Kills++;
                this.Point += 7;
            }
        }

        public void OnSpawn() {
            if(IsLiving == false)  {
                this.IsLiving = true;
                this.Heal(1000);
                this.WeaponID = 0;
            }
        }
        #endregion 
    }
}
