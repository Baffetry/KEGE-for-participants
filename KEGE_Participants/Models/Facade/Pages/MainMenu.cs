using KEGE_Participants.User_Controls;
using System.Windows;

namespace KEGE_Participants.Models.Facade
{
    public class MainMenu
    {
        private readonly MainLogoControl _mainLogo;
        private readonly SideMenuControl _sideMenu;
        private readonly SettingsControl _settings;

        public MainMenu(MainLogoControl logo, SideMenuControl menu, SettingsControl settings)
        {
            _mainLogo = logo;
            _sideMenu = menu;
            _settings = settings;
        }

        public void OpenSettings()
        {
            _settings.Visibility = Visibility.Visible;
            _mainLogo.Visibility = Visibility.Collapsed;
        }

        public void Collapsed()
        {
            _mainLogo.Visibility = Visibility.Collapsed;
            _sideMenu.Visibility = Visibility.Collapsed;
            _settings.Visibility = Visibility.Collapsed;
        }

        public void VisiableWithOutButton()
        {
            _mainLogo.Visibility = Visibility.Visible;
            _sideMenu.Visibility = Visibility.Visible;
            _sideMenu.Home_btn.Visibility = Visibility.Collapsed;
            _sideMenu.Settings_btn.Visibility = Visibility.Collapsed;
            _sideMenu._StartAttempt_btn.Visibility = Visibility.Collapsed;
        }

        public void Visible()
        {
            _mainLogo.Visibility = Visibility.Visible;
            _sideMenu.Visibility = Visibility.Visible;
            _settings.Visibility = Visibility.Collapsed;
        }
    }
}