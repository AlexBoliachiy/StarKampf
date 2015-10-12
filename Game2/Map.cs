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
            /* Идея такая. 
            1.Карта хранится у клиента. 
            2.Сервер отправляет строку какую карту надо грузить 
            3.В конструкторе происходит подсчитывание количества 1-ых битов 
               и сверяем их с количеством таких же битов у сервера
            4. Если равно, то ок, если нет, то бан по айпи за изменение файлов игры
            У этой идеи есть свои недостатки
            */
        }
    }
}
