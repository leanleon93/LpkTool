namespace LpkTool.Library
{
    internal class Header
    {
        private Header()
        {
            Entries = new List<HeaderEntry>();
        }
        internal static readonly int HEADER_ENTRY_SIZE = 528;
        internal List<HeaderEntry> Entries { get; set; }
        internal static Header FromByteArray(byte[] headerArray)
        {
            var header = new Header();
            var numberOfFiles = headerArray.Length / HEADER_ENTRY_SIZE;
            using (var ms = new MemoryStream(headerArray))
            {
                using (var br = new BinaryReader(ms))
                {
                    for (var i = 0; i < numberOfFiles; i++)
                    {
                        header.Entries.Add(HeaderEntry.FromByteArray(br.ReadBytes(HEADER_ENTRY_SIZE)));
                    }
                }
            }
            return header;
        }
    }
}
