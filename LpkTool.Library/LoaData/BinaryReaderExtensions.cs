using System.Text;

namespace LpkTool.Library.LoaData
{
    public static class BinaryReaderExtensions
    {
        public static string ReadStringLoa(this BinaryReader br)
        {
            var length = br.ReadInt32();
            if (length == 0) return "";
            var prefPos = br.BaseStream.Position;
            br.BaseStream.Seek(length - 1, SeekOrigin.Current);
            if (br.BaseStream.Position > br.BaseStream.Length || br.ReadByte() != 0)
            {
                var result = length.ToString();
                br.BaseStream.Position = prefPos;
                return result;
            }
            else
            {
                br.BaseStream.Position = prefPos;
            }

            var strBytes = br.ReadBytes(length - 1);
            var str = Encoding.UTF8.GetString(strBytes);
            br.BaseStream.Seek(1, SeekOrigin.Current);
            return str;
        }
    }
}
