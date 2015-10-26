using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarKampf_server
{
    class MovingUnit : BaseUnit
    {
        protected int Speed;
        protected double Dx, Dy; // Where is going to, moving destination coords//
        protected bool isMoving;
        public bool IsMoving { get { return isMoving; } }
        protected int angle;


        public void SetMoveDest()
        {

        }
        protected void move()
        {

        }
        public override void Act()
        {

        }

    }
}
