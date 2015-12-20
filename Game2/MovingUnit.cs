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
        protected double DestX, DestY;
        protected bool isMoving;
        public bool IsMoving { get { return isMoving; } }
        protected bool isRotating;
        public bool IsRotating { get { return isRotating; } }
        private List<Point> pointList;
        private int wayPoint;
        protected MovingUnit() // Prevent ini any objects this class
        {
        }

        protected float Omega { get { return this.Speed / 200.0f; } }

        protected bool isCollides() // поиск колизий
        {
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

        protected void unitCollision(List<BaseUnit> vecUnits)
        {

        }

        public override void SetMoveDest(int x, int y)
        {
            this.DestX = x;
            this.DestY = y;
            Point start = new Point((int)this.x / (map.tileWidth), (int)this.y / (map.tileHeight));
            Point stop = new Point((int)DestX / (map.tileWidth), (int)DestY / (map.tileHeight));

            pointList = Anti_Aliasing(AStar.FindPath(start, stop, map));

            wayPoint = 0;
            isMoving = true;
            nextWayPoint(map);
            (start - stop).ToVector2().LengthSquared();
        }


        protected void nextWayPoint(Map map)
        {
            this.Dx = pointList[wayPoint].X - this.x;// Берем относительные координаты
            this.Dy = pointList[wayPoint].Y - this.y;//         
            this.Nx = Dx / Math.Sqrt((Math.Pow(Dx, 2) + Math.Pow(Dy, 2)));
            this.Ny = Dy / Math.Sqrt((Math.Pow(Dx, 2) + Math.Pow(Dy, 2)));
            rotateAngle = (float)Math.Atan2(Dy, Dx) - angle;

            if (Math.Abs(rotateAngle) > Math.PI) rotateAngle = Math.Sign(-rotateAngle) * ((float)Math.PI * 2 - Math.Abs(rotateAngle));
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
                if (isCollides())// обработка кол
                {
                    this.x -= Nx * Speed * Interval;
                    this.y -= Ny * Speed * Interval;
                    Dx += Nx * Speed * Interval;
                    Dy += Ny * Speed * Interval;
                }
            }
            else // если нет - просто прыгаем в точку назанчения
            {
                this.x += Dx;
                this.y += Dy;
                Dx = 0;
                Dy = 0;
                wayPoint++;
                if (wayPoint == pointList.Count || pointList.Count == 0)
                {
                    isMoving = false;
                }
                else
                {
                    nextWayPoint(map);
                }
            }
            return;
        }

        private double Length(double Interval) // На какое расстояние продвинемся за текущий цикл
        {
            return (Math.Pow((int)(Nx * Speed * Interval), 2) + Math.Pow((int)(Ny * Speed * Interval), 2));
        }

        public override void Act(double Interval)
        {
            move(Interval);
        }

        private List<Point> Anti_Aliasing(List<Node> list)
        {
            List<Node> result = new List<Node>();
            List<Point> result1 = new List<Point>();
            int SignX = 0;
            int SignY = 0;
            if (list != null)
            {
                result.Add(list[0]);
                for (int i = 2; i < list.Count; i++)
                {
                    if (!lineOfSight(result[result.Count - 1].X, result[result.Count - 1].Y, list[i].X, list[i].Y))
                    {
                        if (list[i - 1].X - result[result.Count - 1].X != 0)
                        {
                            SignY = Math.Sign(list[i - 1].X - result[result.Count - 1].X) * (int)(map.tileWidth / 2.8f);
                        }
                        else
                        {
                            if (result1.Count == 0)
                            {
                                SignY = -Math.Sign(list[i].X - result[result.Count - 1].X) * (int)(map.tileWidth / 2.8f);
                            }
                        }
                        if (list[i - 1].Y - result[result.Count - 1].Y != 0)
                        {
                            SignX = Math.Sign(list[i - 1].Y - result[result.Count - 1].Y) * (int)(map.tileWidth / 2.8f);
                        }
                        else
                        {
                            if (result1.Count == 0)
                            {
                                SignX = -Math.Sign(list[i].Y - result[result.Count - 1].Y) * (int)(map.tileWidth / 2.8f);
                            }
                        }
                        result1.Add(new Point(list[i - 1].X * map.tileWidth - SignY + map.tileWidth / 2
                            , list[i - 1].Y * map.tileHeight - SignX + map.tileHeight / 2));
                        result.Add(list[i - 1]);
                    }
                }
            }
            result1.Add(new Point((int)DestX, (int)DestY));
            return result1;
        }

        bool lineOfSight(int x1, int y1, int x2, int y2) // cheks if we see the target (we cant see through walls)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;

            int f = 0;
            int signY = 1;
            int signX = 1;
            int offsetX = 0;
            int offsetY = 0;
            if (dx == 0)
            {
                signX = 0;
            }
            if (dy == 0)
            {
                signY = 0;
            }
            if (dx < 0)
            {
                dx *= -1;
                signX = -1;
                offsetX = -1;
            }
            if (dy < 0)
            {
                dy *= -1;
                signY = -1;
                offsetY = -1;
            }

            if (dx >= dy)
            {
                while (x1 != x2)
                {
                    if (map[x1 + signX, y1 + signY] == 1) return false;
                    if (map[x1 + signX, y1] == 1) return false;
                    if (map[x1, y1 + signY] == 1) return false;
                    f += dy;
                    if (f >= dx)
                    {
                        if (map[x1 + offsetX, y1 + offsetY] == 1) return false;
                        y1 += signY;
                        f -= dx;
                    }
                    if (f != 0 && map[x1 + offsetX, y1 + offsetY] == 1) return false;
                    //if (dy == 0 && map[x1 + offsetX, y1] == 1 && map[x1 + offsetX, y1 - 1] == 1) return false; // not our case of map
                    if (dy == 0 && map[x1 + offsetX, y1] == 1) return false;
                    x1 += signX;
                }
            }
            else
            {
                while (y1 != y2)
                {
                    if (map[x1 + signX, y1 + signY] == 1) return false;
                    if (map[x1, y1 + signY] == 1) return false;
                    if (map[x1 + signX, y1] == 1) return false;
                    f += dx;
                    if (f >= dy)
                    {
                        if (map[x1 + offsetX, y1 + offsetY] == 1) return false;
                        x1 += signX;
                        f -= dy;
                    }
                    if (f != 0 && map[x1 + offsetX, y1 + offsetY] == 1) return false;
                    //if (dx == 0 && map[x1, y1 + offsetY] == 1 && map[x1 - 1, y1 + offsetY] == 1) return false; // not our case of map
                    if (dx == 0 && map[x1, y1 + offsetY] == 1) return false;
                    y1 += signY;
                }
            }
            return true;
        }

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

    }
}
