namespace LpkTool.Library.LoaData.EFDLItem
{
    public class ParticleSystemParam : LoaSubclass
    {
        public ParticleSystemParam()
        {
        }

        public ParticleSystemParam(BinaryReader br) : base(br)
        {
        }

        public string ArrayKeyword { get; set; }
        public string SecondKeyword { get; set; }

        public LoaProp<string> Name { get; set; }
        public LoaProp<int> ParamType { get; set; }
        public LoaProp<int> Scalar { get; set; }
        public LoaProp<int> Scalar_Low { get; set; }
        public LoaProp<Vector> Vector { get; set; }
        public LoaProp<Vector> Vector_Low { get; set; }
        public LoaProp<Color> Color { get; set; }
        public LoaProp<string> Material { get; set; }
        public LoaProp<string> SocketName { get; set; }
        public LoaProp<bool> UseBone { get; set; }

        public override byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.WriteStringLoa(ArrayKeyword);
                    bw.WriteStringLoa(SecondKeyword);
                    bw.WriteLoaProp(Name);
                    bw.WriteLoaProp(ParamType);
                    bw.WriteLoaProp(Scalar);
                    bw.WriteLoaProp(Scalar_Low);
                    bw.WriteLoaProp(Vector);
                    bw.WriteLoaProp(Vector_Low);
                    bw.WriteLoaProp(Color);
                    bw.WriteLoaProp(Material);
                    bw.WriteLoaProp(SocketName);
                    bw.WriteLoaProp(UseBone);
                }
                return ms.ToArray();
            }
        }

        protected override void Deserialize(BinaryReader br)
        {
            ArrayKeyword = br.ReadStringLoa();
            SecondKeyword = br.ReadStringLoa();
            Name = br.ReadLoaProp(Name);
            ParamType = br.ReadLoaProp(ParamType);
            Scalar = br.ReadLoaProp(Scalar);
            Scalar_Low = br.ReadLoaProp(Scalar_Low);
            Vector = br.ReadLoaProp(Vector);
            Vector_Low = br.ReadLoaProp(Vector_Low);
            Color = br.ReadLoaProp(Color);
            Material = br.ReadLoaProp(Material);
            SocketName = br.ReadLoaProp(SocketName);
            UseBone = br.ReadLoaProp(UseBone);
        }
    }
}
