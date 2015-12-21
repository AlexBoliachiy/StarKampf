using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
namespace Game2
{
    class Support:MovingUnit
    {
        private int BuildID;
        public static Dictionary<int, Dictionary<int, int>> CreatingUnits;
        public bool IsBuild = false;
        public bool IsGoingToBuild = false;
        private Stopwatch timer;

        public Support(int ID, int x, int y, int side, int IN, string name, int MaxHealth, int speed)
        { 
            this.name = name;
            this.ID = ID;
            this.Speed = speed;
            this.x = x;
            this.y = y;
            this.side = side;
            this._MaxHealth = MaxHealth;
            this.angle = 0;
            health = MaxHealth;
            this.IN = IN;
            isDead = false;
            timer = new Stopwatch();
        }

        static Support()
        {
            string str = System.IO.File.ReadAllText("Units//builderb.txt");
            CreatingUnits = new Dictionary<int, Dictionary<int, int>>();
            string[] arr2 = str.Split(new char[] { '|', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int[] arr = new int[arr2.Count()];
            for (int i = 0; i < arr2.Count(); i++)
            {
                arr[i] = int.Parse(arr2[i]);
            }

            for (int i = 0; i != arr.Count(); i += 3)
            {
                if (!CreatingUnits.ContainsKey(arr[i]))
                {
                    CreatingUnits.Add(arr[i], new Dictionary<int, int>());
                    CreatingUnits[arr[i]].Add(arr[i + 1], arr[i + 2]);
                }
                else
                    CreatingUnits[arr[i]].Add(arr[i + 1], arr[i + 2]);
            }
        }


        public override void SetMoveDest(int x, int y)
        {
            base.SetMoveDest(x, y);
        }


        public void SetBuild(int ID, int _x, int _y)
        {
            SetMoveDest(_x, _y);
            BuildID = ID;
            IsBuild = true;
            IsGoingToBuild = true;
    }

        private void Build()
        {
            if (BuildID == -1)
            {
                return;
            }

            else
            {
                if (timer.ElapsedMilliseconds / 1000 > CreatingUnits[this.ID][BuildID]) // Если время постройки вышло отправляем запрос на инициализацию
                {
                    conMan.SendMsgIniUnit(BuildID, (int)x, (int)y);
                    timer.Reset();
                    BuildID = -1;
                    IsBuild = false;
                }
            }

        }
        public override void Act(double Interval)
        {

            if (IsBuild && IsGoingToBuild && IsMoving) // Если топает к месту строительства
                base.Act(Interval);
            else if (IsBuild && IsGoingToBuild && !IsMoving) // если только притопал
            {
                IsGoingToBuild = false;
                timer.Start();
            }
            else if (IsBuild && !IsGoingToBuild && !IsMoving)  //Если строит и не двигается
            {
                Build();
            }
            else if (IsBuild && !IsGoingToBuild && IsMoving) // Если строил но свалил
            {
                BuildID = -1;
                timer.Reset();
                IsBuild = false;
            }
            else if (IsMoving)
                base.Act(Interval);
        }


    }
}
