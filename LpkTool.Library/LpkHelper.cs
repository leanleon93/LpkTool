
using BlowFishCS;
using LpkTool.Library.Models;
using System.Security.Cryptography;
using System.Text;

namespace LpkTool.Library
{
    public class LpkHelper
    {
        private static string _key = "83657ea6ffa1e671375c689a2e99a598";
        private static string _base = "1069d88738c5c75f82b44a1f0a382762";

        public static void DecryptFile(string file)
        {
            using (var fs = new FileStream(file, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    var numberOfFiles = br.ReadInt32();
                    var headerSize = numberOfFiles * Header.HEADER_ENTRY_SIZE;
                    var encryptedHeader = br.ReadBytes(headerSize);
                    var blowfish = new BlowFish(Encoding.UTF8.GetBytes(_key));
                    blowfish.CompatMode = true;
                    var decryptedHeader = blowfish.Decrypt_ECB(encryptedHeader);
                    File.WriteAllBytes(@"C:\Users\leanw\Documents\quickbms\c.bin", decryptedHeader);
                    var header = Header.FromByteArray(decryptedHeader);
                    br.BaseStream.Seek(4, SeekOrigin.Current); //4byte 0 padding after header
                    var offset = headerSize + 8;
                    for (int i = 0; i < numberOfFiles; i++)
                    {
                        br.BaseStream.Position = offset;
                        var fileHeader = header.Entries[i];
                        if (fileHeader.CompressedBlockSizeInBytes != 0)
                        {
                            DecryptNonDbBlock(fileHeader, br);
                        }
                        else
                        {
                            DecryptDbBlock(fileHeader, br);
                        }
                        offset += fileHeader.PaddedBLockSizeInBytes;
                    }
                }
            }
        }

        private static void DecryptDbBlock(HeaderEntry fileHeader, BinaryReader br)
        {
            var baseArray = Convert.FromHexString(_base);
            var baseString = Encoding.UTF8.GetString(baseArray);
            string dbName = GetDbName(fileHeader.FilePath);

            var dbNameHash = GetMD5(Encoding.Unicode.GetBytes(dbName));
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
            var aesKey = GetSha256(Encoding.UTF8.GetBytes(xorResultString));
            var encryptedBlock = br.ReadBytes(fileHeader.PaddedBLockSizeInBytes);
            File.WriteAllBytes(@"C:\Users\leanw\Documents\quickbms\d.bin", encryptedBlock);
            var decryptedBlock = AES.Decrypt(encryptedBlock, aesKey);
            var unpadded = new byte[fileHeader.UnpackedFileSizeInBytes];
            Array.Copy(decryptedBlock, unpadded, unpadded.Length);
            File.WriteAllBytes(@"C:\Users\leanw\Documents\quickbms\e.bin", unpadded);
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

        private static string GetDbName(string filePath)
        {
            var underscoreIndex = filePath.LastIndexOf('_');
            var name = filePath.Substring(underscoreIndex + 1);
            var dotIndex = name.IndexOf('.');
            name = name.Substring(0, dotIndex);
            return name;
        }

        private static void DecryptNonDbBlock(HeaderEntry fileHeader, BinaryReader br)
        {
            var paddedEncryptedBlock = br.ReadBytes(fileHeader.PaddedBLockSizeInBytes);
            var blowfish = new BlowFish(Encoding.UTF8.GetBytes(_key));
            blowfish.CompatMode = true;
            var decryptedCompressedBlock = blowfish.Decrypt_ECB(paddedEncryptedBlock);
            File.WriteAllBytes(@"C:\Users\leanw\Documents\quickbms\d.bin", decryptedCompressedBlock);
            var unpadded = new byte[fileHeader.CompressedBlockSizeInBytes];
            Array.Copy(decryptedCompressedBlock, unpadded, unpadded.Length);
            var decompressedBlock = CompressionHelper.Deflate(unpadded, fileHeader.UnpackedFileSizeInBytes);
            File.WriteAllBytes(@"C:\Users\leanw\Documents\quickbms\e.bin", decompressedBlock);
        }
    }
}