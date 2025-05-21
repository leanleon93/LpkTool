namespace LpkTool.Library.LoaData.IconInfo
{
    public class EFIconPack : LoaSubclass
    {
        public EFIconPack()
        {
        }

        public EFIconPack(BinaryReader br) : base(br)
        {
        }

        public string ArrayKeyword { get; set; }
        public string SecondKeyword { get; set; }

        public LoaProp<string> EachTextureName { get; set; }
        public LoaProp<string> TextureAtlasName { get; set; }
        public LoaProp<int> TextureStartX { get; set; }
        public LoaProp<int> TextureStartY { get; set; }
        public LoaProp<int> TextureWidth { get; set; }
        public LoaProp<int> TextureHeight { get; set; }
        public LoaProp<string> LanguageOption { get; set; }
        public LoaProp<bool> OptionDirectory { get; set; }
        public LoaProp<bool> OptionEncrypt { get; set; }

        public override byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.WriteStringLoa(ArrayKeyword);
                    bw.WriteStringLoa(SecondKeyword);

                    bw.WriteLoaProp(EachTextureName);
                    bw.WriteLoaProp(TextureAtlasName);
                    bw.WriteLoaProp(TextureStartX);
                    bw.WriteLoaProp(TextureStartY);
                    bw.WriteLoaProp(TextureWidth);
                    bw.WriteLoaProp(TextureHeight);
                    bw.WriteLoaProp(LanguageOption);
                    bw.WriteLoaProp(OptionDirectory);
                    bw.WriteLoaProp(OptionEncrypt);
                }
                return ms.ToArray();
            }
        }

        protected override void Deserialize(BinaryReader br)
        {
            ArrayKeyword = br.ReadStringLoa();
            SecondKeyword = br.ReadStringLoa();

            EachTextureName = br.ReadLoaProp(EachTextureName);
            TextureAtlasName = br.ReadLoaProp(TextureAtlasName);
            TextureStartX = br.ReadLoaProp(TextureStartX);
            TextureStartY = br.ReadLoaProp(TextureStartY);
            TextureWidth = br.ReadLoaProp(TextureWidth);
            TextureHeight = br.ReadLoaProp(TextureHeight);
            LanguageOption = br.ReadLoaProp(LanguageOption);
            OptionDirectory = br.ReadLoaProp(OptionDirectory);
            OptionEncrypt = br.ReadLoaProp(OptionEncrypt);
        }
    }
}