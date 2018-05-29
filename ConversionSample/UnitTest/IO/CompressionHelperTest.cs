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
            Compress_Bytes_Text(500);
            Compress_Bytes_Text(10000);
            Compress_Bytes_Text(999999);
        }

        static void Compress_Bytes_Text(int dataSize)
        {
            var dataText = Convert.ToBase64String(CryptoHelper.GenerateBytes(dataSize));
            var data = dataText.ToBytes();

            var compressed = data.Compress();
            var decompressed = compressed.Decompress();

            if (dataSize <= 1024)
            {
                Console.WriteLine(Convert.ToBase64String(data));
                Console.WriteLine(Convert.ToBase64String(compressed));
            }
            Console.WriteLine($"Data: {data.Length} Bytes");
            Console.WriteLine($"Compressed: {compressed.Length} Bytes");

            CollectionAssert.AreEqual(data, decompressed);
        }

        [TestMethod]
        public void Compress_Stream()
        {
            Compress_Stream_Random(100);
            Compress_Stream_Random(500);
            Compress_Stream_Random(10000);
            Compress_Stream_Random(999999);
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
            Console.WriteLine($"Data: {data.Length} Bytes");
            Console.WriteLine($"Compressed: {compressed.Length} Bytes");

            CollectionAssert.AreEqual(data, decompressed);
        }
    }
}
