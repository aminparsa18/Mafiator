using Android.Animation;
using Android.Graphics;
using Paint = Android.Graphics.Paint;
using PointF = Android.Graphics.PointF;

namespace MauiSpotlight.Platforms.Android.Effects;

public interface IEffect
{
    long Duration { get; }

    ITimeInterpolator Interpolator { get; }

    int RepeatMode { get; }

    void Draw(Canvas canvas, PointF point, float value, Paint paint);
}