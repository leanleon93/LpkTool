namespace LpkTool.Library.LoaData.Quest
{
    public class UIOption : LoaSubclass
    {
        public UIOption()
        {
        }

        public UIOption(BinaryReader br) : base(br)
        {
        }

        public override byte[] Serialize()
        {
            throw new NotImplementedException();
        }

        protected override void Deserialize(BinaryReader br)
        {
            UIOptionKey = br.ReadStringLoa();
            m_bShow = br.ReadKvpLoa<bool>().Value;
            m_bAnnounce = br.ReadKvpLoa<bool>().Value;
            m_bHideQuestSymbolWhenSuccess = br.ReadKvpLoa<bool>().Value;
            m_bHideMapSymbolWhenSuccess = br.ReadKvpLoa<bool>().Value;
            m_bDisableTraceWhenSuccess = br.ReadKvpLoa<bool>().Value;
            m_bHiddenIndicatorNPC = br.ReadKvpLoa<bool>().Value;
            m_eTroopNumber = br.ReadKvpLoa<int>().Value;
        }

        public string UIOptionKey { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public bool m_bShow { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]

        public bool m_bAnnounce { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]

        public bool m_bHideQuestSymbolWhenSuccess { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]

        public bool m_bHideMapSymbolWhenSuccess { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]

        public bool m_bDisableTraceWhenSuccess { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]

        public bool m_bHiddenIndicatorNPC { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]

        public int m_eTroopNumber { get; set; }
    }
}