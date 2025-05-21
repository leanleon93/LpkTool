//Testing Project

using LpkTool.Library;
using Microsoft.Win32;

internal class Program
{
    private static void Main(string[] args)
    {
        var lostArkInstallDir = (string?)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 1599340", "InstallLocation", "");
        if (string.IsNullOrEmpty(lostArkInstallDir))
        {
            lostArkInstallDir = @"F:\SteamLibrary\steamapps\common\Lost Ark";
        }
        var efGamePath = Path.Combine(lostArkInstallDir, "EFGame");

        var baseName = "data{0}.lpk";
        var lpkFilePath = Path.Combine(efGamePath, string.Format(baseName, 2));

        for (int i = 0; i <= 0xF; i++)
        {
            var euBase = $"1069d88738c5c75f82b44a1f0a38276{i:X1}";
            Console.WriteLine(euBase);
            var lpk = Lpk.FromFile(lpkFilePath, LpkTool.Library.LpkData.Region.EU, euBase);
            var top = lpk.GetFileByName("EFTable_Item.db");
            if (top == null) return;
            var topData = top.GetData();
            if (topData[0] == 0x53 && topData[1] == 0x51)
            {
                Console.WriteLine("found: " + euBase);
                File.WriteAllBytes("EFTable_Item.db", topData);
                break;
            }
        }

        //top.ReplaceData(@"F:\SteamLibrary\steamapps\common\Lost Ark\EFGame\data4.lpk.files\EFGame_Extra\ClientData\XmlData\LookInfo\Human\EFDLChar_PC_GDH_F.PC_GDH_F.loa");
        //lpk.RepackToFile(lpkFilePath);
    }
}