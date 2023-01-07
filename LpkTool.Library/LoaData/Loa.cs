using LpkTool.Library.LoaData.Table_MovieData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpkTool.Library.LoaData
{
    public abstract class Loa
    {
        public byte[] Magic { get; set; }
        public int FileId { get; set; }
        public int FileSecondaryId { get; set; }
        public string Unk { get; set; }

        public Loa() { }

        public Loa(Stream stream)
        {
            Deserialize(stream);
        }

        public Loa(string filePath)
        {
            Deserialize(File.OpenRead(filePath));
        }

        public Loa(byte[] data)
        {
            Deserialize(new MemoryStream(data));
        }

        private void Deserialize(Stream stream)
        {
            using (var br = new BinaryReader(stream))
            {
                Magic = br.ReadBytes(4);
                FileId = br.ReadInt32();
                FileSecondaryId = br.ReadInt32();
                Unk = br.ReadStringLoa();
                DeserializeDetails(br);
            }
        }

        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(Magic);
                    bw.Write(FileId);
                    bw.Write(FileSecondaryId);
                    bw.WriteStringLoa(Unk);
                    SerializeDetails(bw);
                }
                return ms.ToArray();
            }
        }

        protected abstract void SerializeDetails(BinaryWriter bw);

        protected abstract void DeserializeDetails(BinaryReader br);

    }
}
