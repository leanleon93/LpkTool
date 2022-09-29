using LpkTool.Library;

namespace LostArkPatcher
{
    public static class Patcher
    {
        public static void ApplyAllPatches(string dataPath)
        {
            var docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var patchesDir = Path.Combine(docPath, "LostArk", "patches");
            var exportsDir = Path.Combine(docPath, "LostArk", "exports");
            if (!Directory.Exists(patchesDir))
            {
                Directory.CreateDirectory(patchesDir);
                Console.WriteLine($"Please place patches in: {patchesDir}.");
                return;
            }
            var allFiles = Directory.GetFiles(patchesDir);
            var exportFiles = allFiles.Where(x => Path.GetExtension(x) == ".export").ToArray();
            var lpk = Lpk.FromFile(dataPath);
            var listFile = allFiles.FirstOrDefault(x => Path.GetFileName(x).ToLower() == "list");
            if (listFile != null)
            {
                Directory.CreateDirectory(exportsDir);
                var outPath = Path.Combine(exportsDir, "dblist.txt");
                var allDbs = lpk.Files.Where(x => x.FilePath.EndsWith(".db")).ToList();
                var dbNames = allDbs.Select(x => Path.GetFileName(x.FilePath)).ToList();
                File.WriteAllLines(outPath, dbNames);
                File.Delete(listFile);
            }
            foreach (var file in exportFiles)
            {
                var dbFileName = Path.GetFileNameWithoutExtension(file) + ".db";
                var exportFile = lpk.GetFileByName(dbFileName);
                if (exportFile != null)
                {
                    Directory.CreateDirectory(exportsDir);
                    var exportPath = Path.Combine(exportsDir, dbFileName);
                    File.WriteAllBytes(exportPath, exportFile.GetData());
                }
            }
            var allPatches = allFiles.Where(x => Path.GetExtension(x) == ".sql").ToArray();
            if (allPatches.Length <= 0)
            {
                Console.WriteLine("No patches found in: " + patchesDir);
                return;
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

            Console.Write("\nRepacking ->");
            lpk.RepackToFile(dataPath);
            WriteLineInColor(ConsoleColor.Green, " done");
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
