using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2
{
    /*

    */
    class BaseUnit
    {
        protected static Map map;
        private int textureWight = 75, textureHeight = 50;
        public Rectangle unitBound { get { return new Rectangle((int)this.X - textureWight / 2, (int)this.Y - textureHeight / 2, textureWight, textureHeight); ;} } // unit bounding box, used for detecting collision
        protected int health;
        public int MaxHealth { get { return _MaxHealth; } }
        protected int _MaxHealth;
        protected double x, y;
        public double X { get { return x; } }
        public double Y { get { return y; } }
        protected string name;
        public bool isDead;
        public int side;
        protected int ID;
        public int id { get { return ID; } }
        protected int IN;
        public int GN { get { return IN; } } // global nubmber
        protected float angle; // Угол в радианах
        protected float rotateAngle; // Угол поворота в радианах
        public float Angle { get { return angle; } }

        public static void InitializeMap(Map _map)
        {
            map = _map;
        }

        protected void SetTarget(BaseUnit unit)
        { 
             
        }

        public void takeDamage(int damage)
        {
            this.health -= damage;
        }

        public string Name { get { return name; } }
        public int Health { get { return health; } }

        public void Draw()
        {

        }

        public BaseUnit()
        {

        }

        public virtual void Act(double Interval)
        {

        }

        public virtual string GetUnitProperties { get { return string.Empty; } set { ;} }

        public virtual void SetMoveDest(int x, int y)
        {

        }
        public virtual void SetBuild(int ID)
        {

        }

        public virtual string Attack()
        {
            return null;
        }


    }//private constructor


}
