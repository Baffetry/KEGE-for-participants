using System.IO;
using Exceptions;
using KEGE_Station;
using Testing_Option;
using System.Windows;
using System.Text.Json;
using System.Windows.Controls;
using KEGE_Participants.Windows;

namespace KEGE_Participants.User_Controls
{
    /// <summary>
    /// Interaction logic for SideMenuControl.xaml
    /// </summary>
    public partial class SideMenuControl : UserControl
    {
        private readonly PageFacade _facade = PageFacade.Instance;
        public TaskHandlerControl _taskHandler { get; set; }
        
        public string FirstName => _LoginBoard.FirstName;
        public string SecondName => _LoginBoard.SecondName;
        public string MiddleName => _LoginBoard.MiddleName;

        public SideMenuControl()
        {
            InitializeComponent();
            SetButtonBehavior();
        }

        private void SetButtonBehavior()
        {
            // Green
            ButtonBehavior.Apply(_StartAttempt_btn);
            ButtonBehavior.Apply(Settings_btn);
            ButtonBehavior.Apply(Home_btn);

            // Red
            ButtonBehavior.Apply(_Close_btn, true);
        }

        private void _Close_btn_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }

        private void _StartAttempt_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SecondName)) throw new EmptyLogInExcaption("Введите фамилию.");
                if (string.IsNullOrWhiteSpace(FirstName)) throw new EmptyLogInExcaption("Введите имя.");
                if (string.IsNullOrWhiteSpace(MiddleName)) throw new EmptyLogInExcaption("Введите отчество.");


                var resultCollector = ResultCollector.Instance;

                resultCollector.Name = FirstName;
                resultCollector.SecondName = SecondName;
                resultCollector.MiddleName = MiddleName;

                string cfgPath = App.GetResourceString("ConfigurationPath");
                string dirPath = App.GetResourceString("SavedPath");

                if (string.IsNullOrEmpty(dirPath)) throw new Exception();
                if (string.IsNullOrEmpty(cfgPath)) throw new Exception();

                var json = File.ReadAllText(cfgPath);
                TestingOption option = JsonSerializer.Deserialize<TestingOption>(json);

                resultCollector.OptionId = option.OptionID;

                _taskHandler.FillGridWithButtons(option);
                _taskHandler.SelectedDefaultPanel();
                _facade.OpenWorkedArea();
            }
            catch (EmptyLogInExcaption ex)
            {
                NotificationWindow.QuickShow(
                    "Неверные данные участника.", 
                    ex.Message, 
                    NotificationType.Warning
                    );
                return;
            }
            catch (Exception ex)
            {
                NotificationWindow.QuickShow(
                    "Ошибка конфигурации.", 
                    "Проверьте путь к варианту или к результату в настройках", 
                    NotificationType.Error
                    );
                return;
            }
        }

        private void Settings_btn_Click(object sender, RoutedEventArgs e)
        {
            _facade.OpenSettings();
        }

        private void Home_btn_Click(object sender, RoutedEventArgs e)
        {
            _facade.OpenMainMenu();
        }
    }
}
