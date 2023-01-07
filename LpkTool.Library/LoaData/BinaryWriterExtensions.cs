using System.Text;

namespace LpkTool.Library.LoaData
{
    public static class BinaryWriterExtensions
    {
        public static void WriteStringLoa(this BinaryWriter bw, string text)
        {
            var length = text.Length;
            if (length == 0)
            {
                bw.Write(length);
                return;
            }
            bw.Write(length + 1);
            var textData = Encoding.UTF8.GetBytes(text);
            bw.Write(textData);
            bw.Write((byte)0);
            return;
        }

        internal static KeyValuePair<string, T> KvpFromProp<T>(string key, T value)
        {
            return new KeyValuePair<string, T>(key, value);
        }

        public static void WriteKvpLoa<T>(this BinaryWriter bw, string key, T value)
        {
            var kvp = KvpFromProp(key, value);
            bw.WriteKvpLoa(kvp);
        }

        public static void WriteKvpLoa<T>(this BinaryWriter bw, KeyValuePair<string, T> kvp)
        {
            var key = kvp.Key;
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.String:
                    var value = (string)(object)kvp.Value;
                    bw.WriteStringLoa(key);
                    bw.WriteStringLoa(value);
                    break;
                case TypeCode.Int32:
                    var value1 = (int)(object)kvp.Value;
                    bw.WriteStringLoa(key);
                    bw.Write(value1);
                    break;
                case TypeCode.Int64:
                    var value2 = (long)(object)kvp.Value;
                    bw.WriteStringLoa(key);
                    bw.Write(value2);
                    break;
                case TypeCode.Boolean:
                    var value4 = (bool)(object)kvp.Value;
                    bw.WriteStringLoa(key);
                    bw.Write(value4);
                    bw.Write(new byte[3]);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
