using LpkTool.Library.LoaData.Quest;
using NUnit.Framework;
using System.IO;

namespace LpkTool.Tests.LoaTests
{
    public class QuestTests
    {
        [Test]
        public void TestSerializer()
        {
            var path = Path.Combine("TestData", "loa", "101901.loa");
            var data = new Quest(path);
            var newData = data.Serialize();
            var origData = File.ReadAllBytes(path);
            CollectionAssert.AreEqual(origData, newData);
        }
    }
}
