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
        private TaskData _data;

        public string ParticipantAnswer { get; set; }

        public TaskViewControl(TaskData data)
        {
            InitializeComponent();
            _data = data;
        }

        private void Save_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (!string.IsNullOrEmpty(Answer_TextBox.Text))
            {
                if (!btn.Background.Equals(Brushes.DeepSkyBlue))
                    Save_btn.Background = Brushes.DeepSkyBlue;

                ParticipantAnswer = Answer_TextBox.Text;
            }
            else
            {
                Save_btn.Background = Brushes.White;
            }
        }
    }
}
