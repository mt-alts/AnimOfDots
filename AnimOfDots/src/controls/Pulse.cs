using System;
using System.Drawing;
using System.Windows.Forms;

namespace AnimOfDots {
    public class Pulse : AOD.BaseControl {

        private RectangleF rectF = new RectangleF();
        private float sizeChangeInterval = 0;
        private float dotSize = 40;
        private float colorAlpha = 0;
        private readonly SolidBrush solidBrush = new SolidBrush(Color.DodgerBlue);

        public Pulse() {
            DoubleBuffered = true;
            AnimationSpeedBalance(50);
            ForeColor = Color.DodgerBlue;
            Size = new Size(48, 48);
            Refresh();
        }

        protected override void Animate() {
            base.Animate();
            dotSize = (dotSize + sizeChangeInterval) % Height;
            colorAlpha = (colorAlpha + 8) % 256;
            rectF.X = (Width / 2) - (dotSize / 2);
            rectF.Y = (Height / 2) - (dotSize / 2);
            rectF.Width = dotSize;
            rectF.Height = dotSize;
            solidBrush.Color = Color.FromArgb((byte)(255 - colorAlpha), ForeColor);
            Refresh();
        }

        public override void Start() {
            base.Start();
            dotSize = 0;
            colorAlpha = 0;
        }

        protected override void OnForeColorChanged(EventArgs e) {
            base.OnForeColorChanged(e);
            solidBrush.Color = ForeColor;
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            sizeChangeInterval = Height / 32f;
            dotSize = (Height * 90) / 100;
            rectF = new RectangleF((Width / 2) - (dotSize / 2), (Height / 2) - (dotSize / 2), dotSize, dotSize);
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.FillEllipse(solidBrush, rectF);
        }
    }
}
