using System;
using ConversionLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class ValueHelperTest
    {
        [TestMethod]
        public void To_1()
        {
            Assert.AreEqual(true, true.To<bool>());
            Assert.AreEqual(true, "True".To<bool>());
            Assert.AreEqual(true, 1.To<bool>());
            Assert.AreEqual(1, true.To<int>());
            Assert.AreEqual(123, "123".To<int>());
            Assert.AreEqual(123.456, "123.456".To<double>());
            Assert.AreEqual((short)65, 'A'.To<short>());
            Assert.AreEqual('A', 65.To<char>());
            Assert.AreEqual('A', "A".To<char>());
            Assert.AreEqual(new DateTime(2000, 1, 1, 10, 11, 12), "2000-01-01 10:11:12".To<DateTime>());
            Assert.AreEqual(TimeSpan.FromDays(123), 123.To<TimeSpan>());
            Assert.AreEqual(new TimeSpan(1, 2, 30, 40), "1.02:30:40".To<TimeSpan>());
            Assert.AreEqual(new DateTimeOffset(new DateTime(2000, 1, 1, 10, 11, 12), TimeSpan.FromHours(9)), "2000-01-01 10:11:12 +09:00".To<DateTimeOffset>());
            Assert.AreEqual(ConsoleColor.Cyan, "cyan".To<ConsoleColor>());
            Assert.AreEqual(ConsoleColor.Cyan, 11.To<ConsoleColor>());
            Assert.AreEqual(ConsoleColor.Cyan, "11".To<ConsoleColor>());
        }

        [TestMethod]
        public void To_Nullable()
        {
            Assert.AreEqual(null, ((string)null).To<bool?>());
            Assert.AreEqual(false, "False".To<bool?>());
            Assert.AreEqual(123, "123".To<int?>());
            Assert.AreEqual(123.456, "123.456".To<double?>());
            Assert.AreEqual((short)65, 'A'.To<short?>());
            Assert.AreEqual('A', 65.To<char?>());
            Assert.AreEqual(new DateTime(2000, 1, 1, 10, 11, 12), "2000-01-01 10:11:12".To<DateTime?>());
            Assert.AreEqual(new TimeSpan(1, 2, 30, 40), "1.02:30:40".To<TimeSpan?>());
            Assert.AreEqual(ConsoleColor.Cyan, "Cyan".To<ConsoleColor?>());
        }
    }
}
