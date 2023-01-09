using Android.Views.Animations;
using Android.Widget;
using Mafiator.Game.Services;
using MauiSpotlight.Platforms.Android;
using MauiSpotlight.Platforms.Android.Effects;
using MauiSpotlight.Platforms.Android.Shapes;
using Button = Android.Widget.Button;

namespace Mafiator.Game.Droid.Services;

public class OverlayService : IOverlayService
{
    private readonly List<Target> _targets;
    private Spotlight _spotlight;

    public OverlayService()
    {
        _targets = new List<Target>();
    }

    public void AddOverlay(Dictionary<View, string> targets)
    {
        _targets.Clear();

        foreach (var target in targets)
        {
            var root = new FrameLayout(Platform.CurrentActivity);
            var view = Platform.CurrentActivity.LayoutInflater.Inflate(Resource.Layout.targ, root);
            var next = view?.FindViewById<Button>(Resource.Id.spotlight_next);
            var text = view?.FindViewById<TextView>(Resource.Id.spotlight_text);
            text.Text = target.Value;
            next.Click += delegate
            {
                _spotlight.Next();
            };
            _targets.Add(new Target.Builder()
                .SetAnchor((Android.Views.View)target.Key.Handler.PlatformView)
                .SetShape(new Circle(100))
                .SetEffect(new FlickerEffect(20, Resource.Color.black))
                .SetOverlay(view)
                .Build());
        }
        _spotlight = new Spotlight.Builder(Platform.CurrentActivity)
            .SetTargets(_targets)
            .SetBackgroundColor(Resource.Color.spotlight)
            .SetDuration(800L)
            .SetAnimation(new AccelerateDecelerateInterpolator())
            .Build();
        _spotlight.Start();
    }
}