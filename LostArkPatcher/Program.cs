using Microsoft.Win32;
using System.Diagnostics;

namespace LostArkPatcher
{
    internal class Program
    {
        private static readonly string DataFileName = "data2.lpk";
        private static readonly string FontFileName = "font.lpk";
        //Place Patches in Documents/LostArk/Patches
        private static void Main()
        {
            Patcher.WriteLineInColor(ConsoleColor.Magenta, "- LostArk .lpk patcher by LEaN -\n");
            var lostArkInstallDir = (string?)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 1599340", "InstallLocation", "");
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
                var dataPath = Path.Combine(lostArkInstallDir, "EFGame", DataFileName);
                var fontPath = Path.Combine(lostArkInstallDir, "EFGame", FontFileName);
                try
                {
                    Patcher.ApplyAllPatches(dataPath, fontPath);
                }
                catch (IOException)
                {
                    Patcher.WriteLineInColor(ConsoleColor.Red, "An error occured. Maybe the game is running?");
                }
            }

            Console.WriteLine("\nPress any key to exit!");
            Console.ReadKey();
        }
    }
}