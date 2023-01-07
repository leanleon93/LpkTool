namespace LpkTool.Library.LoaData.Table_MovieData
{
    public class MovieData
    {
        public byte[] Magic { get; set; }
        public long FileId { get; set; }
        public string Unk { get; set; }
        public string Unk2 { get; set; }
        public MovieDataContainer[] MovieDataContainers { get; set; }

        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(Magic);
                    bw.Write(FileId);
                    bw.WriteStringLoa(Unk);
                    bw.WriteStringLoa(Unk2);
                    bw.Write(MovieDataContainers.Length);
                    foreach (var movieDataContainer in MovieDataContainers)
                    {
                        bw.Write(movieDataContainer.Serialize());
                    }
                }
                return ms.ToArray();
            }
        }

        public static MovieData FromFile(string path)
        {
            var movieData = FromStream(new FileStream(path, FileMode.Open));
            return movieData;
        }

        public static MovieData FromByteArray(byte[] data)
        {
            var movieData = FromStream(new MemoryStream(data));
            return movieData;
        }

        private static MovieData FromStream(Stream stream)
        {
            var result = new MovieData();
            using (var br = new BinaryReader(stream))
            {
                result.Magic = br.ReadBytes(4);
                result.FileId = br.ReadInt64();
                if (result.FileId == 19)
                {
                    br.BaseStream.Position = 0;
                    result = MovieData.Deserialize(br);
                }
            }
            return result;
        }

        internal static MovieData Deserialize(BinaryReader br)
        {
            var movieData = new MovieData();

            movieData.Magic = br.ReadBytes(4);
            movieData.FileId = br.ReadInt64();

            var unk = br.ReadStringLoa();
            movieData.Unk = unk;

            var unk2 = br.ReadStringLoa();
            movieData.Unk2 = unk2;

            movieData.MovieDataContainers = new MovieDataContainer[br.ReadInt32()];
            for (int i = 0; i < movieData.MovieDataContainers.Length; i++)
            {
                var movieDataContainer = MovieDataContainer.Deserialize(br);
                movieData.MovieDataContainers[i] = movieDataContainer;
            }
            return movieData;
        }
    }
}
