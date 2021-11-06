using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AnimOfDots {
    public partial class ColorTransition : UserControl {

        private Image image;
        private Timer timer = new Timer();
        private Point bitmapPoint;
        private Rectangle[] rectangles = new Rectangle[3];
        private float rectX = 0.0f;
        private int dotSize = 28;
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

        public override Color BackColor { get { return Color.Transparent; } }

        public override Color ForeColor { get { return Color.Transparent; } }

        public Color PrimaryColor { get; set; } = Color.FromArgb(79, 195, 247);

        public Color SecondaryColor { get; set; } = Color.FromArgb(30, 136, 229);

        public ColorTransition() {
            InitializeComponent();

            this.Width = 112; this.Height = 60;
        }

        private void ColorTransition_Load(object sender, EventArgs e) {
            this.DoubleBuffered = true;

            image = new Bitmap(dotSize * 29, this.Height + 2);

            timer.Interval = ANIMATION_MAX_SPEED - animationSpeed;
            timer.Tick += new EventHandler(timer_tick);
        }

        public void Play() {
            timer.Enabled = true;
        }

        public void Pause() {
            timer.Enabled = false;
        }

        public void Stop() {
            timer.Enabled = false;
            rectX = 0.0f;
        }

        private void timer_tick(Object sender, EventArgs e) {
            rectX = (rectX + dotSize / 3) % ((float)image.Width / 1.25f);
            Invalidate();
        }

        private void SetRectangles() {
            rectangles[0] = new Rectangle(0, (this.Height / 2) / 2, dotSize, dotSize);
            rectangles[1] = new Rectangle(rectangles[0].X + (dotSize + (dotSize / 3)), (this.Height / 2) / 2, dotSize, dotSize);
            rectangles[2] = new Rectangle(rectangles[1].X + (dotSize + (dotSize / 3)), (this.Height / 2) / 2, dotSize, dotSize);
        }

        protected override void OnResize(EventArgs e) {
            dotSize = (this.Height / 2);

            SetRectangles();

            image = new Bitmap(dotSize * 29, this.Height + 2);

            using (var gp = new GraphicsPath()) {
                for (int i = 0; i < rectangles.Length; i++)
                    gp.AddEllipse(rectangles[i]);
                this.Region = new Region(gp);
            }
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e) {
            using (Graphics graphics = Graphics.FromImage(image)) {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle rectangle = new Rectangle(0, 0, image.Width, image.Height);
                LinearGradientBrush gradientBrush = new LinearGradientBrush(rectangle, Color.Transparent, Color.Transparent, 360);
                ColorBlend colorBlend = new ColorBlend(5);
                colorBlend.Colors = new Color[5] { SecondaryColor, SecondaryColor, PrimaryColor, SecondaryColor, SecondaryColor };
                colorBlend.Positions = new float[5] { 0f, 0.1f, 0.4f, 0.8f, 1f };
                gradientBrush.InterpolationColors = colorBlend;
                graphics.FillRectangle(gradientBrush, rectangle);
            }
            base.OnPaint(e);
        }

        private void ColorTransition_Paint(object sender, PaintEventArgs e) {
            RectangleF destinationRect = new RectangleF(0, 0, this.Width, this.Height);
            RectangleF sourceRect = new RectangleF(
                this.bitmapPoint.X + rectX,
                this.bitmapPoint.Y,
                this.Width,
                this.Height);
            e.Graphics.DrawImage(image, destinationRect, sourceRect, GraphicsUnit.Pixel);
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        }
    }
}
