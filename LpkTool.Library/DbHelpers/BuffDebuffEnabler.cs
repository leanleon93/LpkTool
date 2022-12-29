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
    public class BuffDebuffEnabler : IDisposable
    {
        private readonly string _guid;
        private string TempDir => Path.Combine(Path.GetTempPath(), "lpktool_" + _guid);
        private readonly Lpk _lpk;
        private readonly string _tableName = "EFTable_SkillBuff.db";
        public static BuffDebuffEnabler FromFile(string lpkPath)
        {
            return new BuffDebuffEnabler(Lpk.FromFile(lpkPath));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpk"></param>
        public BuffDebuffEnabler(Lpk lpk)
        {
            _lpk = lpk;
            _guid = Guid.NewGuid().ToString();
            Directory.CreateDirectory(TempDir);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowAllBuffsDebuffs()
        {
            var tempFilePath = InitTempFile();
            using (var connection = new SqliteConnection($"Data Source={tempFilePath}"))
            {
                connection.Open();

                ShowIcons(connection);
                ShowDurationUi(connection);
                TranslateKrTitle(connection);
                TranslateKrDescription(connection);
                connection.Close();
                connection.Dispose();
            }
        }



        private void TranslateKrTitle(SqliteConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = $@"SELECT PrimaryKey, SecondaryKey, Tip FROM SkillBuff WHERE Name = '' AND Tip != '';";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var primaryKey = reader.GetInt64(0);
                    var secondaryKey = reader.GetInt64(1);
                    var tip = reader.GetString(2);
                    var skillBuff = new SkillBuff() {
                        PrimaryKey = primaryKey,
                        SecondaryKey = secondaryKey,
                        Tip = tip
                    };
                    TranslateTip(skillBuff);
                }
            }
        }

        private void TranslateTip(SkillBuff skillBuff)
        {
            throw new NotImplementedException();
        }

        private void TranslateKrDescription(SqliteConnection connection)
        {
            throw new NotImplementedException();
        }

        private void ShowDurationUi(SqliteConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = $@"UPDATE SkillBuff SET ShowDuration = 1, DurationUI = 1 WHERE DurationUI = 0;";
            command.ExecuteNonQuery();
        }

        private void ShowIcons(SqliteConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = $@"UPDATE SkillBuff SET IconShow = 1 WHERE IconShow = 0;";
            command.ExecuteNonQuery();
        }

        private string InitTempFile()
        {
            var tempFile = Path.Combine(TempDir, _tableName);
            if (!File.Exists(tempFile))
            {
                File.WriteAllBytes(tempFile, _lpk.GetFileByName(_tableName)!.GetData());
            }
            return tempFile;
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
    }
}
