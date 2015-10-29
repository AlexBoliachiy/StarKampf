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
        protected MovingUnit() // Prevent ini any objects this class
        {
        }

        public void SetMoveDest(int x, int y)
        {
            this.Dx = x;
            this.Dy = y;
        }
        protected void move()
        {
            /* Тут ты должен сделать перемещение юнита из точки в которой он находится в точку 
            /  которую хранят Дх и Ду. Отправка этих значений со стороны клиента - забота моя.
            */
            /* Насчет карты:
            / Еще не задумывался над реализацией. В любом случае известно, что надо создать класс map 
            / на стороне сервера, который будет использоваться при перемещение. Карта будет содержать 
            / как минимум три объета: А) пустой спейс, Б) Препятствие (камень, дерево, етс.) В) вода
            */

        }
        public override void Act()
        {

        }

    }
}
