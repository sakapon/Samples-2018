using System;

namespace RecordGenConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var person = new Person(4, "Shiro", DateTime.UtcNow, null);

            var n = new Number(3, 2.5, DayOfWeek.Sunday);
            Console.WriteLine(n.Scaled);
        }
    }

    public partial class Number
    {
        public double Scaled => (Scale ?? 1) * N;
    }
}
