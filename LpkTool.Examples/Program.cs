// See https://aka.ms/new-console-template for more information


//Testing Project

//ModelSwap.Swaps();
//ModelSwap.Dumps();

using LpkTool.Library;

var dataPath = @"F:\SteamLibrary\steamapps\common\Lost Ark\EFGame\data2.lpk";
var outDir = @"F:\SteamLibrary\steamapps\common\Lost Ark\EFGame\out";
var lpk = Lpk.FromFile(dataPath);
lpk.Export(outDir);

//FontChange.RepackWithRoboto(@"F:\SteamLibrary\steamapps\common\Lost Ark\EFGame\font.lpk", @"F:\SteamLibrary\steamapps\common\Lost Ark\EFGame\Roboto-Regular.ttf", @"F:\SteamLibrary\steamapps\common\Lost Ark\EFGame\FontMap.xml");