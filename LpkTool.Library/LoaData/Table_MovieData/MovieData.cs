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

        public string ContainerName { get; set; }
        public MovieDataContainer[] MovieDataContainers { get; set; }

        protected override void Deserialize(BinaryReader br)
        {
            base.DeserializeHeader(br);
            var unk2 = br.ReadStringLoa();
            ContainerName = unk2;

            MovieDataContainers = new MovieDataContainer[br.ReadInt32()];
            for (int i = 0; i < MovieDataContainers.Length; i++)
            {
                var movieDataContainer = new MovieDataContainer(br);
                MovieDataContainers[i] = movieDataContainer;
            }
        }

        public override byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(base.SerializeHeader());
                    bw.WriteStringLoa(ContainerName);
                    bw.Write(MovieDataContainers.Length);
                    foreach (var movieDataContainer in MovieDataContainers)
                    {
                        bw.Write(movieDataContainer.Serialize());
                    }
                }
                return ms.ToArray();
            }
        }
    }
}
