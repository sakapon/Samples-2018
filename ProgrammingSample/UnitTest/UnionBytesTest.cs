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

            var bytes1_u = Union32(bytes1);
            var bytes2_u = Union32(bytes2);

            var bytes3_u = bytes1_u.Zip(bytes2_u, (x, y) => x + y).ToArray();
            var bytes3 = Spread32(bytes3_u);
        }

        public static Bytes32[] Union32(byte[] bytes) => Enumerable.Range(0, bytes.Length / 4).Select(i => new Bytes32(bytes, 4 * i)).ToArray();

        public static byte[] Spread32(Bytes32[] bytes)
        {
            var result = new byte[4 * bytes.Length];
            for (var i = 0; i < bytes.Length; i++)
                bytes[i].CopyTo(result, 4 * i);
            return result;
        }
    }
}
