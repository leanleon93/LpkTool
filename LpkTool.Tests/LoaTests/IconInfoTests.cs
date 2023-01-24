using LpkTool.Library.LoaData.IconInfo;
using NUnit.Framework;
using System;
using System.IO;

namespace LpkTool.Tests.LoaTests
{
    public class IconInfoTests
    {
        [Test]
        public void TestSerializer()
        {
            var path = Path.Combine("TestData", "loa", "IconInfo.loa");
            var iconInfoData = new IconInfo(path);
            var newIconInfoData = iconInfoData.Serialize();
            var origData = File.ReadAllBytes(path);
            try
            {
                CollectionAssert.AreEqual(origData, newIconInfoData);
            }
            catch (Exception)
            {
                var outPath = path.Replace(".loa", ".fail");
                File.WriteAllBytes(outPath, newIconInfoData);
                throw;
            }
        }
    }
}
