namespace LpkTool.Library.LoaData.EFDLItem
{
    public class SectionInfo : LoaSubclass
    {
        public SectionInfo()
        {
        }

        public SectionInfo(BinaryReader br) : base(br)
        {
        }

        public string ArrayKeyword { get; set; }
        public string SecondKeyword { get; set; }

        public LoaProp<string> StandardBoneName { get; set; }
        public LoaProp<bool> UseExcludeByAngle { get; set; }
        public LoaProp<int> StandardAxis { get; set; }
        public LoaProp<bool> UseExcludeByDistance { get; set; }
        public LoaProp<int> ViewDistance { get; set; }

        public override byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.WriteStringLoa(ArrayKeyword);
                    bw.WriteStringLoa(SecondKeyword);
                    bw.WriteLoaProp(StandardBoneName);
                    bw.WriteLoaProp(UseExcludeByAngle);
                    bw.WriteLoaProp(StandardAxis);
                    bw.WriteLoaProp(UseExcludeByDistance);
                    bw.WriteLoaProp(ViewDistance);
                }
                return ms.ToArray();
            }
        }

        protected override void Deserialize(BinaryReader br)
        {
            ArrayKeyword = br.ReadStringLoa();
            SecondKeyword = br.ReadStringLoa();
            StandardBoneName = br.ReadLoaProp(StandardBoneName);
            UseExcludeByAngle = br.ReadLoaProp(UseExcludeByAngle);
            StandardAxis = br.ReadLoaProp(StandardAxis);
            UseExcludeByDistance = br.ReadLoaProp(UseExcludeByDistance);
            ViewDistance = br.ReadLoaProp(ViewDistance);
        }
    }
}