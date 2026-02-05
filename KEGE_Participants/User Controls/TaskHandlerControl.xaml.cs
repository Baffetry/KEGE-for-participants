using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows;
using Testing_Option;

namespace KEGE_Participants.User_Controls
{
    /// <summary>
    /// Interaction logic for TaskHandlerControl.xaml
    /// </summary>
    public partial class TaskHandlerControl : UserControl
    {
        private Dictionary<string, TaskViewControl> _panels;

        private const int COLUMNS = 4;
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
        }

        public void FillGridWithButtons(TestingOption option)
        {
            _panels = new Dictionary<string, TaskViewControl>();

            _TaskHandlerGrid.Children.Clear();

            var buttonTemplate = (ControlTemplate)this.FindResource("NoMouseOverButtonTemplate");

            // Информация
            var infBtn = CreateButton("i", buttonTemplate);
            infBtn.FontSize = 26;

            Grid.SetRow(infBtn, 0);
            Grid.SetColumn(infBtn, 0);
            _TaskHandlerGrid.Children.Add(infBtn);

            // Задания
            for (int i = 0; i < count; i++)
            {
                string content = (i + 1).ToString();

                var btn = CreateButton(content, buttonTemplate);

                int index = i + 1;
                int row = index / COLUMNS;
                int col = index % COLUMNS;

                Grid.SetRow(btn, row);
                Grid.SetColumn(btn, col);

                _panels[content] = new TaskViewControl(option.TaskList[i]);
                _TaskHandlerGrid.Children.Add(btn);
            }
        }

        private Button CreateButton(string content, ControlTemplate template)
        {
            var btn = new Button
            {
                // Совйства кнопки
                Content = content,
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
                Template = template
            };

            btn.MouseEnter += TaskBtn_MouseEnter;
            btn.MouseLeave += TaskBtn_MouseLeave;
            btn.Click += TaskBtn_Click;

            return btn;
        }

        private void TaskBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button btn)
                btn.Background = (Brush)new BrushConverter().ConvertFrom("#66D9FF");
        }

        private void TaskBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Button btn)
                btn.Background = Brushes.White;
        }

        private void TaskBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var content = btn.Content.ToString();

            if ("i".Equals(content))
            {
                PageFacade.Instance.SetTaskContent(new InfoPanelControl());
            }
            else
            {
                string taskNumber = btn.Content.ToString();
                PageFacade.Instance.SetTaskContent(_panels[taskNumber]);
            }
        }
    }
}