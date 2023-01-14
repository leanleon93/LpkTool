namespace LpkTool.Library.LoaData.EFDLItem
{
    public class Color : LoaSubclass
    {
        public Color()
        {
        }

        public Color(BinaryReader br) : base(br)
        {
        }

        public LoaProp<int> B { get; set; }
        public LoaProp<int> G { get; set; }
        public LoaProp<int> R { get; set; }
        public LoaProp<int> A { get; set; }

        public override byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.WriteLoaProp(B);
                    bw.WriteLoaProp(G);
                    bw.WriteLoaProp(R);
                    bw.WriteLoaProp(A);
                }
                return ms.ToArray();
            }
        }

        protected override void Deserialize(BinaryReader br)
        {
            B = br.ReadLoaProp(B);
            G = br.ReadLoaProp(G);
            R = br.ReadLoaProp(R);
            A = br.ReadLoaProp(A);
        }
    }
}