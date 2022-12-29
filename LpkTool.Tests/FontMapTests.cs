using LpkTool.Library.Models;
using NUnit.Framework;
using System.IO;

namespace LpkTool.Tests
{
    public class FontMapTests
    {
        [Test]
        public void SerializerTest()
        {
            var path = Path.Combine("TestData", "FontMap.xml");
            var origXmlString = File.ReadAllText(path);
            var fontMap = FontMap.FromXml(origXmlString);
            var newXmlString = fontMap.ToXml();
            Assert.AreEqual(origXmlString, newXmlString);
        }
    }
}
