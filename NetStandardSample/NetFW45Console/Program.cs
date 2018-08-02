using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetStandardLib;

namespace NetFW45Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(RandomHelper.GenerateBase64(16));
            Console.WriteLine(RandomHelper2.GenerateBase64s());
            Console.WriteLine("Press [Enter] key to exit.");
            Console.ReadLine();
        }
    }
}
