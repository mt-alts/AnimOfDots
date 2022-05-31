using System;
using System.Drawing;
using System.Windows.Forms;

namespace AnimOfDots
{

    public class MultiplePulse : AOD.BaseControl
    {
        private readonly SolidBrush solidBrush = new SolidBrush(Color.DodgerBlue);
        private readonly RectangleF[] rects = new RectangleF[4];
        private readonly byte[] colorAlpha = new byte[4] { 255, 255, 255, 255 };
        private readonly byte[] colorOpacity = new byte[3];
        private readonly float[] dotSize = new float[4];
        private float expansionStep;

        public MultiplePulse()
        {
            DoubleBuffered = true;
            AnimationSpeedBalance(40);
            Size = new Size(48, 48);
        }

        protected override void Animate()
        {
            base.Animate();
            for (int i = 0; i < dotSize.Length - 1; i++)
            {
                dotSize[i] = (dotSize[i] + expansionStep) % Height;
            }

            for (int i = 0; i < colorOpacity.Length; i++)
            {
                colorOpacity[i] = (byte)((colorOpacity[i] + 5) % 256);
                colorAlpha[i] = (byte)(255 - colorOpacity[i]);
            }
            Refresh();
        }

        private void SetDotsSize()
        {
            dotSize[0] = Height - (Height / 3);
            dotSize[1] = Height / 2;
            dotSize[2] = Height / 3;
            dotSize[3] = Height * 0.1f;
        }

        private void SetDotsOpacity()
        {
            colorOpacity[0] = ((255 - (255 / 3))) - 1;
            colorOpacity[1] = (255 / 2) - 1;
            colorOpacity[2] = (255 / 3) - 1;
        }

        private void SetRectangles()
        {
            for (int i = 0; i < rects.Length; i++)
            {
                rects[i].X = (Width / 2) - (dotSize[i] / 2);
                rects[i].Y = (Height / 2) - (dotSize[i] / 2);
                rects[i].Width = dotSize[i];
                rects[i].Height = dotSize[i];

            }
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            solidBrush.Color = ForeColor;
            Refresh();
        }

        protected override void Reset()
        {
            base.Reset();
            SetDotsSize();
            SetDotsOpacity();
            expansionStep = Height / (256.0f / 5.0f);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Reset();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            SetRectangles();
            for (int i = 0; i < rects.Length; i++)
            {
                solidBrush.Color = Color.FromArgb(colorAlpha[i], ForeColor);
                e.Graphics.FillEllipse(solidBrush, rects[i]);
            }
        }
    }
}
