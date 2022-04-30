using LpkTool.Library.Models;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace LpkTool.Tests
{
    public class LpkTests
    {
        //Place your own lpk files in the TestData directory
        [Test]
        [TestCase(@"font.lpk")]
        [TestCase(@"data.lpk")]
        [TestCase(@"config.lpk")]
        [Parallelizable(ParallelScope.All)]
        public void RepackWithoutChanges(string lpkFile)
        {
            var path = Path.Combine("TestData", lpkFile);
            var lpk = Lpk.FromFile(path);
            var repackResult = lpk.RepackToByteArray();
            var repackResultString = Encoding.UTF8.GetString(repackResult);
            var inputString = Encoding.UTF8.GetString(File.ReadAllBytes(path));
            Assert.AreEqual(inputString, repackResultString);
        }

        //[Test]
        //[TestCase(@"C:\Users\leanw\Documents\quickbms\eu\font.lpk", @"C:\Users\leanw\Documents\quickbms\eu\FontMap.xml", @"\Binaries\Fonts\English\FontMap.xml")]
        //public void CustomStuff(string lpkPath, string replaceFilePath, string replaceFileSearchPath)
        //{
        //    var lpk = Lpk.FromFile(lpkPath);
        //    var found = lpk.GetFileByPath(replaceFileSearchPath);
        //    if (found != null)
        //    {
        //        //lpk.AddFile(@"\Binaries\Fonts\Roboto-Regular.ttf", @"C:\Users\leanw\Documents\quickbms\eu\Roboto-Regular.ttf");
        //        found.ReplaceData(File.ReadAllBytes(replaceFilePath));
        //        var repackResult = lpk.RepackToByteArray();
        //        File.WriteAllBytes(@"F:\SteamLibrary\steamapps\common\Lost Ark\EFGame\font_new.lpk", repackResult);
        //    }
        //}
    }
}
