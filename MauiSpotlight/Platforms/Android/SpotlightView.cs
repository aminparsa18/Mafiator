using Android.Animation;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Paint = Android.Graphics.Paint;
using PointF = Android.Graphics.PointF;
using LayerType = Android.Views.LayerType;
using Color = Android.Graphics.Color;
using AndroidX.Core.Content;

namespace MauiSpotlight.Platforms.Android;

public class SpotlightView : FrameLayout
{
    private Paint _backgroundPaint;
    private Paint _shapePaint;
    private Paint _effectPaint;
    private ValueAnimator _shapeAnimator;
    private ValueAnimator _effectAnimator;
    private Target _target;

    private Paint getBackgroundPaint() => _backgroundPaint;

    private Paint getShapePaint() => _shapePaint;

    private Paint getEffectPaint() => _effectPaint;

    public SpotlightView(Context context) : base(context)
    {
    }

    public SpotlightView(Context context, IAttributeSet attrs) : base(context, attrs)
    {
    }

    public SpotlightView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
    {
    }

    public SpotlightView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
    {
        _backgroundPaint = new Paint
        {
            Color = new Color(ContextCompat.GetColor(Platform.CurrentActivity, defStyleRes))
        };
        _shapePaint = new Paint();
        _shapePaint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.Clear));
        _effectPaint = new Paint();
        SetWillNotDraw(false);
        SetLayerType(LayerType.Hardware, null);
    }

    protected SpotlightView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
    {
    }

    protected override void OnDraw(Canvas canvas)
    {
        base.OnDraw(canvas);
        canvas.DrawRect(0.0F, 0.0F, Width, Height, getBackgroundPaint());
        Target currentTarget = _target;
        ValueAnimator currentShapeAnimator = _shapeAnimator;
        ValueAnimator currentEffectAnimator = _effectAnimator;
        if (currentTarget != null && currentEffectAnimator != null && currentShapeAnimator != null && !currentShapeAnimator.IsRunning)
        {
            currentTarget.Effect.Draw(canvas, currentTarget.Anchor, (float)currentEffectAnimator.AnimatedValue, getEffectPaint());
        }

        if (currentTarget != null && currentShapeAnimator != null)
        {
            currentTarget.Shape.Draw(canvas, currentTarget.Anchor, (float)currentShapeAnimator.AnimatedValue, getShapePaint());
        }
    }

    public void StartSpotlight(long duration, ITimeInterpolator interpolator, Animator.IAnimatorListener listener)
    {
        ObjectAnimator objectAnimator = ObjectAnimator.OfFloat(this, "alpha", new float[] { 0.0F, 1.0F });
        objectAnimator.SetDuration(duration);
        objectAnimator.SetInterpolator(interpolator);
        objectAnimator.AddListener(listener);
        objectAnimator.Start();
    }

    public void FinishSpotlight(long duration, ITimeInterpolator interpolator, Animator.IAnimatorListener listener)
    {
        ObjectAnimator objectAnimator = ObjectAnimator.OfFloat(this, "alpha", new float[] { 1.0F, 0.0F });
        objectAnimator.SetDuration(duration);
        objectAnimator.SetInterpolator(interpolator);
        objectAnimator.AddListener(listener);
        objectAnimator.Start();
    }

    public void StartTarget(Target target)
    {
        RemoveAllViews();
        AddView(target.Overlay, -1, -1);
        int[] location = new int[2];
        GetLocationInWindow(location);
        PointF offset = new PointF(location[0], location[1]);
        target.Anchor.Offset(-offset.X, -offset.Y);
        _target = target;
        _shapeAnimator?.RemoveAllListeners();
        _shapeAnimator?.RemoveAllUpdateListeners();
        _shapeAnimator?.Cancel();

        _shapeAnimator = ValueAnimator.OfFloat(new float[] { 0.0F, 1.0F });
        _shapeAnimator.SetDuration(target.Shape.Duration);
        _shapeAnimator.SetInterpolator(target.Shape.Interpolator);
        _shapeAnimator.Update += OnAnimatorUpdate;
        _shapeAnimator.AnimationEnd += delegate
        {
            _shapeAnimator.RemoveAllListeners();
            _shapeAnimator.RemoveAllUpdateListeners();
        };
        _shapeAnimator.AnimationCancel += delegate
        {
            _shapeAnimator.RemoveAllListeners();
            _shapeAnimator.RemoveAllUpdateListeners();
        };
        _effectAnimator?.RemoveAllListeners();
        _effectAnimator?.RemoveAllUpdateListeners();
        _effectAnimator?.Cancel();

        _effectAnimator = ValueAnimator.OfFloat(new float[] { 0.0F, 1.0F });
        _effectAnimator.StartDelay = target.Shape.Duration;
        _effectAnimator.SetDuration(target.Effect.Duration);
        _effectAnimator.SetInterpolator(target.Effect.Interpolator);
        _effectAnimator.RepeatMode = (ValueAnimatorRepeatMode)target.Effect.RepeatMode;
        _effectAnimator.RepeatCount = -1;
        _effectAnimator.Update += OnAnimatorUpdate;
        _effectAnimator.AnimationEnd += delegate
        {
            _effectAnimator.RemoveAllListeners();
            _effectAnimator.RemoveAllUpdateListeners();
        };
        _effectAnimator.AnimationCancel += delegate
        {
            _effectAnimator.RemoveAllListeners();
            _effectAnimator.RemoveAllUpdateListeners();
        };
        _shapeAnimator?.Start();
        _effectAnimator?.Start();
    }

    private void OnAnimatorUpdate(object sender, ValueAnimator.AnimatorUpdateEventArgs e) => Invalidate();

    public void FinishTarget(Animator.IAnimatorListener listener)
    {
        Target currentTarget = _target;
        if (currentTarget != null)
        {
            var currentAnimatedValue = _shapeAnimator?.AnimatedValue;
            if (currentAnimatedValue != null)
            {
                _shapeAnimator.RemoveAllListeners();
                _shapeAnimator.RemoveAllUpdateListeners();
                _shapeAnimator.Cancel();
                _shapeAnimator = ValueAnimator.OfFloat((float)currentAnimatedValue, 0f);
                _shapeAnimator.SetDuration(currentTarget.Shape.Duration);
                _shapeAnimator.SetInterpolator(currentTarget.Shape.Interpolator);
                _shapeAnimator.Update += OnAnimatorUpdate;
                _shapeAnimator.AddListener(listener);
                _shapeAnimator.AnimationEnd += delegate
                {
                    _shapeAnimator.RemoveAllListeners();
                    _shapeAnimator.RemoveAllUpdateListeners();
                };
                _shapeAnimator.AnimationCancel += delegate
                {
                    _shapeAnimator.RemoveAllListeners();
                    _shapeAnimator.RemoveAllUpdateListeners();
                };

                _effectAnimator?.RemoveAllListeners();
                _effectAnimator?.RemoveAllUpdateListeners();
                _effectAnimator?.Cancel();
                _effectAnimator = null;
                _shapeAnimator?.Start();
            }
        }
    }

    public void Cleanup()
    {
        _effectAnimator?.RemoveAllListeners();
        _effectAnimator?.RemoveAllUpdateListeners();
        _effectAnimator?.Cancel();
        _effectAnimator = null;
        _shapeAnimator?.RemoveAllListeners();
        _shapeAnimator?.RemoveAllUpdateListeners();
        _shapeAnimator?.Cancel();
        _shapeAnimator = null;
        RemoveAllViews();
    }
}