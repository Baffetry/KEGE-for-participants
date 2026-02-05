using System.Windows.Controls;
using System.Windows.Media;
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
            else
            {
                ParticipantAnswer = string.Empty;
                Save_btn.Background = (Brush)new BrushConverter().ConvertFrom("#FFFFFF");
                AnswerChanged?.Invoke(TaskId);
            }
        }

        private void Answer_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Save_btn == null) return;

            var skyBlue = (Brush)new BrushConverter().ConvertFrom("#66D9FF");

            ParticipantAnswer = string.Empty;
            Save_btn.Background = (Brush)new BrushConverter().ConvertFrom("#FFFFFF");
            AnswerChanged?.Invoke(TaskId);

            //if (Save_btn.Background.ToString() == skyBlue.ToString())
            //{
            //    Save_btn.Background = Brushes.White;
            //    AnswerChanged?.Invoke(TaskId);
            //}
            //var skyBlue = ColorConverter.ConvertFromString("#66D9FF");

            //var btn = (Button)sender;

            //if (btn.Background.Equals(skyBlue))
            //    Save_btn.Background = (Brush)ColorConverter.ConvertFromString("#FFFFFF");
        }
    }
}
