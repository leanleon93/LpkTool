//Testing Project

using LpkTool.Library;
using Microsoft.Win32;


internal class Program
{
    private static void Main(string[] args)
    {
        var lostArkInstallDir = (string?)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 1599340", "InstallLocation", "");
        var efGamePath = Path.Combine(lostArkInstallDir, "EFGame");

        var baseName = "data{0}.lpk";
        var outDir = Path.Combine(efGamePath, "out");
        var fontPath = Path.Combine(efGamePath, "font.lpk");

        Console.WriteLine("Unpacking font");
        var lpk1 = Lpk.FromFile(fontPath);
        lpk1.Export(outDir);

        for (var i = 1; i <= 4; i++)
        {
            Console.WriteLine("Unpacking " + i);
            var filePath = Path.Combine(efGamePath, string.Format(baseName, i));
            var lpk = Lpk.FromFile(filePath);
            lpk.Export(outDir);
        }
    }
}