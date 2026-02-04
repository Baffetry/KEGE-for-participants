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
        }

        public void Collapsed()
        {
            _worked.Visibility = Visibility.Collapsed;
        }

        public void Visible()
        {
            _worked.Visibility = Visibility.Visible;
        }
    }
}
