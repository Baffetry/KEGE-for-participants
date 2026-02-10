using System.Windows.Controls;
using System.Windows;


namespace KEGE_Participants.Models.Factory.UIElements
{
    public class CustomImage : Image, IProduct
    {
        public CustomImage()
        {
            Width = 60;
            Height = 60;
            Margin = new Thickness(0, 0, 10, 0);
            VerticalAlignment = VerticalAlignment.Center;
            HorizontalAlignment = HorizontalAlignment.Center;
        }
    }
}
