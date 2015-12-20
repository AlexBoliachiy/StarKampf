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

        private Dictionary<int, int> DictOfCreatingUnit;
        private Dictionary<int, int> DictOfCreatingUpgrade;

        private int FlagX; // Point, where unit would be located after creating 
        private int FlagY;
        public override void SetBuild(int ID)
        {
            QueueOfBuildUnits.Add(ID);
        }
        private void Build(List<BaseUnit> VecUnits)
        {
            
            if (BuildID == -1) 
            {
                if (QueueOfBuildUnits.Count > 0)
                {
                    BuildID = QueueOfBuildUnits.First();
                    QueueOfBuildUnits.RemoveAt(0);
                    FullTime = DictOfCreatingUnit[BuildID];
                    timeOfBuildCurUnit.Reset();
                }
            }
            else
            {
                if (timeOfBuildCurUnit.ElapsedMilliseconds/1000 > FullTime)
                {
                   

                    if (0 <= ID && ID < 10) // Fighter 
                    {
                        
                    }

                    else if (10 <= ID && ID < 20)//Builder
                    {

                    }

                    else if (20 <= ID && ID < 30)//Support
                    {

                    }

                    BuildID = -1;  
                }
            }
            
        }
        public Building(int ID, int x, int y, int side, int IN, string name, int MaxHealth, 
                        Dictionary<int, int> DictOfCreatingUnit, Dictionary<int, int> DictOfCreatingUpgrade)
        {
            this.ID = ID;
            this.x = x;
            this.y = y;
            this.side = side;
            this.IN = IN;
            this.name = name;
            this._MaxHealth = MaxHealth;
            this.health = MaxHealth;
            this.DictOfCreatingUnit = DictOfCreatingUnit;
            this.DictOfCreatingUpgrade = DictOfCreatingUpgrade;

            this.BuildID = -1;
        }
    }
}
