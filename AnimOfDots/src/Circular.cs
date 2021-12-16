using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AnimOfDots
{
    public class Circular : UserControl
    {
        private Image image;
        private Timer timer = new Timer();
        private List<Rectangle> rectangles = new List<Rectangle>();
        private int rotation, coordination = 0, circleSize = 10, dotSize = 5;
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
                    timer.Interval = ANIMATION_MAX_SPEED - value;
                }
                else
                {
                    throw new Exception("Error: Value cannot be greater than " + ANIMATION_MAX_SPEED);
                }
            }
        }

        private bool isEnabled = false;
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
                    rotation = 0;
                    this.Stop();
                    this.Visible = true;
                }
            }
        }

        private Color foreColor = Color.DodgerBlue;
        public override Color ForeColor
        {
            get { return foreColor; }
            set
            {
                foreColor = value;
                this.CreateEllipse();
                this.Invalidate();
            }
        }

        public Circular()
        {
            this.DoubleBuffered = true;

            timer.Tick += new EventHandler(this.timer_tick);

            this.Size = new Size(48, 48);

            this.Initialize();

        }

        private void Initialize()
        {
            coordination = 0;

            this.Configration();
            this.CalcCoordination();

            image = new Bitmap(circleSize * 2, circleSize * 2);

            rectangles.Clear();
            rectangles.Add(new Rectangle(this.Width - ((this.Width / 2) + (CalcPercentage(this.Width, 5))), this.Height - ((this.Height / 2) + CalcPercentage(this.Height, 5)), image.Width, image.Height));
            this.CloneRectangle();

            this.CreateEllipse();
        }

        private void CreateEllipse()
        {
            using (Graphics graph = Graphics.FromImage(image))
            {
                graph.SmoothingMode = SmoothingMode.HighQuality;
                graph.Clear(Color.Transparent);
                graph.FillEllipse(new SolidBrush(foreColor), new Rectangle(circleSize, circleSize, dotSize, dotSize));
            }
        }

        private void CloneRectangle()
        {
            for (int i = 0; i < 5; i++)
            {
                rectangles.Add(new Rectangle(coordination, coordination, dotSize, dotSize));
            }
        }

        private void Configration()
        {
            circleSize = this.Height / 4;
            dotSize = circleSize / 2;
        }

        private void CalcCoordination()
        {
            for (int i = this.Height; i >= 80; i -= 80)
            {
                coordination = (coordination - 1);
            }
        }

        private int CalcPercentage(int num, int percent)
        {
            return (num * percent) / 100;
        }

        private void IsActivated(bool value)
        {
            if (value)
            {
                this.Initialize();
            }
            timer.Enabled = value;
            this.Visible = value;
            isActivated = value;
            isEnabled = value;
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

        private void timer_tick(object sender, EventArgs e)
        {
            rotation = (rotation + 5) % 360;
            this.Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            this.Initialize();
            this.Invalidate();
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            for (int i = 0; i < rectangles.Count; i++)
            {
                e.Graphics.TranslateTransform(rectangles[i].Left + rectangles[i].Width / 10, rectangles[i].Top + rectangles[i].Height / 10);
                e.Graphics.RotateTransform(rotation);
                e.Graphics.DrawImage(image, new Point(-rectangles[i].Width / 10, rectangles[i].Height / 10));
            }
            base.OnPaint(e);
        }
    }
}
