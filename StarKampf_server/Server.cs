﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.IO;
using Lidgren.Network;
using System.Diagnostics;

namespace StarKampf_server
{
    enum Commands
    {
        iniUnit = 0,
        moveUnit = 1,
        iniSide = 228,
        attack = 2
    }

    enum Units
    {
        unicorn = 0,
        afro = 1,
        centr = 10,
        buldozer = 100,
        soldier = 2,
        carrier = 11
    }

    class Server
    {
        NetPeerConfiguration config;
        NetServer server;
        NetOutgoingMessage outMsg; // Outgoing msg
        NetIncomingMessage inMsg; //Incoming msg
        const double timeDivider = 30;
        private int[] ArrOfCms;// for handle command
        private List<string> StrCommand;
        private string OutStrCmd; // string with commands, that sending to client and there execute
        private List<BaseUnit> UnitsList; // array of all units on the map
        private int side = 0;
        Stopwatch sw;//Timer
        Stopwatch TimerWait;

        private bool DG = true;

        private static int IN; //value that determined units id, identifical number
        // 1 0 0 0 0

        public Server()
        {
            config = new NetPeerConfiguration("StarKampf") { Port = 45297 };
            server = new NetServer(config);
            server.Start();
            outMsg = server.CreateMessage();
            StrCommand = new List<string>();
            UnitsList = new List<BaseUnit>();
            int[] arr;
            arr = System.IO.File.ReadAllText("Units/unicorn.txt").Split(' ').Select(n => int.Parse(n)).ToArray();
            //ini timer
            sw = new Stopwatch();
            TimerWait = new Stopwatch();
            TimerWait.Start();

        }
        public void Act()
        { 
            double time = sw.ElapsedMilliseconds / timeDivider;
            sw.Reset(); // reset the timer (change current time to 0)
            sw.Start();
            OutStrCmd = String.Empty;
            if (TimerWait.ElapsedMilliseconds / 1000 > 10)
            {
                AnalyzeMsg();
                TimerWait.Reset();
                DG = false;
            }
            while ((inMsg = server.ReadMessage()) != null)
            {
                switch (inMsg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        
                        AnalyzeMsg();
                        break;

                    case NetIncomingMessageType.StatusChanged:
                          if (((NetConnectionStatus)(inMsg.PeekByte())).ToString() != "RespondedConnect" &&
                               ((NetConnectionStatus)(inMsg.PeekByte())).ToString() != "Connected")
                          {
                              Console.WriteLine(((NetConnectionStatus)(inMsg.ReadByte())).ToString());
                              Console.ReadLine();
                          }
                        
                        break;

                }
                server.Recycle(inMsg);
            }
            UpdateUnits(time);
            
        }

        private int AnalyzeMsg()
        {
            //Read from received string numeric commands 
            try {
                string tmp = inMsg.ReadString();
                StrCommand.Add(tmp);
                LogMsg("Receive msg: " + tmp);
            }
            catch
            {

            }
            // What is doing there
            /*
            * Server Receives message as sequence of numbers separating by symbol '\n'
            * Example: 0 0 0 0 
            *          1 0 0 0 
            * First symbol - command, next symbol - params to command
            * 1 line - 1 command
            * In following code we separate command by symbol '\n'. One string one command
            * then we put it in Int array and start analyze msg
            * by switching
            */
            //Обрабатываем первый элемент.
            if (TimerWait.ElapsedMilliseconds / 1000 < 5 && DG ==true)
                return 0;

            while (StrCommand.Count > 0)
            {
                foreach (string A in StrCommand.First().Split('\n'))
                {
                    if (A == null || A == string.Empty)
                        break;
                    ArrOfCms = A.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                   .Select(n => int.Parse(n))
                   .ToArray();
                    
                    switch ((Commands)ArrOfCms[0])
                    {
                        case Commands.iniUnit:
                            IniUnit();
                            break;

                        case Commands.moveUnit:
                            MoveUnit();
                            break;
                        case Commands.iniSide:
                            outMsg.Write("228 " + side.ToString());
                            server.SendMessage(outMsg, server.Connections.Last(), NetDeliveryMethod.ReliableOrdered);
                            side++;
                            break;
                        case Commands.attack:
                            OutStrCmd += "2 " + ArrOfCms[1].ToString() + " " +  ArrOfCms[2].ToString() +  " "+'\n';
                            break;

                        default:
                            break;
                    }
                }
                SendCommandsToClients();
                StrCommand.Remove(StrCommand.First());// Удаляем обраотанный элемент
            }

            return 0;
        }

        private void MoveUnit()
        {
            BaseUnit movingUnit = FindInList(UnitsList, ArrOfCms[1]);
            if (movingUnit != null)
            { 
                movingUnit.SetMoveDest(ArrOfCms[2], ArrOfCms[3]);
                OutStrCmd += ArrOfCms[0] + " " + ArrOfCms[1] + " " + ArrOfCms[2] + " " +
                             ArrOfCms[3] + " " + "\n";
            }

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
        private int IniUnit()
        {
            /*Сначала инициализируем юнит на сервере.
            * Затем добавляем в строку комманд, которые исполняются на клиенте 
            * что бы они тоже инициализировали у себя юнит
            */
            int[] arr;
            switch ((Units)ArrOfCms[1])
            {
                case Units.unicorn:
                    //Temporary there is characteristic(???)  reading from .txt file.
                    // Someone should make the same , but from .db file
                    // as soon as possible

                    arr = System.IO.File.ReadAllText("Units/unicorn.txt").Split(' ').Select(n => int.Parse(n)).ToArray();

                    UnitsList.Add(new Fighter(ArrOfCms[1], ArrOfCms[2], ArrOfCms[3], ArrOfCms[4], "unicorn", arr[0]
                                                            , arr[1], arr[2], arr[3], IN++));
                    OutStrCmd += "0 " + UnitsList.Last().GetUnitProperties;
                   

                    Console.WriteLine("Ini Unicorn ");
                    break;

                case Units.centr:
                    arr = System.IO.File.ReadAllText("Units/centr.txt").Split(' ').Select(n => int.Parse(n)).ToArray();

                    UnitsList.Add(new Building(ArrOfCms[1], ArrOfCms[2], ArrOfCms[3], ArrOfCms[4], "centr", arr[0],
                                                             IN++));
                    OutStrCmd += "0 " + UnitsList.Last().GetUnitProperties;
                    break;
                case Units.afro:
                    arr = System.IO.File.ReadAllText("Units/afro.txt").Split(' ').Select(n => int.Parse(n)).ToArray();

                    UnitsList.Add(new Fighter(ArrOfCms[1], ArrOfCms[2], ArrOfCms[3], ArrOfCms[4], "unicorn", arr[0]
                                                            , arr[1], arr[2], arr[3], IN++));
                    OutStrCmd += "0 " + UnitsList.Last().GetUnitProperties;
                    break;

                case Units.soldier:
                    arr = System.IO.File.ReadAllText("Units//soldier.txt").Split(' ').Select(n => int.Parse(n)).ToArray();
                    UnitsList.Add(new Fighter(ArrOfCms[1], ArrOfCms[2], ArrOfCms[3], ArrOfCms[4], "unicorn", arr[0]
                                                            , arr[1], arr[2], arr[3], IN++));
                    OutStrCmd += "0 " + UnitsList.Last().GetUnitProperties;
                    break;

                case Units.carrier:
                    arr = System.IO.File.ReadAllText("Units/carrier.txt").Split(' ').Select(n => int.Parse(n)).ToArray();

                    UnitsList.Add(new Building(ArrOfCms[1], ArrOfCms[2], ArrOfCms[3], ArrOfCms[4], "centr", arr[0],
                                                             IN++));
                    OutStrCmd += "0 " + UnitsList.Last().GetUnitProperties;
                    break;
                case Units.buldozer:
                    arr = System.IO.File.ReadAllText("Units/buldozer.txt").Split( new char[]{ ' ' },StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();
                    UnitsList.Add(new Support(ArrOfCms[1], ArrOfCms[2], ArrOfCms[3], ArrOfCms[4], ++IN, "buldozer", arr[0], arr[1]));
                    OutStrCmd += "0 " + UnitsList.Last().GetUnitProperties;
                    break;


                default:
                    break;

            }

            return 0;
        }

        private void UpdateUnits(double Interval)
        {
                foreach (BaseUnit Unit in UnitsList)
                {
                    Unit.Act(Interval);
                }
        }

        private void SendCommandsToClients()
        {
            if (OutStrCmd == null || OutStrCmd == string.Empty)
                return;
            outMsg.Write(OutStrCmd);
            LogMsg("Send msg: " + OutStrCmd);
            Console.WriteLine(server.Connections.Count);
            int i = 0;
       /*     foreach (var A in server.Connections)
            {
                i++;
                try
                {

                    outMsg.Write(OutStrCmd);
                    server.SendMessage(outMsg, A, NetDeliveryMethod.ReliableOrdered);
                    server.SendMessage()
                    Console.WriteLine("Send " + i);
                }

                catch
                {
                    Console.WriteLine("CATHCHED");
                }
                System.Threading.Thread.Sleep(50);
            }*/

            try
            {
                server.SendMessage(outMsg, server.Connections, NetDeliveryMethod.ReliableOrdered,1);
            }
            catch
            {

            }
        }
        public void LogMsg(string message)
        {
         //   File.AppendAllText("serverlog.txt", message);
        }

    }
}
