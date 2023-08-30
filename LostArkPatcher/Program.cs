using Microsoft.Win32;
using Newtonsoft.Json;
using System.Diagnostics;

namespace LostArkPatcher
{

    internal class Program
    {
        [STAThread]
        //Place Patches in Documents/LostArk/Patches
        private static void Main()
        {
            Patcher.WriteLineInColor(ConsoleColor.Magenta, "- LostArk .lpk patcher by LEaN -\n");
            var lostArkInstallDir = GetInstallDir();
            Process[] pname = Process.GetProcessesByName("LOSTARK");
#if RELEASE
            var loc = AppContext.BaseDirectory;
            var sqlitedllName = "e_sqlite3.dll";
            var sqlitePath = Path.Combine(loc, sqlitedllName);
            if (!File.Exists(sqlitePath))
            {
                Patcher.WriteLineInColor(ConsoleColor.Red, $"{sqlitedllName} not found.");
                Console.WriteLine("\nPress any key to exit!");
                Console.ReadKey();
                return;
            }
#endif
            if (pname.Length != 0)
            {
                Patcher.WriteLineInColor(ConsoleColor.Red, "Please close the game first.");
            }
            else if (string.IsNullOrEmpty(lostArkInstallDir))
            {
                Patcher.WriteLineInColor(ConsoleColor.Red, "Lost Ark installation not found.");
            }
            else
            {
                try
                {
                    var patcher = new Patcher(lostArkInstallDir);
                    patcher.ApplyAllPatches();
                }
                catch (IOException)
                {
                    Patcher.WriteLineInColor(ConsoleColor.Red, "An error occured. Maybe the game is running?");
                }
            }

            Console.WriteLine("\nPress any key to exit!");
            Console.ReadKey();
        }

        private static string? GetInstallDir()
        {
            var usersettingsPath = "LostArkPatcher.usersettings.json";
            if (File.Exists(usersettingsPath))
            {
                var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(usersettingsPath));
                if (config != null && !string.IsNullOrEmpty(config.InstallDir))
                {
                    var dataCheckPath = Path.Combine(config.InstallDir, "EFGame", "data2.lpk");
                    if (File.Exists(dataCheckPath))
                    {
                        return config.InstallDir;
                    }
                }
            }
            var lostArkInstallDir = (string?)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 1599340", "InstallLocation", "");
            if (!string.IsNullOrEmpty(lostArkInstallDir) && Directory.Exists(lostArkInstallDir))
            {
                var dataCheckPath = Path.Combine(lostArkInstallDir, "EFGame", "data2.lpk");
                if (File.Exists(dataCheckPath))
                {
                    return lostArkInstallDir;
                }
            }
            while (true)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Select Lost Ark installation directory";
                fbd.ShowNewFolderButton = false;
                fbd.RootFolder = Environment.SpecialFolder.MyComputer;
                var dialogResult = fbd.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    var dataCheckPath = Path.Combine(fbd.SelectedPath, "EFGame", "data2.lpk");
                    if (File.Exists(dataCheckPath))
                    {
                        var config = new Config() {
                            InstallDir = fbd.SelectedPath
                        };
                        File.WriteAllText(usersettingsPath, JsonConvert.SerializeObject(config));
                        return fbd.SelectedPath;
                    }
                    else
                    {
                        MessageBox.Show("Not a valid Lost Ark installation!");
                    }
                }
                if (dialogResult == DialogResult.Cancel)
                {
                    System.Environment.Exit(0);
                }
            }
        }
    }
}