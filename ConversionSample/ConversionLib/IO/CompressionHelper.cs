using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ConversionLib.IO
{
    public static class CompressionHelper
    {
        // ランダムなデータに適用すると長くなります。
        // 2 回適用すると長くなります。
        public static void CompressTo(this Stream input, Stream output)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (output == null) throw new ArgumentNullException(nameof(output));

            using (var gzip = new GZipStream(output, CompressionMode.Compress, true))
            {
                input.CopyTo(gzip);
            }
        }

        public static void DecompressTo(this Stream input, Stream output)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (output == null) throw new ArgumentNullException(nameof(output));

            using (var gzip = new GZipStream(input, CompressionMode.Decompress, true))
            {
                gzip.CopyTo(output);
            }
        }

        public static byte[] Compress(this byte[] data)
        {
            using (var input = new MemoryStream(data))
            using (var output = new MemoryStream())
            {
                input.CompressTo(output);
                return output.ToArray();
            }
        }

        public static byte[] Decompress(this byte[] data)
        {
            using (var input = new MemoryStream(data))
            using (var output = new MemoryStream())
            {
                input.DecompressTo(output);
                return output.ToArray();
            }
        }

        public static void Compress(string inputPath, string outputPath)
        {
            using (var input = File.OpenRead(inputPath))
            using (var output = File.Create(outputPath))
            {
                input.CompressTo(output);
            }
        }

        public static void Decompress(string inputPath, string outputPath)
        {
            using (var input = File.OpenRead(inputPath))
            using (var output = File.Create(outputPath))
            {
                input.DecompressTo(output);
            }
        }
    }
}
