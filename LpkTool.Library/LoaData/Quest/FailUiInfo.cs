namespace LpkTool.Library.LoaData.Quest
{
    public class FailUiInfo : LoaSubclass
    {
        public FailUiInfo()
        {
        }

        public FailUiInfo(BinaryReader br) : base(br)
        {
        }

        public string FailUIInfoKey { get; internal set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public bool bShow { get; set; }

        public Icon Icon { get; set; }

        public Message Message { get; set; }

        public override byte[] Serialize()
        {
            throw new NotImplementedException();
        }

        protected override void Deserialize(BinaryReader br)
        {
            FailUIInfoKey = br.ReadStringLoa();
            bShow = br.ReadKvpLoa<bool>().Value;
            Icon = new Icon(br);
            Message = new Message(br);
        }
    }
}