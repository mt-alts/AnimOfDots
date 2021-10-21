using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AnimOfDots.src
{
    public partial class SpiralLoading : UserControl
    {
        private Image image;
        private Timer timer;
        private List<Rectangle> rectangles = new List<Rectangle>();
        private int rotation;
        private int rotationStep = 5;
        private int coordination = 2;
        private int duration = 40;
        private int circleSize = 10;
        private int dotSize = 5;
        private int dotCount = 15;

        public int RotationStep
        {
            get { return rotationStep; }
            set { rotationStep = value; }
        }

        public int AnimationSpeed
        {
            get { return duration; }
            set { duration = value; }
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
        
        public int DotCount
        {
            get { return dotCount; }
            set { dotCount = value; }
        }

        public SpiralLoading()
        {
            InitializeComponent();
        }

        private void SpiralLoading_Load(object sender, EventArgs e)
        {
            timer = new Timer();
            timer.Interval = duration;
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
            for (int i = 0; i < dotCount - 1; i++)
            {
                rectangles.Add(new Rectangle(coordination, coordination, dotSize, dotSize));
                coordination = coordination + 2;
            }
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

        private void SpiralLoading_Paint(object sender, PaintEventArgs e)
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
