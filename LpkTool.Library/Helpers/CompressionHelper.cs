using System.IO.Compression;

namespace LpkTool.Library.Helpers
{
    /// <summary>
    ///     Helper for zlib compression
    /// </summary>
    internal static class CompressionHelper
    {
        /// <summary>
        ///     Decompress buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="sizeDecompressed"></param>
        /// <returns></returns>
        internal static byte[] Deflate(byte[] buffer, int sizeDecompressed)
        {
            using var input = new MemoryStream(buffer);
            using var zlib = new ZLibStream(input, CompressionMode.Decompress);
            using var output = new MemoryStream();
            zlib.CopyTo(output);
            var array = output.ToArray();
            var array2 = new byte[sizeDecompressed];
            Array.Copy(array, 0, array2, 0, Math.Min(array.Length, sizeDecompressed));
            return array2;
        }

        private static CompressionLevel MapCompressionLevel(int level)
        {
            if (level <= 0) return CompressionLevel.NoCompression;
            if (level <= 5) return CompressionLevel.Fastest;
            return CompressionLevel.Optimal;
        }

        /// <summary>
        ///     Compress buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="sizeDecompressed"></param>
        /// <param name="sizeCompressed"></param>
        /// <param name="compressionLevel"></param>
        /// <returns></returns>
        internal static byte[] Inflate(byte[] buffer, int sizeDecompressed, out int sizeCompressed, int compressionLevel = 6)
        {
            if (compressionLevel > 9) compressionLevel = 9;
            if (compressionLevel < 0) compressionLevel = 0;
            var memoryStream = new MemoryStream();
            using (var zlibStream = new ZLibStream(memoryStream, MapCompressionLevel(compressionLevel), true))
            {
                zlibStream.Write(buffer, 0, sizeDecompressed);
                zlibStream.Flush();
                zlibStream.Close();
            }

            sizeCompressed = (int)memoryStream.Length;
            return memoryStream.ToArray();
        }
    }
}