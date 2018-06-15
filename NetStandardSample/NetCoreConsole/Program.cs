using System;
using NetStandardLib;

namespace NetCoreConsole
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
