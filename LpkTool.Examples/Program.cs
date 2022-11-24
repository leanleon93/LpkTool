// See https://aka.ms/new-console-template for more information


//Testing Project

//ModelSwap.Swaps();
using LpkTool.Library;
using Microsoft.Win32;

//ModelSwap.Dumps();


var lostArkInstallDir = (string?)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 1599340", "InstallLocation", "");
var efGamePath = Path.Combine(lostArkInstallDir, "EFGame");

var baseName = "data{0}.lpk";

var outDir = Path.Combine(efGamePath, "out");
for (int i = 1; i <= 4; i++)
{
    Console.WriteLine("Unpacking " + i);
    var filePath = Path.Combine(efGamePath, string.Format(baseName, i));
    var lpk = Lpk.FromFile(filePath);
    lpk.Export(outDir);
}


//FontChange.RepackWithRoboto(@"F:\SteamLibrary\steamapps\common\Lost Ark\EFGame\font.lpk", @"F:\SteamLibrary\steamapps\common\Lost Ark\EFGame\Roboto-Regular.ttf", @"F:\SteamLibrary\steamapps\common\Lost Ark\EFGame\FontMap.xml");