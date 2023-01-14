namespace LpkTool.Library.LoaData.EFDLItem
{
    public class Vector : LoaSubclass
    {
        public Vector()
        {
        }

        public Vector(BinaryReader br) : base(br)
        {
        }

        public LoaProp<float> X { get; set; }
        public LoaProp<float> Y { get; set; }
        public LoaProp<float> Z { get; set; }

        public override byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.WriteLoaProp(X);
                    bw.WriteLoaProp(Y);
                    bw.WriteLoaProp(Z);
                }
                return ms.ToArray();
            }
        }

        protected override void Deserialize(BinaryReader br)
        {
            X = br.ReadLoaProp(X);
            Y = br.ReadLoaProp(Y);
            Z = br.ReadLoaProp(Z);
        }
    }
}