using System;
using System.Drawing;
using System.Windows.Forms;

namespace AnimOfDots
{
    public class BouncingDots : UserControl
    {
        private Timer timer = new Timer();
        private RectangleF[] rects = new RectangleF[3];
        private int dotSize = 24, count = 0;
        private int[] rectY = new int[3] { 0, 0, 0 };
        private bool isActivated = false;
        private const byte ANIMATION_MAX_SPEED = 101;

        private byte animationSpeed = 50;
        public byte AnimationSpeed
        {
            get { return animationSpeed; }
            set
            {
                if (value < ANIMATION_MAX_SPEED)
                {
                    timer.Interval = (ANIMATION_MAX_SPEED * 2) - (value * 2);
                    animationSpeed = value;
                }
                else
                {
                    throw new Exception("Error: Value cannot be greater than " + ANIMATION_MAX_SPEED);
                }
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
                    this.Reset();
                    this.SetRectangles();
                    this.Visible = true;
                    this.Invalidate();
                }
            }
        }

        public BouncingDots()
        {
            this.DoubleBuffered = true;
            this.Size = new Size(64, 48);

            timer.Tick += new EventHandler(TimerTick);

            dotSize = (int)((this.Height + 1) - (this.Height * 0.75f));

            this.SetRectangles();
        }

        private void Activated(bool value)
        {
            if (value)
            {
                this.Reset();
                this.SetRectangles();
            }
            isActivated = value;
            timer.Enabled = value;
        }

        public void Start()
        {
            if (!isActivated)
            {
                this.Activated(true);
            }
        }

        public void Stop()
        {
            if (isActivated)
            {
                this.Activated(false);
            }
        }

        private void Reset()
        {
            for (int i = 0; i < rectY.Length; i++)
            {
                rectY[i] = 0;
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            this.Reset();
            switch (count)
            {
                case 0:
                    rectY[count] = dotSize / 2;
                    break;
                case 1:
                    rectY[count] = dotSize / 2;
                    break;
                case 2:
                    rectY[count] = dotSize / 2;
                    break;
            }
            count = (count + 1) % 10;
            this.Invalidate();
        }

        public void SetRectangles()
        {
            rects[1] = new RectangleF((this.Width / 2) - (dotSize / 2), (this.Height / 2) - (dotSize / 2) - rectY[1], dotSize, dotSize);
            rects[0] = new RectangleF(rects[1].X - (dotSize + (dotSize / 3)), (this.Height / 2) - (dotSize / 2) - rectY[0], dotSize, dotSize);
            rects[2] = new RectangleF(rects[1].X + (dotSize + (dotSize / 3)), (this.Height / 2) - (dotSize / 2) - rectY[2], dotSize, dotSize);
        }

        protected override void OnResize(EventArgs e)
        {
            dotSize = (int)((this.Height + 1) - (this.Height * 0.75f));
            this.SetRectangles();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.SetRectangles();

            SolidBrush solidBrush = new SolidBrush(this.ForeColor);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.Clear(this.BackColor);

            for (int i = 0; i < rects.Length; i++)
            {
                e.Graphics.FillEllipse(solidBrush, rects[i]);
            }

            base.OnPaint(e);
        }
    }
}
