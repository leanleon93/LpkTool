using System.Text;

namespace LpkTool.Library.LoaData
{
    public static class BinaryReaderExtensions
    {
        public static string ReadStringLoa(this BinaryReader br)
        {
            var length = br.ReadInt32();
            if (length == 0) return "";
            if (length < 0)
            {
                return ReadStringUnicode(br, length);
            }
            else
            {
                return ReadStringUtf8(br, length);
            }
        }

        private static string ReadStringUtf8(BinaryReader br, int length)
        {
            var strBytes = br.ReadBytes(length - 1);
            var str = Encoding.UTF8.GetString(strBytes);
            br.BaseStream.Seek(1, SeekOrigin.Current);
            return str;
        }

        private static string ReadStringUnicode(BinaryReader br, int length)
        {
            var unicodeLength = Math.Abs(length) * 2;
            var strBytes = br.ReadBytes(unicodeLength - 2);
            var str = Encoding.Unicode.GetString(strBytes);
            br.BaseStream.Seek(2, SeekOrigin.Current);
            return str;
        }

        public static LoaProp<LoaProp<T>[]> ReadLoaProp<T>(this BinaryReader br, LoaProp<LoaProp<T>[]> _)
        {
            var key = br.ReadStringLoa();
            var length = br.ReadInt32();
            var array = new LoaProp<T>[length];
            for (var i = 0; i < length; i++)
            {
                var kvp = br.ReadLoaProp<T>();
                array[i] = kvp;
            }
            var result = new LoaProp<LoaProp<T>[]>(key, array);
            return result;
        }

        public static LoaProp<List<T>> ReadLoaProp<T>(this BinaryReader br, LoaProp<List<T>> _) where T : LoaSubclass, new()
        {
            var key = br.ReadStringLoa();
            var num = br.ReadInt32();
            var list = new List<T>();
            for (int i = 0; i < num; i++)
            {
                list.Add((T)Activator.CreateInstance(typeof(T), br));
            }
            var result = new LoaProp<List<T>>(key, list);
            return result;
        }

        public static LoaProp<T> ReadLoaProp<T>(this BinaryReader br, LoaProp<T> _)
        {
            return br.ReadLoaProp<T>();
        }

        public static LoaProp<T> ReadLoaProp<T>(this BinaryReader br)
        {

            if (typeof(T).IsSubclassOf(typeof(LoaSubclass)))
            {
                var key = br.ReadStringLoa();
                var value = (T)Activator.CreateInstance(typeof(T), br);
                var prop = new LoaProp<T>(key, value);
                return prop;
            }
            else
            {
                var kvp = br.ReadKvpLoa<T>();
                var prop = new LoaProp<T>(kvp.Key, kvp.Value);
                return prop;
            }

        }

        public static string GetNextKey(this BinaryReader br)
        {
            var pos = br.BaseStream.Position;
            var nextKey = br.ReadStringLoa();
            br.BaseStream.Position = pos;
            return nextKey;
        }

        private static KeyValuePair<string, T> ReadKvpLoa<T>(this BinaryReader br)
        {
            var key = br.ReadStringLoa();
            KeyValuePair<string, T> result;
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.String:
                    var value = br.ReadStringLoa();
                    result = new KeyValuePair<string, T>(key, (T)(object)value);
                    break;
                case TypeCode.Int32:
                    var value1 = br.ReadInt32();
                    result = new KeyValuePair<string, T>(key, (T)(object)value1);
                    break;
                case TypeCode.Int64:
                    var value2 = br.ReadInt64();
                    result = new KeyValuePair<string, T>(key, (T)(object)value2);
                    break;
                case TypeCode.Boolean:
                    var value3 = br.ReadBoolean();
                    br.BaseStream.Seek(3, SeekOrigin.Current);
                    result = new KeyValuePair<string, T>(key, (T)(object)value3);
                    break;
                case TypeCode.Single:
                    var value4 = br.ReadSingle();
                    result = new KeyValuePair<string, T>(key, (T)(object)value4);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return result;
        }

    }
}
