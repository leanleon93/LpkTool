//Testing Project

using LpkTool.Library;
using LpkTool.Library.LoaData.EFDLItem;
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
        var top = lpk.GetFileByName("EFDLItem_WP_GDH_F_AV_047.WP_GDH_F_AV_047.loa");
        if (top == null) return;
        var item = new EFDLItem(top.GetData());

        //var itemJson = JsonConvert.SerializeObject(item);
        //File.WriteAllText(@"C:\Users\leanw\Documents\LostArk\gs_gun.json", itemJson);

        var list = item.ItemParticleDatSpawns.Value;
        foreach (var part in list)
        {
            part.RelativeScale.Value.X.Value = 5.0f;
            part.RelativeScale.Value.Y.Value = 5.0f;
            part.RelativeScale.Value.Z.Value = 5.0f;
        }
        top.ReplaceData(item.Serialize());
        lpk.RepackToFile(lpkFilePath);
    }
}