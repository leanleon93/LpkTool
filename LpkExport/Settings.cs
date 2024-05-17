using LpkTool.Library.LpkData;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LpkExport
{
    public class Settings
    {

        private const string FILENAME = "LpkExport.usersettings.json";

        [JsonConverter(typeof(StringEnumConverter))]
        public Region Region { get; set; }

        internal static Settings Load()
        {
            if (File.Exists(FILENAME))
            {
                try
                {
                    var settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(FILENAME));
                    if (settings != null)
                    {
                        return settings;
                    }
                }
                catch { }
            }
            var newSettings = new Settings() {
                Region = Region.EU
            };
            newSettings.Save();
            return newSettings;
        }

        internal void Save()
        {
            File.WriteAllText(FILENAME, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}
