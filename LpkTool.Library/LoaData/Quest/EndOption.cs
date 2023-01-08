namespace LpkTool.Library.LoaData.Quest
{
    public class EndOption : LoaSubclass
    {
        public EndOption()
        {
        }

        public EndOption(BinaryReader br) : base(br)
        {
        }

        public override byte[] Serialize()
        {
            throw new NotImplementedException();
        }

        protected override void Deserialize(BinaryReader br)
        {
            EndOptionKey = br.ReadStringLoa();
            End_Type = br.ReadKvpLoa<int>().Value;
            End_NextQuestIndex = br.ReadKvpLoa<int>().Value;
            FailUIInfo = new FailUiInfo(br);
        }

        public string EndOptionKey { get; set; }
        public int End_Type { get; set; }
        public int End_NextQuestIndex { get; set; }
        public FailUiInfo FailUIInfo { get; set; }

    }
}