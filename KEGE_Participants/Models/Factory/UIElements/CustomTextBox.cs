using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KEGE_Participants.Models.Factory.UIElements
{
    public class CustomTextBox : TextBox, IProduct
    {
        public CustomTextBox() 
        {
            FontSize = 18;
            FontWeight = FontWeights.Medium;
            VerticalContentAlignment = VerticalAlignment.Center;
            HorizontalContentAlignment = HorizontalAlignment.Left;
            BorderThickness = new Thickness(0);
            Margin = new Thickness(5, 0, 0, 0);
            Background = Brushes.Transparent;
            MaxLength = 10;
        }
    }
}
