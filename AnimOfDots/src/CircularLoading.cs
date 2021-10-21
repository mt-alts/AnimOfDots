using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AnimOfDots.src
{
    public partial class CircularLoading : UserControl
    {
        private Image image;
        private Timer timer;
        private List<Rectangle> rectangles = new List<Rectangle>();
        private int rotation;
        private int rotationStep = 5;
        private int coordination = 0;
        private int animationSpeed = 120;
        private int circleSize = 10;
        private int dotSize = 5;

        public int RotationStep
        {
            get { return rotationStep; }
            set { rotationStep = value; }
        }

        public int Coordination
        {
            get { return coordination; }
            set { coordination = value; }
        }

        public int AnimationSpeed
        {
            get { return animationSpeed; }
            set { animationSpeed = value; }
        }

        public int CircleSize
        {
            get { return circleSize; }
            set { circleSize = value; }
        }

        public int DotSize
        {
            get { return dotSize; }
            set { dotSize = value; }
        }

        public CircularLoading()
        {
            InitializeComponent();
        }

        private void CircularLoading_Load(object sender, EventArgs e)
        {
            timer = new Timer();
            timer.Interval = animationSpeed;
            timer.Tick += new EventHandler(this.timer_tick);

            image = new Bitmap(circleSize * 2, circleSize * 2);

            rectangles.Add(new Rectangle(this.Size.Width / 2, this.Size.Height / 2, image.Width, image.Height));
            CloneRectangle();

            using (Graphics graph = Graphics.FromImage(image))
            {
                graph.Clear(Color.Transparent);
                graph.SmoothingMode = SmoothingMode.HighQuality;
                graph.FillEllipse(new SolidBrush(this.ForeColor), new Rectangle(circleSize, circleSize, dotSize, dotSize));
            }
        }

        private void CloneRectangle()
        {
            for (int i = 0; i < 5; i++)
                rectangles.Add(new Rectangle(coordination, coordination, dotSize, dotSize));
        }

        public void Start()
        {
            timer.Enabled = true;
        }

        public void Pause()
        {
            timer.Enabled = false;
        }

        private void timer_tick(object sender, EventArgs e)
        {
            rotation = (rotation + rotationStep) % 360;
            this.Invalidate();
        }

        private void CircularLoading_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < rectangles.Count; i++)
            {
                e.Graphics.TranslateTransform(rectangles[i].Left + rectangles[i].Width / 10, rectangles[i].Top + rectangles[i].Height / 10);
                e.Graphics.RotateTransform(rotation);
                e.Graphics.DrawImage(image, new Point(-rectangles[i].Width / 10, rectangles[i].Height / 10));
            }
        }
    }
}
