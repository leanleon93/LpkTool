using LpkTool.Library;
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
        [TestCase(@"data1.lpk")]
        [TestCase(@"data2.lpk")]
        [TestCase(@"config.lpk")]
        [Parallelizable(ParallelScope.All)]
        public void RepackWithoutChangesEU(string lpkFile)
        {
            var path = Path.Combine("TestData", "EU", lpkFile);
            var lpk = Lpk.FromFile(path, Library.LpkData.Region.EU);
            var repackResult = lpk.RepackToByteArray();
            var repackResultString = Encoding.UTF8.GetString(repackResult);
            var inputString = Encoding.UTF8.GetString(File.ReadAllBytes(path));
            Assert.AreEqual(inputString, repackResultString);
        }

        [Test]
        [TestCase(@"font.lpk")]
        [TestCase(@"data1.lpk")]
        [TestCase(@"data2.lpk")]
        [TestCase(@"config.lpk")]
        [Parallelizable(ParallelScope.All)]
        public void RepackWithoutChangesRU(string lpkFile)
        {
            var path = Path.Combine("TestData", "RU", lpkFile);
            var lpk = Lpk.FromFile(path, Library.LpkData.Region.RU);
            var repackResult = lpk.RepackToByteArray();
            var repackResultString = Encoding.UTF8.GetString(repackResult);
            var inputString = Encoding.UTF8.GetString(File.ReadAllBytes(path));
            Assert.AreEqual(inputString, repackResultString);
        }
    }
}
