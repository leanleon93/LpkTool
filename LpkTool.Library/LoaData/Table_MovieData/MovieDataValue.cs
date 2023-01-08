namespace LpkTool.Library.LoaData.Table_MovieData
{
    public class MovieDataValue : LoaSubclass
    {
        public MovieDataValue()
        {
        }

        public MovieDataValue(BinaryReader br) : base(br)
        {
        }

        public string Unk { get; set; }
        public string FileName { get; set; }
        public int FileNameUnk { get; set; }
        public bool Loop { get; set; }
        public int DefaultAudioVolumeRate { get; set; }
        public int ReadBufferSize { get; set; }

        protected override void Deserialize(BinaryReader br)
        {
            var unk = br.ReadStringLoa();
            Unk = unk;

            var fileName = br.ReadKvpLoa<string>();
            FileName = fileName.Value;

            var fileNameUnk = br.ReadKvpLoa<int>();
            FileNameUnk = fileNameUnk.Value;

            var loop = br.ReadKvpLoa<bool>();
            Loop = loop.Value;

            var defaultAudioVolumeRate = br.ReadKvpLoa<int>();
            DefaultAudioVolumeRate = defaultAudioVolumeRate.Value;

            var readBufferSize = br.ReadKvpLoa<int>();
            ReadBufferSize = readBufferSize.Value;
        }

        public override byte[] Serialize()
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