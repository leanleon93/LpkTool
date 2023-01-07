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

        protected override void SerializeDetails(BinaryWriter bw)
        {
            bw.WriteStringLoa(ContainerName);
            bw.Write(MovieDataContainers.Length);
            foreach (var movieDataContainer in MovieDataContainers)
            {
                bw.Write(movieDataContainer.Serialize());
            }
        }

        protected override void DeserializeDetails(BinaryReader br)
        {
            var unk2 = br.ReadStringLoa();
            ContainerName = unk2;

            MovieDataContainers = new MovieDataContainer[br.ReadInt32()];
            for (int i = 0; i < MovieDataContainers.Length; i++)
            {
                var movieDataContainer = new MovieDataContainer(br);
                MovieDataContainers[i] = movieDataContainer;
            }
        }
    }
}
