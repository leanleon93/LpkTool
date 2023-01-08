namespace LpkTool.Library.LoaData.Quest
{
    internal class StageTrigger : LoaSubclass
    {
        public StageTrigger()
        {
        }

        public StageTrigger(BinaryReader br) : base(br)
        {
        }

        public override byte[] Serialize()
        {
            throw new NotImplementedException();
        }

        protected override void Deserialize(BinaryReader br)
        {
            StageTriggerKey = br.ReadStringLoa();
            ZoneIndex = br.ReadKvpLoa<int>().Value;
            TriggerUnitIndex = br.ReadKvpLoa<int>().Value;
        }

        public string StageTriggerKey { get; internal set; }

        public int ZoneIndex { get; set; }
        public int TriggerUnitIndex { get; set; }
    }
}