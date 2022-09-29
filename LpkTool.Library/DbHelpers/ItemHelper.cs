using Microsoft.Data.Sqlite;

namespace LpkTool.Library.DbHelpers
{
    /// <summary>
    /// 
    /// </summary>
    public class ItemHelper : IDisposable
    {
        private readonly string _guid;
        private Dictionary<string, string> _gameMsgDict;
        private string TempDir => Path.Combine(Path.GetTempPath(), "lpktool_" + _guid);
        private readonly Lpk _lpk;

        public static ItemHelper FromFile(string lpkPath)
        {
            return new ItemHelper(Lpk.FromFile(lpkPath));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpk"></param>
        public ItemHelper(Lpk lpk)
        {
            _lpk = lpk;
            _guid = Guid.NewGuid().ToString();
            Directory.CreateDirectory(TempDir);
            InitGameMsgDict();
        }

        private void InitGameMsgDict()
        {
            var tempFile = Path.Combine(TempDir, "EFTable_GameMsg.db");
            if (!File.Exists(tempFile))
            {
                File.WriteAllBytes(tempFile, _lpk.GetFileByName("EFTable_GameMsg.db")!.GetData());
            }
            var result = new Dictionary<string, string>();
            using (var connection = new SqliteConnection($"Data Source={tempFile}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $@"SELECT KEY, MSG from GameMsg_English";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var key = reader.GetString(0);
                        var msg = reader.GetString(1);
                        result.Add(key, msg);
                    }
                }
                connection.Close();
                connection.Dispose();
            }
            _gameMsgDict = result;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Directory.Exists(TempDir))
                Directory.Delete(TempDir, true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Dictionary<string, object>> GetAllItems(bool resolvedNames = false)
        {
            var result = new Dictionary<int, Dictionary<string, object>>();
            var tempFile = Path.Combine(TempDir, "EFTable_Item.db");
            if (!File.Exists(tempFile))
            {
                File.WriteAllBytes(tempFile, _lpk.GetFileByName("EFTable_Item.db")!.GetData());
            }
            using (var connection = new SqliteConnection($"Data Source={tempFile}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $@"Select * from Item";
                AddCommandEvaluationToResult(command, ref result, resolvedNames);
                connection.Close();
                connection.Dispose();
            }
            return result;
        }

        private readonly List<string> KeyWhitelist = new List<string> {
            "PrimaryKey",
            "Name",
            "Comment",
            "Icon",
            "IconIndex",
            "IconSound",
            "DropModel",
            "Model",
            "Type",
            "Grade"
        };

        public List<Dictionary<string, object>> GetOutfitsForClass(string classAttribute, int grade = -1)
        {
            var allItems = GetAllItems(true);
            var outfits = allItems.Where(x => x.Value[classAttribute].ToString() == "1" && x.Value["Type"].ToString() == "9").Select(x => x.Value).OrderBy(x => x["Icon"]).ToList();
            if (grade > -1)
            {
                outfits = outfits.Where(x => x["Grade"].ToString() == grade.ToString()).ToList();
            }
            foreach (var outfit in outfits)
            {
                foreach (var item in outfit.Where(x => !(KeyWhitelist.Contains(x.Key) || (x.Key.StartsWith("For") && x.Value.ToString() == "1"))))
                {
                    outfit.Remove(item.Key);
                }
            }
            return outfits;
        }

        public void RepackToFile(string outPath)
        {
            var itemFile = _lpk.GetFileByName("EFTable_Item.db");
            itemFile.ReplaceData(this.GetFileData("EFTable_Item.db"));
            _lpk.RepackToFile(outPath);
        }

        public void ModelSwap(int targetPrimaryKey, int sourcePrimaryKey)
        {
            var tempFile = Path.Combine(TempDir, "EFTable_Item.db");
            if (!File.Exists(tempFile))
            {
                File.WriteAllBytes(tempFile, _lpk.GetFileByName("EFTable_Item.db")!.GetData());
            }

            using (var connection = new SqliteConnection($"Data Source={tempFile}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT Model From Item WHERE PrimaryKey = {sourcePrimaryKey}";
                string model = "";
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        model = reader.GetString(0);
                    }
                    reader.Close();
                    reader.DisposeAsync().GetAwaiter().GetResult();
                }

                if (!string.IsNullOrEmpty(model))
                {
                    var (query, newValues) = GetModelSwapQuery(targetPrimaryKey, model);
                    var updateCommand = BuildUpdateCommand("Item", query, newValues);
                    var command2 = connection.CreateCommand();
                    command2.CommandText = updateCommand;
                    command2.ExecuteNonQuery();
                }

                connection.Close();
                connection.Dispose();
            }

        }

        public void ModelSwap(int targetPrimaryKey, string model)
        {
            var (query, newValues) = GetModelSwapQuery(targetPrimaryKey, model);
            UpdateValuesForItemTable(query, newValues);
        }

        public static (List<Dictionary<string, object>> query, Dictionary<string, object> newValues) GetModelSwapQuery(int primaryKey, string newModel)
        {
            var query = new List<Dictionary<string, object>>();
            var newValues = new Dictionary<string, object>();
            query.Add(new Dictionary<string, object>() { { "PrimaryKey", primaryKey } });
            newValues.Add("Model", newModel);
            return (query, newValues);
        }

        public void UpdateValuesForItemTable(List<Dictionary<string, object>> query, Dictionary<string, object> newValues)
        {
            UpdateValues("EFTable_Item.db", "Item", query, newValues);
        }

        public void UpdateValues(string fileName, string tableName, List<Dictionary<string, object>> query, Dictionary<string, object> newValues)
        {
            var tempFile = Path.Combine(TempDir, fileName);
            if (!File.Exists(tempFile))
            {
                File.WriteAllBytes(tempFile, _lpk.GetFileByName(fileName)!.GetData());
            }

            using (var connection = new SqliteConnection($"Data Source={tempFile}"))
            {

                var updateCommand = BuildUpdateCommand(tableName, query, newValues);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = updateCommand;
                command.ExecuteNonQuery();
                connection.Close();
                connection.Dispose();
            }

        }

        public byte[] GetFileData(string fileName)
        {
            Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
            GC.Collect();
            var tempFile = Path.Combine(TempDir, fileName);
            if (!File.Exists(tempFile))
            {
                File.WriteAllBytes(tempFile, _lpk.GetFileByName(fileName)!.GetData());
            }
            return File.ReadAllBytes(tempFile);
        }

        private string BuildUpdateCommand(string tableName, List<Dictionary<string, object>> query, Dictionary<string, object> newValues)
        {
            var snippet = "{0} = {1}";
            var command = $"UPDATE {tableName}";
            var setLine = "SET ";
            for (int i = 0; i < newValues.Count; i++)
            {
                var item = newValues.ElementAt(i);
                if (i != 0)
                {
                    setLine += ", ";
                }
                setLine += string.Format(snippet, item.Key, (item.Value is string ? $"'{item.Value}'" : item.Value));
            }
            setLine = setLine.TrimEnd(new char[] { ',', ' ' });
            var whereLine = "WHERE ";
            for (var i = 0; i < query.Count; i++)
            {
                var queryCombo = query[i];
                if (i != 0)
                {
                    whereLine += " OR ";
                }
                whereLine += "(";
                for (int j = 0; j < queryCombo.Count; j++)
                {
                    var item = queryCombo.ElementAt(j);
                    if (j != 0)
                    {
                        whereLine += " AND ";
                    }
                    whereLine += string.Format(snippet, item.Key, (item.Value is string ? $"'{item.Value}'" : item.Value));
                }
                whereLine += ")";
            }
            whereLine = whereLine.TrimEnd(new char[] { ',', ' ' });
            command = $"{command} {setLine} {whereLine}";
            return command;
        }

        private void AddCommandEvaluationToResult(SqliteCommand command, ref Dictionary<int, Dictionary<string, object>> result, bool resolvedNames)
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    object[] objs = new object[reader.FieldCount];
                    var colNames = new string[reader.FieldCount];
                    for (int j = 0; j < reader.FieldCount; j++)
                    {
                        colNames[j] = reader.GetName(j);
                    }

                    reader.GetValues(objs);
                    var resultDict = new Dictionary<string, object>();
                    int primaryKey = 0;
                    for (var j = 0; j < objs.Length; j++)
                    {
                        var obj = objs[j];
                        var colname = colNames[j];

                        if (resolvedNames && colname == "Name")
                        {
                            var name = GetMsg(obj.ToString());
                            resultDict.Add(colname, name);
                        }
                        else if (resolvedNames && colname == "Desc")
                        {
                            var desc = GetMsg(obj.ToString());
                            resultDict.Add(colname, desc);
                        }
                        else if (colname == "PrimaryKey")
                        {
                            primaryKey = Convert.ToInt32(obj);
                            resultDict.Add(colname, obj);
                        }
                        else
                        {
                            resultDict.Add(colname, obj);
                        }
                    }
                    result.TryAdd(primaryKey, resultDict);
                }
            }
        }

        private string GetMsg(string key)
        {
            if (_gameMsgDict.TryGetValue(key, out var msg))
            {
                return msg;
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Dictionary<int, Tuple<string, Dictionary<string, object>>> GetItemsContainingName(string name)
        {
            var nameKeys = GetMessageKeysContainingName(name);
            var items = GetItemsByNameKeys(nameKeys);
            return items;
        }

        private readonly int _limit = 499;

        private Dictionary<int, Tuple<string, Dictionary<string, object>>> GetItemsByNameKeys(Dictionary<string, string> nameKeys)
        {
            var result = new Dictionary<int, Tuple<string, Dictionary<string, object>>>();
            var tempFile = Path.Combine(TempDir, "EFTable_Item.db");
            if (!File.Exists(tempFile))
            {
                File.WriteAllBytes(tempFile, _lpk.GetFileByName("EFTable_Item.db")!.GetData());
            }
            using (var connection = new SqliteConnection($"Data Source={tempFile}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                var count = 0;
                for (int i = 0; i < nameKeys.Count; i++)
                {
                    var kvp = nameKeys.ElementAt(i);
                    if (count == 0)
                    {
                        command.CommandText = $@"Select * from Item Where Name = '{kvp.Key}'";
                    }
                    else
                    {
                        command.CommandText += $@" or Name = '{kvp.Key}'";
                    }
                    count++;
                    if (count >= _limit)
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                object[] objs = new object[reader.FieldCount];
                                var colNames = new string[reader.FieldCount];
                                for (int j = 0; j < reader.FieldCount; j++)
                                {
                                    colNames[j] = reader.GetName(j);
                                }

                                reader.GetValues(objs);
                                var resultDict = new Dictionary<string, object>();
                                string name = "";
                                int primaryKey = 0;
                                for (var j = 0; j < objs.Length; j++)
                                {
                                    var obj = objs[j];
                                    var colname = colNames[j];
                                    resultDict.Add(colname, obj);
                                    if (colname == "Name")
                                    {
                                        name = obj.ToString();
                                    }
                                    if (colname == "PrimaryKey")
                                    {
                                        primaryKey = Convert.ToInt32(obj);
                                    }
                                }
                                result.TryAdd(primaryKey, new Tuple<string, Dictionary<string, object>>(nameKeys[name], resultDict));
                            }
                        }
                        command = connection.CreateCommand();
                        count = 0;
                    }
                }
                if (command.CommandText != "")
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            object[] objs = new object[reader.FieldCount];
                            var colNames = new string[reader.FieldCount];
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                colNames[i] = reader.GetName(i);
                            }

                            reader.GetValues(objs);
                            var resultDict = new Dictionary<string, object>();
                            string name = "";
                            int primaryKey = 0;
                            for (var i = 0; i < objs.Length; i++)
                            {
                                var obj = objs[i];
                                var colname = colNames[i];
                                resultDict.Add(colname, obj);
                                if (colname == "Name")
                                {
                                    name = obj.ToString();
                                }
                                if (colname == "PrimaryKey")
                                {
                                    primaryKey = Convert.ToInt32(obj);
                                }
                            }
                            result.TryAdd(primaryKey, new Tuple<string, Dictionary<string, object>>(nameKeys[name], resultDict));
                        }
                    }
                }
                connection.Close();
                connection.Dispose();
            }
            return result;
        }

        private Dictionary<string, string> GetMessageKeysContainingName(string name)
        {
            return _gameMsgDict.Where(x => x.Value.Contains(name)).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
