using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpkTool.Library.DbHelpers
{
    /// <summary>
    /// 
    /// </summary>
    public class ItemHelper : IDisposable
    {
        private string TempDir = Path.Combine(Path.GetTempPath(), "lpktool_" + Guid.NewGuid().ToString());
        private Lpk _lpk;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpk"></param>
        public ItemHelper(Lpk lpk)
        {
            _lpk = lpk;
            Directory.CreateDirectory(TempDir);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Directory.Delete(TempDir, true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Dictionary<int, Tuple<string, Dictionary<string, object>>> GetItemsByName(string name)
        {
            var nameKeys = GetNameKeys(name);
            var items = GetItemsByKeys(nameKeys);
            return items;
        }

        public void SetModel(string model)
        {

        }

        private int _limit = 499;

        private Dictionary<int, Tuple<string, Dictionary<string, object>>> GetItemsByKeys(Dictionary<string, string> nameKeys)
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

        private Dictionary<string, string> GetNameKeys(string name)
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
                command.CommandText = $@"SELECT KEY, MSG from GameMsg_English where MSG Like '%{name}%'";

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
            return result;
        }
    }
}
