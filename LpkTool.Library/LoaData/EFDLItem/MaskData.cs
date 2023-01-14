namespace LpkTool.Library.LoaData.EFDLItem
{
    public class MaskData : LoaSubclass
    {
        public MaskData()
        {
        }

        public MaskData(BinaryReader br) : base(br)
        {
        }

        public string ArrayKeyword { get; set; }
        public string SecondKeyword { get; set; }

        public LoaProp<int> PartnerPartsNew { get; set; }
        public LoaProp<int> MaxStage { get; set; }
        public LoaProp<int> MaskStage { get; set; }
        public LoaProp<int> MaskOwnLevel { get; set; }
        public LoaProp<string> ParamName { get; set; }

        public override byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.WriteStringLoa(ArrayKeyword);
                    bw.WriteStringLoa(SecondKeyword);
                    bw.WriteLoaProp(PartnerPartsNew);
                    bw.WriteLoaProp(MaxStage);
                    bw.WriteLoaProp(MaskStage);
                    bw.WriteLoaProp(MaskOwnLevel);
                    bw.WriteLoaProp(ParamName);
                }
                return ms.ToArray();
            }
        }

        protected override void Deserialize(BinaryReader br)
        {
            ArrayKeyword = br.ReadStringLoa();
            SecondKeyword = br.ReadStringLoa();
            PartnerPartsNew = br.ReadLoaProp(PartnerPartsNew);
            MaxStage = br.ReadLoaProp(MaxStage);
            MaskStage = br.ReadLoaProp(MaskStage);
            MaskOwnLevel = br.ReadLoaProp(MaskOwnLevel);
            ParamName = br.ReadLoaProp(ParamName);
        }
    }
}