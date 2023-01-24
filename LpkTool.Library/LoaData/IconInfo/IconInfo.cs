namespace LpkTool.Library.LoaData.IconInfo
{
    public class IconInfo : Loa
    {
        public IconInfo()
        {
        }

        public IconInfo(Stream stream) : base(stream)
        {
        }

        public IconInfo(string filePath) : base(filePath)
        {
        }

        public IconInfo(byte[] data) : base(data)
        {
        }

        public LoaProp<List<EFIconPack>> IconInfos { get; set; }

        public override byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(base.SerializeHeader());
                    bw.WriteLoaProp(IconInfos);
                }
                return ms.ToArray();
            }
        }

        protected override void Deserialize(BinaryReader br)
        {
            base.DeserializeHeader(br);
            IconInfos = br.ReadLoaProp(IconInfos);
        }
    }
}
