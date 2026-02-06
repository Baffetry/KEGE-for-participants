using KEGE_Participants.User_Controls;
using System.Windows;

namespace KEGE_Participants.Models.Facade.Pages
{
    public class WorkedArea
    {
        private readonly SideWorkedControl _worked;

        public WorkedArea(SideWorkedControl worked)
        {
            _worked = worked;

            _worked._Timer.TimeUp += () =>
            {
                _worked.FinishExam(true);
            };
        }

        public void Collapsed()
        {
            _worked._Timer.Stop();
            _worked.Visibility = Visibility.Collapsed;
        }

        public void Visible()
        {
            StartTimer();
            _worked.Visibility = Visibility.Visible;
        }

        private void StartTimer()
        {
            int hours = int.Parse(App.GetResourceString("TimeLimit_hours"));
            int minutes = int.Parse(App.GetResourceString("TimeLimit_minutes"));
            int seconds = int.Parse(App.GetResourceString("TimeLimit_seconds"));

            _worked._Timer.Start(hours, minutes, seconds);
        }
    }
}
