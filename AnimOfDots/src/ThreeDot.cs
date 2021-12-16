using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AnimOfDots
{
    public class ThreeDot : UserControl
    {
        private Bitmap bitmapColorPalette;
        private Timer timer = new Timer();
        private RectangleF[] rects = new RectangleF[3];
        private float dotSize = 0;
        private SolidBrush[] solidBrushes = new SolidBrush[3];
        private int[] imagePixel;
        private bool isActivated = false;
        private const byte ANIMATION_MAX_SPEED = 101;

        private byte animationSpeed = 25;
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
                    this.Initialize();
                    this.Visible = true;
                    this.Invalidate();
                }
            }
        }

        public override Color ForeColor { get => Color.Transparent; }

        private Color primaryColor = Color.Wheat;
        public Color PrimaryColor
        {
            get { return primaryColor; }
            set
            {
                primaryColor = value;
                this.CreateColorPalette();
                this.Initialize();
                this.Invalidate();
            }
        }

        private Color secondaryColor = Color.Orange;
        public Color SecondaryColor
        {
            get { return secondaryColor; }
            set
            {
                secondaryColor = value;
                this.CreateColorPalette();
                this.Initialize();
                this.Invalidate();
            }
        }

        public ThreeDot()
        {
            this.DoubleBuffered = true;
            timer.Tick += new EventHandler(Timer_Tick);
            this.Size = new Size(48, 12);
            dotSize = this.Height - 1;
            this.SetRectangles();
            this.Initialize();
        }

        private void Initialize()
        {
            for (int i = 0; i < solidBrushes.Length; i++)
            {
                solidBrushes[i] = new SolidBrush(Color.Transparent);
            }

            imagePixel = new int[3] { 0, (int)(bitmapColorPalette.Width * 0.15f), (int)(bitmapColorPalette.Width * 0.30f) };
            this.SetColors();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < imagePixel.Length; i++)
            {
                imagePixel[i] = (imagePixel[i] + 3) % bitmapColorPalette.Width;
            }

            this.SetColors();
            this.Refresh();
        }

        private void SetRectangles()
        {
            rects[1] = new RectangleF(new PointF((this.Width / 2) - (dotSize / 2), (this.Height / 2) - (dotSize / 2)), new SizeF(dotSize, dotSize));
            rects[0] = new RectangleF(new PointF(rects[1].X + (dotSize * 1.25f), rects[1].Y), new SizeF(dotSize, dotSize));
            rects[2] = new RectangleF(new PointF(rects[1].X - (dotSize * 1.25f), rects[1].Y), new SizeF(dotSize, dotSize));
        }

        private void Activated(bool value)
        {
            if (value)
            {
                this.Initialize();
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

        private void SetColors()
        {
            for (int i = 0; i < solidBrushes.Length; i++)
            {
                solidBrushes[i].Color = bitmapColorPalette.GetPixel(imagePixel[i], 1);
            }
        }

        private void CreateColorPalette()
        {
            bitmapColorPalette = new Bitmap(50, 2);
            using (Graphics graphics = Graphics.FromImage(bitmapColorPalette))
            {
                Rectangle rectangle = new Rectangle(0, 0, bitmapColorPalette.Width, bitmapColorPalette.Height);
                LinearGradientBrush gradientBrush = new LinearGradientBrush(rectangle, Color.Transparent, Color.Transparent, 360);
                ColorBlend colorBlend = new ColorBlend(4);
                colorBlend.Colors = new Color[4] { primaryColor, this.SecondaryColor, primaryColor, primaryColor };
                colorBlend.Positions = new float[4] { 0f, 0.5f, 0.7f, 1f };
                gradientBrush.InterpolationColors = colorBlend;
                graphics.FillRectangle(gradientBrush, rectangle);
            }
        }
        protected override void OnResize(EventArgs e)
        {
            dotSize = this.Height - 1;
            this.CreateColorPalette();
            this.SetRectangles();
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.Clear(this.BackColor);

            for (int i = rects.Length - 1; i >= 0; i--)
            {
                e.Graphics.FillEllipse(solidBrushes[i], rects[i]);
            }

            base.OnPaint(e);
        }
    }
}
