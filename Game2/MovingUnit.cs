using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private Rectangle unitBound; // unit bounding box, used for detecting collision
        private int textureWight = 125 , textureHeight = 63;
        protected MovingUnit() // Prevent ini any objects this class
        {
        }

        protected float Omega { get { return this.Speed / 200.0f; } }

        protected bool IntersectsPixel(Rectangle rect1, Color[] data1, // pixel-precise intersects alg. For the future
                                       Rectangle rect2, Color[] data2)
        {
            int top = Math.Max(rect1.Top, rect2.Top);
            int bottom = Math.Min(rect1.Bottom, rect2.Bottom);
            int left = Math.Max(rect1.Left, rect2.Left);
            int right = Math.Min(rect1.Right, rect2.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color colour1 = data1[(x - rect1.Left) +
                                            (y - rect1.Top * rect1.Width)];

                    Color colour2 = data2[(x - rect2.Left) +
                                            (y - rect2.Top * rect1.Width)];

                    if (colour1.A != 0 && colour2.A != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        protected bool isCollides(Map map) // поиск колизий
        {
            unitBound = new Rectangle((int)this.X - textureWight/2, (int)this.Y - textureHeight/2, textureWight, textureHeight);
            for (int i = 0; i < map.width; i++)
            {
                for (int j = 0; j < map.height; j++)
                {
                    if (map[i, j] == 1)
                    {
                        Rectangle tmp = new Rectangle(i * map.tileWidth, j * map.tileHeight, map.tileWidth, map.tileHeight);
                        if (unitBound.Intersects(tmp))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override void SetMoveDest(int x, int y, Map map)
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


        protected void move(double Interval, Map map)
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
                if (isCollides(map))// обработка кол
                {
                    this.x -= Nx * Speed * Interval;
                    this.y -= Ny * Speed * Interval;
                    Dx = 0;
                    Dy = 0;
                }
            }
            else // если нет - просто прыгаем в точку назанчения
            {
                this.x += Dx;
                this.y += Dy;
                Dx = 0;
                Dy = 0;
                isMoving = false;
            }
            return;
        }

        private double Length(double Interval) // На какое расстояние продвинемся за текущий цикл
        {
            return (Math.Pow((int)(Nx * Speed * Interval), 2) + Math.Pow((int)(Ny * Speed * Interval), 2)); 
        }

        public override void Act(double Interval, Map map)
        {
            move(Interval, map);
        }

    }
}
