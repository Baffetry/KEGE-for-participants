using System.IO;
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
                key = "ConfigurationPath";
                value = "D:\\";

                File.Create(cfgFile);
                File.WriteAllText(cfgFile, $"{key} {value}");
            }
            else
            {
                string[] line = File.ReadAllText(cfgFile).Split();

                key = line[0];
                value = line[1];
            }

            SetResourceString(key, value);

        }
    }
}
