using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Diagnostics;

namespace Game2
{
    class Map
    {
        private static readonly char[] WspaceDelimiters = new char[] { };
        private SpriteBatch sprite;
        private GraphicsDevice graphicsDevice;
        private Texture2D texture;
        private Camera2D camera;

        private List<Rectangle> wallList;

        private int[,] map;

        private bool flag = false;

        public int this[int i, int j] { get { return map[i, j]; } }

        private int tileWidth = 256;
        private int tileHeight = 132;

        public Map()
        {
            map = new int[19, 21];
            StreamReader reader = new StreamReader("Map/map.txt");
            wallList = new List<Rectangle>();

            for (int j = 0; j < 21; j++)
            {
                string array = reader.ReadLine();
                var items = array.Split(WspaceDelimiters, StringSplitOptions.RemoveEmptyEntries);
                int i = 0;
                foreach (var item in items)
                {
                    map[i++,j] = int.Parse(item);
                }
            }
        }

        protected Rectangle ifColides(BaseUnit test)
        {
            foreach (Rectangle wall in wallList)
            {
                if (!Rectangle.Intersect(wall, test.SpriteRectangle).IsEmpty)
                {
                    return wall;
                }
            }
            return new Rectangle();
;
        }

        public int DrawMap(GraphicsDevice graphicsDevice, Camera2D camera, List<BaseUnit> VecUnits)
        {
            sprite = new SpriteBatch(graphicsDevice);
            texture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture.SetData<Color>(new Color[] { Color.Red });

           
            sprite.Begin(SpriteSortMode.BackToFront,
                                   BlendState.AlphaBlend,
                                   null,
                                   null,
                                   null,
                                   null,
                                   camera.GetTransformation(graphicsDevice));
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 21; j++)
                {
                    if (this[i, j] == 1)
                    {
                        Rectangle tmp = new Rectangle(i * tileWidth, j * tileHeight, tileWidth, tileHeight);
                        if (!flag) { wallList.Add(tmp); }
                        sprite.Draw(texture, tmp, Color.Red );
                        if (flag)
                        {
                            foreach (BaseUnit test in VecUnits)
                            {
                                if (!ifColides(test).IsEmpty)
                                {
                                    sprite.Draw(texture, ifColides(test), new Color(100, 100, 100, 100));
                                }
                            }
                        }
                    }
                }
            }

            flag = true;
            sprite.End();
            return 0;
        }
    }
}
