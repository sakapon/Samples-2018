using System;
using NetStandardLib;
using Newtonsoft.Json;

namespace NetCoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(JsonConvert.SerializeObject(RandomHelper.GenerateBase64(16)));
            Console.WriteLine("Press [Enter] key to exit.");
            Console.ReadLine();
        }
    }
}
