using KEGE_Participants.Models.Facade;
using KEGE_Participants.Models.Facade.Pages;

namespace KEGE_Participants
{
    public class PageFacade
    {
        private static PageFacade _instance;
        public static PageFacade Instance => _instance ??= new PageFacade();

        // Main menu
        private MainMenu _mainMenu;
        private WorkedArea _workedArea;

        private PageFacade() { }

        // Инициализация
        public void Initialize(MainMenu mainMenu, WorkedArea workedArea)
        {
            _mainMenu = mainMenu;
            _workedArea = workedArea;
        }

        // Взаимодействие
        public void OpenMainMenu()
        {
            CheckInitialization();
            _workedArea.Collapsed();
            _mainMenu.Visible();
        }

        public void OpenWorkedArea()
        {
            CheckInitialization();
            _mainMenu.Collapsed();
            _workedArea.Visible();
        }

        private void CheckInitialization()
        {
            if (_mainMenu == null || _workedArea == null)
                throw new InvalidOperationException("PageFacade не был инициализирован. Вызовите Initialize() перед использованием.");
        }
    }
}