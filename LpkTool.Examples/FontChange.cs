using LpkTool.Library;
using LpkTool.Library.Helpers;

namespace LpkTool.Examples
{
    internal static class FontChange
    {

        public static void RepackWithRoboto(string fontLpkPath, string fontPath, string fontMapPath)
        {
            var relativeFontPath = @"\Binaries\Fonts\Roboto-Regular.ttf";
            var relativeEnFontMapPath = @"\Binaries\Fonts\English\FontMap.xml";
            var lpk = Lpk.FromFile(fontLpkPath);
            var directory = Path.GetDirectoryName(fontLpkPath);
            lpk.AddFile(relativeFontPath, fontPath);
            var enFontMap = lpk.GetFileByPath(relativeEnFontMapPath);
            enFontMap.ReplaceData(fontMapPath);
            lpk.RepackToFile(Path.Combine(directory, "font_mod.lpk"));
        }

        public static void RepackWithChangedFont(string fontLpkPath, string ttfPath)
        {
            var swapper = new FontSwapper(fontLpkPath);
            swapper.SwapFont(ttfPath);
        }

    }
}
