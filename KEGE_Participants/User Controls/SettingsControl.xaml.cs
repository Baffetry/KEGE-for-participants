using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KEGE_Participants.User_Controls
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();
            LoadCurrentSettings();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            // Регулярное выражение: разрешаем только символы от 0 до 9
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TimeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                string text = tb.Text.Trim();

                // Если поле пустое — ставим два нуля
                if (string.IsNullOrEmpty(text))
                {
                    tb.Text = "00";
                    return;
                }

                // Если введена одна цифра — добавляем 0 СЛЕВА
                if (text.Length == 1)
                    tb.Text = "0" + text;

                // Валидация на максимальные значения для минут и секунд
                if (tb.Name != "Setting_Hours")
                    if (int.TryParse(tb.Text, out int val1) && val1 > 59)
                        tb.Text = "59";
                else // Для часов (например, ограничим 99 часами, чтобы не ломать верстку)
                    if (int.TryParse(tb.Text, out int val2) && val2 > 99)
                        tb.Text = "99";
            }
        }

        private void LoadCurrentSettings()
        {
            Setting_LoadPath.Text = App.GetResourceString("ConfigurationPath");
            Setting_SavePath.Text = App.GetResourceString("SavedPath");

            Setting_Hours.Text = App.GetResourceString("TimeLimit_hours");
            Setting_Minutes.Text = App.GetResourceString("TimeLimit_minutes");
            Setting_Seconds.Text = App.GetResourceString("TimeLimit_seconds");
        }

        private void BrowseLoad_btn_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog { Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*" };
            if (ofd.ShowDialog() == true) Setting_LoadPath.Text = ofd.FileName;
        }

        private void BrowseSave_btn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog { Title = "Выберите папку для сохранения результатов" };
            if (dialog.ShowDialog() == true) Setting_SavePath.Text = dialog.FolderName;
        }

        private void SaveAllSettings_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Формируем строку времени
                string timeValue = $"{Setting_Hours.Text.PadLeft(2, '0')}:{Setting_Minutes.Text.PadLeft(2, '0')}:{Setting_Seconds.Text.PadLeft(2, '0')}";

                // 2. Формируем контент файла
                string configContent = $"ConfigurationPath {Setting_LoadPath.Text}\n" +
                                       $"SavedPath {Setting_SavePath.Text}\n" +
                                       $"TimeLimit {timeValue}";

                // 3. Сохраняем в файл
                string cfgFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");
                File.WriteAllText(cfgFile, configContent);

                // 4. Обновляем ресурсы в реальном времени
                App.SetResourceString("ConfigurationPath", Setting_LoadPath.Text);
                App.SetResourceString("SavedPath", Setting_SavePath.Text);
                App.SetResourceString("TimeLimit_hours", Setting_Hours.Text);
                App.SetResourceString("TimeLimit_minutes", Setting_Minutes.Text);
                App.SetResourceString("TimeLimit_seconds", Setting_Seconds.Text);

                MessageBox.Show("Настройки успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
