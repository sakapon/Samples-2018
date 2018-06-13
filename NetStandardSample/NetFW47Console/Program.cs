using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetStandardLib;

namespace NetFW47Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(RandomHelper.GenerateBase64(16));
            Console.WriteLine("Press [Enter] key to exit.");
            Console.ReadLine();
        }
    }
}
