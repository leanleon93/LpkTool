namespace LpkTool.Library.LoaData.EFDLItem
{
    public class PartsMesh : LoaSubclass
    {
        public PartsMesh()
        {
        }

        public PartsMesh(BinaryReader br) : base(br)
        {
        }
        public string ArrayKeyword { get; set; }
        public string SecondKeyword { get; set; }
        public LoaProp<int> PrimaryKey { get; set; }
        public LoaProp<int> SecondaryKey { get; set; }
        public LoaProp<int> MeshPartsType { get; set; }
        public LoaProp<int> RequireStance { get; set; }
        public LoaProp<string> PartsMeshKey { get; set; }
        public LoaProp<LoaProp<string>[]>? Materials { get; set; }
        public LoaProp<List<MaterialsVariation>>? MaterialsVariations { get; set; }
        public LoaProp<string> AnimSetTemplate { get; set; }
        public LoaProp<string> PhysicsAsset { get; set; }
        public LoaProp<string> SocketGroupName { get; set; }
        public LoaProp<string> SkelControlGroup { get; set; }
        public LoaProp<string> MorphSet { get; set; }
        public LoaProp<string> Trails_Default { get; set; }
        public LoaProp<bool> UseOnePassLightingOnTranslucency { get; set; }
        public LoaProp<float> PartsScale { get; set; }
        public LoaProp<float> TranslucencySortKeyFactor { get; set; }
        public LoaProp<int> TranslucencySortPriority { get; set; }
        public LoaProp<int> LayerLevel { get; set; }
        public LoaProp<List<SectionInfo>>? SectionInfos { get; set; }
        public LoaProp<LoaProp<string>[]>? DyeChangeMaterials { get; set; }
        public LoaProp<bool> DyePartAUsed { get; set; }
        public LoaProp<bool> DyePartBUsed { get; set; }
        public LoaProp<bool> DyePartCUsed { get; set; }

        public LoaProp<bool>? DyePatternAUsed { get; set; }
        public LoaProp<bool>? DyePatternBUsed { get; set; }
        public LoaProp<bool>? DyePatternCUsed { get; set; }

        public override byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.WriteStringLoa(ArrayKeyword);
                    bw.WriteStringLoa(SecondKeyword);
                    bw.WriteLoaProp(PrimaryKey);
                    bw.WriteLoaProp(SecondaryKey);
                    bw.WriteLoaProp(MeshPartsType);
                    bw.WriteLoaProp(RequireStance);
                    bw.WriteLoaProp(PartsMeshKey);
                    if (Materials != null)
                        bw.WriteLoaProp(Materials);
                    if (MaterialsVariations != null)
                        bw.WriteLoaProp(MaterialsVariations);
                    bw.WriteLoaProp(AnimSetTemplate);
                    bw.WriteLoaProp(PhysicsAsset);
                    bw.WriteLoaProp(SocketGroupName);
                    bw.WriteLoaProp(SkelControlGroup);
                    bw.WriteLoaProp(MorphSet);
                    bw.WriteLoaProp(Trails_Default);
                    bw.WriteLoaProp(UseOnePassLightingOnTranslucency);
                    bw.WriteLoaProp(PartsScale);
                    bw.WriteLoaProp(TranslucencySortKeyFactor);
                    bw.WriteLoaProp(TranslucencySortPriority);
                    bw.WriteLoaProp(LayerLevel);
                    if (SectionInfos != null)
                        bw.WriteLoaProp(SectionInfos);
                    if (DyeChangeMaterials != null)
                    {
                        bw.WriteLoaProp(DyeChangeMaterials);
                    }
                    bw.WriteLoaProp(DyePartAUsed);
                    bw.WriteLoaProp(DyePartBUsed);
                    bw.WriteLoaProp(DyePartCUsed);

                    if (DyePatternAUsed != null)
                    {
                        bw.WriteLoaProp(DyePatternAUsed);
                    }
                    if (DyePatternBUsed != null)
                    {
                        bw.WriteLoaProp(DyePatternBUsed);
                    }
                    if (DyePatternCUsed != null)
                    {
                        bw.WriteLoaProp(DyePatternCUsed);
                    }
                }
                return ms.ToArray();
            }
        }

        protected override void Deserialize(BinaryReader br)
        {
            ArrayKeyword = br.ReadStringLoa();
            SecondKeyword = br.ReadStringLoa();
            PrimaryKey = br.ReadLoaProp(PrimaryKey);
            SecondaryKey = br.ReadLoaProp(SecondaryKey);
            MeshPartsType = br.ReadLoaProp(MeshPartsType);
            RequireStance = br.ReadLoaProp(RequireStance);
            PartsMeshKey = br.ReadLoaProp(PartsMeshKey);
            var nextKey = br.GetNextKey();
            if (nextKey == "Materials")
            {
                Materials = br.ReadLoaProp(Materials!);
            }
            nextKey = br.GetNextKey();
            if (nextKey == "MaterialsVariation")
            {
                MaterialsVariations = br.ReadLoaProp(MaterialsVariations!);
            }
            AnimSetTemplate = br.ReadLoaProp(AnimSetTemplate);
            PhysicsAsset = br.ReadLoaProp(PhysicsAsset);
            SocketGroupName = br.ReadLoaProp(SocketGroupName);
            SkelControlGroup = br.ReadLoaProp(SkelControlGroup);
            MorphSet = br.ReadLoaProp(MorphSet);
            Trails_Default = br.ReadLoaProp(Trails_Default);
            UseOnePassLightingOnTranslucency = br.ReadLoaProp(UseOnePassLightingOnTranslucency);
            PartsScale = br.ReadLoaProp(PartsScale);
            TranslucencySortKeyFactor = br.ReadLoaProp(TranslucencySortKeyFactor);
            TranslucencySortPriority = br.ReadLoaProp(TranslucencySortPriority);
            LayerLevel = br.ReadLoaProp(LayerLevel);

            nextKey = br.GetNextKey();

            if (nextKey == "SectionInfos")
            {
                SectionInfos = br.ReadLoaProp(SectionInfos!);
            }
            nextKey = br.GetNextKey();

            if (nextKey == "DyeChangeMaterials")
            {
                DyeChangeMaterials = br.ReadLoaProp(DyeChangeMaterials!);
            }

            DyePartAUsed = br.ReadLoaProp(DyePartAUsed);
            DyePartBUsed = br.ReadLoaProp(DyePartBUsed);
            DyePartCUsed = br.ReadLoaProp(DyePartCUsed);

            nextKey = br.GetNextKey();
            if (nextKey == "DyePatternAUsed")
            {
                DyePatternAUsed = br.ReadLoaProp(DyePatternAUsed!);
            }
            nextKey = br.GetNextKey();
            if (nextKey == "DyePatternBUsed")
            {
                DyePatternBUsed = br.ReadLoaProp(DyePatternBUsed!);
            }
            nextKey = br.GetNextKey();
            if (nextKey == "DyePatternCUsed")
            {
                DyePatternCUsed = br.ReadLoaProp(DyePatternCUsed!);
            }
        }
    }
}