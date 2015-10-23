using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Game2
{
    class Player

    {
        enum Commands
        {
            iniUnit = 0

        }

        enum Units
        {
            Tank = 0
        }

        //For server connection
        private NetPeerConfiguration config;
        private NetClient client;
        private NetIncomingMessage inMsg;
        private NetOutgoingMessage outMsg; 
        private string arrOfUnitProp;
        GraphicsDevice GraphicsDevice;

        //using for drawing object on the map;
        Texture2D[] allTextures;
        SpriteBatch sprite;
        int side;

        bool DeleteThis; // Delete this 

        public Player(GraphicsDevice GraphicsDevice)
        {
            this.GraphicsDevice = GraphicsDevice;
        }
        
        public void Initialize()
        {
            config = new NetPeerConfiguration("StarKampf");
            client = new NetClient(config); 
            client.Start();
            client.Connect(host: "127.0.0.1", port : 12345);
            sprite = new SpriteBatch(GraphicsDevice);
            side = 0; // guys , later some one need make ini side in moment connecting to the server
            outMsg = client.CreateMessage();
            SendMsgIniUnit(0, 100, 100);

        }


        public void Update()
        {
            while ((inMsg = client.ReadMessage()) != null)
            {
                switch (inMsg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        // handle custom messages
                        arrOfUnitProp = inMsg.ReadString();
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        // handle connection status messages


                        break;
                    default:
                        Console.WriteLine("unhandled message with type: "
                            + inMsg.MessageType);
                        break;

                }

                client.Recycle(inMsg);
                if (client.ConnectionStatus == NetConnectionStatus.Connected && DeleteThis == false)
                {
                    DeleteThis = true;
                    SendMsgIniUnit(0, 0, 0);
                }
            }
        }
        public void Draw()  
        {
            // Who will engage with Interface class?
            // You shoud draw it there;
            DrawUnits();
        }
        
        private int DrawUnits()
        {
            
            if (arrOfUnitProp == null)
            {
                return 0;
            }
            foreach (string A in arrOfUnitProp.Split(new char[] { '\n'} )) // Split array of string on string like  "xx xx xx \n"
            {
               
                int[] MapSituatinon = A.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(n => int.Parse(n))
                .ToArray();
                sprite.Begin();
                for (int i=0; i < MapSituatinon.Length; i++)
                {
                    sprite.Draw(allTextures[MapSituatinon[0]], new Vector2(MapSituatinon[1], MapSituatinon[2]), Color.White);
                }
                sprite.End();
                

            }
            return 0;
        }

        public void IniTextures(Texture2D[] texture)
        {
            allTextures = texture;
        }
        private void SendMsgIniUnit(int ID, int x, int y)
        {

            string IncomingCommand = ((int)Commands.iniUnit).ToString() + " " +
                ID.ToString() + " " + x.ToString() + " " + y.ToString() + " " + side.ToString()   ;
            outMsg.Write(IncomingCommand);
            client.SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered); 

        }
        private void EstablishConnection()
        {
            if (client.ConnectionStatus == NetConnectionStatus.Disconnected)
            {
               
            }
        }

    }
    class Interface
    {
        // Вложенный класс в Player, будет отвечать за отрисовку и корректный вывод информации на экран игрока

    }

}
