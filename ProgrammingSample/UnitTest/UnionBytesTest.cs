using System;
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

            var bytes3_u = bytes1_u.Zip(bytes2_u, (x, y) => x + y).ToArray();
            var bytes3 = Bytes32.Spread(bytes3_u);
        }

        [TestMethod]
        public void Bytes_32_Time()
        {
            var bytes1 = Enumerable.Range(0, 32).Select(i => (byte)i).ToArray();
            var bytes2 = Enumerable.Range(0, 32).Select(i => (byte)(3 * i)).ToArray();

            var result_normal = new byte[bytes1.Length];
            for (var i = 0; i < bytes1.Length; i++)
                result_normal[i] = (byte)(bytes1[i] + bytes2[i]);

            var result_union = new byte[bytes1.Length];
            for (var i = 0; i < bytes1.Length; i += 4)
                (new Bytes32(bytes1, i) + new Bytes32(bytes2, i)).CopyTo(result_union, i);
        }
    }
}
