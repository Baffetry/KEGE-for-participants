using KEGE_Participants.Models.Factory.UIElements_factories;

namespace KEGE_Participants.Models.Factory.UIElements
{
    public class TextBoxFactory : UIElementFactory
    {
        public override IProduct FactoryMethod()
        {
            return new CustomTextBox();
        }
    }
}
