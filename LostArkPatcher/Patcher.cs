using LpkTool.Library;
using LpkTool.Library.Helpers;
using LpkTool.Library.LoaData.Table_MovieData;
using Newtonsoft.Json;
using Region = LpkTool.Library.LpkData.Region;

namespace LostArkPatcher
{
    public class Patcher
    {
        private readonly string _exportsDir;
        private readonly string _patchesDir;
        private readonly string _efGameDir;
        private const string DATAFILE_NAME_BASE = "data{0}.lpk";
        private const string FONT_FILE_NAME = "font.lpk";

        private readonly Region _region;

        public Patcher(string lostarkInstallDir, Region region = Region.EU)
        {
            var docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _exportsDir = Path.Combine(docPath, "LostArk", "exports");
            _patchesDir = Path.Combine(docPath, "LostArk", "patches");
            _efGameDir = Path.Combine(lostarkInstallDir, "EFGame");
            _region = region;
            _uncensored = false;
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
            ApplyFullUncensor(allFiles);
            ApplyArtistUncensor(allFiles);
            ApplyFileReplaces(allFiles);
            ApplyFasterStartup(data1Path, allFiles);
            ApplyRemoveAllCutscenes(data1Path, allFiles);
            if (ApplyPatches(data2Lpk, allFiles))
            {
                Console.Write("\nRepacking data2 ->");
                data2Lpk.RepackToFile(data2Path);
                WriteLineInColor(ConsoleColor.Green, " done");
            }
        }

        private void ApplyRemoveAllCutscenes(string lpkPath, string[] allFiles)
        {
            var fasterStartupFile = allFiles.FirstOrDefault(x => Path.GetFileName(x) == "cutsceneRemoval");
            if (fasterStartupFile == null) return;
            Console.Write("Applying remove all cutscenes -> ");

            var lpk = Lpk.FromFile(lpkPath, _region);
            var file = lpk.GetFileByName("Table_MovieData.loa");
            if (file == null) return;
            var movieData = new MovieData(file.GetData());
            var movieDataContainers = movieData.MovieDataContainers.Value;
            foreach (var movieContainer in movieDataContainers.Where(x => x != null))
            {
                movieContainer.MovieDataValueArray.Value = new List<MovieDataValue>();
            }

            var newData = movieData.Serialize();

            file.ReplaceData(newData);
            lpk.RepackToFile(lpkPath);

            WriteLineInColor(ConsoleColor.Green, "done\n");
        }

        private bool _uncensored;

        private void ApplyFullUncensor(string[] allFiles)
        {
            var replaceFile = allFiles.FirstOrDefault(x => Path.GetFileName(x) == "uncensor_plus");
            if (replaceFile == null) return;
            var replaceItemDir = File.ReadAllText(replaceFile);
            Console.Write("Applying full korean uncensor -> ");

            var allItemFiles = Directory.GetFiles(replaceItemDir, "", SearchOption.AllDirectories);
            var artistOutfitFiles = allItemFiles.Where(x => Path.GetFileNameWithoutExtension(x).ToLower().Contains("pc_sp")).ToList();
            artistOutfitFiles.AddRange(allItemFiles.Where(x => Path.GetFileNameWithoutExtension(x).ToLower().Contains("pc_sdm")).ToList());
            artistOutfitFiles.AddRange(allItemFiles.Where(x => x.ToLower().Contains(@"\item\")).ToList());
            artistOutfitFiles.AddRange(allItemFiles.Where(x => x.ToLower().Contains(@"\monster\")).ToList());
            artistOutfitFiles.AddRange(allItemFiles.Where(x => x.ToLower().Contains(@"\human\")).ToList());
            artistOutfitFiles = artistOutfitFiles.Distinct().ToList();
            var replaces = new List<FileReplace>();
            foreach (var outfitFile in artistOutfitFiles)
            {
                replaces.Add(new FileReplace() {
                    DataFileId = 4,
                    NewFilePath = outfitFile,
                    SearchFilename = Path.GetFileName(outfitFile)
                });
            }
            RunReplaces(replaces.ToArray());

            WriteLineInColor(ConsoleColor.Green, "done\n");
            _uncensored = true;
        }

        private void ApplyArtistUncensor(string[] allFiles)
        {
            var replaceFile = allFiles.FirstOrDefault(x => Path.GetFileName(x) == "uncensor");
            if (replaceFile == null || _uncensored) return;
            var replaceItemDir = File.ReadAllText(replaceFile);
            Console.Write("Applying artist uncensor -> ");

            var allItemFiles = Directory.GetFiles(replaceItemDir, "", SearchOption.AllDirectories);
            var artistOutfitFiles = allItemFiles.Where(x => Path.GetFileNameWithoutExtension(x).ToLower().Contains("pc_sp")).ToList();
            artistOutfitFiles.AddRange(allItemFiles.Where(x => Path.GetFileNameWithoutExtension(x).ToLower().Contains("pc_sdm")).ToList());
            artistOutfitFiles = artistOutfitFiles.Distinct().ToList();
            var replaces = new List<FileReplace>();
            foreach (var outfitFile in artistOutfitFiles)
            {
                replaces.Add(new FileReplace() {
                    DataFileId = 4,
                    NewFilePath = outfitFile,
                    SearchFilename = Path.GetFileName(outfitFile)
                });
            }
            RunReplaces(replaces.ToArray());

            WriteLineInColor(ConsoleColor.Green, "done\n");
            _uncensored = true;
        }

        private void ApplyFileReplaces(string[] allFiles)
        {
            var replaceFile = allFiles.FirstOrDefault(x => Path.GetFileName(x) == "replaces.json");
            if (replaceFile == null) return;
            Console.Write("Applying replaces -> ");
            var replaces = JsonConvert.DeserializeObject<FileReplace[]>(File.ReadAllText(replaceFile));
            RunReplaces(replaces);
            WriteLineInColor(ConsoleColor.Green, "done\n");
        }

        private void RunReplaces(FileReplace[]? replaces)
        {
            foreach (var groupedReplaces in replaces.GroupBy(x => x.DataFileId))
            {
                var id = groupedReplaces.Key;
                var dataPath = Path.Combine(_efGameDir, string.Format(DATAFILE_NAME_BASE, id));
                var lpk = Lpk.FromFile(dataPath, _region);
                foreach (var replace in groupedReplaces)
                {
                    var file = lpk.GetFileByName(replace.SearchFilename);
                    if (file == null) continue;
                    file.ReplaceData(replace.NewFilePath);
                }
                lpk.RepackToFile(dataPath);
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
