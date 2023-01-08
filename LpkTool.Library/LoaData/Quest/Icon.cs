namespace LpkTool.Library.LoaData.Quest
{
    public class Icon : LoaSubclass
    {
        public Icon()
        {
        }

        public Icon(BinaryReader br) : base(br)
        {
        }

        public string IconKey { get; set; }
        public string IconPackage { get; set; }

        public string IconName { get; set; }
        public int IconIndex { get; set; }

        public override byte[] Serialize()
        {
            throw new NotImplementedException();
        }

        protected override void Deserialize(BinaryReader br)
        {
            IconKey = br.ReadStringLoa();
            IconPackage = br.ReadKvpLoa<string>().Value;
            IconName = br.ReadKvpLoa<string>().Value;
            IconIndex = br.ReadKvpLoa<int>().Value;
        }
    }
}