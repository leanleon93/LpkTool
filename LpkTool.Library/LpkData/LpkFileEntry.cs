using LpkTool.Library.Helpers;
using System.Text;

namespace LpkTool.Library
{
    /// <summary>
    /// Lost Ark .lpk File Record
    /// </summary>
    public class LpkFileEntry
    {

        internal LpkFileEntry(HeaderEntry headerEntry, int offset)
        {
            _headerEntry = headerEntry;
            _offset = offset;
        }

        internal LpkFileEntry(string relativePath, byte[] data, int eof)
        {
            _headerEntry = new HeaderEntry(relativePath);
            _offset = eof;
            ReplaceData(data);
        }

        /// <summary>
        /// The relative file path
        /// </summary>
        public string FilePath => _headerEntry.FilePath;

        private byte[]? _newData;
        internal bool Modified => _newData != null;

        /// <summary>
        /// Get the decrypted, decompressed data
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        public byte[] GetData(BinaryReader br)
        {
            if (_newData != null)
            {
                return _newData;
            }

            br.BaseStream.Position = _offset;
            if (_headerEntry.CompressedBlockSizeInBytes != 0)
            {
                return DecryptNonDbBlock(_headerEntry, br);
            }
            else
            {
                return DecryptDbBlock(_headerEntry, br);
            }
        }

        /// <summary>
        /// Replace the data from a file
        /// </summary>
        /// <param name="path"></param>
        public void ReplaceData(string path)
        {
            ReplaceData(File.ReadAllBytes(path));
        }

        /// <summary>
        /// Replace the data from a buffer
        /// </summary>
        /// <param name="data"></param>
        public void ReplaceData(byte[] data)
        {
            _newData = data;
        }

        internal byte[] RepackWithChanges()
        {
            if (!Modified || _newData == null)
            {
                throw new Exception("Check for changes elsewhere!");
            }

            if (_headerEntry.CompressedBlockSizeInBytes != 0)
            {
                var data = EncryptNonDbBlock(ref _headerEntry, _newData);
                return data;
            }
            else
            {
                var data = EncryptDbBlock(ref _headerEntry, _newData);
                return data;
            }
        }

        private static byte[] EncryptDbBlock(ref HeaderEntry headerEntry, byte[] newData)
        {
            //TODO: Implement .db Repack
            throw new NotImplementedException();
        }

        private static byte[] EncryptNonDbBlock(ref HeaderEntry fileHeader, byte[] newData)
        {
            fileHeader.UnpackedFileSizeInBytes = newData.Length;
            var compressedBlock = CompressionHelper.Inflate(newData, newData.Length, out var sizeCompressed, 7);
            fileHeader.CompressedBlockSizeInBytes = sizeCompressed;
            var encryptedBlock = EncryptionHelper.BlowfishEncrypt(compressedBlock, Encoding.UTF8.GetBytes(Lpk._key), out var paddedSize);
            fileHeader.PaddedBLockSizeInBytes = paddedSize;
            return encryptedBlock;
        }

        private static byte[] DecryptNonDbBlock(HeaderEntry fileHeader, BinaryReader br)
        {
            var paddedEncryptedBlock = br.ReadBytes(fileHeader.PaddedBLockSizeInBytes);
            var decryptedCompressedBlock = EncryptionHelper.BlowfishDecrypt(paddedEncryptedBlock, Encoding.UTF8.GetBytes(Lpk._key));
            var unpadded = new byte[fileHeader.CompressedBlockSizeInBytes];
            Array.Copy(decryptedCompressedBlock, unpadded, unpadded.Length);
            var decompressedBlock = CompressionHelper.Deflate(unpadded, fileHeader.UnpackedFileSizeInBytes);
            return decompressedBlock;
        }

        private static byte[] DecryptDbBlock(HeaderEntry fileHeader, BinaryReader br)
        {
            var dbName = GetDbName(fileHeader.FilePath);
            var encryptedBlock = br.ReadBytes(fileHeader.PaddedBLockSizeInBytes);
            var decryptedBlock = EncryptionHelper.AesDecrypt(encryptedBlock, dbName);
            var unpadded = new byte[fileHeader.UnpackedFileSizeInBytes];
            Array.Copy(decryptedBlock, unpadded, unpadded.Length);
            return unpadded;
        }


        private static string GetDbName(string filePath)
        {
            var underscoreIndex = filePath.LastIndexOf('_');
            var name = filePath.Substring(underscoreIndex + 1);
            var dotIndex = name.IndexOf('.');
            name = name.Substring(0, dotIndex);
            return name;
        }



        internal int _offset;
        internal HeaderEntry _headerEntry;
    }
}