using System.IO;
using KEGE_Station;
using System.Windows;
using System.Text.Json;
using System.Windows.Controls;
using KEGE_Participants.Windows;

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
            // Red
            ButtonBehavior.Apply(_Close_btn, true);
            ButtonBehavior.Apply(_EndAttempt_btn, true);
        }

        private void _Close_btn_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        public bool FinishExam(bool isAuto = false)
        {
            // 1. Если нажал участник — спрашиваем подтверждение
            if (!isAuto)
            {
                var notification = new NotificationWindow();
                notification.ShowNotification(
                    "Подтверждение.",
                    "Вы уверены, что хотите завершить попытку?",
                    NotificationType.Warning,
                    true
                    );

                if (notification.result != MessageBoxResult.Yes) return false;
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

                NotificationWindow.QuickShow(
                    "Попытка завершена.",
                    "Ответы успешно сохранены.",
                    NotificationType.Success
                    );

                PageFacade.Instance.SetContent(new MainLogoControl());
                _facade.OpenMainMenuWithOutButton();
                return true;
            }
            catch (Exception ex)
            {
                NotificationWindow.QuickShow(
                    "Ошибка сохранения файла.",
                    ex.Message,
                    NotificationType.Error
                    );
                return false;
            }
        }

        private void _EndAttempt_btn_Click(object sender, RoutedEventArgs e)
        {
            FinishExam();
        }
    }
}
