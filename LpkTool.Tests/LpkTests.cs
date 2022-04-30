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
    }
}
