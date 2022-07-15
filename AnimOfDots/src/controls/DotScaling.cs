using System;
using System.Drawing;
using System.Windows.Forms;

namespace AnimOfDots {
    public class DotScaling : AOD.BaseControl {

        private readonly RectangleF[] rects = new RectangleF[3];
        private readonly PointF[] dotsLocation = new PointF[3];
        private float maxDotSize = 0.0f;
        private float minDotSize = 0.0f;
        private readonly float[] dotsSize = new float[3];
        private readonly SolidBrush[] brushes = new SolidBrush[3] { new SolidBrush(Color.DodgerBlue), new SolidBrush(Color.Red), new SolidBrush(Color.Green) };
        private readonly bool[] isShrink = new bool[3] { false, false, false };

        public override Color ForeColor => Color.Transparent;

        public Color[] Colors {
            get {
                Color[] tempColors = new Color[brushes.Length];
                for (int i = 0; i < tempColors.Length; i++) {
                    tempColors[i] = brushes[i].Color;
                }
                return tempColors;
            }
            set {
                Color[] tempColors = value;
                if (tempColors.Length != brushes.Length) {
                    throw new IndexOutOfRangeException("The array should consist of 3 elements.");
                }
                for (int i = 0; i < tempColors.Length; i++) {
                    brushes[i].Color = tempColors[i];
                }
                Refresh();
            }
        }

        public DotScaling() {
            DoubleBuffered = true;
            AnimationSpeedBalance(50);
            Size = new Size(119, 22);
        }

        protected override void Animate() {
            base.Animate();
            for (int i = 0; i < isShrink.Length; i++) {
                if (isShrink[i] == false) {
                    if (dotsSize[i] < maxDotSize) {
                        dotsSize[i] = dotsSize[i] + 1;
                    } else {
                        isShrink[i] = true;
                    }
                } else if (isShrink[i]) {
                    if (dotsSize[i] > minDotSize) {
                        dotsSize[i] = dotsSize[i] - 1;
                    } else {
                        isShrink[i] = false;
                    }
                }
            }
            UpdateRectangles();
            Refresh();
        }

        private void UpdateDotsLocation() {
            /*
             *      DOT ALIGNMENT
             *      
             *  |    •    •    •    |
             *  %0  %25  %50  %75  %100
             */
            int count = 0;
            for (int i = 25; i <= 75; i = i + 25) {
                dotsLocation[count] = new PointF(AodMath.Percentage<float>(Width, i), (Height / 2));
                count += 1;
            }
        }

        private void UpdateRectangles() {
            float tempDotSize = 0.0f;
            for (int i = 0; i < rects.Length; i++) {
                tempDotSize = (dotsSize[i] / 2);
                rects[i].X = dotsLocation[i].X - tempDotSize;
                rects[i].Y = dotsLocation[i].Y - tempDotSize;
                rects[i].Width = dotsSize[i];
                rects[i].Height = dotsSize[i];
            }
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            maxDotSize = AodMath.Percentage<float>(Height, 70.0f);
            minDotSize = AodMath.Percentage<float>(Height, 5.0f);
            dotsSize[0] = AodMath.Percentage<int>(Height, 70);
            dotsSize[1] = AodMath.Percentage<int>(Height, 50);
            dotsSize[2] = AodMath.Percentage<int>(Height, 30);
            UpdateDotsLocation();
            UpdateRectangles();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            for (int i = 0; i < rects.Length; i++) {
                e.Graphics.FillEllipse(brushes[i], rects[i]);
            }
        }
    }
}
