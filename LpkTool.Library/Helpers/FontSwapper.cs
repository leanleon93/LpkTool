using LpkTool.Library.Models;
using System.Text;

namespace LpkTool.Library.Helpers
{
    public class FontSwapper
    {
        private string _fontLpkPath;
        private readonly string _relativeFontPathBase = @"\Binaries\Fonts\{0}\{1}";
        private readonly string _relativeFontMapPathBase = @"\Binaries\Fonts\{0}\FontMap.xml";

        public FontSwapper(string fontLpkPath)
        {
            _fontLpkPath = fontLpkPath;
        }

        public void SwapFont(string fontTtfPath, string languageString = "English")
        {
            var fontFilename = Path.GetFileName(fontTtfPath);
            var relativeFontPath = string.Format(_relativeFontPathBase, languageString, fontFilename);
            var relativeFontMapPath = string.Format(_relativeFontMapPathBase, languageString);
            var lpk = Lpk.FromFile(_fontLpkPath);
            lpk.AddFile(relativeFontPath, fontTtfPath);
            var fontMapFile = lpk.GetFileByPath(relativeFontMapPath);
            var fontMap = FontMap.FromXml(Encoding.UTF8.GetString(fontMapFile.GetData()));
            foreach (var item in fontMap.Items.Where(x => x.Key != $"$WitcherFont")) //ignore text from witcher crossover
            {
                item.File = fontFilename;
            }
            var newFontMapData = Encoding.UTF8.GetBytes(fontMap.ToXml());
            fontMapFile.ReplaceData(newFontMapData);
            lpk.RepackToFile(_fontLpkPath);
        }
    }
}
