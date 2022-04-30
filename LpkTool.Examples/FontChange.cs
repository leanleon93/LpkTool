using LpkTool.Library;

namespace LpkTool.Examples
{
    internal static class FontChange
    {
        private static readonly string EXAMPLE_DATA = "ExampleData";
        public static void RepackWithChangedFont()
        {
            var pathToLpk = Path.Combine(EXAMPLE_DATA, "font.lpk");
            var pathToFont = Path.Combine(EXAMPLE_DATA, "Roboto-Regular.ttf");
            var relativeFontPath = @"\Binaries\Fonts\Roboto-Regular.ttf";
            var pathToFontMap = Path.Combine(EXAMPLE_DATA, "FontMap.xml");
            var relativeEnFontMapPath = @"\Binaries\Fonts\English\FontMap.xml";
            if (!File.Exists(pathToLpk) || !File.Exists(pathToFont) || !File.Exists(pathToFontMap))
            {
                throw new Exception("Move your own files to ExampleData dir.");
            }
            var lpk = Lpk.FromFile(pathToLpk);
            lpk.AddFile(relativeFontPath, pathToFont);
            var enFontMap = lpk.GetFileByPath(relativeEnFontMapPath);
            if (enFontMap != null)
            {
                enFontMap.ReplaceData(pathToFontMap);
            }
            var outPath = Path.Combine(EXAMPLE_DATA, "font_mod.lpk");
            lpk.RepackToFile(outPath);
        }
    }
}
