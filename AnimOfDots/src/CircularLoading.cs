﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AnimOfDots {
    public partial class CircularLoading : UserControl {
        private Image image;
        private Timer timer = new Timer();
        private List<Rectangle> rectangles = new List<Rectangle>();
        private int rotation;
        private int coordination = 0;
        private int circleSize = 10;
        private int dotSize = 5;
        private const int animationMaxSpeed = 100;

        private short animationSpeed = 60;
        public short AnimationSpeed {
            get { return animationSpeed; }
            set {
                if (value < animationMaxSpeed)
                    animationSpeed = value;
                else
                    throw new Exception("Error: Value cannot be greater than " + animationMaxSpeed);
            }
        }

        public override Color ForeColor { get; set; } = Color.DodgerBlue;

        public CircularLoading() {
            InitializeComponent();

            this.Width = 48; this.Height = 48;
            image = new Bitmap(this.Width, this.Height);
        }

        private void CircularLoading_Load(object sender, EventArgs e) {
            timer.Interval = animationMaxSpeed - animationSpeed;
            timer.Tick += new EventHandler(this.timer_tick);

            Configration();
            CalcCoordination();

            image = new Bitmap(circleSize * 2, circleSize * 2);

            rectangles.Add(new Rectangle(this.Width - ((this.Width / 2) + (CalcPercentage(this.Width, 5))), this.Height - ((this.Height / 2) + CalcPercentage(this.Height, 5)), image.Width, image.Height));
            CloneRectangle();

            using (Graphics graph = Graphics.FromImage(image)) {
                graph.Clear(Color.Transparent);
                graph.SmoothingMode = SmoothingMode.HighQuality;
                graph.FillEllipse(new SolidBrush(this.ForeColor), new Rectangle(circleSize, circleSize, dotSize, dotSize));
            }
        }

        private void CloneRectangle() {
            for (int i = 0; i < 5; i++)
                rectangles.Add(new Rectangle(coordination, coordination, dotSize, dotSize));
        }

        private void Configration() {
            circleSize = this.Height / 4;
            dotSize = circleSize / 2;
        }

        private void CalcCoordination() {
            for (int i = this.Height; i >= 80; i -= 80)
                coordination = (coordination - 1);
        }

        private int CalcPercentage(int num, int percent) {
            return (num * percent) / 100;
        }

        public void Play() {
            timer.Enabled = true;
        }

        public void Pause() {
            timer.Enabled = false;
        }

        public void Stop() {
            rotation = 0;
            timer.Enabled = false;
            Invalidate();
        }

        private void timer_tick(object sender, EventArgs e) {
            rotation = (rotation + 5) % 360;
            this.Invalidate();
        }

        private void CircularLoading_Paint(object sender, PaintEventArgs e) {
            for (int i = 0; i < rectangles.Count; i++) {
                e.Graphics.TranslateTransform(rectangles[i].Left + rectangles[i].Width / 10, rectangles[i].Top + rectangles[i].Height / 10);
                e.Graphics.RotateTransform(rotation);
                e.Graphics.DrawImage(image, new Point(-rectangles[i].Width / 10, rectangles[i].Height / 10));
            }
        }
    }
}
