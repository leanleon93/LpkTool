namespace LpkTool.Library.LoaData.Table_MovieData
{
    public class MovieDataContainer
    {
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

        internal static MovieDataContainer Deserialize(BinaryReader br)
        {
            var movieDataContainer = new MovieDataContainer();

            var unk = br.ReadStringLoa();
            movieDataContainer.Unk = unk;

            var key = br.ReadKvpLoa<string>();
            movieDataContainer.Key = key.Value;

            var moviePlayType = br.ReadKvpLoa<int>();
            movieDataContainer.MoviePlayType = moviePlayType.Value;

            var loopContainer = br.ReadKvpLoa<bool>();
            movieDataContainer.LoopContainer = loopContainer.Value;

            var gameThreadWait = br.ReadKvpLoa<bool>();
            movieDataContainer.GameThreadWait = gameThreadWait.Value;

            var targetTextBindingId = br.ReadKvpLoa<string>();
            movieDataContainer.TargetTexBindingId = targetTextBindingId.Value;

            var targetStretch = br.ReadKvpLoa<bool>();
            movieDataContainer.TargetStretch = targetStretch.Value;

            var targetWidth = br.ReadKvpLoa<int>();
            movieDataContainer.TargetWidth = targetWidth.Value;

            var targetHeight = br.ReadKvpLoa<int>();
            movieDataContainer.TargetHeight = targetHeight.Value;

            var unk2 = br.ReadStringLoa();
            movieDataContainer.Unk2 = unk2;

            movieDataContainer.MovieDataValueArray = new MovieDataValue[br.ReadInt32()];
            for (int j = 0; j < movieDataContainer.MovieDataValueArray.Length; j++)
            {
                var movieDataValue = MovieDataValue.Deserialize(br);
                movieDataContainer.MovieDataValueArray[j] = movieDataValue;
            }

            return movieDataContainer;
        }

        internal byte[] Serialize()
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
