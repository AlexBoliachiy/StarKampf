using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Game2
{
    /* Обойдемся без выпендрежи
       сделаем один класс, в котором будут реализованы все функции.
       Это будет намного проще, ибо не придется париться с абстрактными методами,
       делать множество коллекций под строго типизированый класс юнитов.
    */
    class Unit
    {
        SpriteBatch sprite;
        private int health;
        private Vector2 position;
        private int name;
        public bool isDead;
        public char side;

        private Vector2 MoveDestination; // Where is going to
        private bool isMoving;
        public bool IsMoving { get { return isMoving; } }
       
        private Unit Target;
        private int Damage;

        private void SetTarget(ref Unit unit)
        { 
             
        }

        private void Attack()
        {

        }




        public void SetMoveDest()
        {

        }

        protected void Move()
        {

        }

        public int Name { get { return name; } }

        public void Draw()
        {

        }

        public Unit()
        {

        }

        public Unit(string name)
        {
            //Тут придется делеть запрос на базу данных в которо будут хранится данные о юните 
            // select * where NAME = name from UnitsBase

        }

    }


}
