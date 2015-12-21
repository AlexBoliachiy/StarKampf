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
    

    class ConnectionManager
    {
        //Для подключения к серверу
        private NetPeerConfiguration config;
        private NetClient client;
        private NetIncomingMessage inMsg;
        private NetOutgoingMessage outMsg;
        private int[][] IntCommands;

        private UnitsManager unitsManager;
        private bool ShoudSend;
        string OutComingCommandAboutIni;
        int side;
        List<BaseUnit> VecUnits;
        private bool DG;

        public int Initialize(ref List<BaseUnit> VecUnits, Map map)
        {
            config = new NetPeerConfiguration("StarKampf");
            client = new NetClient(config);
            client.Start();
            client.Connect(host: "127.0.0.1", port: 12345);
            outMsg = client.CreateMessage();
            this.VecUnits = VecUnits;
            unitsManager = new UnitsManager(ref VecUnits, map);
            OutComingCommandAboutIni = string.Empty;
            return side;
        }

        public void Update(string ActionCommands)
        {
            while ((inMsg = client.ReadMessage()) != null)
            {
                switch (inMsg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        ReadMsg();
                        AnalyzeCommands();
                        break;

                    case NetIncomingMessageType.StatusChanged:

                        // handle connection status messages


                        break;
                    default:
                        Console.WriteLine("unhandled message with type: "
                            + inMsg.MessageType);
                        break;

                }
                if (client.ConnectionStatus == NetConnectionStatus.Connected && DG == false)// ini there units per once
                {
                    SendMsgIniUnit(0, 368, 368);
                    SendMsgIniUnit(10, 500, 500);
                    DG = true;
                }


                //
                client.Recycle(inMsg);
            }
            if (ActionCommands != null)
            {
                SendFormedRequest(ActionCommands);
            }
            if (ShoudSend == true)
            {
                if (OutComingCommandAboutIni == string.Empty || OutComingCommandAboutIni == null)
                    return;
                outMsg.Write(OutComingCommandAboutIni);
                ShoudSend = false;
                OutComingCommandAboutIni = String.Empty;

                try
                {
                    client.SendMessage(outMsg, NetDeliveryMethod.ReliableUnordered);
                }
                catch { }

            }
        }

        public int SendMsgIniUnit(int ID, int x, int y)
        {

        
            OutComingCommandAboutIni += ((int)Commands.iniUnit).ToString() + " " +
                                       ID.ToString() + " " + x.ToString() + " " + y.ToString() + " " + side.ToString()+ " \n";
            ShoudSend = true;
            return 1;
        }

        private void SendFormedRequest(string request)
        {
            try
            {
                outMsg.Write(request);
                client.SendMessage(outMsg, NetDeliveryMethod.ReliableUnordered);
            }
            catch
            {
                // Одно и тоже сообщение пытается отослаться несколько раз
            }
        }

        private void EstablishConnection()
        {
            if (client.ConnectionStatus == NetConnectionStatus.Disconnected)
            {
                //Reconnect;
            }
        }

        private void ReadMsg()
        {
            /*Приходит строка где записаны команды вот в таком формате разделенные символом перехода на следующую строку :
            * gametime
            * IDofCMD IN param
            * -//-
            * IDpfCMD - айди команды, IN идентификационый номер юнита, param параметры к команде.
            * Итого на выходе из функции имеем зубчатый массив в котором каждая строка это одна команда.
             */
            string ListOfCmd = inMsg.ReadString();
            LogMsg("Receive message : " + ListOfCmd);
            // GetIntervalFromMsg( ref ListOfCmd);
            IntCommands = new int[ListOfCmd.Split('\n').Count()][];
            int i = 0;
            foreach (string A in ListOfCmd.Split('\n'))
            {
                IntCommands[i] = A.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(n => int.Parse(n))
                .ToArray();
                i++;
            }
        }

        private void AnalyzeCommands()
        {
            //Просто пробегаем по массиву и выполняем комманды
            for (int i = 0; i < IntCommands.Count(); i++)
            {
                if (IntCommands[i].Count() == 0)
                    return;
                //Определяем тип комманды
                switch ((Commands)(IntCommands[i][0]))
                {
                    case Commands.iniUnit:
                        unitsManager.IniUnit(IntCommands[i]);
                        break;

                    case Commands.moveUnit://1 0 100 100 означает переместить юнит с ИН 0 в точку х = 100 у = 100;
                        unitsManager.MoveUnit(IntCommands[i]);
                        break;
                    case Commands.iniSide:
                        side = IntCommands[i][1];
                        LogMsg("THIS SIDE +@" + side.ToString() + "@\n");
                        break;
                    case Commands.attack:
                        unitsManager.HandleAttackMsg(IntCommands[i]);
                        break;
                    default:
                        break;
                }
            }
        }

        public void LogMsg(string message)
        {
             File.AppendAllText("log.txt", message);
        }
        


    }
}
