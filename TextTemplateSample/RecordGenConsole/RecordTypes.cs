using System;

namespace RecordGenConsole
{
    public partial class Person
    {
        public int Id { get; }
        public string Name { get; }
        public DateTime Birthday { get; }
        public Uri Uri { get; }

        public Person(int id, string name, DateTime birthday, Uri uri)
        {
            Id = id;
            Name = name;
            Birthday = birthday;
            Uri = uri;
        }
    }

    public partial class Empty
    {

        public Empty()
        {
        }
    }

    public partial class Number
    {
        public int N { get; }
        public double? Scale { get; }
        public DayOfWeek DayOfWeek { get; }

        public Number(int n, double? scale, DayOfWeek dayOfWeek)
        {
            N = n;
            Scale = scale;
            DayOfWeek = dayOfWeek;
        }
    }

}
