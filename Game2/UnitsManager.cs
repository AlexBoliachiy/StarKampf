using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Lidgren.Network;
using System.IO;
using System.Diagnostics;


namespace Game2
{
     class UnitsManager
    {
        
        private List<BaseUnit> VecUnits;

        public UnitsManager(ref List<BaseUnit> _VecUnits, Map map)
        {
            VecUnits = _VecUnits;
            VecUnits.Capacity = 128;
        }




        public void IniUnit(int[] IntCommands)
        {
            int[] arr;
            switch ((Units)IntCommands[1])
            {

                case Units.carrier:
                    arr = System.IO.File.ReadAllText("Units/carrier.txt").Split(' ').Select(n => int.Parse(n)).ToArray();

                    VecUnits.Add(new Building(IntCommands[1],
                                             IntCommands[2],
                                             IntCommands[3],
                                             IntCommands[4],
                                             IntCommands[5],
                                             arr[0]));
                    break;

                case Units.centr:
                     arr = System.IO.File.ReadAllText("Units/centr.txt").Split(' ').Select(n => int.Parse(n)).ToArray();

                    VecUnits.Add(new Building(IntCommands[1],
                                             IntCommands[2],
                                             IntCommands[3],
                                             IntCommands[4],
                                             IntCommands[5],
                                             arr[0]));
                    break;

                case Units.unicorn:
                    //Temporary there is characteristic(???)  reading from .txt file.
                    // Someone should make the same , but from .db file
                    // as soon as possible

                    arr = System.IO.File.ReadAllText("Units/unicorn.txt").Split(' ').Select(n => int.Parse(n)).ToArray();

                    VecUnits.Add(new Fighter(IntCommands[1],
                                             IntCommands[2],
                                             IntCommands[3],
                                             IntCommands[4],
                                             IntCommands[5],
                                             "unicorn", arr[0], arr[1], arr[2], arr[3], arr[4]));

                    break;

                case Units.soldier:
                    //Temporary there is characteristic(???)  reading from .txt file.
                    // Someone should make the same , but from .db file
                    // as soon as possible

                    arr = System.IO.File.ReadAllText("Units/soldier.txt").Split(' ').Select(n => int.Parse(n)).ToArray();

                    VecUnits.Add(new Fighter(IntCommands[1],
                                             IntCommands[2],
                                             IntCommands[3],
                                             IntCommands[4],
                                             IntCommands[5],
                                             "soldier", arr[0], arr[1], arr[2], arr[3], arr[4]));

                    break;

                case Units.afro:
                    arr = System.IO.File.ReadAllText("Units/afro.txt").Split(' ').Select(n => int.Parse(n)).ToArray();

                    VecUnits.Add(new Fighter(IntCommands[1],
                                             IntCommands[2],
                                             IntCommands[3],
                                             IntCommands[4],
                                             IntCommands[5],
                                             "afro", arr[0], arr[1], arr[2], arr[3], arr[4]));


                    break;
                case Units.buldozer:
                    arr = System.IO.File.ReadAllText("Units/afro.txt").Split(' ').Select(n => int.Parse(n)).ToArray();

                    VecUnits.Add(new Support(IntCommands[1],
                                             IntCommands[2],
                                             IntCommands[3],
                                             IntCommands[4],
                                             IntCommands[5],
                                             "buldozer", arr[0], arr[1]));
                    break;

                

                default:
                    break;
            }
        }

        public void MoveUnit(int[] IntCommands)
        {
            var Unit = FindInList(VecUnits, IntCommands[1]);
            if (Unit != null)
                Unit.SetMoveDest(IntCommands[2], IntCommands[3]);
        }

        private BaseUnit FindInList(List<BaseUnit> VecUnits, int IN)
        {
            for (int i = 0; i < VecUnits.Count; i++)
            {
                if (VecUnits[i].GN == IN)
                    return VecUnits[i];
            }
            return null;

        }

        public void HandleAttackMsg(int[] IntCommands)
        {
            Fighter f = FindInList(VecUnits, IntCommands[1]) as Fighter;
            f.setTarget(FindInList(VecUnits, IntCommands[2]));
        }
    }
}
