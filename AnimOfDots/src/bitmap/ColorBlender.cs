using System.Drawing;
using System.Drawing.Drawing2D;

namespace AnimOfDots {
    public class ColorBlender {

        private readonly Color[] colors;
        private readonly float[] positions;
        private readonly int angle = 360;

        public ColorBlender(Color[] colors, float[] positions) {
            this.colors = colors;
            this.positions = positions;
        }

        public Bitmap BlendBitmap(Bitmap bitmap) {
            using (Graphics graphics = Graphics.FromImage(bitmap)) {
                Rectangle rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                LinearGradientBrush gradientBrush = new LinearGradientBrush(rectangle, Color.Transparent, Color.Transparent, angle);
                ColorBlend colorBlend = new ColorBlend(colors.Length);
                colorBlend.Colors = colors;
                colorBlend.Positions = positions;
                gradientBrush.InterpolationColors = colorBlend;
                graphics.FillRectangle(gradientBrush, rectangle);
            }
            return bitmap;
        }
    }
}