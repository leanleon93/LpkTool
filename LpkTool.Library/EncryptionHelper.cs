using BlowFishCS;
using LpkTool.Library.Models;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

[assembly: InternalsVisibleToAttribute("LpkTool.Tests")]

namespace LpkTool.Library
{
    internal static class EncryptionHelper
    {
        internal static byte[] BlowfishEncrypt(byte[] data_in, byte[] key, out int paddedSize, bool forceNoPadding = false)
        {
            paddedSize = GetPaddedSize(data_in.Length, key.Length);
            if (forceNoPadding)
            {
                paddedSize = data_in.Length;
            }
            var data = PadData(data_in, paddedSize);
            var blowfish = new BlowFish(key);
            blowfish.CompatMode = true;
            return blowfish.Encrypt_ECB(data);
        }

        internal static byte[] BlowfishDecrypt(byte[] data, byte[] key)
        {
            var blowfish = new BlowFish(key);
            blowfish.CompatMode = true;
            return blowfish.Decrypt_ECB(data);
        }

        internal static byte[] AesEncrypt(byte[] data, string tableName, out int paddedSize)
        {
            var aesKey = GetAesKey(tableName);
            return AesEncrypt(data, aesKey, out paddedSize);
        }

        internal static byte[] AesEncrypt(byte[] data_in, byte[] aesKey, out int paddedSize)
        {
            paddedSize = GetPaddedSize(data_in.Length, aesKey.Length);
            var data = PadData(data_in, paddedSize);
            var engine = new AesEngine();
            var blockCipher = new CbcBlockCipher(engine);
            var cipher = new BufferedBlockCipher(blockCipher);
            var keyParam = new KeyParameter(aesKey);
            var keyParamWithIV = new ParametersWithIV(keyParam, new byte[16], 0, 16);
            cipher.Init(true, keyParamWithIV);
            return cipher.DoFinal(data);
        }

        private static byte[] PadData(byte[] data_in, int paddedSize)
        {
            var data = new byte[paddedSize];
            Array.Copy(data_in, data, data_in.Length);
            var paddingSize = paddedSize - data_in.Length;
            if (paddingSize != 0)
            {
                var padding = new byte[paddingSize];
                Array.Copy(padding, 0, data, data_in.Length, padding.Length);
            }
            return data;
        }

        internal static byte[] AesDecrypt(byte[] data, string tableName)
        {
            var aesKey = GetAesKey(tableName);
            return AesDecrypt(data, aesKey);
        }

        internal static byte[] AesDecrypt(byte[] data, byte[] aesKey)
        {
            var engine = new AesEngine();
            var blockCipher = new CbcBlockCipher(engine);
            var cipher = new BufferedBlockCipher(blockCipher);
            var keyParam = new KeyParameter(aesKey);
            var keyParamWithIV = new ParametersWithIV(keyParam, new byte[16], 0, 16);
            cipher.Init(false, keyParamWithIV);
            return cipher.DoFinal(data);
        }

        internal static byte[] GetAesKey(string tableName)
        {
            var baseArray = Convert.FromHexString(Lpk._base);
            var dbNameHash = GetMD5(Encoding.Unicode.GetBytes(tableName));
            var xorResult = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                var tmp = 15 - i;
                var baseChar = baseArray[i];
                var hashChar = dbNameHash[tmp];
                var xordChar = baseChar ^ hashChar;
                xorResult[i] = (byte)xordChar;
            }
            var xorResultString = Convert.ToHexString(xorResult).ToLower();
            return GetSha256(Encoding.UTF8.GetBytes(xorResultString));
        }

        private static int GetPaddedSize(int size, int blockSize)
        {
            var result = size;
            while (result % blockSize != 0)
            {
                result++;
            }
            return result;
        }

        private static byte[] GetSha256(byte[] message)
        {
            SHA256 sHA256 = SHA256.Create();
            return sHA256.ComputeHash(message);
        }

        private static byte[] GetMD5(byte[] message)
        {
            MD5 hashString = MD5.Create();
            return hashString.ComputeHash(message);
        }
    }
}
