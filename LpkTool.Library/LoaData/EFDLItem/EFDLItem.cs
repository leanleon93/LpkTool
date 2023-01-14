namespace LpkTool.Library.LoaData.EFDLItem
{
    public class EFDLItem : Loa
    {
        public EFDLItem()
        {
        }

        public EFDLItem(Stream stream) : base(stream)
        {
        }

        public EFDLItem(string filePath) : base(filePath)
        {
        }

        public EFDLItem(byte[] data) : base(data)
        {
        }
        public LoaProp<string> Key { get; set; }
        public LoaProp<string> CountryCode { get; set; }
        public LoaProp<int> EquipPart { get; set; }
        public LoaProp<int> MaterialWeapon { get; set; }
        public LoaProp<int> MaterialArmor { get; set; }
        public LoaProp<int> MaterialHittedSound { get; set; }
        public LoaProp<int> StanceChangeItem { get; set; }
        public LoaProp<int> HideFaceParts { get; set; }
        public LoaProp<int> HideHairPart { get; set; }
        public LoaProp<int> AttachHairSectionHide { get; set; }
        public LoaProp<string> AttributeKeyword { get; set; }
        public LoaProp<List<PartsMesh>>? PartsMeshes { get; set; }
        public LoaProp<List<ItemParticleDatSpawn>>? ItemParticleDatSpawns { get; set; }
        public LoaProp<string>? AoTexture { get; set; }
        public LoaProp<List<MaskData>>? MaskDataArray { get; set; }
        public LoaProp<List<TargetMaskData>>? TargetMaskDataArray { get; set; }
        public LoaProp<TargetAttachInfo> TargetAttachInfo { get; set; }
        public LoaProp<string> ItemSoundSetKey { get; set; }
        public LoaProp<string> FootStepParticleSetKey { get; set; }
        public LoaProp<string> EnhanceKey { get; set; }
        public LoaProp<string> EvolutionKey { get; set; }
        public LoaProp<string> EstherLinkGroupKey { get; set; }
        public LoaProp<LoaProp<int>[]>? AttachHairSectionIds { get; set; }

        public override byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(base.SerializeHeader());
                    bw.WriteLoaProp(Key);
                    bw.WriteLoaProp(CountryCode);
                    bw.WriteLoaProp(EquipPart);
                    bw.WriteLoaProp(MaterialWeapon);
                    bw.WriteLoaProp(MaterialArmor);
                    bw.WriteLoaProp(MaterialHittedSound);
                    bw.WriteLoaProp(StanceChangeItem);
                    bw.WriteLoaProp(HideFaceParts);
                    bw.WriteLoaProp(HideHairPart);
                    bw.WriteLoaProp(AttachHairSectionHide);
                    bw.WriteLoaProp(AttributeKeyword);
                    if (PartsMeshes != null)
                    {
                        bw.WriteLoaProp(PartsMeshes);
                    }
                    if (ItemParticleDatSpawns != null)
                    {
                        bw.WriteLoaProp(ItemParticleDatSpawns);
                    }
                    if (AoTexture != null)
                        bw.WriteLoaProp(AoTexture);
                    if (MaskDataArray != null)
                        bw.WriteLoaProp(MaskDataArray);
                    if (TargetMaskDataArray != null)
                        bw.WriteLoaProp(TargetMaskDataArray);
                    bw.WriteLoaProp(TargetAttachInfo);
                    bw.WriteLoaProp(ItemSoundSetKey);
                    bw.WriteLoaProp(FootStepParticleSetKey);
                    bw.WriteLoaProp(EnhanceKey);
                    bw.WriteLoaProp(EvolutionKey);
                    bw.WriteLoaProp(EstherLinkGroupKey);
                    if (AttachHairSectionIds != null)
                    {
                        bw.WriteLoaProp(AttachHairSectionIds);
                    }
                }
                return ms.ToArray();
            }
        }

        protected override void Deserialize(BinaryReader br)
        {
            base.DeserializeHeader(br);
            Key = br.ReadLoaProp(Key);
            CountryCode = br.ReadLoaProp(CountryCode);
            EquipPart = br.ReadLoaProp(EquipPart);
            MaterialWeapon = br.ReadLoaProp(MaterialWeapon);
            MaterialArmor = br.ReadLoaProp(MaterialArmor);
            MaterialHittedSound = br.ReadLoaProp(MaterialHittedSound);
            StanceChangeItem = br.ReadLoaProp(StanceChangeItem);
            HideFaceParts = br.ReadLoaProp(HideFaceParts);
            HideHairPart = br.ReadLoaProp(HideHairPart);
            AttachHairSectionHide = br.ReadLoaProp(AttachHairSectionHide);
            AttributeKeyword = br.ReadLoaProp(AttributeKeyword);
            var nextKey = br.GetNextKey();
            if (nextKey == "PartsMeshes")
            {
                PartsMeshes = br.ReadLoaProp(PartsMeshes!);
            }


            nextKey = br.GetNextKey();
            if (nextKey == "ItemParticleDatSpawn")
            {
                ItemParticleDatSpawns = br.ReadLoaProp(ItemParticleDatSpawns!);
            }

            nextKey = br.GetNextKey();

            if (nextKey == "AOTexture")
            {
                AoTexture = br.ReadLoaProp(AoTexture!);
            }

            nextKey = br.GetNextKey();

            if (nextKey == "MaskDataArr")
            {
                MaskDataArray = br.ReadLoaProp(MaskDataArray!);
            }

            nextKey = br.GetNextKey();
            if (nextKey == "TargetMaskDataArr")
            {
                TargetMaskDataArray = br.ReadLoaProp(TargetMaskDataArray!);
            }

            TargetAttachInfo = br.ReadLoaProp(TargetAttachInfo);
            ItemSoundSetKey = br.ReadLoaProp(ItemSoundSetKey);
            FootStepParticleSetKey = br.ReadLoaProp(FootStepParticleSetKey);
            EnhanceKey = br.ReadLoaProp(EnhanceKey);
            EvolutionKey = br.ReadLoaProp(EvolutionKey);
            EstherLinkGroupKey = br.ReadLoaProp(EstherLinkGroupKey);
            if (br.BaseStream.Position != br.BaseStream.Length)
            {
                nextKey = br.GetNextKey();
                if (nextKey == "AttachHairSectionIds")
                {
                    AttachHairSectionIds = br.ReadLoaProp(AttachHairSectionIds!);
                }
            }
        }
    }
}
