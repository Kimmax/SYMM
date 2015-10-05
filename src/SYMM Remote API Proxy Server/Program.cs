using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace SYMM_Remote
{
    class Program
    {
        static void Main(string[] args)
        {
            Proxy YTProxy = new Proxy(IPAddress.Any);
            YTProxy.Run();

            Console.WriteLine("Press enter to exit..");
            Console.ReadLine();
        }
    }
}
