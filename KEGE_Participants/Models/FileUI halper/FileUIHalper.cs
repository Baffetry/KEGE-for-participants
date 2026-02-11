using Task_Data;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using KEGE_Participants.Models.Custom_brushes;
using KEGE_Participants.Models.Factory.UIElements;
using KEGE_Participants.Models.Factory.UIElements_factories;

namespace KEGE_Participants.Models.FileUI_halper
{
    public class FileUIHalper()
    {
        private static readonly UIElementFactory _imageFactory = new ImageFactory();
        private static readonly UIElementFactory _textBlockFactory = new TextBlockFactory();
        private static readonly UIElementFactory _borderFactory = new BorderFactory();

        public static UIElement CreateFileControl(FileData file, string iconPath, RoutedEventHandler onHyperlinkClick)
        {
            // Рамка 
            var rowBorder = (CustomBorder)_borderFactory.FactoryMethod();
            rowBorder.BorderThickness = new Thickness(0);
            rowBorder.BorderBrush = Brushes.Transparent;
            rowBorder.Margin = new Thickness(10, 5, 10, 5);
            rowBorder.Cursor = Cursors.Hand;
            rowBorder.Tag = file;

            var panel = new StackPanel { Orientation = Orientation.Vertical };

            // Иконка
            var icon = (CustomImage)_imageFactory.FactoryMethod();
            icon.Source = new BitmapImage(new Uri(iconPath, UriKind.RelativeOrAbsolute));
            icon.HorizontalAlignment = HorizontalAlignment.Center;
            icon.VerticalAlignment = VerticalAlignment.Center;

            // Текст-ссылка
            var textBlock = (CustomTextBlock)_textBlockFactory.FactoryMethod();
            textBlock.FontSize = 22;
            textBlock.FontWeight = FontWeights.SemiBold;
            textBlock.Margin = new Thickness(0, 10, 0, 0);
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.FontFamily = new FontFamily("/Resources/Fonts/#Inter");

            // Гиперссылка
            var run = new Run(file.FileName);
            var hyperlink = new Hyperlink(run)
            {
                Tag = file,
                Foreground = CustomBrusher.LightBlue,
                TextDecorations = null
            };
            hyperlink.Click += onHyperlinkClick;
            
            textBlock.Inlines.Add(hyperlink);

            panel.Children.Add(icon);
            panel.Children.Add(textBlock);
            rowBorder.Child = panel;

            return rowBorder;
        }
    }
}
