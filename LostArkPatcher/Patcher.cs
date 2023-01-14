using LpkTool.Library;
using LpkTool.Library.Helpers;
using LpkTool.Library.LoaData.Table_MovieData;
using LpkTool.Library.LpkData;

namespace LostArkPatcher
{
    public class Patcher
    {
        private readonly string _exportsDir;
        private readonly string _patchesDir;
        private readonly string _efGameDir;
        private const string DATAFILE_NAME_BASE = "data{0}.lpk";
        private const string FONT_FILE_NAME = "font.lpk";

        private Region _region;

        public Patcher(string lostarkInstallDir, Region region = Region.EU)
        {
            var docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _exportsDir = Path.Combine(docPath, "LostArk", "exports");
            _patchesDir = Path.Combine(docPath, "LostArk", "patches");
            _efGameDir = Path.Combine(lostarkInstallDir, "EFGame");
            _region = region;
        }

        public void ApplyAllPatches()
        {
            if (!Directory.Exists(_patchesDir))
            {
                Directory.CreateDirectory(_patchesDir);
                Console.WriteLine($"Please place patches in: {_patchesDir}.");
                return;
            }
            var allFiles = Directory.GetFiles(_patchesDir);

            var data1Path = Path.Combine(_efGameDir, string.Format(DATAFILE_NAME_BASE, 1));
            var data2Path = Path.Combine(_efGameDir, string.Format(DATAFILE_NAME_BASE, 2));
            var fontPath = Path.Combine(_efGameDir, FONT_FILE_NAME);

            var data2Lpk = Lpk.FromFile(data2Path, _region);

            ApplyExports(data2Lpk, allFiles);
            ApplyList(data2Lpk, allFiles);
            ApplyFontSwap(fontPath, allFiles);

            ApplyFasterStartup(data1Path, allFiles);

            if (ApplyPatches(data2Lpk, allFiles))
            {
                Console.Write("\nRepacking data2 ->");
                data2Lpk.RepackToFile(data2Path);
                WriteLineInColor(ConsoleColor.Green, " done");
            }
        }

        private void ApplyFasterStartup(string lpkPath, string[] allFiles)
        {
            var fasterStartupFile = allFiles.FirstOrDefault(x => Path.GetFileName(x) == "fastStartup");
            if (fasterStartupFile == null) return;
            Console.Write("Applying faster startup -> ");
            var lpk = Lpk.FromFile(lpkPath, _region);
            var file = lpk.GetFileByName("Table_MovieData.loa");
            if (file == null) return;
            var movieData = new MovieData(file.GetData());
            var movieDataContainers = movieData.MovieDataContainers.Value;
            var introContainer = movieDataContainers.FirstOrDefault(x => x.Key.Value == "FullScreen.Intro");
            if (introContainer == null) return;
            introContainer.MovieDataValueArray.Value = new List<MovieDataValue>();

            var newData = movieData.Serialize();

            file.ReplaceData(newData);
            lpk.RepackToFile(lpkPath);
            WriteLineInColor(ConsoleColor.Green, "done\n");
        }

        private void ApplyFontSwap(string fontPath, string[] allFiles)
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
            swapper.SwapFont(fontFilePath, _region);
            WriteLineInColor(ConsoleColor.Green, " done");
            Console.WriteLine();
        }

        private bool ApplyPatches(Lpk lpk, string[] allFiles)
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

        private void ApplyList(Lpk lpk, string[] allFiles)
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

        private void ApplyExports(Lpk lpk, string[] allFiles)
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
