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

        private static Dictionary<int, Dictionary<int, int>> CreatingUnits;

        private int FlagX; // Point, where unit would be located after creating 
        private int FlagY;

        public override void SetBuild(int ID)
        {
            QueueOfBuildUnits.Add(ID);
        }

        static Building()
        {

            int[] arr = System.IO.File.ReadAllText("Units//timeofbuilding.txt").Split(' ').Select(n => int.Parse(n)).ToArray();
            for (int i = 0; i != arr.Count()-1; i += 3)
            {
                CreatingUnits[arr[i]][arr[i + 1]] = arr[i + 2];
            }

        }

        private void Build(List<BaseUnit> VecUnits)
        {
            
            if (BuildID == -1) 
            {
                if (QueueOfBuildUnits.Count > 0)
                {
                    BuildID = QueueOfBuildUnits.First();
                    QueueOfBuildUnits.RemoveAt(0);
                    FullTime = CreatingUnits[this.ID][BuildID];
                    timeOfBuildCurUnit.Reset();
                }
            }

            else
            {
                if (timeOfBuildCurUnit.ElapsedMilliseconds/1000 > FullTime) // Если время постройки вышло отправляем запрос на инициализацию
                {
                    conMan.SendMsgIniUnit(BuildID, FlagX, FlagY);
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
    }
}
