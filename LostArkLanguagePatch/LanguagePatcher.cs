using LpkTool.Library;
using LpkTool.Library.LpkData;
using Microsoft.Data.Sqlite;

namespace LostArkLanguagePatch
{
    internal class LanguagePatcher
    {
        private readonly Dictionary<string, string?> _gameMessages;
        private readonly string _guid, _outPath;
        private string TempDir => Path.Combine(Path.GetTempPath(), "loalanguagepatcher_" + _guid);
        private readonly Lpk _targetLpk, _sourceLpk;

        public LanguagePatcher(Lpk targetLpk, Lpk sourceLpk, string outPath)
        {
            _outPath = outPath;
            _targetLpk = targetLpk;
            _sourceLpk = sourceLpk;
            _gameMessages = new Dictionary<string, string?>();
            _guid = Guid.NewGuid().ToString();
        }

        public void Patch()
        {
            try
            {
                Directory.CreateDirectory(TempDir);
                FillGameMessageKeys();
                FillGameMessageValues();
                ApplyGameMessagesToTargetAndSave();
            }
            catch
            {
                Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                throw;
            }
            finally
            {
                Directory.Delete(TempDir, true);
            }
        }

        private void ApplyGameMessagesToTargetAndSave()
        {
            var tempFile = Path.Combine(TempDir, "EFTable_GameMsg_target.db");
            var targetFile = _targetLpk.GetFileByName("EFTable_GameMsg.db");
            if (!File.Exists(tempFile))
            {
                File.WriteAllBytes(tempFile, targetFile!.GetData());
            }
            using (var connection = new SqliteConnection($"Data Source={tempFile}"))
            {
                connection.Open();
                var gameMsgKey = RegionHelper.GetGameMsgString(_targetLpk.Region);
                var gameMessages = _gameMessages.Where(x => x.Value != null).ToDictionary(x => x.Key, x => x.Value);
                string curCommand = "";
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var kvp in gameMessages)
                        {
                            using (var command = new SqliteCommand($"UPDATE {gameMsgKey} SET MSG = \"{Sanitize(kvp.Value)}\" WHERE KEY = \"{kvp.Key}\"", connection, transaction))
                            {
                                curCommand = command.CommandText;
                                command.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.Error.WriteLine("I did nothing, because something wrong happened: {0}", ex);
                        throw;
                    }
                }

                connection.Close();
                connection.Dispose();
            }
            Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            targetFile.ReplaceData(tempFile);
            _targetLpk.RepackToFile(_outPath);
            File.Delete(tempFile);
        }

        private string Sanitize(string? value)
        {
            value = value.Replace("\"", "\"\"");
            return value;
        }

        private void FillGameMessageValues()
        {
            var tempFile = Path.Combine(TempDir, "EFTable_GameMsg_source.db");
            if (!File.Exists(tempFile))
            {
                File.WriteAllBytes(tempFile, _sourceLpk.GetFileByName("EFTable_GameMsg.db")!.GetData());
            }
            using (var connection = new SqliteConnection($"Data Source={tempFile}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                var gameMsgKey = RegionHelper.GetGameMsgString(_sourceLpk.Region);
                command.CommandText = $@"SELECT KEY, MSG from {gameMsgKey}";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var key = reader.GetString(0);
                        var msg = reader.GetString(1);
                        if (_gameMessages.ContainsKey(key))
                        {
                            _gameMessages[key] = msg;
                        }
                    }
                }
                connection.Close();
                connection.Dispose();
            }
            Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            File.Delete(tempFile);
        }

        private void FillGameMessageKeys()
        {
            var tempFile = Path.Combine(TempDir, "EFTable_GameMsg_target.db");
            if (!File.Exists(tempFile))
            {
                File.WriteAllBytes(tempFile, _targetLpk.GetFileByName("EFTable_GameMsg.db")!.GetData());
            }
            using (var connection = new SqliteConnection($"Data Source={tempFile}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                var gameMsgKey = RegionHelper.GetGameMsgString(_targetLpk.Region);
                command.CommandText = $@"SELECT KEY from {gameMsgKey}";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var key = reader.GetString(0);
                        _gameMessages.Add(key, null);
                    }
                }
                connection.Close();
                connection.Dispose();
            }
            Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            File.Delete(tempFile);
        }
    }
}
