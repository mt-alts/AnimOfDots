using System;
using System.Drawing;
using System.Windows.Forms;

namespace AnimOfDots {
    public class DoubleDotSpin : AOD.BaseControl {

        private Bitmap bitmap;
        private float dotSize = 0f;
        private readonly PointF[] pf = new PointF[2];
        private float sizeW = 0f;
        private float sizeH = 0f;
        private readonly RectangleF[] rectF = new RectangleF[2];
        private int angle = 0;

        private SolidBrush primaryColor = new SolidBrush(Color.DodgerBlue);
        public Color PrimaryColor {
            get { return primaryColor.Color; }
            set {
                primaryColor.Color = value;
                FillEllipse();
            }
        }

        private SolidBrush secondaryColor = new SolidBrush(Color.Orange);
        public Color SecondaryColor {
            get { return secondaryColor.Color; }
            set {
                secondaryColor.Color = value;
                FillEllipse();
            }
        }

        public DoubleDotSpin() {
            DoubleBuffered = true;
            AnimationSpeedBalance(40);
            Size = new Size(36, 36);
            PrimaryColor = Color.DodgerBlue;
        }

        protected override void Animate() {
            base.Animate();
            angle = (angle + 10) % 360;
            Refresh();
        }

        private void FillEllipse() {
            using (Graphics graphics = Graphics.FromImage(bitmap)) {
                graphics.Clear(BackColor);
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.FillEllipse(primaryColor, rectF[0]);
                graphics.FillEllipse(secondaryColor, rectF[1]);
            }
            Refresh();
        }

        protected override void Reset() {
            base.Reset();
            angle = 0;
        }

        protected override void OnForeColorChanged(EventArgs e) {
            base.OnForeColorChanged(e);
            PrimaryColor = ForeColor;
            SecondaryColor = ForeColor;
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            bitmap = new Bitmap(Width, Height);
            dotSize = Height / 4.5f;
            sizeW = Width - dotSize;
            sizeH = Height - dotSize;
            pf[0] = new PointF(sizeW / 2, 1);
            pf[1] = new PointF(sizeW / 2, sizeH - 1);
            rectF[0] = new RectangleF(pf[0].X, pf[0].Y, dotSize, dotSize);
            rectF[1] = new RectangleF(pf[1].X, pf[1].Y, dotSize, dotSize);
            FillEllipse();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            e.Graphics.TranslateTransform((float)bitmap.Width / 2, (float)bitmap.Height / 2);
            e.Graphics.RotateTransform(angle);
            e.Graphics.TranslateTransform(-(float)bitmap.Width / 2, -(float)bitmap.Height / 2);
            e.Graphics.DrawImage(bitmap, 0, 0);
        }
    }
}
