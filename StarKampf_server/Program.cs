using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarKampf_server
{
    class Program
    {
        static void Main(string[] args)
        {
            Kek a = new Kek() ;
            Kek b = a;
            a.i = 228;
            Console.Write(b.i);
            Console.Read();
        }
    }
    class Kek
    {
        public int i;
    }
}
