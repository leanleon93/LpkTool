using LpkTool.Library.LoaData.Table_MovieData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpkTool.Tests.LoaTests
{
    public class MovieDataTests
    {
        [Test]
        public void TestSerializer()
        {
            var path = Path.Combine("TestData", "loa", "Table_MovieData.loa");
            var movieData = new MovieData(path);
            var newMovieData = movieData.Serialize();
            var origData = File.ReadAllBytes(path);
            CollectionAssert.AreEqual(origData, newMovieData);
        }
    }
}
