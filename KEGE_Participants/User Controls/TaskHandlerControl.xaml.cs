using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace KEGE_Participants.User_Controls
{
    /// <summary>
    /// Interaction logic for TaskHandlerControl.xaml
    /// </summary>
    public partial class TaskHandlerControl : UserControl
    {
        private const int COLUMNS = 5;
        private int count = 27;
        public TaskHandlerControl()
        {
            InitializeComponent();
            SetHandlerGrid();
        }

        private void SetHandlerGrid()
        {
            _TaskHandlerGrid.ColumnDefinitions.Clear();
            _TaskHandlerGrid.RowDefinitions.Clear();
            _TaskHandlerGrid.Children.Clear();

            int rows = (count + COLUMNS - 1) / COLUMNS;

            for (int col = 0; col < COLUMNS; col++)
            {
                _TaskHandlerGrid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Auto)
                });
            }

            for (int row = 0; row < rows; row++)
            {
                _TaskHandlerGrid.RowDefinitions.Add(new RowDefinition
                {
                    Height = GridLength.Auto
                });
            }

            FillGridWithButtons();
        }

        private void FillGridWithButtons()
        {
            _TaskHandlerGrid.Children.Clear();

            var buttonTemplate = (ControlTemplate)this.FindResource("NoMouseOverButtonTemplate");

            for (int i = 0; i < count; i++)
            {
                var btn = new Button
                {
                    // Совйства кнопки
                    Content = (i + 1).ToString(),
                    Margin = new Thickness(3),
                    Height = 65,
                    Width = 65,

                    // Свойства шаблона
                    Background = Brushes.White,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(3),

                    // Совйства текста
                    FontSize = 20,
                    FontWeight = FontWeights.SemiBold,
                    FontFamily = new FontFamily("/Resources/Fonts/#Inter"),

                    // Привязка шаблона
                    Template = buttonTemplate
                };

                btn.MouseEnter += TaskBtn_MouseEnter;
                btn.MouseLeave += TaskBtn_MouseLeave;

                int row = i / COLUMNS;
                int col = i % COLUMNS;

                Grid.SetRow(btn, row);
                Grid.SetColumn(btn, col);

                _TaskHandlerGrid.Children.Add(btn);
            }
        }

        private void TaskBtn_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is Button btn)
                btn.Background = (Brush)new BrushConverter().ConvertFrom("#66D9FF");
        }

        private void TaskBtn_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is Button btn)
                btn.Background = Brushes.White;
        }
    }
}