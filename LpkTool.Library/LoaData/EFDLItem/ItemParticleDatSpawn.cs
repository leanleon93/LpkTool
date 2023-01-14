namespace LpkTool.Library.LoaData.EFDLItem
{
    public class ItemParticleDatSpawn : LoaSubclass
    {
        public ItemParticleDatSpawn()
        {
        }

        public ItemParticleDatSpawn(BinaryReader br) : base(br)
        {
        }

        public string ArrayKeyword { get; set; }
        public string SecondKeyword { get; set; }

        public LoaProp<int> FxLod { get; set; }
        public LoaProp<int> ParticleDataType { get; set; }
        public LoaProp<int> DataIndex { get; set; }
        public LoaProp<string> ParticleSystem { get; set; }
        public LoaProp<bool> SpawnedEmitter { get; set; }
        public LoaProp<bool> SpawnedEmitterAbsoluteRotation { get; set; }
        public LoaProp<bool> Attach { get; set; }
        public LoaProp<bool> IgnoreAttachLocation { get; set; }

        public LoaProp<bool> IgnoreAttachRotation { get; set; }
        public LoaProp<bool> IgnoreAttachWorldRotation { get; set; }
        public LoaProp<bool> ApplyLocalRotation { get; set; }
        public LoaProp<bool> ApplyPawnRotation { get; set; }
        public LoaProp<bool> UseCastShadow { get; set; }
        public LoaProp<bool> BeamParticle { get; set; }
        public LoaProp<int> OwnerPartsType { get; set; }
        public LoaProp<int> OwnerPartsMeshIndex { get; set; }
        public LoaProp<LoaProp<string>[]>? OwnerBoneNames { get; set; }
        public LoaProp<LoaProp<string>[]>? OwnerSocketNames { get; set; }
        public LoaProp<int> OwnerBoneSocketRandomMaxCount { get; set; }
        public LoaProp<float> ModifyParentVelocity { get; set; }
        public LoaProp<float> ModifyParentAcceleration { get; set; }
        public LoaProp<Vector> RelativeLocation { get; set; }
        public LoaProp<Vector> RelativeWorldLocation { get; set; }
        public LoaProp<Rotation> RelativeRotation { get; set; }
        public LoaProp<RandomTransformRotation> RandomTransformRotation { get; set; }
        public LoaProp<Vector> RelativeScale { get; set; }
        public LoaProp<List<ParticleSystemParam>>? ParticleSystemParamList { get; set; }
        public LoaProp<string> SoundModule { get; set; }
        public LoaProp<string> OwnerMaterialParamModule { get; set; }
        public LoaProp<string> ReplaceOwnerMaterialParamModule { get; set; }


        public override byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.WriteStringLoa(ArrayKeyword);
                    bw.WriteStringLoa(SecondKeyword);
                    bw.WriteLoaProp(FxLod);
                    bw.WriteLoaProp(ParticleDataType);
                    bw.WriteLoaProp(DataIndex);
                    bw.WriteLoaProp(ParticleSystem);
                    bw.WriteLoaProp(SpawnedEmitter);
                    bw.WriteLoaProp(SpawnedEmitterAbsoluteRotation);
                    bw.WriteLoaProp(Attach);
                    bw.WriteLoaProp(IgnoreAttachLocation);
                    bw.WriteLoaProp(IgnoreAttachRotation);
                    bw.WriteLoaProp(IgnoreAttachWorldRotation);
                    bw.WriteLoaProp(ApplyLocalRotation);
                    bw.WriteLoaProp(ApplyPawnRotation);
                    bw.WriteLoaProp(UseCastShadow);
                    bw.WriteLoaProp(BeamParticle);
                    bw.WriteLoaProp(OwnerPartsType);
                    bw.WriteLoaProp(OwnerPartsMeshIndex);
                    if (OwnerBoneNames != null)
                    {
                        bw.WriteLoaProp(OwnerBoneNames);
                    }
                    if (OwnerSocketNames != null)
                    {
                        bw.WriteLoaProp(OwnerSocketNames);
                    }
                    bw.WriteLoaProp(OwnerBoneSocketRandomMaxCount);
                    bw.WriteLoaProp(ModifyParentVelocity);
                    bw.WriteLoaProp(ModifyParentAcceleration);
                    bw.WriteLoaProp(RelativeLocation);
                    bw.WriteLoaProp(RelativeWorldLocation);
                    bw.WriteLoaProp(RelativeRotation);
                    bw.WriteLoaProp(RandomTransformRotation);
                    bw.WriteLoaProp(RelativeScale);
                    if (ParticleSystemParamList != null)
                    {
                        bw.WriteLoaProp(ParticleSystemParamList);
                    }
                    bw.WriteLoaProp(SoundModule);
                    bw.WriteLoaProp(OwnerMaterialParamModule);
                    bw.WriteLoaProp(ReplaceOwnerMaterialParamModule);
                }
                return ms.ToArray();
            }
        }

        protected override void Deserialize(BinaryReader br)
        {
            ArrayKeyword = br.ReadStringLoa();
            SecondKeyword = br.ReadStringLoa();

            FxLod = br.ReadLoaProp(FxLod);
            ParticleDataType = br.ReadLoaProp(ParticleDataType);
            DataIndex = br.ReadLoaProp(DataIndex);
            ParticleSystem = br.ReadLoaProp(ParticleSystem);
            SpawnedEmitter = br.ReadLoaProp(SpawnedEmitter);
            SpawnedEmitterAbsoluteRotation = br.ReadLoaProp(SpawnedEmitterAbsoluteRotation);
            Attach = br.ReadLoaProp(Attach);
            IgnoreAttachLocation = br.ReadLoaProp(IgnoreAttachLocation);
            IgnoreAttachRotation = br.ReadLoaProp(IgnoreAttachRotation);
            IgnoreAttachWorldRotation = br.ReadLoaProp(IgnoreAttachWorldRotation);
            ApplyLocalRotation = br.ReadLoaProp(ApplyLocalRotation);
            ApplyPawnRotation = br.ReadLoaProp(ApplyPawnRotation);
            UseCastShadow = br.ReadLoaProp(UseCastShadow);
            BeamParticle = br.ReadLoaProp(BeamParticle);
            OwnerPartsType = br.ReadLoaProp(OwnerPartsType);
            OwnerPartsMeshIndex = br.ReadLoaProp(OwnerPartsMeshIndex);
            var nextKey = br.GetNextKey();
            if (nextKey == "OwnerBoneName")
            {
                OwnerBoneNames = br.ReadLoaProp(OwnerBoneNames!);
            }

            nextKey = br.GetNextKey();
            if (nextKey == "OwnerSoketName")
            {
                OwnerSocketNames = br.ReadLoaProp(OwnerSocketNames!);
            }

            OwnerBoneSocketRandomMaxCount = br.ReadLoaProp(OwnerBoneSocketRandomMaxCount);
            ModifyParentVelocity = br.ReadLoaProp(ModifyParentVelocity);
            ModifyParentAcceleration = br.ReadLoaProp(ModifyParentAcceleration);
            RelativeLocation = br.ReadLoaProp(RelativeLocation);
            RelativeWorldLocation = br.ReadLoaProp(RelativeWorldLocation);
            RelativeRotation = br.ReadLoaProp(RelativeRotation);
            RandomTransformRotation = br.ReadLoaProp(RandomTransformRotation);
            RelativeScale = br.ReadLoaProp(RelativeScale);
            nextKey = br.GetNextKey();
            if (nextKey == "ParticleSystemParamList")
            {
                ParticleSystemParamList = br.ReadLoaProp(ParticleSystemParamList!);
            }
            SoundModule = br.ReadLoaProp(SoundModule);
            OwnerMaterialParamModule = br.ReadLoaProp(OwnerMaterialParamModule);
            ReplaceOwnerMaterialParamModule = br.ReadLoaProp(ReplaceOwnerMaterialParamModule);
        }
    }
}