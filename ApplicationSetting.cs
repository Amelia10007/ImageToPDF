using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ImageToPDF
{
    static class ApplicationSetting
    {
        public static string Of(string key)
        {
            if (!read)
            {
                LoadSetting();
                read = true;
            }
            return pairs[key.ToLower()];
        }

        private static readonly string settingFilename = "settings.txt";
        private static readonly IDictionary<string, string> pairs = new Dictionary<string, string>();
        private static bool read = false;

        private static void LoadSetting()
        {
            var settingFileDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var settingFilePath = $@"{settingFileDirectory}\{settingFilename}";
            var lines = File.ReadAllLines(settingFilePath);
            foreach (var line in lines)
            {
                var split = line.Split('=');
                if (split.Length != 2)
                {
                    continue;
                }
                var key = split[0].Trim(' ', '\t').ToLower();
                var value = split[1].Trim(' ', '\t');
                pairs.Add(key, value);
            }
        }
    }
}
