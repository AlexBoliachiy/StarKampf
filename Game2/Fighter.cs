using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
//Осторожно, некоторые строки в сервере и в клиенте отличаются
namespace Game2
{
    class Fighter : MovingUnit
    {
        public Fighter(int ID, int x, int y, int side, int IN, string name, int MaxHealth, int speed, int damage, int countdown)
        {
            this.name = name;
            this.ID = ID;
            this.Speed = speed;
            this.x = x; 
            this.y = y;
            this.damage = damage;
            this.Countdown = countdown;
            this.side = side;
            this._MaxHealth = MaxHealth;
            this.angle = 0;
            health = MaxHealth;
            this.IN = IN;
            isDead = false;
        }
        private int damage;
        private int Countdown;
        private bool isAttacking;
        private Stopwatch sw;
        public override string GetUnitProperties
        {
            get
            {
                return ID.ToString() + " " + x.ToString() + " " + y.ToString() + " "
                       + side.ToString() + " " + IN.ToString() + "\n";
            }
        }
        public override void Act(double Interval)
        {
            base.Act(Interval);
        }

        public override string Attack()
        {
            isAttacking = true;
            this.isMoving = false;

            rotateAngle = (float)Math.Atan2(Dy, Dx) - angle;

            return null;
        }

    }
}
