using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TCPServerLibrary;

namespace Lab1_2
{
    class Program
    {
        static void Main(string[] args)
        {
            //MyServer server = new MyServerSync(IPAddress.Parse("127.0.0.1"), 3000);
            MyServerAPM server1 = new MyServerAPM(IPAddress.Parse("127.0.0.1"), 3000);

            Console.WriteLine("Poczatek!");
            //server.Start();
            server1.Start();
            //MyServerSync.ServerUp();
            Console.WriteLine("Koniec!");
        }
    }
}
