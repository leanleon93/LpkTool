using System.Text;

namespace LpkTool.Library.LoaData
{
    public static class BinaryWriterExtensions
    {
        public static void WriteStringLoa(this BinaryWriter bw, string text, bool unicode = false)
        {
            var length = text.Length;
            if (length == 0)
            {
                bw.Write(length);
                return;
            }
            if (unicode)
            {
                WriteStringUnicode(bw, text, length);
            }
            else
            {
                WriteStringUtf8(bw, text, length);
            }
        }

        private static void WriteStringUtf8(BinaryWriter bw, string text, int length)
        {
            bw.Write(length + 1);
            var textData = Encoding.UTF8.GetBytes(text);
            bw.Write(textData);
            bw.Write((byte)0);
        }

        private static void WriteStringUnicode(BinaryWriter bw, string text, int length)
        {
            var unicodeLength = 0 - length - 1;
            bw.Write(unicodeLength);
            var textData = Encoding.Unicode.GetBytes(text);
            bw.Write(textData);
            bw.Write((byte)0);
            bw.Write((byte)0);
        }

        public static void WriteLoaProp<T>(this BinaryWriter bw, LoaProp<LoaProp<T>[]> prop)
        {
            bw.WriteStringLoa(prop.Key);
            var array = prop.Value;
            var length = array.Length;
            bw.Write(length);
            for (var i = 0; i < length; i++)
            {
                var kvp = array[i];
                bw.WriteLoaProp(kvp);
            }
        }

        public static void WriteLoaProp<T>(this BinaryWriter bw, LoaProp<List<T>> prop) where T : LoaSubclass, new()
        {
            bw.WriteStringLoa(prop.Key);
            var list = prop.Value;
            var length = list.Count;
            bw.Write(length);
            for (var i = 0; i < length; i++)
            {
                var item = list[i];
                bw.Write(item.Serialize());
            }
        }

        public static void WriteLoaProp<T>(this BinaryWriter bw, LoaProp<T> prop)
        {
            if (prop.Value is LoaSubclass ls)
            {
                var data = ls.Serialize();
                bw.WriteStringLoa(prop.Key);
                bw.Write(data);
            }
            else
            {
                var kvp = new KeyValuePair<string, T>(prop.Key, prop.Value);
                bw.WriteKvpLoa(kvp);
            }
        }

        private static void WriteKvpLoa<T>(this BinaryWriter bw, KeyValuePair<string, T> kvp)
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
                    var value3 = (bool)(object)kvp.Value;
                    bw.WriteStringLoa(key);
                    bw.Write(value3);
                    bw.Write(new byte[3]);
                    break;
                case TypeCode.Single:
                    var value4 = (float)(object)kvp.Value;
                    bw.WriteStringLoa(key);
                    bw.Write(value4);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
