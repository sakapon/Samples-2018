using System;

class A
{
    static void Main()
    {
        var n = int.Parse(Console.ReadLine());
        Console.WriteLine(n % 2 == 0 ? n : 2 * n);
    }
}
