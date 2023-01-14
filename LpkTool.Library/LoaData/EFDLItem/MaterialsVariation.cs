namespace LpkTool.Library.LoaData.EFDLItem
{
    public class MaterialsVariation : LoaSubclass
    {
        public MaterialsVariation()
        {
        }

        public MaterialsVariation(BinaryReader br) : base(br)
        {
        }

        public string ArrayKeyword { get; set; }
        public string SecondKeyword { get; set; }

        public LoaProp<int> TargetIndex { get; set; }

        public LoaProp<Color> DiffuseColor { get; set; }
        public LoaProp<Color> DiffuseColor_A { get; set; }
        public LoaProp<Color> DiffuseColor_B { get; set; }
        public LoaProp<Color> DiffuseColor_C { get; set; }

        public LoaProp<string> MaskVariation_1 { get; set; }
        public LoaProp<string> MaskVariation_2 { get; set; }
        public LoaProp<string> MaskVariation_3 { get; set; }
        public LoaProp<string> MaskVariation_4 { get; set; }


        public override byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.WriteStringLoa(ArrayKeyword);
                    bw.WriteStringLoa(SecondKeyword);
                    bw.WriteLoaProp(TargetIndex);
                    bw.WriteLoaProp(DiffuseColor);
                    bw.WriteLoaProp(DiffuseColor_A);
                    bw.WriteLoaProp(DiffuseColor_B);
                    bw.WriteLoaProp(DiffuseColor_C);
                    bw.WriteLoaProp(MaskVariation_1);
                    bw.WriteLoaProp(MaskVariation_2);
                    bw.WriteLoaProp(MaskVariation_3);
                    bw.WriteLoaProp(MaskVariation_4);
                }
                return ms.ToArray();
            }
        }

        protected override void Deserialize(BinaryReader br)
        {
            ArrayKeyword = br.ReadStringLoa();
            SecondKeyword = br.ReadStringLoa();
            TargetIndex = br.ReadLoaProp(TargetIndex);
            DiffuseColor = br.ReadLoaProp(DiffuseColor);
            DiffuseColor_A = br.ReadLoaProp(DiffuseColor_A);
            DiffuseColor_B = br.ReadLoaProp(DiffuseColor_B);
            DiffuseColor_C = br.ReadLoaProp(DiffuseColor_C);
            MaskVariation_1 = br.ReadLoaProp(MaskVariation_1);
            MaskVariation_2 = br.ReadLoaProp(MaskVariation_2);
            MaskVariation_3 = br.ReadLoaProp(MaskVariation_3);
            MaskVariation_4 = br.ReadLoaProp(MaskVariation_4);
        }
    }
}