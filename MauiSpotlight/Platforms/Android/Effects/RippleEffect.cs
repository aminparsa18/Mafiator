using Android.Animation;
using Android.Graphics;
using Java.Lang;
using Color = Android.Graphics.Color;

namespace MauiSpotlight.Platforms.Android.Effects;

public class RippleEffect : IEffect
{
    private readonly float _offset;
    private readonly float _radius;
    private readonly int _color;
    private readonly long _duration;
    private readonly ITimeInterpolator _interpolator;
    private readonly int _repeatMode;

    public RippleEffect(float offset, float radius, int color, long duration, ITimeInterpolator interpolator, int repeatMode) : base()
    {
        _offset = offset;
        _radius = radius;
        _color = color;
        _duration = duration;
        _interpolator = interpolator;
        _repeatMode = repeatMode;
        if (_offset < _radius)
        {
            throw new IllegalArgumentException("holeRadius should be bigger than rippleRadius.");
        }
    }

    public void Draw(Canvas canvas, global::Android.Graphics.PointF point, float value, global::Android.Graphics.Paint paint)
    {
        float radius = _offset + (_radius - _offset) * value;
        int alpha = (int)(255 - value * 255);
        paint.Color = new Color(_color);
        paint.Alpha = alpha;
        canvas.DrawCircle(point.X, point.Y, radius, paint);
    }

    public long Duration => _duration;
    public ITimeInterpolator Interpolator => _interpolator;
    public int RepeatMode => _repeatMode;
}