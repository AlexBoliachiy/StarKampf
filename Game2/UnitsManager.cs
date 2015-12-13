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


        enum Commands
        {
            iniUnit = 0,
            moveUnit = 1
        }

        enum Units
        {
            unicorn = 0
        }

        private void IniUnit(int NumberOfCurCMS)
        {
            switch ((Units)IntCommands[NumberOfCurCMS][1])
            {
                case Units.unicorn:
                    //Temporary there is characteristic(???)  reading from .txt file.
                    // Someone should make the same , but from .db file
                    // as soon as possible

                    int[] arr = System.IO.File.ReadAllText("Units/unicorn.txt").Split(' ').Select(n => int.Parse(n)).ToArray();

                    VecUnits.Add(new Fighter(IntCommands[NumberOfCurCMS][1],
                                             IntCommands[NumberOfCurCMS][2],
                                             IntCommands[NumberOfCurCMS][3],
                                             IntCommands[NumberOfCurCMS][4],
                                             IntCommands[NumberOfCurCMS][5],
                                             "unicorn", arr[0], arr[1], arr[2], arr[3]));

                    break;


                default:
                    break;
            }
        }

        private void MoveUnit(int NumberOfCurCMS)
        {
            BaseUnit movingUnit = FindInList(VecUnits, IntCommands[NumberOfCurCMS][1]);
            movingUnit.SetMoveDest(IntCommands[NumberOfCurCMS][2], IntCommands[NumberOfCurCMS][3]);

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
    }
}
