using KEGE_Station;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace KEGE_Participants.User_Controls
{
    /// <summary>
    /// Interaction logic for SideMenuControl.xaml
    /// </summary>
    public partial class SideMenuControl : UserControl
    {
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
            ButtonBehavior.Apply(_EndAttempt_btn, true);
        }

        private void _Close_btn_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }

        private void _StartAttempt_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void _EndAttempt_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
