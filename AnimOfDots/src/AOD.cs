using System;
using System.Drawing;
using System.Windows.Forms;

namespace AnimOfDots {
    public class AOD {
        public class BaseControl : Control {

            private Animator animator;

            public override string Text => "";
            public override Font Font { get => base.Font; }

            public int AnimationSpeed {
                get => animator.AnimationSpeed;
                set => animator.AnimationSpeed = value;
            }

            protected BaseControl() {
                SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                BackColor = System.Drawing.Color.Transparent;
            }

            protected void AnimationSpeedBalance(int speedBalance) {
                animator = new Animator(speedBalance);
            }

            protected virtual void Animate() { }

            public virtual void Start() {
                Show();
                animator.Start(Animate);
            }

            public virtual void Stop() {
                Hide();
                animator.Stop();
                Reset();
            }

            protected virtual void Reset() { }

        }
    }

    public static class AodMath {

        public static T Percentage<T>(T num, T perc) {
            dynamic _num = num;
            dynamic _perc = perc;
            return (_num * _perc) / 100;
        }

    }
}