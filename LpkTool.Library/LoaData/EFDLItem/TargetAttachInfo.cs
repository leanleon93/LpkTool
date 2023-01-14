namespace LpkTool.Library.LoaData.EFDLItem
{
    public class TargetAttachInfo : LoaSubclass
    {
        public TargetAttachInfo()
        {
        }

        public TargetAttachInfo(BinaryReader br) : base(br)
        {
        }
        public LoaProp<int> TargetParts { get; set; }
        public LoaProp<int> TargetSocket { get; set; }
        public override byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.WriteLoaProp(TargetParts);
                    bw.WriteLoaProp(TargetSocket);
                }
                return ms.ToArray();
            }
        }

        protected override void Deserialize(BinaryReader br)
        {
            TargetParts = br.ReadLoaProp(TargetParts);
            TargetSocket = br.ReadLoaProp(TargetSocket);
        }
    }
}