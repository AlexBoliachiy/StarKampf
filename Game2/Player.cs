using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        unicorn = 0,
        afro = 1,
        centr = 10
    }
    class Player

    {
     

        private ConnectionManager conMan;

        private const double timeDivider = 30;
     
        private Rectangle wallBound;

        private KeyboardState keyboardState; // for testing

        //
        private GraphicsDevice GraphicsDevice;

        //using for drawing object on the map;
        private Texture2D[] allTextures;
        Texture2D wallTexture;
        private SpriteBatch sprite;
        private Vector2 spriteOrigin;// Центр спрайта

        private int side; // определяет сторону игрока .

        public Interface Inter;
        public Map map;
        //
        private double interval;
        private int[][] IntCommands; // не удалять
        bool DG = false;
        private Stopwatch sw;
        private string UnitSItuation = string.Empty; // Тут собираются все взаимодействия между юнитами, картой , потом это отправляется серверу

        private List<BaseUnit> VecUnits;

        public Player(GraphicsDevice GraphicsDevice, SpriteFont font)
        {
            this.GraphicsDevice = GraphicsDevice;
            conMan = new ConnectionManager();
            VecUnits = new List<BaseUnit>();
            sprite = new SpriteBatch(GraphicsDevice);
            map = new Map();

            Inter = new Interface(GraphicsDevice, 0, map, font, allTextures );
            sw = new Stopwatch();
        }

        public void Initialize()
        {
          
            
            conMan.Initialize(VecUnits, map);
            BaseUnit.InitializeMap(map, conMan);
            Building.Initialize(ref VecUnits);

        }


        public void Update()
        {
            interval = sw.ElapsedMilliseconds / timeDivider;
            sw.Reset(); // reset the timer (change current time to 0)
            sw.Start();
            conMan.Update(Inter.Update(VecUnits));

            int x = GraphicsDevice.Viewport.X;
           
           

            foreach (BaseUnit Unit in VecUnits)
            {
                Unit.Act(interval);
            }
        }

        public void Draw()
        {

            Inter.DrawHealthUnderAllUnit(VecUnits, allTextures);
            
 
            DrawMap(); // Should be map.Draw(); // Otherside, it's not so important. 
            DrawUnits();
            Inter.Draw();
        }



        private int DrawUnits()
        {
            sprite.Begin(SpriteSortMode.BackToFront,
                       BlendState.AlphaBlend,
                       null,
                       null,
                       null,
                       null,
                       Inter.camera.GetTransformation(GraphicsDevice));

            for (int i = 0; i < VecUnits.Count; i++)
            {
                int id = VecUnits[i].id;
                spriteOrigin = new Vector2(allTextures[id].Width / 2, allTextures[id].Height / 2);

                sprite.Draw(allTextures[id], new Vector2((int)VecUnits[i].X, (int)VecUnits[i].Y),
                    null, Color.White, VecUnits[i].Angle, spriteOrigin, 1f, SpriteEffects.None, 0f);
            }
            sprite.End();

            
            return 0;
        }

        private int DrawMap()
        {
            sprite.Begin(SpriteSortMode.BackToFront,
                                   BlendState.AlphaBlend,
                                   null,
                                   null,
                                   null,
                                   null,
                                   Inter.camera.GetTransformation(GraphicsDevice));
            for (int i = 0; i < map.width; i++)
            {
                for (int j = 0; j < map.height; j++)
                {
                    if (map[i, j] == 1) // Вынести все это дерьмо в метод карты. То есть должно быть map.draw();
                    {
                        Rectangle tmp = new Rectangle(i * map.tileWidth, j * map.tileHeight, map.tileWidth, map.tileHeight);
                        sprite.Draw(wallTexture, tmp, Color.White);
                    }
                }
            }
            sprite.End();
            return 0;
        }

        public void IniTextures(Texture2D[] texture, Texture2D wallTexture)
        {
            allTextures = texture;
            this.wallTexture = wallTexture;
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

    class Log
    {
        public void Write(string message)
        {
            File.AppendAllText("log.txt", message);
        }
    }

}
