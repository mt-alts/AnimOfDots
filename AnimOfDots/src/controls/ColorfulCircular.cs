using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AnimOfDots {
    public class ColorfulCircular : AOD.BaseControl {

        private Bitmap bmp;
        private readonly SolidBrush solidBrush = new SolidBrush(Color.Transparent);
        private RectangleF rectF = new RectangleF();
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private int colorListCount = 0, rotation, coordination = 0, circleSize = 10, dotSize = 5;

        private Color[] colors = new Color[] {  Color.Red, Color.DodgerBlue, Color.LimeGreen, Color.Gold };
        public Color[] Colors {
            get => colors;
            set {
                colors = value;
                FillEllipse();
                Refresh();
            }
        }

        public ColorfulCircular() {
            DoubleBuffered = true;
            AnimationSpeedBalance(40);
            Size = new Size(48, 48);
        }

        protected override void Animate() {
            base.Animate();
            rotation = (rotation + 5) % 360;
            if (rotation == 0) {
                FillEllipse();
            }
            Refresh();
        }

        private void CloneRectangle() {
            for (int i = 0; i < 5; i++) {
                rectangles.Add(new Rectangle(coordination, coordination, dotSize, dotSize));
            }
        }

        private void Configration() {
            circleSize = (int)((Height - 1) / 4.1f);
            dotSize = circleSize / 2;
        }

        private void CalcCoordination()
        {
            coordination = 0;
            for (int i = Height; i >= 80; i -= 80) {
                coordination = (coordination - 1);
            }
        }

        private Color GetColorFromColorList() {
            colorListCount = (colorListCount + 1) % Colors.Length;
            return Colors[colorListCount];
        }

        private void FillEllipse() {
            using (Graphics graph = Graphics.FromImage(bmp)) {
                graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graph.Clear(Color.Transparent);
                solidBrush.Color = GetColorFromColorList();
                rectF.X = circleSize;
                rectF.Y = circleSize;
                rectF.Width = dotSize;
                rectF.Height = dotSize;
                graph.FillEllipse(solidBrush, rectF);
            }
        }

        protected override void Reset() {
            base.Reset();
            colorListCount = -1;
            rotation = 0;
            FillEllipse();
            Refresh();
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            Configration();
            CalcCoordination();
            bmp = new Bitmap(circleSize * 2, circleSize * 2);
            rectangles.Clear();
            rectangles.Add(new Rectangle(Width - ((Width / 2) + (AodMath.Percentage<int>(Width, 5))), Height - ((Height / 2) + AodMath.Percentage<int>(Height, 5)), bmp.Width, bmp.Height));
            CloneRectangle();
            colorListCount = -1;
            FillEllipse();
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            for (int i = 0; i < rectangles.Count; i++) {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                e.Graphics.TranslateTransform(rectangles[i].Left + rectangles[i].Width / 10, rectangles[i].Top + rectangles[i].Height / 10);
                e.Graphics.RotateTransform(rotation);
                e.Graphics.DrawImage(bmp, -rectangles[i].Width / 10, rectangles[i].Height / 10);
            }
        }
    }
}
