using System;
using System.IO;
using Ionic.Zlib;

namespace LpkTool.Library
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
        public static byte[] Deflate(byte[] buffer, int sizeDecompressed)
        {
            var array = ZlibStream.UncompressBuffer(buffer);
            var array2 = new byte[sizeDecompressed];
            if (array.Length > sizeDecompressed)
                Array.Copy(array, 0, array2, 0, sizeDecompressed);
            else
                Array.Copy(array, 0, array2, 0, array.Length);
            return array2;
        }

        /// <summary>
        ///     Compress buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="sizeDecompressed"></param>
        /// <param name="sizeCompressed"></param>
        /// <param name="compressionLevel"></param>
        /// <returns></returns>
        public static byte[] Inflate(byte[] buffer, int sizeDecompressed, out int sizeCompressed, int compressionLevel = 6)
        {
            if (compressionLevel > 9) compressionLevel = 9;
            if (compressionLevel < 0) compressionLevel = 0;
            var memoryStream = new MemoryStream();
            var zlibStream = new ZlibStream(memoryStream, CompressionMode.Compress, (CompressionLevel)compressionLevel,
                true);
            zlibStream.Write(buffer, 0, sizeDecompressed);
            zlibStream.Flush();
            zlibStream.Close();
            sizeCompressed = (int)memoryStream.Length;
            return memoryStream.ToArray();
        }
    }
}