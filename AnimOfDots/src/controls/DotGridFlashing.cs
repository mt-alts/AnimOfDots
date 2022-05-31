using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AnimOfDots
{
    public class DotGridFlashing : AOD.BaseControl
    {
        private Bitmap bitmapColorPalette = null;
        private byte colorAlpha = 150;
        private float dotsSize = 0;
        private readonly PointF[] pointF = new PointF[9];
        private readonly SolidBrush[] solidBrush = new SolidBrush[9];
        private readonly int[] solidBrushCountList = new int[9];
        private readonly RectangleF[] rectsF = new RectangleF[9];
        private readonly Random random = new Random();
        private readonly Color[] colors = new Color[3] { Color.SkyBlue, Color.SkyBlue, Color.DeepSkyBlue };

        public byte ColorAlpha
        {
            get => colorAlpha;
            set
            {
                colorAlpha = value;
                CreateColorPalettte();
                SetColors();
                Refresh();
            }
        }

        public DotGridFlashing()
        {
            DoubleBuffered = true;
            AnimationSpeedBalance(50);
            Size = new Size(100, 100);

            solidBrushCountList[0] = 0;
            solidBrushCountList[1] = 5;
            solidBrushCountList[2] = 1;
            solidBrushCountList[3] = 6;
            solidBrushCountList[4] = 2;
            solidBrushCountList[5] = 7;
            solidBrushCountList[6] = 3;
            solidBrushCountList[7] = 8;
            solidBrushCountList[8] = 4;


            for (int i = 0; i < solidBrush.Length; i++)
            {
                solidBrush[i] = new SolidBrush(colors[random.Next(0, 3)]);
            }
            SetPoints();
            ForeColor = Color.DodgerBlue;
            CreateColorPalettte();
        }

        protected override void Animate()
        {
            base.Animate();
            SetColors();
            Refresh();
        }

        private void SetColors()
        {
            for (int i = 0; i < solidBrushCountList.Length; i++)
            {
                solidBrushCountList[i] = (solidBrushCountList[i] + 1) % 10;
                solidBrush[i].Color = bitmapColorPalette.GetPixel(solidBrushCountList[i], 1);
            }
        }

        private void CreateColorPalettte()
        {
            bitmapColorPalette = new ColorPalette(10, 2).CreateBlendedColorPalette(
                new ColorBlender(new Color[3] { Color.FromArgb(colorAlpha, ForeColor), ForeColor, Color.FromArgb(colorAlpha, ForeColor) },
                new float[3] { 0.0f, 0.5f, 1.0f }));
        }

        private void SetPoints()
        {
            pointF[0] = new PointF(((Width / 3) / 2), ((Height / 3) / 2));
            pointF[1] = new PointF((Width / 2), pointF[0].Y);
            pointF[2] = new PointF(((Width - (Width / 3)) + pointF[0].X), pointF[0].Y);
            pointF[3] = new PointF(pointF[0].X, (Height / 2));
            pointF[4] = new PointF(pointF[1].X, pointF[3].Y);
            pointF[5] = new PointF(pointF[2].X, pointF[3].Y);
            pointF[6] = new PointF(pointF[0].X, ((Height - (Height / 3)) + pointF[0].Y));
            pointF[7] = new PointF(pointF[1].X, pointF[6].Y);
            pointF[8] = new PointF(pointF[2].X, pointF[6].Y);
        }

        private void SetRectangles()
        {
            for (int i = 0; i < rectsF.Length; i++)
            {
                rectsF[i] = new RectangleF(pointF[i].X - (dotsSize / 2), pointF[i].Y - (dotsSize / 2), dotsSize, dotsSize);
            }
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            CreateColorPalettte();
            SetColors();
            Refresh();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            dotsSize = Height * 0.2f;
            SetPoints();
            SetRectangles();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            for (int i = 0; i < rectsF.Length; i++)
            {
                e.Graphics.FillEllipse(solidBrush[i], rectsF[i]);
            }
            base.OnPaint(e);
        }
    }
}
