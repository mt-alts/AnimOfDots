using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AnimOfDots
{
    public class Spiral : UserControl
    {
        private Image image;
        private Timer timer = new Timer();
        private List<Rectangle> rectangles = new List<Rectangle>();
        private int rotation, coordination = 2, bitmapSize, circleSize = 10, dotSize = 5;
        private bool isActivated = false;
        private const byte ANIMATION_MAX_SPEED = 101;

        private byte animationSpeed = 60;
        public byte AnimationSpeed
        {
            get { return animationSpeed; }
            set
            {
                if (value < ANIMATION_MAX_SPEED)
                {
                    animationSpeed = value;
                    timer.Interval = (byte)(ANIMATION_MAX_SPEED - value);
                }
                else
                {
                    throw new Exception("Error: Value cannot be greater than " + ANIMATION_MAX_SPEED);
                }
            }
        }

        private int dotCount = 15;
        public int DotCount
        {
            get { return dotCount; }
            set
            {
                dotCount = value;
                this.RectanglesReset();
                this.Invalidate();
            }
        }

        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                this.CreateEllipse();
                this.Invalidate();
            }
        }

        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (value)
                {
                    isEnabled = value;
                    this.Start();
                }
                else
                {
                    isEnabled = value;
                    this.Stop();
                    this.Visible = true;
                }
            }
        }


        public Spiral()
        {
            this.DoubleBuffered = true;
            timer.Tick += new EventHandler(this.timer_tick);
            bitmapSize = circleSize * 2;
            image = new Bitmap(bitmapSize, bitmapSize);
            this.Initialize();
        }

        private void Initialize()
        {
            this.RectanglesReset();
            this.CreateEllipse();
        }

        private void timer_tick(object sender, EventArgs e)
        {
            rotation = (rotation + 5) % 360;
            this.Invalidate();
        }

        private void IsActivated(bool value)
        {
            if (value)
            {
                this.Initialize();
            }

            rotation = 0;
            isActivated = value;
            timer.Enabled = value;
            this.Visible = value;
        }

        public void Start()
        {
            if (!isActivated)
            {
                this.IsActivated(true);
            }
        }

        public void Stop()
        {
            if (isActivated)
            {
                this.IsActivated(false);
            }
        }

        private void CreateEllipse()
        {
            using (Graphics graph = Graphics.FromImage(image))
            {
                graph.SmoothingMode = SmoothingMode.HighQuality;
                graph.Clear(Color.Transparent);
                graph.FillEllipse(new SolidBrush(this.ForeColor), new Rectangle(circleSize, circleSize, dotSize, dotSize));
            }
        }

        private void RectanglesReset()
        {
            coordination = 2;
            rectangles.RemoveRange(0, rectangles.Count);
            rectangles.Add(new Rectangle(this.Size.Width / 2, this.Size.Height / 2, image.Width, image.Height));
            this.CloneRectangle();
        }

        private void CloneRectangle()
        {
            for (int i = 0; i < dotCount - 1; i++)
            {
                rectangles.Add(new Rectangle(coordination, coordination, dotSize, dotSize));
                coordination = coordination + 2;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
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
