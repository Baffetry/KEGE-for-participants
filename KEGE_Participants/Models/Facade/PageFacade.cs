using KEGE_Participants.Models.Facade;
using KEGE_Participants.Models.Facade.Pages;
using System.Windows;
using System.Windows.Controls;
using Testing_Option;

namespace KEGE_Participants
{
    public class PageFacade
    {
        private static PageFacade _instance;
        public static PageFacade Instance => _instance ??= new PageFacade();

        private Grid _mainGrid;

        // Main menu
        private MainMenu _mainMenu;
        private WorkedArea _workedArea;

        private PageFacade() { }

        public void GridInit(Grid grid)
        {
            _mainGrid = grid;
        }

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

        public void SetContent(UserControl control)
        {
            if (_mainGrid is null) return;

            var oldContent = _mainGrid.Children
                .Cast<UIElement>()
                .FirstOrDefault(e => Grid.GetColumn(e) == 1 && e != _mainGrid.FindName("MainMenu_Logo"));

            if (oldContent != null)
                _mainGrid.Children.Remove(oldContent);

            Grid.SetColumn(control, 1);
            _mainGrid.Children.Add(control);
        }

        private void CheckInitialization()
        {
            if (_mainMenu == null || _workedArea == null)
                throw new InvalidOperationException("PageFacade не был инициализирован. Вызовите Initialize() перед использованием.");
        }
    }
}