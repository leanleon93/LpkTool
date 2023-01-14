namespace LpkTool.Library.LoaData.EFDLItem
{
    public class Rotation : LoaSubclass
    {
        public Rotation()
        {
        }

        public Rotation(BinaryReader br) : base(br)
        {
        }

        public LoaProp<int> Pitch { get; set; }
        public LoaProp<int> Yaw { get; set; }
        public LoaProp<int> Roll { get; set; }

        public override byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.WriteLoaProp(Pitch);
                    bw.WriteLoaProp(Yaw);
                    bw.WriteLoaProp(Roll);
                }
                return ms.ToArray();
            }
        }

        protected override void Deserialize(BinaryReader br)
        {
            Pitch = br.ReadLoaProp(Pitch);
            Yaw = br.ReadLoaProp(Yaw);
            Roll = br.ReadLoaProp(Roll);
        }
    }
}