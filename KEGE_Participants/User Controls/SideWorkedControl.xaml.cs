using KEGE_Station;
using Participant_Result;
using System.IO;
using System.Runtime.InteropServices.Marshalling;
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
            Window.GetWindow(this).Close();
        }

        private void _EndAttempt_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var collection = _TaskHandler.GetPanels();
                ResultCollector.Instance.SetAnswers(collection);

                var result = ResultCollector.Instance.GetResult();

                string dirPath = @"D:\Temp\Results\";
                string filePath = dirPath + $"{result.SecondName}_{result.Name}_{result.MiddleName}_{DateTime.Now:dd-MM-yyyy}.json";


                string json = JsonSerializer.Serialize(result, new JsonSerializerOptions
                {
                    WriteIndented = true,
                });

                File.WriteAllText(filePath, json);

                PageFacade.Instance.SetContent(new MainLogoControl());
                _facade.OpenMainMenuWithOutButton();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
