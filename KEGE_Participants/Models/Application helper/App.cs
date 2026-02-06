using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace KEGE_Participants
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializeConfig();
        }

        public static string GetResourceString(string key)
        {
            return Current.Resources.Contains(key) ? Current.Resources[key] as string : string.Empty;
        }

        public static void SetResourceString(string key, string value)
        {
            if (Current.Resources.Contains(key))
                Current.Resources[key] = value;
            else
                Current.Resources.Add(key, value);
        }

        public static void InitializeConfig()
        {
            string cfgFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");

            if (!File.Exists(cfgFile))
            {
                // Значения по умолчанию, если файла нет
                string defaultCfg = "ConfigurationPath D:\\\n" +
                                    "SavedPath D:\\\n" +
                                    "TimeLimit 03:55:00";
                File.WriteAllText(cfgFile, defaultCfg);
            }

            string[] lines = File.ReadAllLines(cfgFile);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                // Делим строку на Ключ и Значение (по первому пробелу)
                var parts = line.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2) continue;

                string key = parts[0];
                string value = parts[1];

                // Если значение похоже на время (HH:mm:ss)
                if (Regex.IsMatch(value, @"^[0-9]{1,2}:[0-5][0-9]:[0-5][0-9]"))
                {
                    var time = value.Split(':');
                    SetResourceString(key + "_hours", time[0]);
                    SetResourceString(key + "_minutes", time[1]);
                    SetResourceString(key + "_seconds", time[2]);
                }
                else
                    SetResourceString(key, value);
            }
        }
    }
}
