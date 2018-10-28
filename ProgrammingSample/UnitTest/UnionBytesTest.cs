using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnionBytesTest
    {
        [TestMethod]
        public void Bytes_32()
        {
            var bytes1 = Enumerable.Range(0, 32).Select(i => (byte)i).ToArray();
            var bytes2 = Enumerable.Range(0, 32).Select(i => (byte)(3 * i)).ToArray();

            var bytes1_u = Bytes32.Union(bytes1);
            var bytes2_u = Bytes32.Union(bytes2);

            var sum = bytes1.Zip(bytes2, (x, y) => (byte)(x + y)).ToArray();

            var bytes3_u = bytes1_u.Zip(bytes2_u, (x, y) => x + y).ToArray();
            var sum_union = Bytes32.Spread(bytes3_u);

            CollectionAssert.AreEqual(sum, sum_union);
        }

        [TestMethod]
        public void Bytes_32_Time_1()
        {
            var bytes1 = new byte[100_000_000];
            var bytes2 = new byte[100_000_000];
            for (var i = 0; i < bytes1.Length; i++)
            {
                bytes1[i] = (byte)i;
                bytes2[i] = (byte)(3 * i);
            }

            var sw = Stopwatch.StartNew();
            var sum = new byte[bytes1.Length];
            for (var i = 0; i < bytes1.Length; i++)
                sum[i] = (byte)(bytes1[i] + bytes2[i]);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }

        [TestMethod]
        public void Bytes_32_Time_2()
        {
            var bytes1 = new byte[100_000_000];
            var bytes2 = new byte[100_000_000];
            for (var i = 0; i < bytes1.Length; i++)
            {
                bytes1[i] = (byte)i;
                bytes2[i] = (byte)(3 * i);
            }

            var sw = Stopwatch.StartNew();
            var sum_union = new byte[bytes1.Length];
            for (var i = 0; i < bytes1.Length; i += 4)
                (new Bytes32(bytes1, i) + new Bytes32(bytes2, i)).CopyTo(sum_union, i);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }
    }
}
