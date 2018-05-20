using System;
using System.IO;
using ConversionLib.Cryptography;
using ConversionLib.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.IO
{
    [TestClass]
    public class CompressionHelperTest
    {
        [TestMethod]
        public void Compress_Bytes()
        {
            Compress_Bytes_Text(100);
        }

        static void Compress_Bytes_Text(int dataSize)
        {
            var dataText = Convert.ToBase64String(CryptoHelper.GenerateBytes(dataSize));
            var data = dataText.ToBytes();

            var compressed = data.Compress();
            var decompressed = compressed.Decompress();

            if (dataSize <= 1024)
            {
                Console.WriteLine(dataText);
                Console.WriteLine(Convert.ToBase64String(compressed));
            }
            CollectionAssert.AreEqual(data, decompressed);
        }

        [TestMethod]
        public void Compress_Stream()
        {
            Compress_Stream_Random(100);
        }

        static void Compress_Stream_Random(int dataSize)
        {
            var data = CryptoHelper.GenerateBytes(dataSize);
            var compressedStream = new MemoryStream();
            var decompressedStream = new MemoryStream();

            new MemoryStream(data).CompressTo(compressedStream);
            var compressed = compressedStream.ToArray();
            new MemoryStream(compressed).DecompressTo(decompressedStream);
            var decompressed = decompressedStream.ToArray();

            if (dataSize <= 1024)
            {
                Console.WriteLine(Convert.ToBase64String(data));
                Console.WriteLine(Convert.ToBase64String(compressed));
            }
            CollectionAssert.AreEqual(data, decompressed);
        }
    }
}
