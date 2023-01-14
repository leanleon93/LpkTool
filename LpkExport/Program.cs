using LpkTool.Library;

namespace LpkExport
{
    internal class Program
    {
        private static readonly string _usage = "Usage LpkExport.exe \"data.lpk\"";
        private static readonly string _exporting = "Exporting \"{0}\"";

        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine(_usage);
                return;
            }
            var fileArg = args[0];
            if (!fileArg.EndsWith(".lpk") || !File.Exists(fileArg))
            {
                Console.WriteLine(_usage);
                return;
            }
            var lpk = Lpk.FromFile(fileArg, LpkTool.Library.LpkData.Region.EU);
            var dir = Path.GetDirectoryName(fileArg);
            var filename = Path.GetFileName(fileArg);
            dir = Path.Combine(dir!, filename + ".files");
            Console.WriteLine(string.Format(_exporting, filename));
            lpk.Export(dir);
        }
    }
}