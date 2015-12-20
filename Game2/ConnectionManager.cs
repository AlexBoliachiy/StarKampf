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
    enum Commands
    {
        iniUnit = 0,
        moveUnit = 1
    }

    enum Units
    {
        unicorn = 0
    }

    class ConnectionManager
    {
        //Для подключения к серверу
        private NetPeerConfiguration config;
        private NetClient client;
        private NetIncomingMessage inMsg;
        private NetOutgoingMessage outMsg;
        private int[][] IntCommands;

        private UnitsManager unitsManager;

        int side;

        private bool DG, DG2;

        public int Initialize(List<BaseUnit> VecUnits, Map map)
        {
            config = new NetPeerConfiguration("StarKampf");
            client = new NetClient(config);
            client.Start();
            client.Connect(host: "127.0.0.1", port: 12345);
            side = 0; //  later somebody need make ini side in moment connecting to the server // later means never
            outMsg = client.CreateMessage();
            unitsManager = new UnitsManager(VecUnits, map);
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
                if (client.ConnectionStatus == NetConnectionStatus.Connected && DG == true && DG2 == false)
                {
                    SendMsgIniUnit(0, 512, 512);
                    DG2 = true;
                }
                if (client.ConnectionStatus == NetConnectionStatus.Connected && DG == false)// ini there units per once
                {
                    SendMsgIniUnit(0, 368, 368);
                    DG = true;
                }
                
                
                
                //
                client.Recycle(inMsg);
            }
            if (ActionCommands != null)
            {
                SendFormedRequest(ActionCommands);
            }
        }

        private void SendMsgIniUnit(int ID, int x, int y)
        {

            string IncomingCommand = ((int)Commands.iniUnit).ToString() + " " +
                                       ID.ToString() + " " + x.ToString() + " " + y.ToString() + " " + side.ToString();

            outMsg.Write(IncomingCommand);
            client.SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered);
        }

        private void SendFormedRequest(string request)
        {
            try
            {
                outMsg.Write(request);
                client.SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered);
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
            for (int i = 0; i < IntCommands.Count() - 1; i++)
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

                    default:
                        break;
                }
            }
        }

        public void LogMsg(string message)
        {
            // File.AppendAllText("log.txt", message);
        }



    }
}
