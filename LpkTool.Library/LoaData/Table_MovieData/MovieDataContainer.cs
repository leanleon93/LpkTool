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
        public string Key { get; set; }
        public int MoviePlayType { get; set; }
        public bool LoopContainer { get; set; }
        public bool GameThreadWait { get; set; }
        public string TargetTexBindingId { get; set; }
        public bool TargetStretch { get; set; }
        public int TargetWidth { get; set; }
        public int TargetHeight { get; set; }
        public string Unk2 { get; set; }
        public MovieDataValue[] MovieDataValueArray { get; set; }

        protected override void Deserialize(BinaryReader br)
        {
            var unk = br.ReadStringLoa();
            Unk = unk;

            var key = br.ReadKvpLoa<string>();
            Key = key.Value;

            var moviePlayType = br.ReadKvpLoa<int>();
            MoviePlayType = moviePlayType.Value;

            var loopContainer = br.ReadKvpLoa<bool>();
            LoopContainer = loopContainer.Value;

            var gameThreadWait = br.ReadKvpLoa<bool>();
            GameThreadWait = gameThreadWait.Value;

            var targetTextBindingId = br.ReadKvpLoa<string>();
            TargetTexBindingId = targetTextBindingId.Value;

            var targetStretch = br.ReadKvpLoa<bool>();
            TargetStretch = targetStretch.Value;

            var targetWidth = br.ReadKvpLoa<int>();
            TargetWidth = targetWidth.Value;

            var targetHeight = br.ReadKvpLoa<int>();
            TargetHeight = targetHeight.Value;

            var unk2 = br.ReadStringLoa();
            Unk2 = unk2;

            MovieDataValueArray = new MovieDataValue[br.ReadInt32()];
            for (int j = 0; j < MovieDataValueArray.Length; j++)
            {
                var movieDataValue = new MovieDataValue(br);
                MovieDataValueArray[j] = movieDataValue;
            }
        }

        public override byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.WriteStringLoa(Unk);
                    bw.WriteKvpLoa(nameof(Key), Key);
                    bw.WriteKvpLoa(nameof(MoviePlayType), MoviePlayType);
                    bw.WriteKvpLoa(nameof(LoopContainer), LoopContainer);
                    bw.WriteKvpLoa(nameof(GameThreadWait), GameThreadWait);
                    bw.WriteKvpLoa(nameof(TargetTexBindingId), TargetTexBindingId);
                    bw.WriteKvpLoa(nameof(TargetStretch), TargetStretch);
                    bw.WriteKvpLoa(nameof(TargetWidth), TargetWidth);
                    bw.WriteKvpLoa(nameof(TargetHeight), TargetHeight);
                    bw.WriteStringLoa(Unk2);
                    bw.Write(MovieDataValueArray.Length);
                    foreach (var movieDataValue in MovieDataValueArray)
                    {
                        bw.Write(movieDataValue.Serialize());
                    }
                }
                return ms.ToArray();
            }
        }
    }
}
