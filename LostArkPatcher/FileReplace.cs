namespace LostArkPatcher
{
    [Serializable]
    public class FileReplace
    {
        public int DataFileId { get; set; }
        public string SearchFilename { get; set; }
        public string NewFilePath { get; set; }
    }
}
