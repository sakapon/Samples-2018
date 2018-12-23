using System;

namespace NumericConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Square root by the Newton's method.
            var a = 5.0;
            var x = a;

            for (var i = 0; i < 100; i++)
            {
                var xi = (x + a / x) / 2;

                if (x == xi) break;
                x = xi;
            }

            Console.WriteLine(x);
        }
    }
}
