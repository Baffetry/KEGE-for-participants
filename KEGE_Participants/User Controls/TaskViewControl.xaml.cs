using System.IO;
using System.Windows;
using System.Windows.Controls;
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
        public string TaskId { get; set; }

        private TaskData _data;

        public string ParticipantAnswer { get; set; }

        public TaskViewControl(TaskData data)
        {
            InitializeComponent();
            _data = data;

            SetImage(_data.Image);
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
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Ошибка загрузки изображения");
                return;
            }
        }

        public void ResetUnsavedChanges()
        {
            Answer_TextBox.Text = ParticipantAnswer ?? string.Empty;

            if (string.IsNullOrEmpty(ParticipantAnswer))
                Save_btn.Background = (Brush)new BrushConverter().ConvertFrom("#FFFFFF");
        }

        private void Save_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Answer_TextBox.Text))
            {
                ParticipantAnswer = Answer_TextBox.Text;
                Save_btn.Background = (Brush)new BrushConverter().ConvertFrom("#66D9FF");
                AnswerSaved?.Invoke(TaskId);
            }
        }

        private void Answer_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Save_btn == null) return;

            ParticipantAnswer = string.Empty;
            Save_btn.Background = (Brush)new BrushConverter().ConvertFrom("#FFFFFF");
            AnswerChanged?.Invoke(TaskId);
        }
    }
}
