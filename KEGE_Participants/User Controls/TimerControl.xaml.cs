using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Threading;

namespace KEGE_Participants.User_Controls
{
    /// <summary>
    /// Interaction logic for TimerControl.xaml
    /// </summary>
    public partial class TimerControl : UserControl
    {
        public event Action TimeUp;

        private DispatcherTimer _timer;
        private TimeSpan _time;

        public TimerControl()
        {
            InitializeComponent();
        }

        public void Start(int hours, int minutes, int seconds)
        {
            _time = new TimeSpan(hours, minutes, seconds);

            Timer_TextBlock.Text = _time.ToString(@"hh\:mm\:ss");

            if (_timer != null) _timer.Stop();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) =>
            {
                if (_time.TotalSeconds > 0)
                {
                    _time = _time.Subtract(TimeSpan.FromSeconds(1));

                    Timer_TextBlock.Text = _time.ToString(@"hh\:mm\:ss");

                    if (_time.TotalMinutes < 5)
                        Timer_TextBlock.Foreground = (Brush)new BrushConverter().ConvertFrom("#E63946");
                    else if (_time.TotalMinutes < 1)
                        Timer_TextBlock.Foreground = (Brush)new BrushConverter().ConvertFrom("9D1C24");
                }
                else
                {
                    _timer.Stop();
                    MessageBox.Show("Время вышло!");
                    TimeUp?.Invoke();
                }
            };
            _timer.Start();
        }

        public void Stop()
        {
            _timer?.Stop();
        }
    }
}
