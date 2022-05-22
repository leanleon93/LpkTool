using LpkTool.Library.LoaData;
using System.Xml.Linq;

namespace LpkTool.Library
{
    //BULLSHIT FIRST TAKE!!!
    public class Loa
    {

        public static Loa FromFile(string path)
        {
            var loa = FromStream(new FileStream(path, FileMode.Open));
            loa._isFile = true;
            loa._filePath = path;
            return loa;
        }

        public static Loa FromByteArray(byte[] loaArray)
        {
            var loa = FromStream(new MemoryStream(loaArray));
            loa._isFile = false;
            loa._fileBuffer = loaArray;
            return loa;
        }

        private byte[] _header;
        private string _fileName;
        private string _fileType;
        private Dictionary<int, List<string>> _entries;

        private static Loa FromStream(Stream stream)
        {
            var result = new Loa();
            using (var br = new BinaryReader(stream))
            {
                result._header = br.ReadBytes(12);
                result._fileName = br.ReadStringLoa();
                result._fileType = br.ReadStringLoa();
                var entryCount = br.ReadInt32();
                var allStrings = new List<string>();
                while (true)
                {
                    if (br.BaseStream.Position == br.BaseStream.Length)
                    {
                        break;
                    }
                    try
                    {
                        allStrings.Add(br.ReadStringLoa());
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Not supported");
                    }

                }
                if (allStrings.Count % entryCount != 0)
                {
                    throw new Exception("Error in deserialization");
                }
                var entryAttributeCount = allStrings.Count / entryCount;
                result._entries = new Dictionary<int, List<string>>();
                var entryIndex = 0;
                for (int i = 0; i < allStrings.Count; i += entryAttributeCount)
                {
                    var entry = new List<string>();
                    for (int j = 0; j < entryAttributeCount; j++)
                    {
                        entry.Add(allStrings[i + j]);
                    }
                    result._entries.Add(entryIndex, entry);
                    entryIndex++;
                }

                var isEof = br.BaseStream.Position == br.BaseStream.Length;
                result._eof = (int)br.BaseStream.Length;
            }
            return result;
        }

        public void ExportAsXml(string outPath)
        {
            var doc = new XDocument();
            var root = new XElement(_fileType);
            doc.Add(root);
            foreach (var entry in _entries)
            {
                var record = new XElement("record");
                root.Add(record);
                for (var i = 0; i < entry.Value.Count; i += 2)
                {
                    var key = entry.Value[i];
                    var value = entry.Value[i + 1];
                    record.Add(new XAttribute(key, value));
                }
            }
            doc.Save(outPath);
        }

        private int _eof;
        internal string? _filePath;
        internal byte[]? _fileBuffer;
        internal bool _isFile = false;
    }
}
