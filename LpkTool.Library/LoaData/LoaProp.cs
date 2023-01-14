namespace LpkTool.Library.LoaData
{
    public class LoaProp<T>
    {
        public LoaProp(string key)
        {
            Key = key;
        }
        public LoaProp(string key, T value)
        {
            Key = key;
            Value = value;
        }
        public string Key { get; private set; }
        public T Value { get; set; }
    }
}
