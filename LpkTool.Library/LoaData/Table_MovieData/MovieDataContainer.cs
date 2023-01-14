namespace LpkTool.Library.LoaData.Table_MovieData
{
    public class MovieDataContainer : LoaSubclass
    {
        public MovieDataContainer()
        {
        }

        public MovieDataContainer(BinaryReader br) : base(br)
        {
        }

        public string Unk { get; set; }
        public LoaProp<string> Key { get; set; }
        public LoaProp<int> MoviePlayType { get; set; }
        public LoaProp<bool> LoopContainer { get; set; }
        public LoaProp<bool> GameThreadWait { get; set; }
        public LoaProp<string> TargetTexBindingId { get; set; }
        public LoaProp<bool> TargetStretch { get; set; }
        public LoaProp<int> TargetWidth { get; set; }
        public LoaProp<int> TargetHeight { get; set; }
        public LoaProp<List<MovieDataValue>> MovieDataValueArray { get; set; }

        protected override void Deserialize(BinaryReader br)
        {
            var unk = br.ReadStringLoa();
            Unk = unk;

            Key = br.ReadLoaProp(Key);

            MoviePlayType = br.ReadLoaProp(MoviePlayType);

            LoopContainer = br.ReadLoaProp(LoopContainer);

            GameThreadWait = br.ReadLoaProp(GameThreadWait);

            TargetTexBindingId = br.ReadLoaProp(TargetTexBindingId);

            TargetStretch = br.ReadLoaProp(TargetStretch);

            TargetWidth = br.ReadLoaProp(TargetWidth);

            TargetHeight = br.ReadLoaProp(TargetHeight);

            MovieDataValueArray = br.ReadLoaProp(MovieDataValueArray);
        }

        public override byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.WriteStringLoa(Unk);
                    bw.WriteLoaProp(Key);
                    bw.WriteLoaProp(MoviePlayType);
                    bw.WriteLoaProp(LoopContainer);
                    bw.WriteLoaProp(GameThreadWait);
                    bw.WriteLoaProp(TargetTexBindingId);
                    bw.WriteLoaProp(TargetStretch);
                    bw.WriteLoaProp(TargetWidth);
                    bw.WriteLoaProp(TargetHeight);
                    bw.WriteLoaProp(MovieDataValueArray);
                }
                return ms.ToArray();
            }
        }
    }
}
