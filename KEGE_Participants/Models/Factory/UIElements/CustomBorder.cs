using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KEGE_Participants.Models.Factory.UIElements
{
    public class CustomBorder : Border, IProduct
    {
        public CustomBorder()
        {
            BorderBrush = (Brush)new BrushConverter().ConvertFrom("#CCCCCC");
            BorderThickness = new Thickness(0.5);
            SnapsToDevicePixels = true;
        }
    }
}
