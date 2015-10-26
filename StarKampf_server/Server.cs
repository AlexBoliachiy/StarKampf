using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.IO;
using Lidgren.Network;
namespace StarKampf_server
{
    enum Commands
    {
        iniUnit = 0,
        msgYourUnit = 1
    }

    enum Units
    {
        Tank = 0
    }

    class Server
    {
        NetPeerConfiguration config;
        NetServer server;
        NetOutgoingMessage outMsg; // Outgoing msg
        NetIncomingMessage inMsg; //Incoming msg

        private int[] ArrOfCms;// for handle command
        private string StrCommand;
        private List<BaseUnit>[] UnitsList; // array of list units. 0 list it units first player, etc.

        public Server()
        {
            config = new NetPeerConfiguration("StarKampf") { Port = 12345 };
            server = new NetServer(config);
            server.Start();
            UnitsList = new List<BaseUnit>[4];
            UnitsList[0] = new List<BaseUnit>(0);
            UnitsList[1] = new List<BaseUnit>(0);
            UnitsList[2] = new List<BaseUnit>(0);
            UnitsList[3] = new List<BaseUnit>(0);
            int[] arr;
            arr = System.IO.File.ReadAllText("Units/unicorn.txt").Split(' ').Select(n => int.Parse(n)).ToArray();

            //Initializing  a few units for debugind needs
            UnitsList[0].Add(new Fighter(0, 0, 0, 0, "unicorn", 100, 10, 100, 1));
            UnitsList[0].Add(new Fighter(0, 100, 100, 0, "unicorn", 100, 10, 100, 1));

            outMsg = server.CreateMessage();

        }
        public void Act()
        {
            
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

            SendMapSituation();
            Console.Clear();
            Console.WriteLine("Count of connection {0}", server.ConnectionsCount);
        }

        private int AnalyzeMsg()
        {
            //Read from received string numeric commands 
            StrCommand = inMsg.ReadString();
            ArrOfCms = StrCommand.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(n => int.Parse(n))
                .ToArray();
            switch ((Commands)ArrOfCms[0])
            {
                case Commands.iniUnit:
                    IniUnit();
                    break;
                default:
                    break;
            }

            return 0;


        }
        private int IniUnit()
        {
            int[] arr;
           
           
            switch ((Units)ArrOfCms[1])
            {

                case Units.Tank:
                    //Temporary there is characteristic(???)  reading from .txt file.
                    // Someone should make the same , but from .db file
                    // as soon as possible

                    arr = System.IO.File.ReadAllText("Units/unicorn.txt").Split(' ').Select(n => int.Parse(n)).ToArray();

                    UnitsList[ArrOfCms[4]].Add(new Fighter(ArrOfCms[1], ArrOfCms[2], ArrOfCms[3], ArrOfCms[4], "unicorn", arr[0]
                                                            , arr[1], arr[2], arr[3]));
                    Console.Write("ini tank");
                    break;
                default:
                    break;
                
                }
            
            return 0;
        }
        private int SendMapSituation()
        {
            //In this code block we pack all information about units to the MapSituation
            // When this messege will receive to client , he will read line and draw unit;
            // Example message  0 0 0 0
            // It means tank , xCoord = 0, yCoord = 0, angle = 0
            string MapSituation = System.String.Empty;
            for (int i=0; i < UnitsList.Length; i++)
            {
                for (int j=0; j < UnitsList[i].Count; j++)
                {
                    MapSituation += UnitsList[i][j].GetUnitProperties;
                }
            }

            // 

            outMsg.Write(MapSituation);
            for (int i=0; i < 4; i++) // No named const it's bad I know it, but I doesn't see another way.
            {
                try
                {
                    if (server.Connections[i] != null)
                        server.SendMessage(outMsg, server.Connections[i], NetDeliveryMethod.ReliableOrdered);
                }
                catch 
                {
                    //Becouse if list of connections < 2 and we try  contact to third connection it cause error
                }

            }
            return 0;
        }

    }
}
