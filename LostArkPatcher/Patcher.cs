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
                Console.WriteLine(patchesDir + " does not exist.\nPlease create the directory and place patches inside.");
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
                Console.WriteLine("No patches found in " + patchesDir);
                return;
            }
            else
            {
                Console.WriteLine($"{allPatches.Length} patches found.\nApplying:");
                foreach (var file in allPatches)
                {
                    Console.WriteLine($"\t-{Path.GetFileNameWithoutExtension(file)}");
                }
            }
            lpk.ApplySqlFiles(allPatches);
            lpk.RepackToFile(dataPath);
            Console.WriteLine("Done");
        }
    }
}
