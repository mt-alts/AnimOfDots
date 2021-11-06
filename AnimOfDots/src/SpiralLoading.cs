using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AnimOfDots {
    public partial class SpiralLoading : UserControl {
        private Image image;
        private Timer timer = new Timer();
        private List<Rectangle> rectangles = new List<Rectangle>();
        private int rotation;
        private int coordination = 2;
        private int bitmapSize;
        private int circleSize = 10;
        private int dotSize = 5;
        private const int animationMaxSpeed = 100;

        private short animationSpeed = 60;
        public short AnimationSpeed {
            get { return animationSpeed; }
            set {
                if (value < animationMaxSpeed)
                    animationSpeed = value;
                else
                    throw new Exception("Error: Value cannot be greater than " + animationMaxSpeed);
            }
        }

        private int dotCount = 15;
        public int DotCount {
            get { return dotCount; }
            set {
                dotCount = value;
                RectRefresh();
                this.Invalidate();
            }
        }

        public override Color ForeColor { get; set; } = Color.DodgerBlue;

        public SpiralLoading() {
            InitializeComponent();

            bitmapSize = circleSize * 2;
            image = new Bitmap(bitmapSize, bitmapSize);
        }

        private void SpiralLoading_Load(object sender, EventArgs e) {
            timer.Interval = animationMaxSpeed - animationSpeed;
            timer.Tick += new EventHandler(this.timer_tick);

            bitmapSize = circleSize * 2;
            image = new Bitmap(bitmapSize, bitmapSize);

            RectRefresh();

            using (Graphics graph = Graphics.FromImage(image)) {
                graph.Clear(Color.Transparent);
                graph.SmoothingMode = SmoothingMode.HighQuality;
                graph.FillEllipse(new SolidBrush(this.ForeColor), new Rectangle(circleSize, circleSize, dotSize, dotSize));
            }
        }

        private void RectRefresh() {
            coordination = 2;
            rectangles.RemoveRange(0, rectangles.Count);
            rectangles.Add(new Rectangle(this.Size.Width / 2, this.Size.Height / 2, image.Width, image.Height));
            CloneRectangle();
        }

        private void CloneRectangle() {
            for (int i = 0; i < dotCount - 1; i++) {
                rectangles.Add(new Rectangle(coordination, coordination, dotSize, dotSize));
                coordination = coordination + 2;
            }
        }

        public void Play() {
            timer.Enabled = true;
        }

        public void Pause() {
            timer.Enabled = false;
        }

        public void Stop() {
            timer.Enabled = false;
            rotation = 0;
            this.Invalidate();
        }

        private void timer_tick(object sender, EventArgs e) {
            rotation = (rotation + 5) % 360;
            this.Invalidate();
        }

        private void SpiralLoading_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < rectangles.Count; i++) {
                e.Graphics.TranslateTransform(rectangles[i].Left + rectangles[i].Width / 10, rectangles[i].Top + rectangles[i].Height / 10);
                e.Graphics.RotateTransform(rotation);
                e.Graphics.DrawImage(image, new Point(-rectangles[i].Width / 10, rectangles[i].Height / 10));
            }
        }
    }
}
