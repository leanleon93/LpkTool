namespace LpkTool.Library.LoaData.EFDLItem
{
    public class RandomTransformRotation : LoaSubclass
    {
        public RandomTransformRotation()
        {
        }

        public RandomTransformRotation(BinaryReader br) : base(br)
        {
        }

        public LoaProp<Rotation> Min { get; set; }
        public LoaProp<Rotation> Max { get; set; }

        public override byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.WriteLoaProp(Min);
                    bw.WriteLoaProp(Max);
                }
                return ms.ToArray();
            }
        }

        protected override void Deserialize(BinaryReader br)
        {
            Min = br.ReadLoaProp(Min);
            Max = br.ReadLoaProp(Max);
        }
    }
}