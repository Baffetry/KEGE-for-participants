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
            _facade.OpenMainMenu();
        }
    }
}
