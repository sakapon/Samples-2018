using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace UnitTest
{
    [StructLayout(LayoutKind.Explicit)]
    [DebuggerDisplay(@"\{{All}\}")]
    public struct Bytes32
    {
        [FieldOffset(0)]
        public uint All;

        [FieldOffset(0)]
        public byte Byte0;
        [FieldOffset(1)]
        public byte Byte1;
        [FieldOffset(2)]
        public byte Byte2;
        [FieldOffset(3)]
        public byte Byte3;

        public Bytes32(byte[] bytes, int start)
        {
            All = 0;
            Byte0 = bytes[start];
            Byte1 = bytes[start + 1];
            Byte2 = bytes[start + 2];
            Byte3 = bytes[start + 3];
        }

        Bytes32(uint all)
        {
            Byte0 = 0;
            Byte1 = 0;
            Byte2 = 0;
            Byte3 = 0;
            All = all;
        }

        public void CopyTo(byte[] bytes, int start)
        {
            bytes[start] = Byte0;
            bytes[start + 1] = Byte1;
            bytes[start + 2] = Byte2;
            bytes[start + 3] = Byte3;
        }

        public static Bytes32 operator +(Bytes32 x, Bytes32 y) => new Bytes32(x.All + y.All);

        public static Bytes32[] Union(byte[] bytes) => Enumerable.Range(0, bytes.Length / 4)
            .Select(i => new Bytes32(bytes, 4 * i))
            .ToArray();

        public static byte[] Spread(Bytes32[] bytes)
        {
            var result = new byte[4 * bytes.Length];
            for (var i = 0; i < bytes.Length; i++)
                bytes[i].CopyTo(result, 4 * i);
            return result;
        }
    }
}
