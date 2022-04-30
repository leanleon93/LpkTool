// See https://aka.ms/new-console-template for more information
using LpkTool.Examples;
using LpkTool.Library;

FontChange.RepackWithChangedFont();
var pathToLpk = Path.Combine("ExampleData", "font_mod.lpk");
var lpk = Lpk.FromFile(pathToLpk);
lpk.Export(Path.Combine("ExampleData", "out"));