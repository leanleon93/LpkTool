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
        public LoaProp<string> FileName { get; set; }
        public LoaProp<int> FileNameUnk { get; set; }
        public LoaProp<bool> Loop { get; set; }
        public LoaProp<int> DefaultAudioVolumeRate { get; set; }
        public LoaProp<int> ReadBufferSize { get; set; }

        protected override void Deserialize(BinaryReader br)
        {
            var unk = br.ReadStringLoa();
            Unk = unk;

            FileName = br.ReadLoaProp(FileName);

            FileNameUnk = br.ReadLoaProp(FileNameUnk);

            Loop = br.ReadLoaProp(Loop);

            DefaultAudioVolumeRate = br.ReadLoaProp(DefaultAudioVolumeRate);

            ReadBufferSize = br.ReadLoaProp(ReadBufferSize);
        }

        public override byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.WriteStringLoa(Unk);
                    bw.WriteLoaProp(FileName);
                    bw.WriteLoaProp(FileNameUnk);
                    bw.WriteLoaProp(Loop);
                    bw.WriteLoaProp(DefaultAudioVolumeRate);
                    bw.WriteLoaProp(ReadBufferSize);
                }
                return ms.ToArray();
            }
        }
    }
}