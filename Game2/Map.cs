using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2
{
    class Map
    {
        private static readonly char[] WspaceDelimiters = new char[] { };

        private int[,] map;
        public int height, width;

        public int this[int i, int j] { get { return map[i, j]; } }

        public int tileWidth = 64;
        public int tileHeight = 64;
        private SpriteBatch sprite;


        public Map(SpriteBatch sprite)
        {
            StreamReader reader = new StreamReader("Map/map.txt");
            this.width = int.Parse(reader.ReadLine());
            this.height = int.Parse(reader.ReadLine());

            this.map = new int[width, height];

            for (int j = 0; j < height; j++)
            {
                string array = reader.ReadLine();
                var items = array.Split(WspaceDelimiters, StringSplitOptions.RemoveEmptyEntries);
                int i = 0;
                foreach (var item in items)
                {
                    map[i++,j] = int.Parse(item);
                }
            }
            this.sprite = sprite;
        }

        public int DrawMap(Texture2D[] wallTexture, Interface inter)
        {
            sprite.Begin(SpriteSortMode.BackToFront,
                                   BlendState.AlphaBlend,
                                   null,
                                   null,
                                   null,
                                   null,
                                   inter.camera.GetTransformation(sprite.GraphicsDevice));
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (map[i, j] == 0) // Вынести все это дерьмо в метод карты. То есть должно быть map.draw();
                    {
                        Rectangle tmp = new Rectangle(i * tileWidth, j * tileHeight, tileWidth, tileHeight);
                        sprite.Draw(wallTexture[0], tmp, Color.White);
                    }
                    if (map[i, j] == 1) // Вынести все это дерьмо в метод карты. То есть должно быть map.draw();
                    {
                        Rectangle tmp = new Rectangle(i * tileWidth, j * tileHeight, tileWidth, tileHeight);
                        sprite.Draw(wallTexture[1], tmp, Color.White);
                    }
                    if (map [i, j] == 2) // Вынести все это дерьмо в метод карты. То есть должно быть map.draw();
                    {
                        Rectangle tmp = new Rectangle(i * tileWidth, j * tileHeight, tileWidth, tileHeight);
                        sprite.Draw(wallTexture [2], tmp, Color.White);
                    }
                    if (map [i, j] == 3) // Вынести все это дерьмо в метод карты. То есть должно быть map.draw();
                    {
                        Rectangle tmp = new Rectangle(i * tileWidth, j * tileHeight, tileWidth, tileHeight);
                        sprite.Draw(wallTexture [3], tmp, Color.White);
                    }
                    if (map [i, j] == 4) // Вынести все это дерьмо в метод карты. То есть должно быть map.draw();
                    {
                        Rectangle tmp = new Rectangle(i * tileWidth, j * tileHeight, tileWidth, tileHeight);
                        sprite.Draw(wallTexture [4], tmp, Color.White);
                    }
                }
            }
            sprite.End();
            return 0;
        }
    }
}
