using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren;
namespace StarKampf_server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            while(true)
            {
                server.Act();
            }
        }
    }
    
}
