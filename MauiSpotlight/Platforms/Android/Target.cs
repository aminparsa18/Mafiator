using MauiSpotlight.Platforms.Android.Effects;
using MauiSpotlight.Platforms.Android.Shapes;
using IEffect = MauiSpotlight.Platforms.Android.Effects.IEffect;
using IShape = MauiSpotlight.Platforms.Android.Shapes.IShape;
using PointF = Android.Graphics.PointF;
using View = Android.Views.View;

namespace MauiSpotlight.Platforms.Android;

public class Target
{
    private readonly PointF _anchor;
    private readonly IShape _shape;
    private readonly IEffect _effect;
    private readonly View _overlay;
    private readonly Action _onTargetStart;
    private readonly Action _onTargetEnd;

    public Target(PointF anchor, IShape shape, IEffect effect, View overlay, Action onTargetStart, Action onTargetEnd) : base()
    {
        _anchor = anchor;
        _shape = shape;
        _effect = effect;
        _overlay = overlay;
        _onTargetStart = onTargetStart;
        _onTargetEnd = onTargetEnd;
    }

    public PointF Anchor => _anchor;

    public IShape Shape => _shape;

    public IEffect Effect => _effect;

    public View Overlay => _overlay;

    public Action OnTargetStart => _onTargetStart;

    public Action OnTargetEnd => _onTargetEnd;

    public class Builder
    {
        private PointF _anchor;
        private IShape _shape;
        private IEffect _effect;
        private View _overlay;
        private Action _onTargetStart;
        private Action _onTargetEnd;

        private static readonly PointF DEFAULT_ANCHOR = new(0.0F, 0.0F);
        private static readonly Circle DEFAULT_SHAPE = new(100.0F, 0L, null);
        private static readonly EmptyEffect DEFAULT_EFFECT = new(0L, null, 0);

        public Builder()
        {
            _anchor = DEFAULT_ANCHOR;
            _shape = DEFAULT_SHAPE;
            _effect = DEFAULT_EFFECT;
        }

        public Builder SetAnchor(View view)
        {
            int[] location = new int[2];
            view.GetLocationInWindow(location);
            float x = location[0] + view.Width / 2.0F;
            float y = location[1] + view.Height / 2.0F;
            return SetAnchor(x, y);
        }

        public Builder SetAnchor(float x, float y) => SetAnchor(new PointF(x, y));

        public Builder SetAnchor(PointF anchor)
        {
            _anchor = anchor;
            return this;
        }

        public Builder SetShape(IShape shape)
        {
            _shape = shape;
            return this;
        }

        public Builder SetEffect(IEffect effect)
        {
            _effect = effect;
            return this;
        }

        public Builder SetOverlay(View overlay)
        {
            _overlay = overlay;
            return this;
        }

        public Builder SetOnTargetStart(Action onTargetStart)
        {
            _onTargetStart = onTargetStart;
            return this;
        }

        public Builder SetOnTargetEnd(Action onTargetEnd)
        {
            _onTargetEnd = onTargetEnd;
            return this;
        }

        public Target Build() => new(_anchor, _shape, _effect, _overlay, _onTargetStart, _onTargetEnd);
    }
}