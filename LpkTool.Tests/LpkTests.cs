using LpkTool.Library.Models;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace LpkTool.Tests
{
    public class LpkTests
    {
        [Test]
        [TestCase(@"C:\Users\leanw\Documents\quickbms\eu\font.lpk", @"C:\Users\leanw\Documents\quickbms\eu\out\Binaries\Fonts\FontMap.xml", @"\Binaries\Fonts\FontMap.xml")]
        public void RepackWithoutChanges(string lpkPath, string replaceFilePath, string replaceFileSearchPath)
        {
            var lpk = Lpk.FromFile(lpkPath);
            var found = lpk.GetFileByPath(replaceFileSearchPath);
            if (found != null)
            {
                //found.ReplaceData(File.ReadAllBytes(replaceFilePath));
                var repackResult = lpk.RepackToByteArray();
                File.WriteAllBytes(@"F:\SteamLibrary\steamapps\common\Lost Ark\EFGame\font_new.lpk", repackResult);
                var repackResultString = Encoding.UTF8.GetString(repackResult);
                var inputString = Encoding.UTF8.GetString(File.ReadAllBytes(lpkPath));
                Assert.AreEqual(inputString, repackResultString);
            }
        }

        [Test]
        [TestCase(@"C:\Users\leanw\Documents\quickbms\eu\font.lpk", @"C:\Users\leanw\Documents\quickbms\eu\FontMap.xml", @"\Binaries\Fonts\English\FontMap.xml")]
        public void RepackWithSameFileChanged(string lpkPath, string replaceFilePath, string replaceFileSearchPath)
        {
            var lpk = Lpk.FromFile(lpkPath);
            var found = lpk.GetFileByPath(replaceFileSearchPath);
            if (found != null)
            {
                lpk.AddFile(@"\Binaries\Fonts\ComicNeue-Regular.ttf", @"C:\Users\leanw\Documents\quickbms\eu\ComicNeue-Regular.ttf");
                found.ReplaceData(File.ReadAllBytes(replaceFilePath));
                var repackResult = lpk.RepackToByteArray();
                File.WriteAllBytes(@"F:\SteamLibrary\steamapps\common\Lost Ark\EFGame\font_new.lpk", repackResult);
            }
        }
    }
}
