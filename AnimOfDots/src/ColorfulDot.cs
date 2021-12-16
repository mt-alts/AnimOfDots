using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AnimOfDots
{
    public class ColorfulDot : UserControl
    {
        private Bitmap bitmapColorPalette;
        private Timer timer = new Timer();
        private RectangleF frameRect, rect;
        private SolidBrush frameColor = new SolidBrush(Color.Yellow);
        private Color color;
        private int imagePixel = 0;
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
                    timer.Interval = ANIMATION_MAX_SPEED - value;
                    animationSpeed = value;
                }
                else
                {
                    throw new Exception("Error: Value cannot be greater than " + ANIMATION_MAX_SPEED);
                }
            }
        }

        public Color FrameColor
        {
            get { return frameColor.Color; }
            set { frameColor.Color = value; }
        }

        public int FrameSize { get; set; } = 10;

        private bool isEnabled = false;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                if (value)
                {
                    this.Start();
                }
                else
                {
                    this.Stop();
                    this.Invalidate();
                    this.Visible = true; 
                }
            }
        }


        public ColorfulDot()
        {
            this.DoubleBuffered = true;

            timer.Tick += new EventHandler(TimerTick);

            this.Size = new Size(48, 48);
            this.CreateColorPalette();
            this.SetRect();

            color = bitmapColorPalette.GetPixel(imagePixel, 1);

            timer.Interval = animationSpeed;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            imagePixel = (imagePixel + 5) % bitmapColorPalette.Width;
            color = bitmapColorPalette.GetPixel(imagePixel, 1);
            this.Invalidate();
        }

        private void IsActivated(bool value)
        {
            if (value)
            {
                this.SetRect();
            }

            isActivated = value;
            imagePixel = 0;
            timer.Enabled = value;
            this.Visible = value;
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

        private void SetRect()
        {
            frameRect = new RectangleF(0, 0, this.Width - 1, this.Height - 1);
            rect = new RectangleF(frameRect.X + (this.FrameSize / 2.0f), frameRect.Y + (this.FrameSize / 2.0f), frameRect.Width - this.FrameSize, frameRect.Height - this.FrameSize);
        }

        private void CreateColorPalette()
        {
            bitmapColorPalette = new Bitmap(global::AnimOfDots.res.resource.colors);
        }

        protected override void OnResize(EventArgs e)
        {
            this.SetRect();
            this.Invalidate();
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.Clear(this.BackColor);
            e.Graphics.FillEllipse(new SolidBrush(Color.Black), frameRect);
            e.Graphics.FillEllipse(new SolidBrush(color), rect);
            base.OnPaint(e);
        }
    }
}
