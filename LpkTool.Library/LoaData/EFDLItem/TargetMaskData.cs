namespace LpkTool.Library.LoaData.EFDLItem
{
    public class TargetMaskData : LoaSubclass
    {
        public TargetMaskData()
        {
        }

        public TargetMaskData(BinaryReader br) : base(br)
        {
        }

        public string ArrayKeyword { get; set; }
        public string SecondKeyword { get; set; }

        public LoaProp<int> TargetParts { get; set; }
        public LoaProp<int> TargetMaskType { get; set; }

        public override byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.WriteStringLoa(ArrayKeyword);
                    bw.WriteStringLoa(SecondKeyword);
                    bw.WriteLoaProp(TargetParts);
                    bw.WriteLoaProp(TargetMaskType);
                }
                return ms.ToArray();
            }
        }

        protected override void Deserialize(BinaryReader br)
        {
            ArrayKeyword = br.ReadStringLoa();
            SecondKeyword = br.ReadStringLoa();
            TargetParts = br.ReadLoaProp(TargetParts);
            TargetMaskType = br.ReadLoaProp(TargetMaskType);
        }
    }
}