using LpkTool.Library;
using NUnit.Framework;
using System.IO;

namespace LpkTool.Tests
{
    public class LoaTests
    {
        //Place your own loa files in the TestData directory
        [Test]
        [TestCase(@"ColorOption.loa")]
        [TestCase(@"IconInfo.loa")]
        [TestCase(@"ActionCategory.loa")]
        [TestCase(@"FunctionNPCLocation.loa")]
        public void RepackWithoutChanges(string loaFile)
        {
            Assert.Fail();
        }

        [Test]
        [TestCase(@"ColorOption.loa")]
        [TestCase(@"IconInfo.loa")]
        [TestCase(@"ActionCategory.loa")]
        [TestCase(@"FunctionNPCLocation.loa")]
        public void ExportToXmlTest(string loaFile)
        {
            var path = Path.Combine("TestData", loaFile);
            var filename = Path.GetFileNameWithoutExtension(path);
            var dir = Path.GetDirectoryName(path);

            var loa = Loa.FromFile(path);
            loa.ExportAsXml(Path.Combine(dir, filename + ".xml"));
        }
    }
}
