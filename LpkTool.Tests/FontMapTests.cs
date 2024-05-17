using LpkTool.Library.Models;
using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.AreEqual(origXmlString, newXmlString);
        }
    }
}
