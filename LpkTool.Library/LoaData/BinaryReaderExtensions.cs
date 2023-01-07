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

        public static KeyValuePair<string, T> ReadKvpLoa<T>(this BinaryReader br)
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
                default:
                    throw new NotImplementedException();
            }
            return result;
        }

    }
}
