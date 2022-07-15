using System;
using System.Drawing;
using System.Windows.Forms;

namespace AnimOfDots {
    public class DotFlashing : AOD.BaseControl {

        private Bitmap bitmapColorPalette;
        readonly RectangleF[] rects = new RectangleF[3];
        private float dotSize = 0;
        readonly SolidBrush[] solidBrushes = new SolidBrush[3];
        private int[] imagePixel;

        public override Color ForeColor => Color.Transparent;

        private Color primaryColor = Color.Orange;
        public Color PrimaryColor {
            get => primaryColor;
            set {
                primaryColor = value;
                ChangeColor();
            }
        }

        private Color secondaryColor = Color.Wheat;
        public Color SecondaryColor {
            get => secondaryColor;
            set {
                secondaryColor = value;
                ChangeColor();
            }
        }

        public DotFlashing() {
            DoubleBuffered = true;
            AnimationSpeedBalance(100);
            Size = new Size(48, 12);
            dotSize = Height - 1;
            UpdateRectangles();
            Reset();
            UpdateColors();
        }

        protected override void Animate() {
            base.Animate();
            for (int i = 0; i < imagePixel.Length; i++) {
                imagePixel[i] = (imagePixel[i] + 3) % bitmapColorPalette.Width;
            }
            UpdateColors();
            Refresh();
        }

        private void UpdateRectangles() {
            rects[1] = new RectangleF(new PointF((Width / 2) - (dotSize / 2), (Height / 2) - (dotSize / 2)), new SizeF(dotSize, dotSize));
            rects[0] = new RectangleF(new PointF(rects[1].X + (dotSize * 1.25f), rects[1].Y), new SizeF(dotSize, dotSize));
            rects[2] = new RectangleF(new PointF(rects[1].X - (dotSize * 1.25f), rects[1].Y), new SizeF(dotSize, dotSize));
        }

        private void ChangeColor() {
            CreateColorPalette();
            Reset();
            UpdateColors();
            Refresh();
        }

        private void UpdateColors() {
            for (int i = 0; i < solidBrushes.Length; i++) {
                solidBrushes[i].Color = bitmapColorPalette.GetPixel(imagePixel[i], 1);
            }
        }

        private void CreateColorPalette() {
            bitmapColorPalette = new ColorPalette(50, 2).CreateBlendedColorPalette(
                new ColorBlender(new Color[5] { SecondaryColor, PrimaryColor, PrimaryColor, SecondaryColor, SecondaryColor }, new float[5] { 0f, 0.3f, 0.5f, 0.7f, 1f }));
        }

        protected override void Reset() {
            base.Reset();
            CreateColorPalette();
            for (int i = 0; i < solidBrushes.Length; i++) {
                solidBrushes[i] = new SolidBrush(Color.Transparent);
            }
            imagePixel = new int[3] { 0, (int)(bitmapColorPalette.Width * 0.15f), (int)(bitmapColorPalette.Width * 0.30f) };
        }

        protected override void OnResize(EventArgs e) {
            dotSize = Height * 0.90f;
            UpdateRectangles();
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e) {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            for (int i = rects.Length - 1; i >= 0; i--) {
                e.Graphics.FillEllipse(solidBrushes[i], rects[i]);
            }
            base.OnPaint(e);
        }
    }
}