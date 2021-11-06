using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AnimOfDots {
    public partial class ColorfulCircularLoading : UserControl {
        private Image image;
        private Timer timer = new Timer();
        private List<Rectangle> rectangles = new List<Rectangle>();
        private int colorListCount = 0;
        private int rotation;
        private int coordination = 0;
        private int circleSize = 10;
        private int dotSize = 5;
        private const int ANIMATION_MAX_SPEED = 100;

        private short animationSpeed = 60;
        public short AnimationSpeed {
            get { return animationSpeed; }
            set {
                if (value < ANIMATION_MAX_SPEED)
                    animationSpeed = value;
                else
                    throw new Exception("Error: Value cannot be greater than " + ANIMATION_MAX_SPEED);
            }
        }

        private Color[] colors = new Color[] {  Color.FromArgb(255, 0, 0),
                                                Color.FromArgb(118, 255, 3),
                                                Color.FromArgb(0, 229, 255),
                                                Color.FromArgb(255, 255, 0) };
        public Color[] Colors {
            get { return colors; }
            set {
                colors = value;
                FillRectangles();
                this.Refresh();
            }
        }

        public ColorfulCircularLoading() {
            InitializeComponent();

            this.Width = 48; this.Height = 48;
            image = new Bitmap(this.Width, this.Height);
        }

        private void ColorfulCircularLoading_Load(object sender, EventArgs e) {
            timer.Interval = ANIMATION_MAX_SPEED - animationSpeed;
            timer.Tick += new EventHandler(this.timer_tick);

            Configration();
            CalcCoordination();

            image = new Bitmap(circleSize * 2, circleSize * 2);

            rectangles.Add(new Rectangle(this.Width - ((this.Width / 2) + (CalcPercentage(this.Width, 5))), this.Height - ((this.Height / 2) + CalcPercentage(this.Height, 5)), image.Width, image.Height));
            CloneRectangle();

            colorListCount = -1;

            using (Graphics graph = Graphics.FromImage(image)) {
                graph.Clear(Color.Transparent);
                graph.SmoothingMode = SmoothingMode.HighQuality;
                graph.FillEllipse(new SolidBrush(GetColor()), new Rectangle(circleSize, circleSize, dotSize, dotSize));
            }
        }

        private void CloneRectangle() {
            for (int i = 0; i < 5; i++)
                rectangles.Add(new Rectangle(coordination, coordination, dotSize, dotSize));
        }

        private void Configration() {
            circleSize = this.Height / 4;
            dotSize = circleSize / 2;
        }

        private void CalcCoordination() {
            for (int i = this.Height; i >= 80; i -= 80)
                coordination = (coordination - 1);
        }

        private int CalcPercentage(int num, int percent) {
            return (num * percent) / 100;
        }

        private Color GetColor() {
            colorListCount = (colorListCount + 1) % Colors.Length;
            return Colors[colorListCount];
        }

        private void FillRectangles() {
            using (Graphics graph = Graphics.FromImage(image))
            {
                graph.FillEllipse(new SolidBrush(GetColor()), new Rectangle(circleSize, circleSize, dotSize, dotSize));
            }
        }

        private void timer_tick(object sender, EventArgs e) {
            rotation = (rotation + 5) % 360;
            if (rotation == 0)
                FillRectangles();
            this.Invalidate();
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
            colorListCount = -1;
            this.Invalidate();
        }

        private void ColorfulCircularLoading_Paint(object sender, PaintEventArgs e) {
            for (int i = 0; i < rectangles.Count; i++) {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.TranslateTransform(rectangles[i].Left + rectangles[i].Width / 10, rectangles[i].Top + rectangles[i].Height / 10);
                e.Graphics.RotateTransform(rotation);
                e.Graphics.DrawImage(image, new Point(-rectangles[i].Width / 10, rectangles[i].Height / 10));
            }
        }
    }
}
