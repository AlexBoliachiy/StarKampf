using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarKampf_server
{
    /*

    */
    class BaseUnit
    {
        protected int health;
        protected int MaxHealth;
        protected double x, y;
        protected string name;
        public bool isDead;
        public int side;
        protected int ID;

        protected void SetTarget(BaseUnit unit)
        { 
             
        } 

        public string Name { get { return name; } }

        public void Draw()
        {

        }

        public BaseUnit()
        {

        }

        public virtual void Act()
        {

        }
        public virtual string GetUnitProperties { get; }
    }


}
