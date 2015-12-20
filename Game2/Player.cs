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
    class Player

    {
        enum Commands
        {
            iniUnit = 0,
            moveUnit = 1
        }

        enum Units
        {
            unicorn = 0,
        }

        private ConnectionManager conMan;

        private const double timeDivider = 30;
        //
        private GraphicsDevice GraphicsDevice;

        //using for drawing object on the map;
        private Texture2D[] allTextures;
        private Texture2D[] mapTextures;
        private SpriteBatch sprite;
        private Vector2 spriteOrigin;// Центр спрайта

        private int side; // определяет сторону игрока .

        public Interface Inter;
        public Map map;
        //
        private double interval;
        private int[][] IntCommands;
        bool DG = false;
        private Stopwatch sw;
        private string UnitSItuation = string.Empty; // Тут собираются все взаимодействия между юнитами, картой , потом это отправляется серверу

        private List<BaseUnit> VecUnits;

        public Player(GraphicsDevice GraphicsDevice)
        {
            this.GraphicsDevice = GraphicsDevice;
            conMan = new ConnectionManager();
            VecUnits = new List<BaseUnit>();
            sprite = new SpriteBatch(GraphicsDevice);
        }

        public void Initialize()
        {
          
            sw = new Stopwatch();
            //
            //
            
            //ini timer 
            map = new Map(sprite);
            Inter = new Interface(GraphicsDevice, 0, map); // Обязательно исправить когда будет корректная инициализация сервером.
            
            conMan.Initialize(VecUnits, map);

        }

        public void Update()
        {
            interval = sw.ElapsedMilliseconds / timeDivider;
            sw.Reset(); // reset the timer (change current time to 0)
            sw.Start();
            conMan.Update(Inter.Update(VecUnits));
           
           

            foreach (BaseUnit Unit in VecUnits)
            {
                Unit.Act(interval);
            }
        }

        public void Draw()
        {
            //map.DrawMap(GraphicsDevice, Inter.camera);

            Inter.DrawHealthUnderAllUnit(VecUnits, allTextures);
            
            // Who will engage with Interface class?
            // You shoud draw it there;
            map.DrawMap(mapTextures, Inter);
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

        public void IniTextures(Texture2D[] texture, Texture2D[] mapTextures)
        {
            allTextures = texture;
            this.mapTextures = mapTextures;
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


        public void LogMsg(string message)
        {
            File.AppendAllText("log.txt", message);
        }
    }


}
