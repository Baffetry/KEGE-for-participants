using System.Windows;
using KEGE_Participants.Models.Facade;
using KEGE_Participants.Models.Facade.Pages;

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

            PageFacade.Instance.GridInit(_MainGrid);
            
            _facade.Initialize(
                new MainMenu(MainMenu_Logo, MainMenu_SideMenu, _Settings),
                new WorkedArea(WorkedArea_SideMenu)
            );
            
            MainMenu_SideMenu._taskHandler = WorkedArea_SideMenu._TaskHandler;
            _facade.OpenMainMenu();
        }
    }
}