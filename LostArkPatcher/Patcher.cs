using LpkTool.Library;
using LpkTool.Library.Helpers;

namespace LostArkPatcher
{
    public static class Patcher
    {
        private static readonly string _docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static readonly string _exportsDir = Path.Combine(_docPath, "LostArk", "exports");
        private static readonly string _patchesDir = Path.Combine(_docPath, "LostArk", "patches");

        public static void ApplyAllPatches(string dataPath, string fontPath)
        {
            if (!Directory.Exists(_patchesDir))
            {
                Directory.CreateDirectory(_patchesDir);
                Console.WriteLine($"Please place patches in: {_patchesDir}.");
                return;
            }
            var allFiles = Directory.GetFiles(_patchesDir);
            var lpk = Lpk.FromFile(dataPath);

            ApplyExports(lpk, allFiles);
            ApplyList(lpk, allFiles);
            ApplyFontSwap(fontPath, allFiles);

            var requireDataRepack = ApplyPatches(lpk, allFiles);

            if (requireDataRepack)
            {
                Console.Write("\nRepacking data ->");
                lpk.RepackToFile(dataPath);
                WriteLineInColor(ConsoleColor.Green, " done");
            }
        }

        private static void ApplyFontSwap(string fontPath, string[] allFiles)
        {
            var fontswapFilePath = allFiles.FirstOrDefault(x => Path.GetFileName(x) == "font");
            if (fontswapFilePath == null) return;
            var fontFileContent = File.ReadAllLines(fontswapFilePath);
            if (fontFileContent.Length != 2)
            {
                WriteLineInColor(ConsoleColor.Yellow, "Font file malformed. Skipping font swap.");
                return;
            }
            var language = fontFileContent[0];
            var fontFilename = fontFileContent[1];
            var fontFilePath = Path.Combine(_patchesDir, fontFilename);
            if (!File.Exists(fontFilePath))
            {
                WriteLineInColor(ConsoleColor.Yellow, "Font file not found. Skipping font swap.");
                return;
            }
            Console.WriteLine("Applying font swap:\n");
            WriteInColor(ConsoleColor.Cyan, $"\t-{language}: \"{fontFilename}\"");
            Console.Write($" ->");
            var swapper = new FontSwapper(fontPath);
            swapper.SwapFont(fontFilePath);
            WriteLineInColor(ConsoleColor.Green, " done");
            Console.WriteLine();
        }

        private static bool ApplyPatches(Lpk lpk, string[] allFiles)
        {
            var allPatches = allFiles.Where(x => Path.GetExtension(x) == ".sql").ToArray();
            if (allPatches.Length <= 0)
            {
                Console.WriteLine("No patches found in: " + _patchesDir);
                return false;
            }
            else
            {
                int fails = 0;
                Console.WriteLine($"{allPatches.Length} patches found.\nApplying:\n");
                foreach (var file in allPatches)
                {
                    WriteInColor(ConsoleColor.Cyan, $"\t-{Path.GetFileNameWithoutExtension(file)}");
                    Console.Write($" ->");
                    var success = lpk.ApplySqlFile(file);
                    if (success)
                    {
                        WriteLineInColor(ConsoleColor.Green, " done");
                    }
                    else
                    {
                        WriteLineInColor(ConsoleColor.Red, " failed");
                        fails++;
                    }
                }
                if (fails > 0)
                {
                    WriteLineInColor(ConsoleColor.DarkYellow, $"\n{fails} {(fails == 1 ? "patch" : "patches")} not applied!");
                }
            }
            return true;
        }

        private static void ApplyList(Lpk lpk, string[] allFiles)
        {
            var listFile = allFiles.FirstOrDefault(x => Path.GetFileName(x).ToLower() == "list");
            if (listFile != null)
            {
                Directory.CreateDirectory(_exportsDir);
                var outPath = Path.Combine(_exportsDir, "dblist.txt");
                var allDbs = lpk.Files.Where(x => x.FilePath.EndsWith(".db")).ToList();
                var dbNames = allDbs.Select(x => Path.GetFileName(x.FilePath)).ToList();
                File.WriteAllLines(outPath, dbNames);
                File.Delete(listFile);
            }
        }

        private static void ApplyExports(Lpk lpk, string[] allFiles)
        {
            var exportFiles = allFiles.Where(x => Path.GetExtension(x) == ".export").ToArray();

            if (exportFiles.Length > 0)
            {
                Console.Write("Running exports: ");
                foreach (var file in exportFiles)
                {
                    var dbFileName = Path.GetFileNameWithoutExtension(file) + ".db";
                    var exportFile = lpk.GetFileByName(dbFileName);
                    if (exportFile != null)
                    {
                        Directory.CreateDirectory(_exportsDir);
                        var exportPath = Path.Combine(_exportsDir, dbFileName);
                        File.WriteAllBytes(exportPath, exportFile.GetData());
                    }
                }
                WriteLineInColor(ConsoleColor.Green, " done.\n");
            }
        }

        internal static void WriteInColor(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void WriteLineInColor(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}
