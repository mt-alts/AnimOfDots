using System.Drawing;

namespace AnimOfDots {
    public class ColorPalette {

        private readonly Bitmap bitmap;

        public ColorPalette(Size size) {
            bitmap = new Bitmap(size.Width, size.Height);
        }

        public ColorPalette(int width, int height) {
            bitmap = new Bitmap(width, height);
        }

        public Bitmap CreateBlendedColorPalette(ColorBlender colorBlender) {
            return colorBlender.BlendBitmap(bitmap);
        }
    }
}