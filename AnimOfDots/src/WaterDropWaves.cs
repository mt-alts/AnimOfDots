using System;
using System.Drawing;
using System.Windows.Forms;

namespace AnimOfDots
{
    public class WaterDropWaves : UserControl
    {
        private Timer timer = new Timer();
        private RectangleF[] rects = new RectangleF[4];
        private byte[] colorAlpha = new byte[4] { 255, 255, 255, 255 };
        private byte[] colorOpacity = new byte[3];
        private float[] dotSize = new float[4];
        private float expansionStep;
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
                    timer.Interval = ANIMATION_MAX_SPEED - value;
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
                    this.Initialize();
                    this.Visible = true;
                    this.Invalidate();
                }
            }
        }


        public WaterDropWaves()
        {
            timer.Tick += new EventHandler(TimerTick);
            this.DoubleBuffered = true;
            this.Size = new Size(48, 48);
            this.Initialize();
        }

        private void Initialize()
        {
            this.SetDotsSize();
            this.SetDotsOpacity();
            expansionStep = this.Height / (256.0f / 5.0f);
        }

        private void Activated(bool value)
        {
            if (value)
            {
                this.Initialize();
            }

            timer.Enabled = value;
            this.Visible = value;
            isActivated = value;
        }

        public void Start()
        {
            if (!isActivated)
            {
                Activated(true);
            }
        }

        public void Stop()
        {
            if (isActivated)
            {
                Activated(false);
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            for (int i = 0; i < dotSize.Length - 1; i++)
            {
                dotSize[i] = (dotSize[i] + expansionStep) % this.Height;
            }

            for (int i = 0; i < colorOpacity.Length; i++)
            {
                colorOpacity[i] = (byte)((colorOpacity[i] + 5) % 256);
                colorAlpha[i] = (byte)(255 - colorOpacity[i]);
            }

            this.Invalidate();
        }

        private void SetDotsSize()
        {
            dotSize[0] = this.Height - (this.Height / 3);
            dotSize[1] = this.Height / 2;
            dotSize[2] = this.Height / 3;
            dotSize[3] = this.Height * 0.1f;
        }

        private void SetDotsOpacity()
        {
            colorOpacity[0] = ((255 - (255 / 3))) - 1;
            colorOpacity[1] = (255 / 2) - 1;
            colorOpacity[2] = (255 / 3) - 1;
        }

        private void SetRectangles()
        {
            for (int i = 0; i < rects.Length; i++)
            {
                rects[i] = new RectangleF((this.Width / 2) - (dotSize[i] / 2), (this.Height / 2) - (dotSize[i] / 2), dotSize[i], dotSize[i]);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            this.Initialize();
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.SetRectangles();

            for (int i = 0; i < rects.Length; i++)
            {
                e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(colorAlpha[i], this.ForeColor)), rects[i]);
            }
             
            base.OnPaint(e);
        }
    }
}
