using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarKampf_server
{
    class Fighter : MovingUnit
    {
        public Fighter(int ID, int x, int y, int side, string name, int MaxHealth, int speed, int damage, int countdown,int IN)
        {
            this.name = name;
            this.ID = ID;
            this.Speed = speed;
            this.x = x;
            this.y = y;
            this.damage = damage;
            this.Countdown = countdown;
            this.side = side;
            this.MaxHealth = MaxHealth;
            health = MaxHealth;
            this.IN = IN;
            isDead = false;
        }
        private int damage;
        private int Countdown;
        public override string GetUnitProperties
        {
            get
            {
                return ID.ToString() + " " + x.ToString() + " " + y.ToString() + " "
                       + angle.ToString() + " " + side.ToString() + " " + IN.ToString() + "\n";
            }
        }
    }
}
