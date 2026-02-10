using KEGE_Participants.Models.Factory.UIElements;

namespace KEGE_Participants.Models.Factory.UIElements_factories
{
    public class ImageFactory : UIElementFactory
    {
        public override IProduct FactoryMethod()
        {
            return new CustomImage();
        }
    }
}
