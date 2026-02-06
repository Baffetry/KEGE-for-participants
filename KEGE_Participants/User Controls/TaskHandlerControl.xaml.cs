using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Testing_Option;

namespace KEGE_Participants.User_Controls
{
    /// <summary>
    /// Interaction logic for TaskHandlerControl.xaml
    /// </summary>
    public partial class TaskHandlerControl : UserControl
    {
        private Dictionary<string, Button> _taskButtons;
        private Dictionary<string, TaskViewControl> _panels;

        private const int COLUMNS = 4;
        private int count = 27;
        public TaskHandlerControl()
        {
            InitializeComponent();
            SetHandlerGrid();

            _taskButtons = new Dictionary<string, Button>();
        }

        public Dictionary<string, TaskViewControl> GetPanels()
        {
            return _panels;
        }

        public void SelectedDefaultPanel()
        {
            if (_taskButtons.ContainsKey("i"))
                TaskBtn_Click(_taskButtons["i"], new RoutedEventArgs());
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
            _taskButtons.Clear();

            var buttonTemplate = (ControlTemplate)this.FindResource("NoMouseOverButtonTemplate");

            // Информация
            var infBtn = CreateButton("i", buttonTemplate);
            infBtn.FontSize = 26;

            Grid.SetRow(infBtn, 0);
            Grid.SetColumn(infBtn, 0);
            _TaskHandlerGrid.Children.Add(infBtn);
            _taskButtons["i"] = infBtn;
            // Задания
            for (int i = 0; i < count; i++)
            {
                string content = (i + 1).ToString();

                var btn = CreateButton(content, buttonTemplate);
                _taskButtons[content] = btn;

                int index = i + 1;
                int row = index / COLUMNS;
                int col = index % COLUMNS;

                Grid.SetRow(btn, row);
                Grid.SetColumn(btn, col);

                var taskView = new TaskViewControl(option.TaskList[i]);
                taskView.TaskId = content;

                taskView.AnswerSaved += (id) =>
                {
                    if (_taskButtons.ContainsKey(id))
                        _taskButtons[id].Background = (Brush)new BrushConverter().ConvertFrom("#66D9FF");
                };

                taskView.AnswerChanged += (id) => {
                    if (_taskButtons.ContainsKey(id))
                    {
                        _taskButtons[id].Background = (Brush)new BrushConverter().ConvertFrom("#FFFFFF");
                    }
                };

                _panels[content] = /*new TaskViewControl(option.TaskList[i]);*/ taskView;
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
            {
                string content = btn.Content.ToString();

                if (_panels.ContainsKey(content) && !string.IsNullOrWhiteSpace(_panels[content].ParticipantAnswer))
                    btn.Background = (Brush)new BrushConverter().ConvertFrom("#66D9FF");
                else
                    btn.Background = (Brush)new BrushConverter().ConvertFrom("#FFFFFF");
            }
        }

        private void TaskBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var content = btn.Content.ToString();


            foreach (var panel in _panels.Values)
                panel.ResetUnsavedChanges();

            if ("i".Equals(content))
            {
                PageFacade.Instance.SetContent(new InfoPanelControl());
            }
            else
            {
                string taskNumber = btn.Content.ToString();
                PageFacade.Instance.SetContent(_panels[taskNumber]);
            }
        }
    }
}