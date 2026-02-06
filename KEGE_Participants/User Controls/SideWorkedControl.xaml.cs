using KEGE_Station;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace KEGE_Participants.User_Controls
{
    /// <summary>
    /// Interaction logic for SideWorkedControl.xaml
    /// </summary>
    public partial class SideWorkedControl : UserControl
    {
        private readonly PageFacade _facade = PageFacade.Instance;

        public SideWorkedControl()
        {
            InitializeComponent();
            SetButtonBehavior();
        }

        private void SetButtonBehavior()
        {
            // Green

            // Red
            ButtonBehavior.Apply(_Close_btn, true);
            ButtonBehavior.Apply(_EndAttempt_btn, true);
        }

        private void _Close_btn_Click(object sender, RoutedEventArgs e)
        {
            FinishExam();
            Window.GetWindow(this).Close();
        }

        public void FinishExam(bool isAuto = false)
        {
            // 1. Если нажал человек — спрашиваем подтверждение
            if (!isAuto)
            {
                var confirm = MessageBox.Show(
                    "Вы уверены, что хотите завершить попытку?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (confirm != MessageBoxResult.Yes) return;
            }

            try
            {
                var collection = _TaskHandler.GetPanels();
                ResultCollector.Instance.SetAnswers(collection);
                var result = ResultCollector.Instance.GetResult();

                string dirPath = App.GetResourceString("SavedPath");

                if (!Directory.Exists(dirPath)) 
                    Directory.CreateDirectory(dirPath);

                string fileName = $"{result.SecondName}_{result.Name}_{result.MiddleName}_{DateTime.Now:dd-MM-yyyy_HH-mm}.json";
                string filePath = Path.Combine(dirPath, fileName);

                string json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);

                MessageBox.Show(
                    "Попытка завершена. Ваши ответы успешно сохранены.",
                    "Пробный экзамен окончен",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                PageFacade.Instance.SetContent(new MainLogoControl());
                _facade.OpenMainMenuWithOutButton();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void _EndAttempt_btn_Click(object sender, RoutedEventArgs e)
        {
            FinishExam();
        }
    }
}
