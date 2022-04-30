using System.Text;

namespace LpkTool.Library.Models
{
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

        public string FilePath => _headerEntry.FilePath;

        private byte[] _newData;
        public bool Modified => _newData != null;

        public byte[] GetData(BinaryReader br)
        {
            if (_newData != null) return _newData;
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

        public void ReplaceData(byte[] data)
        {
            _newData = data;
        }

        public byte[] RepackWithChanges()
        {
            if (!Modified) throw new Exception("Check for changes elsewhere!");
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
            //TODO: Oh hell no
            throw new NotImplementedException();
        }

        private static byte[] EncryptNonDbBlock(ref HeaderEntry fileHeader, byte[] newData)
        {
            fileHeader.UnpackedFileSizeInBytes = newData.Length;
            var compressedBlock = CompressionHelper.Inflate(newData, newData.Length, out int sizeCompressed, 7);
            fileHeader.CompressedBlockSizeInBytes = sizeCompressed;
            var encryptedBlock = EncryptionHelper.BlowfishEncrypt(compressedBlock, Encoding.UTF8.GetBytes(Lpk._key), out int paddedSize);
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
            string dbName = GetDbName(fileHeader.FilePath);
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