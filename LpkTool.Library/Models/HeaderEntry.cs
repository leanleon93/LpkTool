using System.Text;

namespace LpkTool.Library.Models
{
    internal class HeaderEntry
    {
        private HeaderEntry()
        {
            FilePath = "";
        }
        internal HeaderEntry(string filepath)
        {
            FilePath = filepath;
            FilePathLength = filepath.Length;
            CompressedBlockSizeInBytes = 1;
        }
        internal int FilePathLength { get; set; }
        internal string FilePath { get; set; }
        internal int UnpackedFileSizeInBytes { get; set; }
        internal int CompressedBlockSizeInBytes { get; set; }
        internal int PaddedBLockSizeInBytes { get; set; }


        internal static HeaderEntry FromByteArray(byte[] entryArray)
        {
            var entry = new HeaderEntry();
            using (var ms = new MemoryStream(entryArray))
            {
                using (var br = new BinaryReader(ms))
                {
                    entry.FilePathLength = br.ReadInt32();
                    entry.FilePath = Encoding.UTF8.GetString(br.ReadBytes(entry.FilePathLength));
                    br.BaseStream.Position = br.BaseStream.Length - 12;
                    entry.UnpackedFileSizeInBytes = br.ReadInt32();
                    entry.PaddedBLockSizeInBytes = br.ReadInt32();
                    entry.CompressedBlockSizeInBytes = br.ReadInt32();
                }
            }
            return entry;
        }
    }
}