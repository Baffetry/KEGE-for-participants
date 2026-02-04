using System.Windows.Controls;

namespace KEGE_Participants.User_Controls
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        public string SecondName => Login_SecondName?.Text;
        public string FirstName => Login_FirstName?.Text;
        public string MiddleName => Login_MiddleName?.Text;

        public LoginControl()
        {
            InitializeComponent();
        }
    }
}
