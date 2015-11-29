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
        public int side; // На чьей стороне воюет
        protected int ID; // этим полем определяется тип юнита, (танк, центр)
        protected int IN; // Идентификационый номер отдельного юнита
        public int GN { get { return IN; } } // global nubmber
        public string Name { get { return name; } }
        protected float angle; // Угол в радианах
        protected float rotateAngle; // Угол поворота в радианах
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
    }//private constructor


}
