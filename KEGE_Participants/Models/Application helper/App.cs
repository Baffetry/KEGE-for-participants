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
            string key = string.Empty, value = string.Empty;


            if (!File.Exists(cfgFile))
            {
                string defaultCfg = "ConfigurationPath D:\\\n TimeLimit 3:50:00";

                File.Create(cfgFile);
                File.WriteAllText(cfgFile, defaultCfg);
            }
            else
            {
                string[] lines = File.ReadAllLines(cfgFile);

                foreach (var line in lines)
                {
                    var pages = line.Split(new[] { ' ' }, 
                        StringSplitOptions.RemoveEmptyEntries);

                    key = pages[0];
                    value = pages[1];

                    if (Regex.Match(value, @"^[0-9]{1,2}:[0-5][0-9]:[0-5][0-9]").Success)
                    {
                        var time = value.Split(new[] { ':' },
                            StringSplitOptions.RemoveEmptyEntries);

                        string hours = time[0];
                        string minutes = time[1];
                        string seconds = time[2];

                        App.SetResourceString(key + "_hours", hours);
                        App.SetResourceString(key + "_minutes", minutes);
                        App.SetResourceString(key + "_seconds", seconds);

                        continue;
                    }

                    App.SetResourceString(key, value);
                }
            }

            SetResourceString(key, value);

        }
    }
}
