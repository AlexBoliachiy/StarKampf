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
        private Stopwatch timeOfBuildCurUnut;
        private int FullTime;
        private bool IsBuild;
        private Dictionary<int, int> DictOfCreatingUnit;
        private Dictionary<int, int> DictOfCreatingUpgrade;
        private int FlagX; // Point, where unit located after creating 
        private int FlagY;
        public void SetBuild(int ID)
        {

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
        }
    }
}
