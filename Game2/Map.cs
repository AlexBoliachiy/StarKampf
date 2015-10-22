using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2
{
    class Map
    {
        private int [,] arr;
        public int this[int i,int j] { get { return arr[i, j];  } }
        public  Map()
        {
            
        }
    }
}
