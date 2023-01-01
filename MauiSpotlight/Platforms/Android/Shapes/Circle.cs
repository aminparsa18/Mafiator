using Android.Animation;
using Android.Graphics;

namespace MauiSpotlight.Platforms.Android.Shapes;

public class Circle : IShape
{
    private float _radius;
    private long _duration;
    private ITimeInterpolator _interpolator;

    public Circle(float radius, long duration, ITimeInterpolator interpolator) : base()
    {
        _radius = radius;
        _duration = duration;
        _interpolator = interpolator;
    }

    public Circle(float radius) : this(radius, 0, null) { }

    public void Draw(Canvas canvas, global::Android.Graphics.PointF point, float value, global::Android.Graphics.Paint paint) => 
        canvas.DrawCircle(point.X, point.Y, value * _radius, paint);

    public long Duration => _duration;

    public ITimeInterpolator Interpolator => _interpolator;
}