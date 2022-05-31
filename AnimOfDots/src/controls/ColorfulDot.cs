using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AnimOfDots
{
    public class ColorfulDot : AOD.BaseControl
    {
        private RectangleF borderRect, rect;
        private readonly SolidBrush borderColor = new SolidBrush(Color.Black);
        private Bitmap bitmap;
        private readonly ColorBlend colorBlend = new ColorBlend(3);
        private int angle = 0;
        readonly float[] positions = new float[] { 0f, 0.5f, 1f };
        private readonly float[] two_nd_position = new float[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.7f, 0.6f, 0.5f, 0.4f, 0.3f, 0.2f, 0.1f };
        private int currentPosition = 0;

        public override Color ForeColor => Color.Transparent;

        Color[] colors = new Color[3]
            {
                Color.FromArgb(134,154,241),
                Color.FromArgb(213,31,140),
                Color.FromArgb(255,160,1)
            };

        public Color[] Colors
        {
            get => colors;
            set
            {
                if (colorBlend.Colors.Length == value.Length)
                {
                    colors = value;
                    CreateColorPalette();
                }
                else
                {
                    throw new Exception("Array must have three elements");
                }
            }
        }

        public Color BorderColor
        {
            get => borderColor.Color;
            set
            {
                borderColor.Color = value;
                CreateColorPalette();
            }
        }

        public int BorderSize { get; set; } = 10;

        public ColorfulDot()
        {
            DoubleBuffered = true;
            AnimationSpeedBalance(100);
            Size = new Size(48, 48);
            CreateColorPalette();
        }

        protected override void Animate()
        {
            base.Animate();
            angle = (angle + 10) % 360;
            CreateColorPalette();
        }

        private void SetRect()
        {
            borderRect = new RectangleF(0, 0, Width - 1, Height - 1);
            rect = new RectangleF(borderRect.X + (BorderSize / 2.0f), borderRect.Y + (BorderSize / 2.0f), borderRect.Width - BorderSize, borderRect.Height - BorderSize);
        }

        private void CreateColorPalette()
        {
            positions[1] = two_nd_position[currentPosition];
            currentPosition = (currentPosition + 1) % 14;
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.Clear(Color.Transparent);
                LinearGradientBrush gradientBrush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, angle);
                colorBlend.Colors = colors;
                colorBlend.Positions = positions;
                gradientBrush.InterpolationColors = colorBlend;
                graphics.FillEllipse(borderColor, borderRect);
                graphics.FillEllipse(gradientBrush, rect);
            }
            Refresh();
        }

        protected override void Reset()
        {
            base.Reset();
            angle = 0;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            bitmap = new Bitmap(Width * 2, Height * 2);
            SetRect();
            CreateColorPalette();
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.Clear(BackColor);
            e.Graphics.DrawImage(bitmap, 0, 0);
        }
    }
}
