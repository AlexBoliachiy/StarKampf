using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    class MovingUnit : BaseUnit
    {
        protected int Speed;
        protected double Dx, Dy; // Относительные координаты движения
        protected double Nx, Ny; // Нормализованый вектор направления
        protected bool isMoving;
        public bool IsMoving { get { return isMoving; } }
        protected bool isRotating;
        public bool IsRotating { get { return isRotating; } }


        protected MovingUnit() // Prevent ini any objects this class
        {
        }

        protected float Omega { get { return this.Speed / 200.0f; } }



        public override void SetMoveDest(int x, int y)
        {
            this.Dx = x - this.x;// Берем относительные координаты
            this.Dy = y - this.y;//

            this.Nx = Dx / Math.Sqrt((Math.Pow(Dx, 2) + Math.Pow(Dy, 2)));
            this.Ny = Dy / Math.Sqrt((Math.Pow(Dx, 2) + Math.Pow(Dy, 2)));
            rotateAngle = (float)Math.Atan2(Dy, Dx) - angle;

            if (Math.Abs(rotateAngle) > Math.PI) rotateAngle = Math.Sign(-rotateAngle)*((float)Math.PI * 2 - Math.Abs(rotateAngle));

            isMoving = true;
            if (rotateAngle != 0.0f) isRotating = true;// Если смотрим в направлении движения - не разворачиваемся
        }

        protected void rotate(double Interval)
        {
            if (isRotating == false) return;

            if (Math.Abs(rotateAngle) > (float)Interval * Omega)
            {
                angle += Math.Sign(rotateAngle) * (float)Interval * Omega;
                rotateAngle -= Math.Sign(rotateAngle) * (float)Interval * Omega;
                if (Math.Abs(angle) >= Math.PI) angle = Math.Sign(-angle) * ((float)Math.PI * 2 - angle);
                return;
            }
            else
            {
                angle += rotateAngle;
                rotateAngle = 0;
                isRotating = false;
                return;
            }
        }


        protected void move(double Interval)
        {
            /* Тут ты должен сделать перемещение юнита из точки в которой он находится в точку 
            /  которую хранят Дх и Ду. Отправка этих значений со стороны клиента - забота моя.
            */
            /* Насчет карты:
            / Еще не задумывался над реализацией. В любом случае известно, что надо создать класс map 
            / на стороне сервера, который будет использоваться при перемещение. Карта будет содержать 
            / как минимум три объета: А) пустой спейс, Б) Препятствие (камень, дерево, етс.) В) вода
            */

            if (isMoving == false) return; // Не собрались двигаться - прочь из метода

            if (IsRotating == true) // Разворот в сторону движения перед самим движением
            {
                rotate(Interval);
                return;
            }

            double Distance = Math.Pow(Dx, 2) + Math.Pow(Dy, 2); // Дистанция к точке назначеня

            if (Distance >= this.Length(Interval)) // Если дистанция больше атрибута двигаемся на полную длинну
            {
                this.x += Nx * Speed * Interval;
                this.y += Ny * Speed * Interval;
                Dx -= Nx * Speed * Interval;
                Dy -= Ny * Speed * Interval;
                return;
            }

            else // если нет - просто прыгаем в точку назанчения
            {
                this.x += Dx;
                this.y += Dy;
                Dx = 0;
                Dy = 0;
                isMoving = false;
                return;
            }
        }

        private double Length(double Interval) // На какое расстояние продвинемся за текущий цикл
        {
            return (Math.Pow((int)(Nx * Speed * Interval), 2) + Math.Pow((int)(Ny * Speed * Interval), 2)); 
        }

        public override void Act(double Interval)
        {
            move(Interval);
        }

    }
}
