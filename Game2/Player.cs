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
            iniUnit = 0,
            msgYourUnit = 1 //wtf is it?
        }

        enum Units
        {
            unicorn = 0
        }

        //Для подключения к серверу
        private NetPeerConfiguration config;
        private NetClient client;
        private NetIncomingMessage inMsg;
        private NetOutgoingMessage outMsg;

        
        // 
        private string arrOfUnitProp;
        private string SelectedUnits;
        GraphicsDevice GraphicsDevice;

        //using for drawing object on the map;
        Texture2D[] allTextures;
        SpriteBatch sprite;
        Rectangle spriteRectangle;// Для корректной отрисовки поворота юнита
        Vector2 spriteOrigin;// Центр спрайта
        int side;

        public Interface Inter;


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


            Inter = new Interface(GraphicsDevice);
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
                
            }
            Inter.Update(arrOfUnitProp);
        }
        public void Draw()  
        {
            // Who will engage with Interface class?
            // You shoud draw it there;
            DrawUnits();
            Inter.Draw();
        }
        
        private int DrawUnits()
        {
            
            if (arrOfUnitProp == null)
            {
                return 0;
            }
            sprite.Begin(SpriteSortMode.BackToFront,
                       BlendState.AlphaBlend,
                       null,
                       null,
                       null,
                       null,
                       Inter.camera.GetTransformation(GraphicsDevice));
            foreach (string A in arrOfUnitProp.Split(new char[] { '\n'} )) // Split array of string to string like  "xx xx xx \n"
            {
               
                float[] MapSituatinon = A.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(n => float.Parse(n)) // angle must be inf float, not int
                .ToArray();
                for (int i = 0; i < MapSituatinon.Length; i++)
                {
                    spriteRectangle = new Rectangle((int)MapSituatinon[1], (int)MapSituatinon[2], allTextures[(int)MapSituatinon[0]].Width, allTextures[(int)MapSituatinon[0]].Height);
                    spriteOrigin = new Vector2(allTextures[(int)MapSituatinon[0]].Width/2, allTextures[(int)MapSituatinon[0]].Height/2);

                    sprite.Draw(allTextures[(int)MapSituatinon[0]], new Vector2(MapSituatinon[1], MapSituatinon[2]), null, Color.White, MapSituatinon[3], spriteOrigin, 1.0f, SpriteEffects.None, 0f);
                }
            }
            sprite.End();
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
              //What should client doing?????????////???
            }
        }

    }
    

}
