using System;
using System.Linq;

class B
{
    static void Main()
    {
        Console.ReadLine();
        var a = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
        Console.WriteLine(a.Max() - a.Min());
    }
}
