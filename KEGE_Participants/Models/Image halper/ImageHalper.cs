using System.IO;
using System.Windows.Media.Imaging;

namespace KEGE_Participants.Models.Image_halper
{
    public class ImageHalper
    {
        public static BitmapImage LoadImage(byte[] imageBytes)
        {
            if (imageBytes is null || imageBytes.Length == 0) return null;

            var bitmap = new BitmapImage();

            using (var ms = new MemoryStream(imageBytes))
            {
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
            }

            bitmap.Freeze();
            return bitmap;
        }
    }
}
