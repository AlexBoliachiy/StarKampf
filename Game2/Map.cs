using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
namespace Game2
{
    class Map
    {
        private static readonly char[] WspaceDelimiters = new char[] { };

        private int[,] map;
        public int height, width;

        public int this[int i, int j] { get { return map[i, j]; } }

        public int tileWidth = 256;
        public int tileHeight = 256;



        public Map()
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
        }
    }
}
