﻿using LpkTool.Library.Helpers;
using Microsoft.Data.Sqlite;
using System.Text;

namespace LpkTool.Library
{
    /// <summary>
    /// Lost Ark .lpk File Record
    /// </summary>
    public class LpkFileEntry
    {

        internal LpkFileEntry(Lpk lpk, HeaderEntry headerEntry, int offset)
        {
            _lpk = lpk;
            _headerEntry = headerEntry;
            _offset = offset;
            _guid = Guid.NewGuid().ToString();
        }

        internal LpkFileEntry(Lpk lpk, string relativePath, byte[] data, int eof)
        {
            _lpk = lpk;
            _headerEntry = new HeaderEntry(relativePath);
            _offset = eof;
            _guid = Guid.NewGuid().ToString();
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
        /// <returns></returns>
        public byte[] GetData()
        {
            if (_newData != null)
            {
                return _newData;
            }
            using (Stream stream = _lpk._isFile ? new FileStream(_lpk._filePath!, FileMode.Open) : new MemoryStream(_lpk._fileBuffer!))
            using (var br = new BinaryReader(stream))
            {
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
                FixDbData(ref _newData);
                var data = EncryptDbBlock(ref _headerEntry, _newData);
                return data;
            }
        }
        private readonly string _guid;
        private string TempDir => Path.Combine(Path.GetTempPath(), "lpktool_" + _guid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        public void ApplySqlFiles(string[] files)
        {
            foreach (var file in files)
            {
                ApplySqlFile(file);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        public void ApplySqlFile(string file)
        {
            var dbfullname = Path.GetFileName(FilePath);
            var tempFile = Path.Combine(TempDir, dbfullname);
            Directory.CreateDirectory(Path.GetDirectoryName(tempFile));
            if (!File.Exists(tempFile))
            {
                File.WriteAllBytes(tempFile, GetData());
            }
            using (var connection = new SqliteConnection($"Data Source={tempFile}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = File.ReadAllText(file);
                command.ExecuteNonQuery();
                connection.Close();
                connection.Dispose();
            }
            Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
            GC.Collect();
            ReplaceData(tempFile);
            File.Delete(tempFile);
            Directory.Delete(Path.GetDirectoryName(tempFile), true);
        }

        private static void FixDbData(ref byte[] data)
        {
            var dbPageSize = 1008;
            var dbReservedSpace = 16;
            using (var ms = new MemoryStream(data))
            {
                using (var bw = new BinaryWriter(ms))
                {
                    while (bw.BaseStream.Length > bw.BaseStream.Position + dbPageSize)
                    {
                        bw.BaseStream.Position += dbPageSize;
                        var pos = bw.BaseStream.Position;
                        bw.Write(Enumerable.Repeat<byte>(0x10, dbReservedSpace).ToArray());
                        bw.BaseStream.Position = pos + dbReservedSpace;
                    }
                }
            }
        }

        private byte[] EncryptDbBlock(ref HeaderEntry fileHeader, byte[] newData)
        {
            var dbName = GetDbName(fileHeader.FilePath);
            fileHeader.UnpackedFileSizeInBytes = newData.Length;
            var chunks = SplitIntoChunks(newData);
            var encryptedData = new byte[newData.Length];
            for (var i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                var encryptedChunk = EncryptionHelper.AesEncrypt(_lpk._base, chunk, dbName, out int _);
                Array.Copy(encryptedChunk, 0, encryptedData, (i * _maxDbChunkSize), encryptedChunk.Length);
            }
            fileHeader.PaddedBLockSizeInBytes = encryptedData.Length;
            return encryptedData;
        }

        private byte[] EncryptNonDbBlock(ref HeaderEntry fileHeader, byte[] newData)
        {
            fileHeader.UnpackedFileSizeInBytes = newData.Length;
            var compressedBlock = CompressionHelper.Inflate(newData, newData.Length, out var sizeCompressed, 7);
            fileHeader.CompressedBlockSizeInBytes = sizeCompressed;
            var encryptedBlock = EncryptionHelper.BlowfishEncrypt(compressedBlock, Encoding.UTF8.GetBytes(_lpk._key), out var paddedSize);
            fileHeader.PaddedBLockSizeInBytes = paddedSize;
            return encryptedBlock;
        }

        private byte[] DecryptNonDbBlock(HeaderEntry fileHeader, BinaryReader br)
        {
            var paddedEncryptedBlock = br.ReadBytes(fileHeader.PaddedBLockSizeInBytes);
            var decryptedCompressedBlock = EncryptionHelper.BlowfishDecrypt(paddedEncryptedBlock, Encoding.UTF8.GetBytes(_lpk._key));
            var unpadded = new byte[fileHeader.CompressedBlockSizeInBytes];
            Array.Copy(decryptedCompressedBlock, unpadded, unpadded.Length);
            var decompressedBlock = CompressionHelper.Deflate(unpadded, fileHeader.UnpackedFileSizeInBytes);
            return decompressedBlock;
        }

        private static readonly int _maxDbChunkSize = 1024;

        private byte[] DecryptDbBlock(HeaderEntry fileHeader, BinaryReader br)
        {
            var dbName = GetDbName(fileHeader.FilePath);
            var encryptedBlock = br.ReadBytes(fileHeader.PaddedBLockSizeInBytes);
            var chunks = SplitIntoChunks(encryptedBlock);
            var decryptedUnpadded = new byte[chunks.Count * _maxDbChunkSize];
            for (var i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                var decryptedChunk = EncryptionHelper.AesDecrypt(_lpk._base, chunk, dbName);
                Array.Copy(decryptedChunk, 0, decryptedUnpadded, (i * _maxDbChunkSize), chunk.Length);
            }
            var unpadded = new byte[fileHeader.UnpackedFileSizeInBytes];
            Array.Copy(decryptedUnpadded, unpadded, unpadded.Length);
            return unpadded;
        }

        private static List<byte[]> SplitIntoChunks(byte[] data)
        {
            var pos = 0;
            var remainingLength = data.Length;
            var chunks = new List<byte[]>();
            while (remainingLength >= _maxDbChunkSize)
            {
                var chunk = new byte[_maxDbChunkSize];
                Array.Copy(data, pos, chunk, 0, chunk.Length);
                chunks.Add(chunk);
                pos += _maxDbChunkSize;
                remainingLength -= _maxDbChunkSize;
            }
            return chunks;
        }

        private static string GetDbName(string filePath)
        {
            var underscoreIndex = filePath.LastIndexOf('_');
            var name = filePath.Substring(underscoreIndex + 1);
            var dotIndex = name.IndexOf('.');
            name = name.Substring(0, dotIndex);
            return name;
        }

        internal Lpk _lpk;

        internal int _offset;
        internal HeaderEntry _headerEntry;
    }
}