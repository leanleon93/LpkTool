namespace LpkTool.Library.LoaData
{
    public abstract class LoaSubclass : ILoaSerializable
    {
        public LoaSubclass()
        {
        }

        public LoaSubclass(BinaryReader br)
        {
            Deserialize(br);
        }

        public abstract byte[] Serialize();

        protected abstract void Deserialize(BinaryReader br);
    }
}
