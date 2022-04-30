namespace LpkTool.Library.Models
{
    internal class Header
    {
        private Header() { }
        internal static readonly int HEADER_ENTRY_SIZE = 528;
        internal List<HeaderEntry> Entries { get; set; }
        internal static Header FromByteArray(byte[] headerArray)
        {
            var header = new Header();
            header.Entries = new List<HeaderEntry>();
            var numberOfFiles = headerArray.Length / HEADER_ENTRY_SIZE;
            using (var ms = new MemoryStream(headerArray))
            {
                using (var br = new BinaryReader(ms))
                {
                    for (int i = 0; i < numberOfFiles; i++)
                    {
                        header.Entries.Add(HeaderEntry.FromByteArray(br.ReadBytes(HEADER_ENTRY_SIZE)));
                    }
                }
            }
            return header;
        }
    }
}
