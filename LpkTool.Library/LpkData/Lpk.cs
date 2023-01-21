using LpkTool.Library.Helpers;
using LpkTool.Library.LpkData;
using System.Text;

namespace LpkTool.Library
{
    /// <summary>
    /// Lost Ark .lpk File
    /// </summary>
    public class Lpk
    {
        internal readonly string _key, _base;

        private const string _euKey = "83657ea6ffa1e671375c689a2e99a598";
        private const string _euBase = "1069d88738c5c75f82b44a1f0a382762";

        private const string _ruKey = "a7f33db20dfb711a16d5d3dd3d4cef4d";
        private const string _ruBase = "ee36ace0d87a9eaea565e6884a058b63";

        public Region Region { get; private set; }

        private Lpk(Region region)
        {
            Region = region;
            switch (region)
            {
                case Region.EU:
                    _key = _euKey;
                    _base = _euBase;
                    break;
                case Region.RU:
                    _key = _ruKey;
                    _base = _ruBase;
                    break;
            }
            Files = new List<LpkFileEntry>();
        }

        /// <summary>
        /// All files in the .lpk file
        /// </summary>
        public List<LpkFileEntry> Files { get; private set; }

        /// <summary>
        /// Export all files to a directory
        /// </summary>
        /// <param name="outDir"></param>
        public void Export(string outDir)
        {
            foreach (var file in Files)
            {
                var outPath = Path.Combine(outDir, file.FilePath.TrimStart("\\..\\".ToCharArray()));
                Directory.CreateDirectory(Path.GetDirectoryName(outPath)!);
                File.WriteAllBytes(outPath, file.GetData());
            }
        }

        /// <summary>
        /// Get a file by full "relative" path
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public LpkFileEntry? GetFileByPath(string filepath)
        {
            var fileEntry = Files.Where(x => x.FilePath.ToLower() == filepath.ToLower()).FirstOrDefault();
            return fileEntry;
        }

        /// <summary>
        /// Get a file by its name. (Can be misleading)
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public LpkFileEntry? GetFileByName(string filename)
        {
            var fileEntry = Files.Where(x => Path.GetFileName(x.FilePath).ToLower() == filename.ToLower()).FirstOrDefault();
            return fileEntry;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        public bool ApplySqlFiles(string[] files)
        {
            var allSucceeded = true;
            foreach (var file in files)
            {
                if (!ApplySqlFile(file))
                {
                    allSucceeded = false;
                }
            }
            return allSucceeded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        public bool ApplySqlFile(string file)
        {
            try
            {
                var sqlFileContent = File.ReadAllLines(file);
                var fileComment = sqlFileContent.Length > 0 ? sqlFileContent[0] : null;
                if (fileComment == null || !fileComment.StartsWith("--File:")) return false;
                var dbFileName = fileComment.Replace("--File:", "");
                var dbFile = GetFileByName(dbFileName);
                if (dbFile == null) return false;
                dbFile?.ApplySqlFile(file);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Add a new file
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="filePath"></param>
        /// <param name="overwriteExisting"></param>
        public void AddFile(string relativePath, string filePath, bool overwriteExisting = false)
        {
            AddFile(relativePath, File.ReadAllBytes(filePath), overwriteExisting);
        }

        /// <summary>
        /// Add a new file
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="fileData"></param>
        /// <param name="overwriteExisting"></param>
        public void AddFile(string relativePath, byte[] fileData, bool overwriteExisting = false)
        {
            var fileEntry = GetFileByPath(relativePath);
            if (fileEntry != null)
            {
                if (overwriteExisting)
                {
                    fileEntry.ReplaceData(fileData);
                }
                else
                {
                    return;
                }
            }
            else
            {
                Files.Add(new LpkFileEntry(this, relativePath, fileData, _eof));
            }
        }

        /// <summary>
        /// Repack
        /// </summary>
        /// <param name="path"></param>
        public void RepackToFile(string path)
        {
            File.WriteAllBytes(path, Repack());
        }

        /// <summary>
        /// Repack
        /// </summary>
        /// <returns></returns>
        public byte[] RepackToByteArray()
        {
            return Repack();
        }

        private byte[] Repack()
        {
            if (_isFile)
            {
                using (var stream = new FileStream(_filePath!, FileMode.Open))
                {
                    return Repack(stream);
                }
            }
            else
            {
                using (var stream = new MemoryStream(_fileBuffer!))
                {
                    return Repack(stream);
                }
            }
        }

        private byte[] Repack(Stream stream)
        {
            using (var br = new BinaryReader(stream))
            {
                using (var ms = new MemoryStream())
                {
                    using (var bw = new BinaryWriter(ms))
                    {
                        bw.BaseStream.Position = 0;
                        br.BaseStream.Position = _headerOffset;
                        foreach (var file in Files)
                        {
                            if (file.Modified)
                            {
                                var dataToCopy = file._offset - (int)br.BaseStream.Position;
                                if (dataToCopy > 0)
                                {
                                    bw.Write(br.ReadBytes(dataToCopy));
                                }
                                br.BaseStream.Seek(file._headerEntry.PaddedBLockSizeInBytes, SeekOrigin.Current);
                                var newData = file.RepackWithChanges();
                                bw.Write(newData);
                            }
                        }
                        if (br.BaseStream.Position != br.BaseStream.Length)
                        {
                            var dataToCopy = (int)br.BaseStream.Length - (int)br.BaseStream.Position;
                            bw.Write(br.ReadBytes(dataToCopy));
                        }
                        bw.BaseStream.Position = 0;
                        using (var ms2 = new MemoryStream())
                        {
                            using (var bw2 = new BinaryWriter(ms2))
                            {
                                foreach (var file in Files)
                                {
                                    var lenght = 4 + file._headerEntry.FilePathLength;
                                    bw2.Write(file._headerEntry.FilePathLength);
                                    bw2.Write(Encoding.UTF8.GetBytes(file._headerEntry.FilePath));
                                    var pad = new byte[Header.HEADER_ENTRY_SIZE - lenght - 12];
                                    bw2.Write(pad);
                                    bw2.Write(file._headerEntry.UnpackedFileSizeInBytes);
                                    bw2.Write(file._headerEntry.PaddedBLockSizeInBytes);
                                    bw2.Write(file._headerEntry.CompressedBlockSizeInBytes);
                                }
                                if (bw2.BaseStream.Length != Files.Count * Header.HEADER_ENTRY_SIZE)
                                {
                                    throw new Exception();
                                }

                                var header = ms2.ToArray();
                                var encryptedHeader = EncryptionHelper.BlowfishEncrypt(header, Encoding.UTF8.GetBytes(_key), out var _, true);

                                var body = ms.ToArray();
                                var result = new byte[encryptedHeader.Length + body.Length + 8];
                                Array.Copy(BitConverter.GetBytes(Files.Count), result, 4);
                                Array.Copy(encryptedHeader, 0, result, 4, encryptedHeader.Length);
                                Array.Copy(new byte[4], 0, result, 4 + encryptedHeader.Length, 4);
                                Array.Copy(body, 0, result, encryptedHeader.Length + 8, body.Length);
                                return result;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get a new Lpk Instance from a .lpk file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public static Lpk FromFile(string path, Region region)
        {
            var lpk = FromStream(new FileStream(path, FileMode.Open), region);
            lpk._isFile = true;
            lpk._filePath = path;
            return lpk;
        }

        /// <summary>
        /// Get a new Lpk Instance from a .lpk buffer
        /// </summary>
        /// <param name="lpkArray"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public static Lpk FromByteArray(byte[] lpkArray, Region region)
        {
            var lpk = FromStream(new MemoryStream(lpkArray), region);
            lpk._isFile = false;
            lpk._fileBuffer = lpkArray;
            return lpk;
        }

        private static Lpk FromStream(Stream stream, Region region)
        {
            var result = new Lpk(region);
            using (var br = new BinaryReader(stream))
            {
                var numberOfFiles = br.ReadInt32();
                var headerSize = numberOfFiles * Header.HEADER_ENTRY_SIZE;
                var encryptedHeader = br.ReadBytes(headerSize);
                var decryptedHeader = EncryptionHelper.BlowfishDecrypt(encryptedHeader, Encoding.UTF8.GetBytes(result._key));
                var header = Header.FromByteArray(decryptedHeader);
                var offset = headerSize + 8;
                result._headerOffset = offset;
                for (var i = 0; i < numberOfFiles; i++)
                {
                    var fileHeaderEntry = header.Entries[i];
                    result.Files.Add(new LpkFileEntry(result, fileHeaderEntry, offset));
                    offset += fileHeaderEntry.PaddedBLockSizeInBytes;
                }
                result._eof = (int)br.BaseStream.Length;
            }
            return result;
        }
        private int _eof;
        private int _headerOffset;
        internal string? _filePath;
        internal byte[]? _fileBuffer;
        internal bool _isFile = false;
    }
}
