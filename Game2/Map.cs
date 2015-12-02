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
        private SpriteBatch sprite;
        private GraphicsDevice graphicsDevice;
        private Texture2D texture;
        private Camera2D camera;

        private int[,] map = {{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},  
                              {1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,1},
                              {1,0,1,1,0,0,1,1,0,1,0,1,1,0,0,1,1,0,1,1},
                              {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1},
                              {1,0,1,1,0,0,0,1,1,1,1,1,0,0,0,1,1,0,1,1},
                              {1,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,0,1},
                              {1,1,1,1,0,0,1,1,1,0,1,0,1,1,0,0,1,1,1,1},
                              {1,1,1,1,0,0,0,0,1,0,0,0,0,0,0,0,1,1,1,1},
                              {1,1,1,1,0,0,0,1,1,1,1,1,1,0,0,0,1,1,1,1},
                              {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                              {1,1,1,1,0,0,0,1,1,0,0,1,1,0,0,0,1,1,1,1},
                              {1,1,1,1,0,0,0,0,0,0,0,0,1,0,0,0,1,1,1,1},
                              {1,1,1,1,0,0,0,1,1,1,1,1,0,1,0,0,1,1,1,1},
                              {1,0,0,0,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1},
                              {1,0,1,1,0,0,1,1,0,1,0,1,1,0,0,1,1,1,0,1},
                              {1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,1},
                              {1,1,0,1,0,0,0,1,1,1,1,1,0,0,0,1,0,1,1,1},
                              {1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,1},
                              {1,0,1,1,1,0,1,1,0,1,0,1,1,0,1,1,1,0,1,1},
                              {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1},
                              {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1},
                              {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}};

        public int this[int i, int j] { get { return map[i, j]; } }

        static int tileWidth = 256;
        static int tileHeight = 132;

        public Map()
        {
        }

        public int DrawMap(GraphicsDevice graphicsDevice, Camera2D camera)
        {
            sprite = new SpriteBatch(graphicsDevice);
            texture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture.SetData<Color>(new Color[] { Color.Red });
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 22; j++)
                {
                    if (this[j, i] == 1)
                    {
                        sprite.Begin(SpriteSortMode.BackToFront,
                                   BlendState.AlphaBlend,
                                   null,
                                   null,
                                   null,
                                   null,
                                   camera.GetTransformation(graphicsDevice));
                        sprite.Draw(texture, new Rectangle(i * tileWidth - 2560, j * tileHeight - 1452, tileWidth, tileHeight), Color.Red);
                        sprite.End();
                    }
                }
            }
            return 0;
        }
    }
}
