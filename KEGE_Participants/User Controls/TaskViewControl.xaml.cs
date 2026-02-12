using System.IO;
using Task_Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Documents;
using KEGE_Participants.Windows;
using KEGE_Participants.Models.Factory;
using KEGE_Participants.Models.Image_halper;
using KEGE_Participants.Models.File_manager;
using KEGE_Participants.Models.FileUI_halper;
using KEGE_Participants.Models.Table_manager;
using KEGE_Participants.Models.Custom_brushes;


namespace KEGE_Participants.User_Controls
{
    /// <summary>
    /// Interaction logic for TaskViewControl.xaml
    /// </summary>
    public partial class TaskViewControl : UserControl
    {
        public event Action<string> AnswerSaved;
        public event Action<string> AnswerChanged;

        private readonly TableManager _tableManager;
        private readonly FileManager _fileManager = new();
        private readonly List<(string taskNumber, int rowCount)> _tableConfigs = new List<(string taskNumber, int rowCount)>()
        {
            ("17", 1), ("18", 1), ("20", 1), ("25", 10), ("26", 1), ("27", 2)
        };

        private TaskData _data;

        public string TaskId { get; set; }
        private bool _isUpdating = false;
        public string ParticipantAnswer { get; set; }

        public TaskViewControl(TaskData data)
        {
            InitializeComponent();
            _data = data;
            TaskId = _data.TaskNumber;
            _tableManager = new TableManager(Answer_Table_Grid);

            InitializeUI();
        }
        private void InitializeUI()
        {
            _TaskNumber_Box.Text = $"Задание {_data.TaskNumber}";
            _TaskWeight_Box.Text = _data.TaskWeight == 1 
                ? "1 балл" 
                : $"{_data.TaskWeight} балла";

            TaskImage.Source = ImageHalper.LoadImage(_data.Image);

            SetupTaskLayout();
            SetFilesToPanel();
        }

        #region Answer logic

        public string GetAnswer()
        {
            if (Bottom_Answer_Border.Visibility == Visibility.Visible)
                return string.IsNullOrWhiteSpace(Answer_TextBox.Text)
                    ? "%noAnswer%"
                    : Answer_TextBox.Text.Trim();

            var answers = _tableManager.ExtractAnswers();
            return answers.Count == 0
                ? "%noAnswer%"
                : string.Join(" ", answers);
        }

        public void ResetUnsavedChanges()
        {
            _isUpdating = true;
            ClearVisualStates();

            if (string.IsNullOrEmpty(ParticipantAnswer))
            {
                Answer_TextBox.Text = string.Empty;
                _isUpdating = false;
                return;
            }

            Answer_TextBox.Text = ParticipantAnswer;

            if (Table_Answer_Border.Visibility == Visibility.Visible)
            {
                _tableManager.RestoreTable(ParticipantAnswer);
                SetButtonActive(TableSave_btn);
            }
            else
            {
                //Answer_TextBox.Text = ParticipantAnswer;
                SetButtonActive(Save_btn);
            }

            _isUpdating = false;
        }

        private void Answer_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdating) return;

            ParticipantAnswer = string.Empty;
            ClearVisualStates();
            AnswerChanged?.Invoke(TaskId);
        }

        private void Save_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Answer_TextBox.Text))
            {
                ParticipantAnswer = Answer_TextBox.Text;
                SetButtonActive(Save_btn);
                AnswerSaved?.Invoke(TaskId);
            }
        }
        
        private void TableSave_btn_Click(object sender, RoutedEventArgs e)
        {
            var answers = _tableManager.ExtractAnswers();

            if (answers.Any())
            {
                if (answers.All(r => r.Equals("%noAnswer%"))) return;

                ParticipantAnswer = string.Join(" ", answers);
                SetButtonActive(TableSave_btn);
                AnswerSaved?.Invoke(TaskId);
            }
        }

        #endregion

        #region Layout ans table generation
        private void SetupTaskLayout()
        {
            var cfg = _tableConfigs.FirstOrDefault(x => x.taskNumber.Equals(TaskId));

            if (cfg is not { taskNumber: null })
            {
                Table_Answer_Border.Visibility = Visibility.Visible;
                Bottom_Answer_Border.Visibility = Visibility.Collapsed;

                //TaskContainerGrid.ColumnDefinitions[1].MinWidth = 400;

                GenerateTable(cfg.rowCount);
            }
            else
            {
                Table_Answer_Border.Visibility = Visibility.Collapsed;
                Bottom_Answer_Border.Visibility = Visibility.Visible;

                Answer_Table_Grid.Children.Clear();
            }
        }

        private void GenerateTable(int rowCount)
        {
            // Очищаем старые данные
            Answer_Table_Grid.Children.Clear();
            Answer_Table_Grid.RowDefinitions.Clear();
            Answer_Table_Grid.ColumnDefinitions.Clear();

            // Усталавливаем размеры колон
            Answer_Table_Grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(55) });
            Answer_Table_Grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            Answer_Table_Grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            var factory = new TableUIFactory(Answer_TextBox_TextChanged, Table_TextBox_PreviewKeyDown);

            for (int row = 0; row < rowCount; row++)
            {
                Answer_Table_Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });

                for (int col = 0; col < 3; col++)
                {
                    var cell = factory.CreateCell(row, col, rowCount);
                    Grid.SetRow(cell, row);
                    Grid.SetColumn(cell, col);
                    Answer_Table_Grid.Children.Add(cell);
                }
            }   
        }

        #endregion

        #region File management

        private void SetFilesToPanel()
        {
            if (_data.Files is null || !_data.Files.Any())
            {
                _FilesPanel.Visibility = Visibility.Collapsed;
                return;
            }

            _FilesPanel.Visibility = Visibility.Visible;
            _FilesContainer.Children.Clear();

            foreach (var file in _data.Files)
            {
                var fileControl = FileUIHalper.CreateFileControl(
                    file,
                    _fileManager.GetIconPath(Path.GetExtension(file.FileName)),
                    Hyperlink_Click
                    );

                _FilesContainer.Children.Add(fileControl);
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Hyperlink link && link.Tag is FileData file)
            {
                try { _fileManager.OpenFile(file); }
                catch (Exception ex)
                {
                    NotificationWindow.QuickShow(
                        "Обработка файла",
                        "Не удалось открыть файл",
                        NotificationType.Error
                        );
                }
            }
        }

        #endregion

        #region Utilities
        private void Table_TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                e.Handled = true; // Отменяем стандартную вставку

                string text = Clipboard.GetText();
                if (string.IsNullOrWhiteSpace(text)) return;

                var parts = text.Split(new[] { ' ', '\t', '\n', '\r' },
                    StringSplitOptions.RemoveEmptyEntries);

                var border = (sender as TextBox)?.Parent as Border;

                if (border is not null)
                {
                    _isUpdating = true;
                    _tableManager.PasteDate(parts, Grid.GetRow(border), Grid.GetColumn(border));
                    _isUpdating = false;
                    AnswerChanged?.Invoke(TaskId);
                }
            }
        }

        private void ClearVisualStates()
        {
            Save_btn.Background = CustomBrusher.White;
            TableSave_btn.Background = CustomBrusher.White;
        }

        private void SetButtonActive(Button btn)
        {
            btn.Background = CustomBrusher.Blue;
        }

        #endregion
    }
}