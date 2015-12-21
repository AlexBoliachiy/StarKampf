using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
namespace StarKampf_server
{
    class Support:MovingUnit
    {
        

        public Support(int ID, int x, int y, int side, int IN, string name, int MaxHealth, int speed)
        { 
            this.name = name;
            this.ID = ID;
            this.Speed = speed;
            this.x = x;
            this.y = y;
            this.side = side;
            this.MaxHealth = MaxHealth;
            this.angle = 0;
            health = MaxHealth;
            this.IN = IN;
            isDead = false;
        }



        public override string GetUnitProperties
        {
            get
            {
                return ID.ToString() + " " + x.ToString() + " " + y.ToString() + " "
                        + side.ToString() + " " + IN.ToString() + " " + "\n";
            }
        }

        public override void SetMoveDest(int x, int y)
        {
            base.SetMoveDest(x, y);
        }


        public override void Act(double Interval)
        {

            base.Act(Interval);
        }
        


    }
}
