//Testing Project

using LpkTool.Library;
using LpkTool.Library.LoaData.Table_MovieData;
using Microsoft.Win32;

internal class Program
{
    private static void Main(string[] args)
    {
        var lostArkInstallDir = (string?)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 1599340", "InstallLocation", "");
        var efGamePath = Path.Combine(lostArkInstallDir, "EFGame");

        var baseName = "data{0}.lpk";
        var lpkFilePath = Path.Combine(efGamePath, string.Format(baseName, 1));

        var lpk = Lpk.FromFile(lpkFilePath);
        var file = lpk.GetFileByName("Table_MovieData.loa");
        if (file == null) return;
        var movieData = new MovieData(file.GetData());

        var introContainer = movieData.MovieDataContainers.FirstOrDefault(x => x.Key == "FullScreen.Intro");
        if (introContainer == null) return;
        introContainer.MovieDataValueArray = new MovieDataValue[0];

        var newData = movieData.Serialize();

        file.ReplaceData(newData);
        lpk.RepackToFile(lpkFilePath);
    }
}