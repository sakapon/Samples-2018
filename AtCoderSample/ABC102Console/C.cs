using System;
using System.Linq;

class C
{
    static void Main()
    {
        var n = int.Parse(Console.ReadLine());
        var a = Console.ReadLine().Split(' ').Select((s, i) => int.Parse(s) - i - 1).ToArray();
        var b = a.OrderBy(x => x).ElementAt(n / 2);
        Console.WriteLine(a.Sum(x => (long)Math.Abs(x - b)));
    }
}
