using Android.Animation;
using Android.Graphics;

namespace MauiSpotlight.Platforms.Android.Effects;

public class EmptyEffect : IEffect
{
    private readonly long _duration;
    private readonly ITimeInterpolator _interpolator;
    private readonly int _repeatMode;

    public EmptyEffect(long duration, ITimeInterpolator interpolator, int repeatMode) : base()
    {
        _duration = duration;
        _interpolator = interpolator;
        _repeatMode = repeatMode;
    }

    public void Draw(Canvas canvas, global::Android.Graphics.PointF point, float value, global::Android.Graphics.Paint paint)
    {

    }

    public long Duration => _duration;

    public ITimeInterpolator Interpolator => _interpolator;

    public int RepeatMode => _repeatMode;
}