using System;
using System.IO;
using System.Xml.Serialization;

namespace feeder_s
{
    [Serializable]
    public class RSS_Feed
    {
        public bool Status { get; set; } = true;
        public string Link { get; set; } = "https://habr.com/rss/interesting/";
    }

    public class Settings
    {
        public static string path = "";

        public int Time { get; set; } = 60000;
        public RSS_Feed[] Feeds { get; set; } = new[]{ new RSS_Feed() };
        public bool Description_format { get; set; } = true;

        public static Settings LoadSettings(string path)
        {
            Settings.path = path;

            return LoadSettings();
        }

        public static Settings LoadSettings()
        {
            FileStream file = File.Open(path, FileMode.Open);
            XmlSerializer formatter = new XmlSerializer(typeof(Settings));

            Settings s = (Settings)formatter.Deserialize(file);

            file.Close();

            return s;
        }

        public static void SaveSettings(string path, Settings s)
        {
            Settings.path = path;

            SaveSettings(s);
        }

        public static void SaveSettings(Settings s)
        {
            File.WriteAllText(path, string.Empty);
            FileStream file = File.Open(path, FileMode.Open);

            XmlSerializer formatter = new XmlSerializer(typeof(Settings));

            formatter.Serialize(file, s);

            file.Close();
        }
    }
}
