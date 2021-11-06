using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AnimOfDots {
    public partial class Overlay : UserControl {

        private Image image;
        private Timer timer = new Timer();
        private RectangleF[] rectangles = new RectangleF[8];
        private float dotSize = 24;
        private PointF[] pf = new PointF[8];
        private float sizeW = 0, sizeH = 0;
        private int rotation = 0;
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

        public short RotationStep { get; set; } = 7;

        public Color PrimaryColor { get; set; } = Color.FromArgb(21, 101, 192);

        public Color SecondaryColor { get; set; } = Color.FromArgb(100, 181, 246);

        public override Color BackColor { get => Color.Transparent; }

        public override Color ForeColor { get => Color.Transparent; }

        public Overlay()  {
            InitializeComponent();

            this.Width = 48; this.Height = 48;
            image = new Bitmap(this.Width, this.Height);
        }

        private void Overlay_Load(object sender, EventArgs e) {
            this.DoubleBuffered = true;

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
            rotation = 0;
        }

        private void timer_tick(Object sender, EventArgs e) {
            rotation = (rotation + RotationStep) % 360;
            Invalidate();
        }

        private void SetScale() {
            dotSize = this.Height / 4.5f;
            sizeW = this.Width - dotSize;
            sizeH = this.Height - dotSize;
        }

        private void SetPoints() {
            SetScale();
            pf[0] = new PointF(sizeW / 2, 0);
            pf[2] = new PointF(sizeW, sizeH / 2);
            pf[4] = new PointF(sizeW / 2, sizeH);
            pf[6] = new PointF(0, sizeH / 2);
            pf[1] = new PointF((sizeW - pf[0].X / 3), (pf[2].Y / 3));
            pf[3] = new PointF(sizeW - pf[4].X / 3, sizeH - pf[2].Y / 3);
            pf[5] = new PointF(pf[4].X / 3, sizeH - (pf[6].Y / 3));
            pf[7] = new PointF(pf[0].X / 3, pf[6].Y / 3);
        }

        protected override void OnResize(EventArgs e) {
            SetPoints();

            for (int i = 0; i < rectangles.Length; i++)
                rectangles[i] = new RectangleF(pf[i].X, pf[i].Y, dotSize, dotSize);

            using (GraphicsPath gp = new GraphicsPath()) {
                for (int i = 0; i < rectangles.Length; i++)
                    gp.AddEllipse(rectangles[i]);
                this.Region = new Region(gp);
            }
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e) {
            image = new Bitmap(this.Width, this.Height);
            using (Graphics graphics = Graphics.FromImage(image)) {
                graphics.Clear(Color.Transparent);
                GraphicsPath graphicsPath = new GraphicsPath();
                graphicsPath.AddEllipse(this.ClientRectangle);

                PathGradientBrush pathGradientBrush = new PathGradientBrush(graphicsPath);

                pathGradientBrush.CenterPoint = new PointF(this.ClientRectangle.Width / 2, 0);
                pathGradientBrush.CenterColor = PrimaryColor;
                pathGradientBrush.SurroundColors = new Color[] { SecondaryColor };

                graphics.FillPath(pathGradientBrush, graphicsPath);

                pathGradientBrush.Dispose();
                graphicsPath.Dispose();
            }
            base.OnPaint(e);
        }

        private void Overlay_Paint(object sender, PaintEventArgs e) {
            e.Graphics.TranslateTransform(this.Width / 2, this.Height / 2);
            e.Graphics.RotateTransform(rotation);
            e.Graphics.TranslateTransform(-this.Width / 2, -this.Height / 2);
            e.Graphics.DrawImage(image, new Point(0, 0));
        }
    }
}
