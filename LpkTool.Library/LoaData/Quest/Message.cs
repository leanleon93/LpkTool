namespace LpkTool.Library.LoaData.Quest
{
    public class Message : LoaSubclass
    {
        public Message()
        {
        }

        public Message(BinaryReader br) : base(br)
        {
        }

        public string MessageKey { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<loa>")]
        public string strMsg { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<loa>")]
        public int eMsgType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<loa>")]
        public string strMsgID { get; set; }

        public override byte[] Serialize()
        {
            throw new NotImplementedException();
        }

        protected override void Deserialize(BinaryReader br)
        {
            MessageKey = br.ReadStringLoa();
            strMsg = br.ReadKvpLoa<string>().Value;
            eMsgType = br.ReadKvpLoa<int>().Value;
            strMsgID = br.ReadKvpLoa<string>().Value;
        }
    }
}