using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class SingletonTest
    {
        [TestMethod]
        public void Singleton_1()
        {
            Parallel.For(0, 100, i =>
            {
                var o = Settings1.Default;
            });
        }

        [TestMethod]
        public void Singleton_2()
        {
            Parallel.For(0, 100, i =>
            {
                var o = Settings2.Default;
            });
        }

        [TestMethod]
        public void Singleton_3()
        {
            Parallel.For(0, 100, i =>
            {
                var o = Settings3.Default;
            });
        }

        [TestMethod]
        public void Singleton_4()
        {
            Parallel.For(0, 100, i =>
            {
                var o = Settings4.Default;
            });
        }

        [TestMethod]
        public void StaticConstructor()
        {
            var o = Settings3.Default;

            var initializer = typeof(Settings3).TypeInitializer;

            for (var i = 0; i < 5; i++)
                initializer.Invoke(null, null);
        }
    }
}
