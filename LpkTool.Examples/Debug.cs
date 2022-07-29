using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpkTool.Examples
{
    internal class Debug
    {
        private static string _inPath = @"F:\SteamLibrary\steamapps\common\Lost Ark\EFGame\data.lpk";
        private static string _outDir = @"C:\Users\leanw\Documents\quickbms\debug";

        public static void UnpackDb(string name)
        {
            var lpk = LpkTool.Library.Lpk.FromFile(_inPath);
            var file = lpk.GetFileByName(name);
            var outPath = Path.Combine(_outDir, name);
            File.WriteAllBytes(outPath, file.GetData());
        }

        public static void ReplaceFile(string name, string path)
        {
            var lpk = LpkTool.Library.Lpk.FromFile(_inPath);
            var file = lpk.GetFileByName(name);
            //var data = File.ReadAllBytes(path);
            //FixDbData(ref data);
            //var outPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileName(path) + ".fix");
            //File.WriteAllBytes(outPath, data);
            file.ReplaceData(path);
            lpk.RepackToFile(_inPath);
        }

        private static void FixDbData(ref byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var bw = new BinaryWriter(ms))
                {
                    var i = 1;
                    while (bw.BaseStream.Length > bw.BaseStream.Position + 1008)
                    {
                        bw.BaseStream.Position += 1008;
                        var pos = bw.BaseStream.Position;
                        bw.Write(Enumerable.Repeat<byte>(0x10, 16).ToArray());
                        bw.BaseStream.Position = pos + 16;
                        i++;
                    }
                }
            }
        }

    }
}
