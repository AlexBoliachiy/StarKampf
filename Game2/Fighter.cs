﻿using System;
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
        public Fighter(int ID, int x, int y, int side, int IN, string name, int MaxHealth, int speed, int damage, int countdown, int radius)
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
            sw = new Stopwatch();
            this.radius = radius;
        }
        private int radius;
        private int damage;
        private double Countdown;
        public bool isAttacking;
        private Stopwatch sw;
        private BaseUnit target;
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
            if (isAttacking) Attack();
        }

        public void setTarget(BaseUnit target)
        {
            this.target = target;
            isAttacking = true;
        }

        public string Attack()
        {
            if (lineOfSight((int)x / 256, (int)y / 256, (int)target.X / 256, (int)target.Y / 256) && radius > Math.Sqrt(Math.Pow((int)target.X - x, 2) + Math.Pow((int)target.Y - y, 2))) // проверяем 
            {// видим ли мы противника
                isMoving = false;
                Dx = (int)target.X - x;
                Dy = (int)target.Y - y;
                if ((float)Math.Atan2(Dy, Dx) - angle != 0)
                {
                    rotateAngle = (float)Math.Atan2(Dy, Dx) - angle;
                    if (Math.Abs(rotateAngle) > Math.PI) rotateAngle = Math.Sign(-rotateAngle) * ((float)Math.PI * 2 - Math.Abs(rotateAngle));
                    isRotating = true;
                    rotate(0.5f);
                }
                else
                {
                    if (sw.Elapsed.TotalSeconds > Countdown || sw.ElapsedMilliseconds == 0)
                    {
                        
                        target.health -= damage;
                        sw.Restart();
                    }
                }
            }
            else
            {
                if (isMoving == false)
                {
                    SetMoveDest((int)target.X, (int)target.Y);
                }
            }
            return null;
        }
    }
}
