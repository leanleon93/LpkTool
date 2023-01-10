namespace LpkTool.Library.LoaData.Quest
{
    public class ExportStageSequence : LoaSubclass
    {
        public ExportStageSequence()
        {
            Properties = new Dictionary<string, dynamic>();
        }

        public ExportStageSequence(BinaryReader br) : base(br)
        {
            Properties = new Dictionary<string, dynamic>();
        }

        public string Unk { get; set; }
        public string StageSequenceType { get; set; }
        public Dictionary<string, object> Properties { get; set; }

        private void CEFQuestSeq_StageEnd(BinaryReader br)
        {
            var bKeepPlaySound = br.ReadKvpLoa<bool>();
            Properties.Add(bKeepPlaySound.Key, bKeepPlaySound.Value);
        }

        private void CEFQuestSeq_StageStep(BinaryReader br)
        {
            var bkeepPlaySound = br.ReadKvpLoa<bool>();
            Properties.Add(bkeepPlaySound.Key, bkeepPlaySound.Value);
            var endConditionMsg = new Message(br);
            Properties.Add(endConditionMsg.MessageKey, endConditionMsg);

            var endOptionCheckType = br.ReadKvpLoa<int>();
            Properties.Add(endOptionCheckType.Key, endOptionCheckType.Value);

            var zoneLevelCheckType = br.ReadKvpLoa<int>();
            Properties.Add(zoneLevelCheckType.Key, zoneLevelCheckType.Value);

            var unlockContentsWhenComplete = br.ReadKvpLoa<bool>();
            Properties.Add(unlockContentsWhenComplete.Key, unlockContentsWhenComplete.Value);

            var endConditionsName = br.ReadStringLoa();
            var endConditions = new EndCondition[br.ReadInt32()];
            for (int i = 0; i < endConditions.Length; i++)
            {
                endConditions[i] = new EndCondition(br);
            }
            Properties.Add(endConditionsName, endConditions);

            var partyShare = br.ReadKvpLoa<bool>();
            Properties.Add(partyShare.Key, partyShare.Value);

            var isPercent = br.ReadKvpLoa<bool>();
            Properties.Add(isPercent.Key, isPercent.Value);

            var bHideQuestSymbol = br.ReadKvpLoa<bool>();
            Properties.Add(bHideQuestSymbol.Key, bHideQuestSymbol.Value);

            var bHideMapSymbol = br.ReadKvpLoa<bool>();
            Properties.Add(bHideMapSymbol.Key, bHideMapSymbol.Value);

            var bShowProgressBar = br.ReadKvpLoa<bool>();
            Properties.Add(bShowProgressBar.Key, bShowProgressBar.Value);

            var bVisibleObjectiveCount = br.ReadKvpLoa<bool>();
            Properties.Add(bVisibleObjectiveCount.Key, bVisibleObjectiveCount.Value);

            var completeCount = br.ReadKvpLoa<int>();
            Properties.Add(completeCount.Key, completeCount.Value);

            var initCompleteCount = br.ReadKvpLoa<int>();
            Properties.Add(initCompleteCount.Key, initCompleteCount.Value);

            var eQuestObjectiveType = br.ReadKvpLoa<int>();
            Properties.Add(eQuestObjectiveType.Key, eQuestObjectiveType.Value);

            var endCountSuffix = br.ReadKvpLoa<string>();
            Properties.Add(endCountSuffix.Key, endCountSuffix.Value);

            var endCountNoun = br.ReadKvpLoa<string>();
            Properties.Add(endCountNoun.Key, endCountNoun.Value);

            var endConditionVisibilityKey = br.ReadStringLoa();
            var endConditionVisibility = br.ReadKvpLoa<bool>();
            Properties.Add(endConditionVisibilityKey, endConditionVisibility);

            var questId = br.ReadKvpLoa<int>();
            Properties.Add(questId.Key, questId.Value);

            var stageStepId = br.ReadKvpLoa<int>();
            Properties.Add(stageStepId.Key, stageStepId.Value);

            var volumePropVisitCount = br.ReadKvpLoa<int>();
            Properties.Add(volumePropVisitCount.Key, volumePropVisitCount.Value);

            var volumeEventType = br.ReadKvpLoa<int>();
            Properties.Add(volumeEventType.Key, volumeEventType.Value);

            var volumeStayTime = br.ReadKvpLoa<int>();
            Properties.Add(volumeStayTime.Key, volumeStayTime.Value);

            var volumePropIndexListKey = br.ReadStringLoa();
            var volumePropIndexList = new int[br.ReadInt32()];
            for (int i = 0; i < volumePropIndexList.Length; i++)
            {
                br.ReadStringLoa();
                volumePropIndexList[i] = br.ReadInt32();
            }

            Properties.Add(volumePropIndexListKey, volumePropIndexList);

            var vehicleId = br.ReadKvpLoa<int>();
            Properties.Add(vehicleId.Key, vehicleId.Value);

            var npcCheckDistance = br.ReadKvpLoa<int>();
            Properties.Add(npcCheckDistance.Key, npcCheckDistance.Value);

            var failUiInfo = new FailUiInfo(br);
            Properties.Add(failUiInfo.FailUIInfoKey, failUiInfo);

            var stageStartTrigger = new StageTrigger(br);
            Properties.Add(stageStartTrigger.StageTriggerKey, stageStartTrigger);

            var stageCompleteTrigger = new StageTrigger(br);
            Properties.Add(stageCompleteTrigger.StageTriggerKey, stageCompleteTrigger);

            var stageFailTrigger = new StageTrigger(br);
            Properties.Add(stageFailTrigger.StageTriggerKey, stageFailTrigger);

            var updateDelayClearTrigger = new StageTrigger(br);
            Properties.Add(updateDelayClearTrigger.StageTriggerKey, updateDelayClearTrigger);

            var stepId = br.ReadKvpLoa<int>();
            Properties.Add(stepId.Key, stepId.Value);

            var startQuestIndex = br.ReadKvpLoa<int>();
            Properties.Add(startQuestIndex.Key, startQuestIndex.Value);

            var gameNoteIndex = br.ReadKvpLoa<int>();
            Properties.Add(gameNoteIndex.Key, gameNoteIndex.Value);

            var stageStartGameNoteIndex = br.ReadKvpLoa<int>();
            Properties.Add(stageStartGameNoteIndex.Key, stageStartGameNoteIndex.Value);

            var questNoteIndex = br.ReadKvpLoa<int>();
            Properties.Add(questNoteIndex.Key, questNoteIndex.Value);

            var stageStartQuestNoteIndex = br.ReadKvpLoa<int>();
            Properties.Add(stageStartQuestNoteIndex.Key, stageStartQuestNoteIndex.Value);

            var progressZoneId = br.ReadKvpLoa<int>();
            Properties.Add(progressZoneId.Key, progressZoneId.Value);

            var hotkeyItemId = br.ReadKvpLoa<int>();
            Properties.Add(hotkeyItemId.Key, hotkeyItemId.Value);

            var disableTrace = br.ReadKvpLoa<bool>();
            Properties.Add(disableTrace.Key, disableTrace.Value);

            var traceCurZone = br.ReadKvpLoa<bool>();
            Properties.Add(traceCurZone.Key, traceCurZone.Value);

            var learnMusicWhenComplete = br.ReadKvpLoa<bool>();
            Properties.Add(learnMusicWhenComplete.Key, learnMusicWhenComplete.Value);

            var doCommonActionWhenComplete = br.ReadKvpLoa<bool>();
            Properties.Add(doCommonActionWhenComplete.Key, doCommonActionWhenComplete.Value);

            var offerBuffWhenComplete = br.ReadKvpLoa<bool>();
            Properties.Add(offerBuffWhenComplete.Key, offerBuffWhenComplete.Value);

            var stageStartUiTutorial = br.ReadKvpLoa<bool>();
            Properties.Add(stageStartUiTutorial.Key, stageStartUiTutorial.Value);
        }

        private void CEFQuestSeq_StageStart(BinaryReader br)
        {
            var bkeepPlaySound = br.ReadKvpLoa<bool>();
            Properties.Add(bkeepPlaySound.Key, bkeepPlaySound.Value);
        }

        public override byte[] Serialize()
        {
            throw new NotImplementedException();
        }

        protected override void Deserialize(BinaryReader br)
        {
            if (Properties == null)
                Properties = new Dictionary<string, dynamic>();
            Unk = br.ReadStringLoa();
            StageSequenceType = br.ReadStringLoa();
            switch (StageSequenceType)
            {
                case "CEFQuestSeq_StageStart":
                    CEFQuestSeq_StageStart(br);
                    break;
                case "CEFQuestSeq_StageStep":
                    CEFQuestSeq_StageStep(br);
                    break;
                case "CEFQuestSeq_StageEnd":
                    CEFQuestSeq_StageEnd(br);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}