using Exceptions;
using KEGE_Station;
using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using Testing_Option;

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
                if (string.IsNullOrWhiteSpace(FirstName)) throw new EmptyLogInExcaption("Вы не ввели имя");
                if (string.IsNullOrWhiteSpace(SecondName)) throw new EmptyLogInExcaption("Вы не ввели фамилию");
                if (string.IsNullOrWhiteSpace(MiddleName)) throw new EmptyLogInExcaption("Вы не ввели отчество");


                var resultCollector = ResultCollector.Instance;

                resultCollector.Name = FirstName;
                resultCollector.SecondName = SecondName;
                resultCollector.MiddleName = MiddleName;

                TestingOption option = null;
                string cfgPath = App.GetResourceString("ConfigurationPath");

                var json = File.ReadAllText(cfgPath);
                option = JsonSerializer.Deserialize<TestingOption>(json);

                resultCollector.OptionId = option.OptionID;

                _taskHandler.FillGridWithButtons(option);
                _taskHandler.SelectedDefaultPanel();
                _facade.OpenWorkedArea();
            }
            catch (EmptyLogInExcaption ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неверный путь к файлу");
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
