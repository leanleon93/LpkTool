namespace LpkTool.Library.LoaData.Table_MovieData
{
    public class MovieData : Loa
    {
        public MovieData() : base() { }
        public MovieData(Stream stream) : base(stream)
        {
        }

        public MovieData(string filePath) : base(filePath)
        {
        }

        public MovieData(byte[] data) : base(data)
        {
        }

        public LoaProp<List<MovieDataContainer>> MovieDataContainers { get; set; }

        protected override void Deserialize(BinaryReader br)
        {
            base.DeserializeHeader(br);

            MovieDataContainers = br.ReadLoaProp(MovieDataContainers);
        }

        public override byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(base.SerializeHeader());
                    bw.WriteLoaProp(MovieDataContainers);
                }
                return ms.ToArray();
            }
        }
    }
}
