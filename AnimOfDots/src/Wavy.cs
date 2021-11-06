using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AnimOfDots {
    public partial class Wavy : UserControl {

        private Image image;
        private Timer timer = new Timer();
        private RectangleF[] rect = new RectangleF[3];
        private int dotSize = 24;
        private int[] rectY = new int[3] { 0, 0, 0 };
        private int count = 0;

        public override Color BackColor { get => Color.Transparent; }

        public override Color ForeColor { get; set; } = Color.DodgerBlue;

        public Wavy() { 
            InitializeComponent();

            this.Width = 60; this.Height = 28;
            image = new Bitmap(this.Width, this.Height);
        }

        private void Wavy_Load(object sender, EventArgs e) {
            this.DoubleBuffered = true;

            timer.Interval = 100;
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
            Reset();
        }

        private void Reset() {
            for (int i = 0; i < rectY.Length; i++)
                rectY[i] = 0;
        }

        private void timer_tick(Object sender, EventArgs e) {
            Reset();

            switch (count) {
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
            Invalidate();
        }

        public void SetRectangles() {
            rect[1] = new RectangleF((this.Width / 2) - (dotSize / 2), (this.Height / 2) - (dotSize / 2) - rectY[1], dotSize, dotSize);
            rect[0] = new RectangleF(rect[1].X - (dotSize + (dotSize / 3)), (this.Height / 2) - (dotSize / 2) - rectY[0], dotSize, dotSize);
            rect[2] = new RectangleF(rect[1].X + (dotSize + (dotSize / 3)), (this.Height / 2) - (dotSize / 2) - rectY[2], dotSize, dotSize);
        }

        protected override void OnResize(EventArgs e) {
            dotSize = this.Height / 2;
            image = new Bitmap(this.Width, this.Height);
            SetRectangles();
        }

        protected override void OnPaint(PaintEventArgs e) {
            dotSize = this.Height / 2;
            SetRectangles();

            using (var gp = new GraphicsPath()) {
                gp.AddEllipse(rect[1]);
                gp.AddEllipse(rect[0]);
                gp.AddEllipse(rect[2]);
                this.Region = new Region(gp);
            }

            using (Graphics graphics = Graphics.FromImage(image)) {
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.FillRectangle(new SolidBrush(this.ForeColor), new Rectangle(0, 0, image.Width, image.Height));
            }
            base.OnPaint(e);
        }

        private void Wavy_Paint(object sender, PaintEventArgs e) {
            e.Graphics.DrawImage(image, new Point(0, 0));
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        }
    }
}
