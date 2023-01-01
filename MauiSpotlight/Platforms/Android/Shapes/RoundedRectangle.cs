using Android.Animation;
using Android.Graphics;
using RectF = Android.Graphics.RectF;

namespace MauiSpotlight.Platforms.Android.Shapes;

public class RoundedRectangle : IShape
{
    private float _height;
    private float _width;
    private float _radius;
    private long _duration;
    private ITimeInterpolator _interpolator;

    public RoundedRectangle(float height, float width, float radius, long duration, ITimeInterpolator interpolator) : base()
    {
        _height = height;
        _width = width;
        _radius = radius;
        _duration = duration;
        _interpolator = interpolator;
    }

    public void Draw(Canvas canvas, global::Android.Graphics.PointF point, float value, global::Android.Graphics.Paint paint)
    {
        float halfWidth = _width / (float)2 * value;
        float halfHeight = _height / (float)2 * value;
        float left = point.X - halfWidth;
        float top = point.Y - halfHeight;
        float right = point.X + halfWidth;
        float bottom = point.Y + halfHeight;
        RectF rect = new RectF(left, top, right, bottom);
        canvas.DrawRoundRect(rect, _radius, _radius, paint);
    }

    public long Duration => _duration;

    public ITimeInterpolator Interpolator => _interpolator;

   
}