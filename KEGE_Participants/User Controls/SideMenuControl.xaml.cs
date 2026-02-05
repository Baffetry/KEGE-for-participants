using KEGE_Participants.Models.Exceptions;
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
                TestingOption option = null;
                string cfgPath = App.GetResourceString("ConfigurationPath");

                var ofd = new OpenFileDialog { InitialDirectory = cfgPath };

                if (ofd.ShowDialog() is true)
                {
                    var json = File.ReadAllText(ofd.FileName);
                    option = JsonSerializer.Deserialize<TestingOption>(json);
                }

                _taskHandler.FillGridWithButtons(option);
                _facade.OpenWorkedArea();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неверный путь к файлу");
                return;
            }
        }
    }
}
