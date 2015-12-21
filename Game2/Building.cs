using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


namespace Game2
{
    class Building:BaseUnit
    {

        private List<int> QueueOfBuildUnits;
        private Stopwatch timeOfBuildCurUnit;
        private int FullTime;
        private bool IsBuild;
        private int BuildID;
        private static List<BaseUnit> VecUnits;

        public static Dictionary<int, Dictionary<int, int>> CreatingUnits;

        private int FlagX; // Point, where unit would be located after creating 
        private int FlagY;
        public static void Initialize(ref List<BaseUnit> _VecUnits)
        {
            VecUnits = _VecUnits;
        }

        public override void SetBuild(int ID)
        {
            QueueOfBuildUnits.Add(ID);
        }
        public Building(int ID, int x, int y, int side, int IN, int MaxHealth)
        {
            timeOfBuildCurUnit = new Stopwatch();
            QueueOfBuildUnits = new List<int>();
            this.ID = ID;
            this.IN = IN;
            this._MaxHealth = MaxHealth;
            this.health = MaxHealth;
            this.x = x;
            this.y = y;
            this.FlagX = (int)X - 100;
            this.FlagY = (int)Y + 100;
            BuildID = -1;


        }

        static Building()
        {
            string str = System.IO.File.ReadAllText("Units//centrb.txt");
            CreatingUnits = new Dictionary<int, Dictionary<int, int>>();
            string[] arr2 = str.Split(new char[] { '|', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int[] arr = new int[arr2.Count()];
            for (int i=0; i < arr2.Count(); i++)
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


                //CreatingUnits[arr[i]][arr[i + 1]] = arr[i + 2];
            }

        }

        private void Build()
        {
            
            if (BuildID == -1) 
            {
                if (QueueOfBuildUnits.Count > 0)
                {
                    BuildID = QueueOfBuildUnits.First();
                    QueueOfBuildUnits.RemoveAt(0);
                    FullTime = CreatingUnits[this.ID][BuildID];
                    timeOfBuildCurUnit.Start();
                }
            }

            else
            {
                if (timeOfBuildCurUnit.ElapsedMilliseconds/1000 > FullTime) // Если время постройки вышло отправляем запрос на инициализацию
                {
                    conMan.SendMsgIniUnit(BuildID, FlagX, FlagY);
                    timeOfBuildCurUnit.Reset();
                    BuildID = -1;  
                }
            }
            
        }

        public Building(int ID, int x, int y, int side, int IN, string name, int MaxHealth)
        {
            this.ID = ID;
            this.x = x;
            this.y = y;
            this.side = side;
            this.IN = IN;
            this.name = name;
            this._MaxHealth = MaxHealth;
            this.health = MaxHealth;

            this.BuildID = -1;
        }

        public override void Act(double Interval)
        {
            Build();
        }
    }
}
