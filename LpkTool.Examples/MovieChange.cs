using LpkTool.Library;

namespace LpkTool.Examples
{
    internal static class MovieChange
    {
        private static readonly string EXAMPLE_DATA = "ExampleData";
        public static void RepackWithChangedMovie()
        {
            var pathToLpk = Path.Combine(EXAMPLE_DATA, "data.lpk");
            var pathToMovieDb = Path.Combine(EXAMPLE_DATA, "EFTable_Movie.db");
            var dbName = @"EFTable_Movie.db";
            if (!File.Exists(pathToLpk) || !File.Exists(pathToMovieDb))
            {
                throw new Exception("Move your own files to ExampleData dir.");
            }
            var lpk = Lpk.FromFile(pathToLpk);
            lpk.AddFile(dbName, pathToMovieDb);
            var dbFile = lpk.GetFileByName(dbName);
            if (dbFile != null)
            {
                dbFile.ReplaceData(pathToMovieDb);
            }
            var outPath = Path.Combine(EXAMPLE_DATA, "data_mod.lpk");
            lpk.RepackToFile(outPath);
        }
    }
}
