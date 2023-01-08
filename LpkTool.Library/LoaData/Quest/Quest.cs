namespace LpkTool.Library.LoaData.Quest
{
    public class Quest : Loa
    {
        public Quest()
        {
        }

        public Quest(Stream stream) : base(stream)
        {
        }

        public Quest(string filePath) : base(filePath)
        {
        }

        public Quest(byte[] data) : base(data)
        {
        }

        public string ContainerName { get; set; }
        public ExportStageSequence[] ExportStageSequences { get; set; }

        public Message Quest_name { get; set; }

        public Message Quest_Desc { get; set; }

        public Message Quest_SuccessMsg { get; set; }

        public Message Quest_SuccessSummary { get; set; }

        public int Quest_Type { get; set; }
        public int DungeonQuest_Type { get; set; }
        public int BasicQuest_Type { get; set; }
        public int Quest_Continent { get; set; }
        public int Quest_MapLevel { get; set; }
        public int Expedition_Quest_Type { get; set; }
        public int Start_Kind { get; set; }
        public int QuestUnlockContents { get; set; }
        public int Quest_Level { get; set; }
        public int Quest_MapIndex { get; set; }
        public Message Quest_AreaName { get; set; }
        public int Quest_AdventurePoint { get; set; }
        public int Quest_IndunClear { get; set; }
        public int Quest_Share { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<loa>")]
        public bool bExpedition { get; set; }
        public int FirstTraceCurCity { get; set; }
        public int Quest_CompleteZoneId { get; set; }
        public int Quest_People { get; set; }
        public int GameNoteIndex { get; set; }
        public int QuestNoteIndex { get; set; }
        public int Start_KindValue { get; set; }
        public EndOption EndOption { get; set; }
        public int PeriodQuestOption { get; set; }
        public int CoopQuestOption { get; set; }
        public int GroupQuestOption { get; set; }
        public int QuestGiveupOption { get; set; }
        public int TownQuestOption { get; set; }
        public int GuideQuestOption { get; set; }
        public UIOption UIOpiton { get; set; }
        public int DeletedQuestSound { get; set; }

        protected override void Deserialize(BinaryReader br)
        {
            base.DeserializeHeader(br);

            var unk2 = br.ReadStringLoa();
            ContainerName = unk2;

            ExportStageSequences = new ExportStageSequence[br.ReadInt32()];
            for (int i = 0; i < ExportStageSequences.Length; i++)
            {
                var exportStageSequences = new ExportStageSequence(br);
                ExportStageSequences[i] = exportStageSequences;
            }

            Quest_name = new Message(br);
            Quest_Desc = new Message(br);
            Quest_SuccessMsg = new Message(br);
            Quest_SuccessSummary = new Message(br);
            Quest_Type = br.ReadKvpLoa<int>().Value;
            DungeonQuest_Type = br.ReadKvpLoa<int>().Value;
            BasicQuest_Type = br.ReadKvpLoa<int>().Value;
            Quest_Continent = br.ReadKvpLoa<int>().Value;
            Quest_MapLevel = br.ReadKvpLoa<int>().Value;
            Expedition_Quest_Type = br.ReadKvpLoa<int>().Value;
            Start_Kind = br.ReadKvpLoa<int>().Value;
            QuestUnlockContents = br.ReadKvpLoa<int>().Value;
            Quest_Level = br.ReadKvpLoa<int>().Value;
            Quest_MapIndex = br.ReadKvpLoa<int>().Value;
            Quest_AreaName = new Message(br);
            Quest_AdventurePoint = br.ReadKvpLoa<int>().Value;
            Quest_IndunClear = br.ReadKvpLoa<int>().Value;
            Quest_Share = br.ReadKvpLoa<int>().Value;
            bExpedition = br.ReadKvpLoa<bool>().Value;
            FirstTraceCurCity = br.ReadKvpLoa<int>().Value;
            Quest_CompleteZoneId = br.ReadKvpLoa<int>().Value;
            Quest_People = br.ReadKvpLoa<int>().Value;
            GameNoteIndex = br.ReadKvpLoa<int>().Value;
            QuestNoteIndex = br.ReadKvpLoa<int>().Value;
            Start_KindValue = br.ReadKvpLoa<int>().Value;
            EndOption = new EndOption(br);
            PeriodQuestOption = br.ReadKvpLoa<int>().Value;
            CoopQuestOption = br.ReadKvpLoa<int>().Value;
            GroupQuestOption = br.ReadKvpLoa<int>().Value;
            QuestGiveupOption = br.ReadKvpLoa<int>().Value;
            TownQuestOption = br.ReadKvpLoa<int>().Value;
            GuideQuestOption = br.ReadKvpLoa<int>().Value;
            UIOpiton = new UIOption(br);
            DeletedQuestSound = br.ReadKvpLoa<int>().Value;
        }

        public override byte[] Serialize()
        {
            var header = base.SerializeHeader();
            throw new NotImplementedException();
        }

    }
}
