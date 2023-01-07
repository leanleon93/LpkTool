namespace LpkTool.Library.LoaData.Table_MovieData
{
    public class MovieDataValue
    {
        public string Unk { get; set; }
        public string FileName { get; set; }
        public int FileNameUnk { get; set; }
        public bool Loop { get; set; }
        public int DefaultAudioVolumeRate { get; set; }
        public int ReadBufferSize { get; set; }

        internal static MovieDataValue Deserialize(BinaryReader br)
        {
            var movieDataValue = new MovieDataValue();

            var unk = br.ReadStringLoa();
            movieDataValue.Unk = unk;

            var fileName = br.ReadKvpLoa<string>();
            movieDataValue.FileName = fileName.Value;

            var fileNameUnk = br.ReadKvpLoa<int>();
            movieDataValue.FileNameUnk = fileNameUnk.Value;

            var loop = br.ReadKvpLoa<bool>();
            movieDataValue.Loop = loop.Value;

            var defaultAudioVolumeRate = br.ReadKvpLoa<int>();
            movieDataValue.DefaultAudioVolumeRate = defaultAudioVolumeRate.Value;

            var readBufferSize = br.ReadKvpLoa<int>();
            movieDataValue.ReadBufferSize = readBufferSize.Value;

            return movieDataValue;
        }

        internal byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.WriteStringLoa(Unk);
                    bw.WriteKvpLoa(nameof(FileName), FileName);
                    bw.WriteStringLoa("FileName");
                    bw.Write(FileNameUnk);
                    bw.WriteKvpLoa(nameof(Loop), Loop);
                    bw.WriteKvpLoa(nameof(DefaultAudioVolumeRate), DefaultAudioVolumeRate);
                    bw.WriteKvpLoa(nameof(ReadBufferSize), ReadBufferSize);
                }
                return ms.ToArray();
            }
        }

    }
}