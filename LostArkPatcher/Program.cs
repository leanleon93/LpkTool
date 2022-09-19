using Microsoft.Win32;

namespace LostArkPatcher
{
    internal class Program
    {
        //Place Patches in Documents/LostArk/Patches
        static void Main()
        {
            Console.WriteLine("LostArk data.lpk patcher");
            var lostArkInstallDir = (string?)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 1599340", "InstallLocation", "");
            if (string.IsNullOrEmpty(lostArkInstallDir))
            {
                Console.WriteLine("Lost Ark installation not found");
            }
            else
            {
                var dataPath = Path.Combine(lostArkInstallDir, "EFGame", "data.lpk");
                Patcher.ApplyAllPatches(dataPath);
            }

            Console.WriteLine("Press any key to exit!");
            Console.ReadKey();
        }
    }
}