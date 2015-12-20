using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


namespace StarKampf_server
{
    class Building : BaseUnit
    {

        private List<int> QueueOfBuildUnits;
        private Stopwatch timeOfBuildCurUnit;
        private int FullTime;
        private bool IsBuild;
        private int BuildID;

        private static Dictionary<int, Dictionary<int, int>> CreatingUnits;

        private int FlagX; // Point, where unit would be located after creating 
        private int FlagY;
        public Building(int ID, int x, int y, int side, string name, int MaxHealth, int IN)
        {
            this.ID = ID;
            this.x = x;
            this.y = y;
            this.side = side;
            this.IN = IN;
            this.name = name;
            this.MaxHealth= MaxHealth;
            this.health = MaxHealth;

        }
        public override string GetUnitProperties
        {
            get
            {
                return ID.ToString() + " " + x.ToString() + " " + y.ToString() + " "
                        + side.ToString() + " " + IN.ToString() + " " + "\n";
            }
        }
    }
}
