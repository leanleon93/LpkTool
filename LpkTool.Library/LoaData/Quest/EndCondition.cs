namespace LpkTool.Library.LoaData.Quest
{
    public class EndCondition : LoaSubclass
    {
        public EndCondition()
        {
        }

        public EndCondition(BinaryReader br) : base(br)
        {
        }

        public string Unk { get; set; }
        public string Type { get; set; }
        public int EndOptionIndex { get; set; }

        public Message QuestEnd_Msg { get; set; }
        public int EndOption_ConditionIndex { get; set; }

        public override byte[] Serialize()
        {
            throw new NotImplementedException();
        }

        protected override void Deserialize(BinaryReader br)
        {
            Unk = br.ReadStringLoa();
            Type = br.ReadStringLoa();
            EndOptionIndex = br.ReadKvpLoa<int>().Value;
            QuestEnd_Msg = new Message(br);
            EndOption_ConditionIndex = br.ReadKvpLoa<int>().Value;
        }
    }
}