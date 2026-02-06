using KEGE_Participants.User_Controls;
using System.Windows;

namespace KEGE_Participants.Models.Facade
{
    public class MainMenu
    {
        private readonly MainLogoControl _mainLogo;
        private readonly SideMenuControl _sideMenu;

        public MainMenu(MainLogoControl logo, SideMenuControl menu)
        {
            _mainLogo = logo;
            _sideMenu = menu;
        }

        public void Collapsed()
        {
            _mainLogo.Visibility = Visibility.Collapsed;
            _sideMenu.Visibility = Visibility.Collapsed;
        }

        public void VisiableWithOutButton()
        {
            _mainLogo.Visibility = Visibility.Visible;
            _sideMenu.Visibility = Visibility.Visible;
            _sideMenu._StartAttempt_btn.Visibility = Visibility.Collapsed;
        }

        public void Visible()
        {
            _mainLogo.Visibility = Visibility.Visible;
            _sideMenu.Visibility = Visibility.Visible;
        }
    }
}