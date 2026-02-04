using KEGE_Participants.Models.Facade.Pages;
using KEGE_Participants.Models.Facade;
using System.Windows;

namespace KEGE_Participants
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PageFacade _facade = PageFacade.Instance;

        public MainWindow()
        {
            InitializeComponent();

            _facade.Initialize(
                new MainMenu(MainMenu_Logo, MainMenu_SideMenu),
                new WorkedArea(WorkedArea_SideMenu)
            );

            _facade.OpenMainMenu();
        }
    }
}