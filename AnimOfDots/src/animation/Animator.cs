using System;
using System.Windows.Forms;

namespace AnimOfDots {
    public class Animator {

        private bool isStarted = false;
        private const int MAX_SPEED = 101;
        private readonly int maxValue = 0;
        private int animationSpeed = 50;
        private readonly Timer timer = new Timer();
        private AnimatorStart animStart;

        public int AnimationSpeed {
            get => animationSpeed;
            set {
                if (value < MAX_SPEED) {
                    animationSpeed = value;
                    timer.Interval = ((maxValue + 1) - ((maxValue * value) / 100));
                } else {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        public Animator(int speedBalance) {
            maxValue = (speedBalance * 2) + 1;
            timer.Interval = speedBalance;
            timer.Tick += new EventHandler(TimerTick);
        }

        public void Start(AnimatorStart animStart) {
            if (!isStarted) {
                if (animStart != this.animStart) {
                    this.animStart = animStart;
                }
                timer.Enabled = true;
                isStarted = true;
            }
        }

        public void Stop() {
            if (isStarted) {
                timer.Enabled = false;
                isStarted = false;
            }
        }

        private void TimerTick(object sender, EventArgs eventArgs) {
            animStart();
        }
    }
}