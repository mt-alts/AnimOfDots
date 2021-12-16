using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AnimOfDots
{
    public class Overlay : UserControl
    {
        private Bitmap bitmapColorPalette;
        private Timer timer = new Timer();
        private RectangleF[] rects = new RectangleF[8];
        private Color[] colorPixelArray = new Color[8];
        private float dotSize = 24;
        private PointF[] pf = new PointF[8];
        private float sizeW = 0, sizeH = 0;
        private int[] imagePixel;
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
                    timer.Interval = (ANIMATION_MAX_SPEED * 2) - (value * 2);
                    animationSpeed = value;
                }
                else
                {
                    throw new Exception("Error: Value cannot be greater than " + ANIMATION_MAX_SPEED);
                }
            }
        }

        private Color[] colors = new Color[3] { Color.DodgerBlue,
                                               Color.FromArgb(100, Color.DeepSkyBlue),
                                               Color.FromArgb(0, Color.LightSkyBlue) };
        public Color[] Colors
        {
            get { return colors; }
            set
            {
                colors = value;
                this.CreateColorPalette();
                this.SetColors();
                this.Invalidate();
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
                    this.Stop();
                    this.Initialize();
                    this.Visible = true;
                }
            }
        }

        public override Color ForeColor { get => base.ForeColor; }

        public Overlay()
        {
            timer.Tick += new EventHandler(Timer_Tick);

            this.Size = new Size(48, 48);
            this.DoubleBuffered = true;
            this.Initialize();
        }

        private void Initialize()
        {
            imagePixel = new int[8] { 7, 6, 5, 4, 3, 2, 1, 0 };

            this.CreateColorPalette();
            this.SetPoints();
            this.SetColors();
            this.SetRectangles();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < imagePixel.Length; i++)
            {
                imagePixel[i] = (imagePixel[i] + 1) % bitmapColorPalette.Width;
            }

            SetColors();
            this.Refresh();
        }

        private void Activate(bool value)
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
                this.Initialize();
                this.Activate(true);
            }
        }

        public void Stop()
        {
            if (isActivated)
            {
                this.Activate(false);
            }
        }

        private void SetScale()
        {
            dotSize = this.Height / 4.5f;
            sizeW = this.Width - dotSize;
            sizeH = this.Height - dotSize;
        }

        private void SetPoints()
        {
            this.SetScale();
            pf[0] = new PointF(sizeW / 2, 0);
            pf[2] = new PointF(sizeW, sizeH / 2);
            pf[4] = new PointF(sizeW / 2, sizeH);
            pf[6] = new PointF(0, sizeH / 2);
            pf[1] = new PointF((sizeW - pf[0].X / 3), (pf[2].Y / 3));
            pf[3] = new PointF(sizeW - pf[4].X / 3, sizeH - pf[2].Y / 3);
            pf[5] = new PointF(pf[4].X / 3, sizeH - (pf[6].Y / 3));
            pf[7] = new PointF(pf[0].X / 3, pf[6].Y / 3);
        }

        private void CreateColorPalette()
        {
            bitmapColorPalette = new Bitmap(8, 2);
            using (Graphics graphics = Graphics.FromImage(bitmapColorPalette))
            {
                Rectangle rectangle = new Rectangle(0, 0, bitmapColorPalette.Width, bitmapColorPalette.Height);
                LinearGradientBrush gradientBrush = new LinearGradientBrush(rectangle, Color.Transparent, Color.Transparent, 360);
                ColorBlend colorBlend = new ColorBlend(3);
                colorBlend.Colors = this.Colors;
                colorBlend.Positions = new float[3] { 0.0f, 0.5f, 1.0f };
                gradientBrush.InterpolationColors = colorBlend;
                graphics.FillRectangle(gradientBrush, rectangle);
            }
        }

        private void SetColors()
        {
            for (int i = 0; i < colorPixelArray.Length; i++)
            {
                colorPixelArray[i] = bitmapColorPalette.GetPixel(imagePixel[i], 1);
            }
        }

        private void SetRectangles()
        {
            for (int i = 0; i < rects.Length; i++)
            {
                rects[i] = new RectangleF(pf[i].X, pf[i].Y, dotSize, dotSize);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            this.Initialize();
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.Clear(this.BackColor);

            for (int i = 0; i < rects.Length; i++)
            {
                e.Graphics.FillEllipse(new SolidBrush(colorPixelArray[i]), rects[i]);
            }

            base.OnPaint(e);
        }
    }
}
