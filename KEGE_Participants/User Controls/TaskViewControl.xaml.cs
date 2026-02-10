using KEGE_Participants.Models.Factory.UIElements;
using KEGE_Participants.Models.Factory.UIElements_factories;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Task_Data;

namespace KEGE_Participants.User_Controls
{
    /// <summary>
    /// Interaction logic for TaskViewControl.xaml
    /// </summary>
    public partial class TaskViewControl : UserControl
    {
        public event Action<string> AnswerSaved;
        public event Action<string> AnswerChanged;

        private readonly List<(string taskNumber, int rowCount)> _tableConfigs = new List<(string taskNumber, int rowCount)>()
        {
            ("17", 1),
            ("18", 1),
            ("20", 1),
            ("25", 10),
            ("26", 1),
            ("27", 2)
        };

        private TaskData _data;

        public string TaskId { get; set; }
        private bool _isUpdating = false;


        public string ParticipantAnswer { get; set; }

        public TaskViewControl(TaskData data)
        {
            InitializeComponent();
            _data = data;

            _TaskNumber_Box.Text = $"Задание {_data.TaskNumber}";

            string content = $"{_data.TaskWeight}";
            _TaskWeight_Box.Text = _data.TaskWeight == 1
                ? content + " балл"
                : content + " балла";

            TaskId = _data.TaskNumber;

            SetImage(_data.Image);
            SetupTaskLayout();
            SetFilesToPanel();
        }

        public string GetAnswer()
        {
            if (Bottom_Answer_Border.Visibility == Visibility.Visible)
            {
                return string.IsNullOrWhiteSpace(Answer_TextBox.Text)
                    ? "%noAnswer%"
                    : Answer_TextBox.Text.Trim();
            }

            var allCells = new List<string>();
            var lastFilledIndex = -1;

            for (int row = 0; row < Answer_Table_Grid.RowDefinitions.Count; row++)
            {
                for (int col = 1; col <= 2; col++)
                {
                    var cellBorder = Answer_Table_Grid.Children
                        .OfType<Border>()
                        .FirstOrDefault(
                            idx => Grid.GetRow(idx) == row &&
                            Grid.GetColumn(idx) == col
                        );

                    if (cellBorder?.Child is TextBox tb)
                    {
                        string text = tb.Text?.Trim();
                        bool isEmpty = string.IsNullOrWhiteSpace(text);

                        allCells.Add(isEmpty ? "%noAnswer%" : text);

                        if (!isEmpty)
                            lastFilledIndex = allCells.Count - 1;
                    }
                }
            }

            if (lastFilledIndex == -1) return "%noAnswer%";

            return string.Join(" ", allCells);
        }

        public void SetImage(byte[] imageBytes)
        {
            if (imageBytes is null || imageBytes.Length == 0) return;

            try
            {
                var bitmap = new BitmapImage();

                using (var ms = new MemoryStream(imageBytes))
                {
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                }

                bitmap.Freeze();
                TaskImage.Source = bitmap;

                RenderOptions.SetBitmapScalingMode(TaskImage, BitmapScalingMode.HighQuality);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки изображения");
                return;
            }
        }

        public void ResetUnsavedChanges()
        {
            _isUpdating = true;

            if (string.IsNullOrEmpty(ParticipantAnswer))
            {
                Save_btn.Background = Brushes.White;
                TableSave_btn.Background = Brushes.White;
                // Очищаем поля, если ответа нет
                Answer_TextBox.Text = string.Empty;
                return;
            }

            bool hasAnswer = !string.IsNullOrWhiteSpace(ParticipantAnswer);
            Brush activeColor = (Brush)new BrushConverter().ConvertFrom("#66D9FF");
            Brush idleColor = (Brush)new BrushConverter().ConvertFrom("#FFFFFF");

            // 1. Восстанавливаем обычное поле
            Answer_TextBox.Text = ParticipantAnswer;

            // 2. Восстанавливаем таблицу, если она активна
            if (Table_Answer_Border.Visibility == Visibility.Visible)
            {
                RestoreTableAnswers();
                Save_btn.Background = hasAnswer ? activeColor : idleColor;
            }
            else
            {
                TableSave_btn.Background = hasAnswer ? activeColor : idleColor;
            }

            _isUpdating = false;
        }

        private void RestoreTableAnswers()
        {
            if (string.IsNullOrWhiteSpace(ParticipantAnswer)) return;

            // Разбиваем строку ответов (в КЕГЭ они обычно через пробел)
            string[] parts = ParticipantAnswer.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int partIndex = 0;

            for (int i = 0; i < Answer_Table_Grid.RowDefinitions.Count; i++)
            {
                for (int j = 1; j <= 2; j++) // Колонки с вводом
                {
                    if (partIndex >= parts.Length) return;

                    var cell = Answer_Table_Grid.Children
                        .OfType<Border>()
                        .FirstOrDefault(b => Grid.GetRow(b) == i && Grid.GetColumn(b) == j);

                    if (cell?.Child is TextBox tb)
                    {
                        tb.Text = parts[partIndex++];
                    }
                }
            }
        }

        private void Answer_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string color = new BrushConverter().ConvertFrom("#66D9FF").ToString();

            bool flag = Save_btn.Background.ToString() != color && TableSave_btn.Background.ToString() != color;

            if (_isUpdating && flag) return;

            ParticipantAnswer = string.Empty;
            Save_btn.Background = (Brush)new BrushConverter().ConvertFrom("#FFFFFF");
            TableSave_btn.Background = (Brush)new BrushConverter().ConvertFrom("#FFFFFF");
            AnswerChanged?.Invoke(TaskId);
        }

        private void SetupTaskLayout()
        {
            var cfg = _tableConfigs.FirstOrDefault(x => x.taskNumber.Equals(TaskId));

            if (cfg is not { taskNumber: null })
            {
                Table_Answer_Border.Visibility = Visibility.Visible;
                Bottom_Answer_Border.Visibility = Visibility.Collapsed;
                GenerateTable(cfg.rowCount);
            }
            else
            {
                Table_Answer_Border.Visibility = Visibility.Collapsed;
                Bottom_Answer_Border.Visibility = Visibility.Visible;

                Answer_Table_Grid.Children.Clear();
            }
        }

        private void Save_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Answer_TextBox.Text))
            {
                ParticipantAnswer = Answer_TextBox.Text;
                Save_btn.Background = (Brush)new BrushConverter().ConvertFrom("#66D9FF");
                AnswerSaved?.Invoke(TaskId);
            }
        }

        private void TableSave_btn_Click(object sender, RoutedEventArgs e)
        {
            var resultList = new List<string>();

            for (int i = 0; i < Answer_Table_Grid.RowDefinitions.Count; i++)
            {
                for (int j = 1; j <= 2; j++)
                {
                    // Ищем Border в ячейке, затем TextBox внутри него
                    var cellBorder = Answer_Table_Grid.Children
                        .OfType<Border>()
                        .FirstOrDefault(b => Grid.GetRow(b) == i && Grid.GetColumn(b) == j);

                    if (cellBorder?.Child is TextBox tb && !string.IsNullOrWhiteSpace(tb.Text))
                    {
                        resultList.Add(tb.Text.Trim());
                    }
                }
            }

            if (resultList.Count > 0)
            {
                var temp = string.Join(" ", resultList);

                if (!string.IsNullOrEmpty(temp))
                {
                    ParticipantAnswer = temp;
                    TableSave_btn.Background = (Brush)new BrushConverter().ConvertFrom("#66D9FF");
                    AnswerSaved?.Invoke(TaskId);
                }
            }
        }

        private void GenerateTable(int rowCount)
        {
            // Очищаем старые данные
            Answer_Table_Grid.Children.Clear();
            Answer_Table_Grid.RowDefinitions.Clear();
            Answer_Table_Grid.ColumnDefinitions.Clear();

            // 1. Создаем колонки
            // Колонка 0: Номер строки
            Answer_Table_Grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(55) });
            // Колонки 1 и 2: Поля ввода
            Answer_Table_Grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            Answer_Table_Grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            // 2. Генерируем строки
            UIElementFactory borderFactory = new BorderFactory();
            UIElementFactory textBlockFactory = new TextBlockFactory();
            UIElementFactory textBoxFactory = new TextBoxFactory();

            for (int i = 0; i < rowCount; i++)
            {
                // Высота строки
                Answer_Table_Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });

                for (int j = 0; j < 3; j++)
                {
                    // Создаем рамку для каждой ячейки
                    var cellBorder = (Border)borderFactory.FactoryMethod();
                    cellBorder.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#A0A0A0");
                    cellBorder.BorderThickness = new Thickness(0, 0, (j < 2 ? 1 : 0), (i < rowCount - 1 ? 1 : 0));

                    if (j == 0)
                    {
                        // Ячейка с номером
                        var textBlock = (TextBlock)textBlockFactory.FactoryMethod();
                        textBlock.Text = (i + 1).ToString();

                        cellBorder.Child = textBlock;
                        cellBorder.Background = (Brush)new BrushConverter().ConvertFrom("#E0E0E0");
                    }
                    else
                    {
                        // Ячейка с вводом текста
                        var textBox = (TextBox)textBoxFactory.FactoryMethod();
                        textBox.TextChanged += Answer_TextBox_TextChanged;
                        textBox.PreviewKeyDown += Table_TextBox_PreviewKeyDown;
                        cellBorder.Child = textBox;
                    }

                    // Устанавливаем позицию в Grid
                    Grid.SetRow(cellBorder, i);
                    Grid.SetColumn(cellBorder, j);
                    Answer_Table_Grid.Children.Add(cellBorder);
                }
            }
        }

        private void SetFilesToPanel()
        {
            var files = _data.Files;

            if (files is null || files.Count == 0)
            {
                _FilesPanel.Visibility = Visibility.Collapsed;
                return;
            }

            _FilesPanel.Visibility = Visibility.Visible;
            _FilesContainer.Children.Clear();


            UIElementFactory imageFactory = new ImageFactory();
            UIElementFactory textBlockFactory = new TextBlockFactory();
            UIElementFactory borderFactory = new BorderFactory();

            foreach (var file in files)
            {
                // Рамка файла
                var rowBorder = (CustomBorder)borderFactory.FactoryMethod();
                rowBorder.BorderThickness = new Thickness(0);
                rowBorder.BorderBrush = Brushes.Transparent;
                rowBorder.Margin = new Thickness(10, 5, 10, 5);
                rowBorder.Cursor = Cursors.Hand;
                rowBorder.Tag = file;

                var panel = new StackPanel();

                var icon = (CustomImage)imageFactory.FactoryMethod();

                string extension = Path.GetExtension(file.FileName).ToLower();
                string iconPath = GetIconPath(extension);

                icon.Source = new BitmapImage(new Uri(iconPath,
                    UriKind.RelativeOrAbsolute));
                icon.HorizontalAlignment = HorizontalAlignment.Center;
                icon.VerticalAlignment = VerticalAlignment.Center;


                var textBlock = (CustomTextBlock)textBlockFactory.FactoryMethod();
                //textBlock.Text = file.FileName;
                textBlock.FontSize = 22;
                textBlock.FontWeight = FontWeights.SemiBold;
                textBlock.Margin = new Thickness(0, 10, 0, 0);
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.FontFamily = new FontFamily("/Resources/Fonts/#Inter");
                textBlock.Foreground = (Brush)new BrushConverter().ConvertFrom("#000000");

                var run = new Run(file.FileName);
                var hyperlink = new Hyperlink(run)
                {
                    TextDecorations = null,
                    Foreground = (Brush)new BrushConverter().ConvertFrom("#2d6dfe") // Синий цвет ссылки
                };

                hyperlink.Tag = file;
                hyperlink.Click += Hyperlink_Click;

                textBlock.Inlines.Add(hyperlink);

                panel.Children.Add(icon);
                panel.Children.Add(textBlock);
                rowBorder.Child = panel;

                _FilesContainer.Children.Add(rowBorder);
            }
        }

        private string GetIconPath(string extension)
        {
            switch (extension)
            {
                case ".txt": return "/Resources/Extensions/txt96x96.png";

                case ".doc":
                case ".docx": return "/Resources/Extensions/word96x96.png";

                case ".xls":
                case ".xlsx": return "/Resources/Extensions/exel96x96.png";

                case ".ods": return "/Resources/Extensions/ods96x96.png";

                default: return "/Resources/Extensions/unknownFile96xx96.png";
            }
        }

        private void Table_TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                e.Handled = true; // Отменяем стандартную вставку

                string clipboardText = Clipboard.GetText();
                if (string.IsNullOrWhiteSpace(clipboardText)) return;

                // Разбиваем текст по пробелам, табуляциям и переносам строк
                string[] parts = clipboardText.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 0) return;

                var currentTextBox = sender as TextBox;
                var currentBorder = currentTextBox?.Parent as Border;
                if (currentBorder == null) return;

                int startRow = Grid.GetRow(currentBorder);
                int startCol = Grid.GetColumn(currentBorder);

                FillTableFromPosition(parts, startRow, startCol);
            }
        }

        private void FillTableFromPosition(string[] data, int row, int col)
        {
            _isUpdating = true; // Блокируем сброс ParticipantAnswer (флаг из прошлого шага)
            int dataIndex = 0;

            for (int i = row; i < Answer_Table_Grid.RowDefinitions.Count; i++)
            {
                // Начинаем с текущей колонки, если это первая итерируемая строка, иначе с колонки 1
                int startJ = (i == row) ? col : 1;

                for (int j = startJ; j <= 2; j++)
                {
                    if (dataIndex >= data.Length) break;

                    // Ищем ячейку
                    var cell = Answer_Table_Grid.Children
                        .OfType<Border>()
                        .FirstOrDefault(b => Grid.GetRow(b) == i && Grid.GetColumn(b) == j);

                    if (cell?.Child is TextBox tb)
                    {
                        tb.Text = data[dataIndex++];
                    }
                }
                if (dataIndex >= data.Length) break;
            }

            _isUpdating = false;
            // После массовой вставки уведомляем о изменениях один раз
            AnswerChanged?.Invoke(TaskId);
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var hyperlink = (Hyperlink)sender;
            var fileData = (FileData)hyperlink.Tag;

            if (fileData?.Data == null || fileData.Data.Length == 0)
            {
                MessageBox.Show("Файл пуст или данные не загружены", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                string tempFolder = Path.Combine(Path.GetTempPath(), "EgeClient_Temp");
                if (!Directory.Exists(tempFolder)) Directory.CreateDirectory(tempFolder);

                string filePath = Path.Combine(tempFolder, fileData.FileName);

                // Сохраняем байты во временный файл
                File.WriteAllBytes(filePath, fileData.Data);

                // Открываем файл системным приложением
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть файл: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}