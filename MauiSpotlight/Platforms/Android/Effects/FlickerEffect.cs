using Android.Animation;
using Android.Graphics;
using Color = Android.Graphics.Color;

namespace MauiSpotlight.Platforms.Android.Effects;

public class FlickerEffect : IEffect
{
    private readonly float _radius;
    private readonly int _color;
    private readonly long _duration;
    private readonly ITimeInterpolator _interpolator;
    private readonly int _repeatMode;

    public FlickerEffect(float radius, int color, long duration, ITimeInterpolator interpolator, int repeatMode) : base()
    {
        _radius = radius;
        _color = color;
        _duration = duration;
        _interpolator = interpolator;
        _repeatMode = repeatMode;
    }

    public FlickerEffect(float radius, int color) : this(radius, color, 0, null, 0)
    {
    }

    public void Draw(Canvas canvas, global::Android.Graphics.PointF point, float value, global::Android.Graphics.Paint paint)
    {
        paint.Color = new Color(_color);
        paint.Alpha = (int)value * 255;
        canvas.DrawCircle(point.X, point.Y, _radius, paint);
    }

    public long Duration => _duration;

    public ITimeInterpolator Interpolator => _interpolator;
    
    public int RepeatMode => _repeatMode;
}