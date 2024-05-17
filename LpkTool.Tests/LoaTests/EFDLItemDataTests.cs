using LpkTool.Library.LoaData.EFDLItem;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.IO;

namespace LpkTool.Tests.LoaTests
{
    public class EFDLItemDataTests
    {
        [Test]
        public void TestSerializer()
        {
            var path = Path.Combine("TestData", "loa", "EFDLItem_PC_GN_F_AV_118D_Upper.PC_GN_F_AV_118D_Upper.loa");
            var data = new EFDLItem(path);
            var newData = data.Serialize();
            var origData = File.ReadAllBytes(path);
            try
            {
                CollectionAssert.AreEqual(origData, newData);
            }
            catch (Exception)
            {
                var outPath = path.Replace(".loa", ".fail");
                File.WriteAllBytes(outPath, newData);
                throw;
            }
        }

        //[Test]
        //public void MassTest()
        //{
        //    var inDir = @"F:\SteamLibrary\steamapps\common\Lost Ark\EFGame\data4.lpk.files\EFGame_Extra\ClientData\XmlData\LookInfo\Item";
        //    var files = Directory.GetFiles(inDir);
        //    foreach (var file in files)
        //    {
        //        var data = new EFDLItem(file);
        //        var newData = data.Serialize();
        //        var origData = File.ReadAllBytes(file);
        //        try
        //        {
        //            CollectionAssert.AreEqual(origData, newData);
        //        }
        //        catch (Exception e)
        //        {
        //            var outPath = file.Replace(".loa", ".fail");
        //            File.WriteAllBytes(outPath, newData);
        //            throw;
        //        }

        //    }
        //}
    }
}
