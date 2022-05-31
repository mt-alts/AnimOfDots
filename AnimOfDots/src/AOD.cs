using System;
using System.Windows.Forms;

namespace AnimOfDots
{
    public class AOD
    {
        public class BaseControl : Control
        {
            private Animator animator;

            public override string Text => "";

            public int AnimationSpeed
            {
                get => animator.AnimationSpeed;
                set => animator.AnimationSpeed = value;
            }

            public BaseControl()
            {
                SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            }

            protected void AnimationSpeedBalance(int speedBalance)
            {
                animator = new Animator(speedBalance);
            }

            protected virtual void Animate() { }

            public virtual void Start()
            {
                this.Show();
                animator.Start(Animate);
            }

            public virtual void Stop()
            {
                this.Hide();
                animator.Stop();
                Reset();
            }

            protected virtual void Reset() { }
        }
    }
}