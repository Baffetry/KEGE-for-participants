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
        public SideMenuControl()
        {
            InitializeComponent();
        }

        private void _Close_btn_Click(object sender, RoutedEventArgs e)
        {
            if (_Border.Width < 600)
                if (this.FindResource("ExpandMenu") is Storyboard expandSb)
                    expandSb.Begin();
            else
                Window.GetWindow(this)?.Close();
        }

        private void _StartAttempt_btn_Click(object sender, RoutedEventArgs e)
        {
            if (this.Resources["CollapseMenu"] is Storyboard sb)
                sb.Begin();

            _StartAttempt_btn.IsEnabled = false;
        }

        private void _EndAttempt_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
