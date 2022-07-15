using System;
using System.Drawing;
using System.Windows.Forms;

namespace AnimOfDots {
    public class Overlay : AOD.BaseControl {

        private Bitmap bitmapColorPalette = null;
        private Bitmap bmp;
        private readonly SolidBrush[] solidBrushes = new SolidBrush[8];
        private readonly RectangleF[] rects = new RectangleF[8];
        private readonly Color[] colorPixelArray = new Color[8];
        private float dotSize = 24;
        private readonly PointF[] pf = new PointF[8];
        private float sizeW = 0, sizeH = 0;
        private int[] imagePixel;

        public Color[] colors = new Color[3] { Color.DodgerBlue,
                                               Color.FromArgb(100, Color.DeepSkyBlue),
                                               Color.FromArgb(0, Color.LightSkyBlue) };
        public Color[] Colors {
            get => colors;
            set {
                if (colors.Length == value.Length) {
                    colors = value;
                    CreateColorPalette();
                    UpdateColors();
                    Refresh();
                } else {
                    throw new Exception("Array must have three elements");
                }
            }
        }

        public override Color ForeColor => base.ForeColor;

        public Overlay() {
            DoubleBuffered = true;
            AnimationSpeedBalance(100);
            for (int i = 0; i < solidBrushes.Length; i++) {
                solidBrushes[i] = new SolidBrush(Color.DodgerBlue);
            }
            Size = new Size(48, 48);
        }

        protected override void Animate() {
            base.Animate();
            for (int i = 0; i < imagePixel.Length; i++) {
                imagePixel[i] = (imagePixel[i] + 1) % bitmapColorPalette.Width;
                colorPixelArray[i] = bitmapColorPalette.GetPixel(imagePixel[i], 1);
                solidBrushes[i].Color = colorPixelArray[i];
            }
            FillBitmap();
            Refresh();
        }

        private void FillBitmap() {
            using (Graphics graphics = Graphics.FromImage(bmp)) {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.Clear(Color.Transparent);
                for (int i = 0; i < solidBrushes.Length; i++) {
                    graphics.FillEllipse(solidBrushes[i], rects[i]);
                }
            }
        }

        private void UpdateScale() {
            dotSize = Height / 4.5f;
            sizeW = Width - dotSize - 1;
            sizeH = Height - dotSize - 1;
        }

        private void UpdatePoints() {
            UpdateScale();
            pf[0] = new PointF(sizeW / 2, 0);
            pf[2] = new PointF(sizeW, sizeH / 2);
            pf[4] = new PointF(sizeW / 2, sizeH);
            pf[6] = new PointF(0, sizeH / 2);
            pf[1] = new PointF((sizeW - pf[0].X / 3), (pf[2].Y / 3));
            pf[3] = new PointF(sizeW - pf[4].X / 3, sizeH - pf[2].Y / 3);
            pf[5] = new PointF(pf[4].X / 3, sizeH - (pf[6].Y / 3));
            pf[7] = new PointF(pf[0].X / 3, pf[6].Y / 3);
        }

        private void CreateColorPalette() {
            bitmapColorPalette = new ColorPalette(8, 2).CreateBlendedColorPalette(
                new ColorBlender(colors, new float[3] { 0f, 0.5f, 1f }));
        }

        private void UpdateColors() {
            if (bitmapColorPalette == null) {
                CreateColorPalette();
            }
            for (int i = 0; i < colorPixelArray.Length; i++) {
                colorPixelArray[i] = bitmapColorPalette.GetPixel(imagePixel[i], 1);
                solidBrushes[i].Color = colorPixelArray[i];
            }
            FillBitmap();
            Refresh();
        }

        private void UpdateRectangles() {
            for (int i = 0; i < rects.Length; i++) {
                rects[i] = new RectangleF(pf[i].X, pf[i].Y, dotSize, dotSize);
            }
        }

        protected override void Reset() {
            base.Reset();
            bmp = new Bitmap(Width, Height);
            imagePixel = new int[8] { 7, 6, 5, 4, 3, 2, 1, 0 };
            UpdatePoints();
            UpdateRectangles();
            UpdateColors();
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            Reset();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(bmp, 0, 0);
        }
    }
}