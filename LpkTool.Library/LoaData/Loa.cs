namespace LpkTool.Library.LoaData
{
    public abstract class Loa : ILoaSerializable
    {
        public byte[] Magic { get; set; }
        public int FileId { get; set; }
        public int FileSecondaryId { get; set; }
        public string Unk { get; set; }

        public Loa() { }

        public Loa(Stream stream)
        {
            using (var br = new BinaryReader(stream))
            {
                Deserialize(br);
            }
        }

        public Loa(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                using (var br = new BinaryReader(stream))
                {
                    Deserialize(br);
                }
            }
        }

        public Loa(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var br = new BinaryReader(ms))
                {
                    Deserialize(br);
                }
            }
        }

        protected abstract void Deserialize(BinaryReader br);
        public abstract byte[] Serialize();

        protected void DeserializeHeader(BinaryReader br)
        {
            Magic = br.ReadBytes(4);
            FileId = br.ReadInt32();
            FileSecondaryId = br.ReadInt32();
            Unk = br.ReadStringLoa();
        }

        protected byte[] SerializeHeader()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(Magic);
                    bw.Write(FileId);
                    bw.Write(FileSecondaryId);
                    bw.WriteStringLoa(Unk);
                }
                return ms.ToArray();
            }
        }
    }
}
