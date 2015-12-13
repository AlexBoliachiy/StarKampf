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
            unicorn = 0
        }

        
        private const double timeDivider = 30;
     
        //
        private GraphicsDevice GraphicsDevice;

        //using for drawing object on the map;
        private Texture2D[] allTextures;
        private SpriteBatch sprite;
        private Rectangle spriteRectangle;// Для корректной отрисовки поворота юнита
        private Vector2 spriteOrigin;// Центр спрайта
        private int side; // определяет сторону игрока .

        //
        

        //
        public Interface Inter;
        public Map map;
        //
        private double interval;
        private int[][] IntCommands;
        bool DG = false;
        private Stopwatch sw;
        private string UnitSItuation = string.Empty; // Тут собираются все взаимодействия между юнитами, картой , потом это отправляется серверу

        public Player(GraphicsDevice GraphicsDevice)
        {
            this.GraphicsDevice = GraphicsDevice;
        }

        public void Initialize()
        {
          
            sw = new Stopwatch();

            //
            //
            VecUnits = new List<BaseUnit>();
            VecUnits.Capacity = 128;
            //ini timer 
            Inter = new Interface(GraphicsDevice, 0); // Обязательно исправить когда будет корректная инициализация сервером.
            map = new Map();
        }


        public void Update()
        {
            interval = sw.ElapsedMilliseconds / timeDivider;
            sw.Reset(); // reset the timer (change current time to 0)
            sw.Start();
           

            //Тут все очень просто. Если интерфейс возвращает непустую строку, значит там команды взаимодействия.
            //Отправляем их
           

            foreach (BaseUnit Unit in VecUnits)
            {
                Unit.Act(interval);
            }
        }

        public void Draw()
        {
            map.DrawMap(GraphicsDevice, Inter.camera);

            Inter.DrawHealthUnderAllUnit(VecUnits, allTextures);
            
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
                spriteRectangle = new Rectangle((int)VecUnits[i].X,
                                                (int)VecUnits[i].Y,
                                                allTextures[id].Width,
                                                allTextures[id].Height);
                spriteOrigin = new Vector2(allTextures[id].Width / 2, allTextures[id].Height / 2);

                sprite.Draw(allTextures[id], new Vector2((int)VecUnits[i].X, (int)VecUnits[i].Y),
                    null, Color.White, VecUnits[i].Angle, spriteOrigin, 1f, SpriteEffects.None, 0f);
            }
            sprite.End();

            
            return 0;
        }

        public void IniTextures(Texture2D[] texture)
        {
            allTextures = texture;
        }

       


       


        
    }


}
