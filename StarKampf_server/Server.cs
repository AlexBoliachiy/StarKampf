using System;
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
        moveUnit = 1
    }

    enum Units
    {
        unicorn = 0
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

        Stopwatch sw;//Timer
        

        private static int IN; //value that determined units id, identifical number
        // 1 0 0 0 0

        public Server()
        {
            config = new NetPeerConfiguration("StarKampf") { Port = 12345 };
            server = new NetServer(config);
            server.Start();
            outMsg = server.CreateMessage();

            StrCommand = new List<string>();
            UnitsList = new List<BaseUnit>();
            int[] arr;
            arr = System.IO.File.ReadAllText("Units/unicorn.txt").Split(' ').Select(n => int.Parse(n)).ToArray();
            //ini timer
            sw = new Stopwatch();
        }
        public void Act()
        { 
            double time = sw.ElapsedMilliseconds / timeDivider;
            sw.Reset(); // reset the timer (change current time to 0)
            sw.Start();
            OutStrCmd = String.Empty;

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
            string tmp = inMsg.ReadString();
            StrCommand.Add(tmp);
            LogMsg("Receive msg: " + tmp);
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
            while (StrCommand.Count > 0)
            {
                foreach (string A in StrCommand.First().Split('\n'))
                {
                    if (A == null || A == string.Empty)
                        break;
                    ArrOfCms = A.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                   .Select(n => int.Parse(n))
                   .ToArray();
                    if (ArrOfCms.Count() == 0)
                        continue;
                    switch ((Commands)ArrOfCms[0])
                    {
                        case Commands.iniUnit:
                            IniUnit();
                            break;

                        case Commands.moveUnit:
                            MoveUnit();
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
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if (server.Connections[i] != null)
                        server.SendMessage(outMsg, server.Connections[i], NetDeliveryMethod.ReliableOrdered);
                }
                catch
                {
                    //if list of connections < 2 and we try  contact to third connection this will be throw exception 
                }
            }
        }
        public void LogMsg(string message)
        {
         //   File.AppendAllText("serverlog.txt", message);
        }

    }
}
