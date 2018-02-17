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
            Assert.AreEqual(true, "True".To<bool>());
            Assert.AreEqual(123, "123".To<int>());
            Assert.AreEqual(123.456, "123.456".To<double>());
            Assert.AreEqual(new DateTime(2012, 1, 1, 10, 11, 12), "2012-01-01 10:11:12".To<DateTime>());
            Assert.AreEqual(new TimeSpan(1, 2, 30, 40), "1.02:30:40".To<TimeSpan>());
            Assert.AreEqual(ConsoleColor.Cyan, "Cyan".To<ConsoleColor>());
            Assert.AreEqual(ConsoleColor.Cyan, "11".To<ConsoleColor>());
        }
    }
}
