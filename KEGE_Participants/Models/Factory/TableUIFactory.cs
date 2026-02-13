using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using KEGE_Participants.Models.Custom_brushes;
using KEGE_Participants.Models.Factory.UIElements;
using KEGE_Participants.Models.Factory.UIElements_factories;

namespace KEGE_Participants.Models.Factory
{
    public class TableUIFactory(TextChangedEventHandler onTextChanged, KeyEventHandler onKeyDown)
    {
        private readonly UIElementFactory _borderFactory = new BorderFactory();
        private readonly UIElementFactory _textBlockFactory = new TextBlockFactory();
        private readonly UIElementFactory _textBoxFactory = new TextBoxFactory();

        private readonly TextChangedEventHandler _onTextChanged = onTextChanged;
        private readonly KeyEventHandler _onPreviewKeyDown = onKeyDown;

        public Border CreateCell(int row, int col, int totalRows)
        {
            var cellBorder = (CustomBorder)_borderFactory.FactoryMethod();
            cellBorder.BorderThickness = new Thickness(0, 0, (col < 2 ? 1 : 0), (row < totalRows - 1 ? 1 : 0));

            if (col == 0)
            {
                var textBlock = (CustomTextBlock)_textBlockFactory.FactoryMethod();
                if (textBlock != null) textBlock.Text = (row + 1).ToString();

                cellBorder.Background = CustomBrusher.LightGray;
                cellBorder.Child = textBlock;
            }
            else
            {
                var textBox = (CustomTextBox)_textBoxFactory.FactoryMethod();

                textBox.TextChanged += _onTextChanged;
                textBox.PreviewKeyDown += _onPreviewKeyDown;

                cellBorder.Child = textBox;
            }

            return cellBorder;
        }
    }
}
