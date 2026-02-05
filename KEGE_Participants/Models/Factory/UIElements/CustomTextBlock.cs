using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KEGE_Participants.Models.Factory.UIElements
{
    public class CustomTextBlock : TextBlock, IProduct
    {
        public CustomTextBlock()
        {
            FontSize = 18;
            FontWeight = FontWeights.Bold;
            Foreground = Brushes.DimGray;
            VerticalAlignment = VerticalAlignment.Center;
            HorizontalAlignment = HorizontalAlignment.Center;
        }
    }
}
