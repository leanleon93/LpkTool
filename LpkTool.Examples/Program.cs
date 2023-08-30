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
        var lpkFilePath = Path.Combine(efGamePath, string.Format(baseName, 4));

        var lpk = Lpk.FromFile(lpkFilePath, LpkTool.Library.LpkData.Region.EU);
        var top = lpk.GetFileByName("EFDLChar_PC_GAM.PC_GAM.loa");
        if (top == null) return;

        top.ReplaceData(@"F:\SteamLibrary\steamapps\common\Lost Ark\EFGame\data4.lpk.files\EFGame_Extra\ClientData\XmlData\LookInfo\Human\EFDLChar_PC_GDH_F.PC_GDH_F.loa");
        lpk.RepackToFile(lpkFilePath);
    }
}