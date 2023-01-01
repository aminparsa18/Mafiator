using Android.Animation;
using Android.Graphics;
using Paint = Android.Graphics.Paint;
using PointF = Android.Graphics.PointF;

namespace MauiSpotlight.Platforms.Android.Shapes;

public interface IShape
{
    long Duration { get; }
    ITimeInterpolator Interpolator { get; }
    void Draw(Canvas canvas, PointF point, float value, Paint paint);
}