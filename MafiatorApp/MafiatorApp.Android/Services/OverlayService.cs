using Android.Views.Animations;
using Android.Widget;
using Com.Takusemba.Spotlight;
using Com.Takusemba.Spotlight.Effet;
using Com.Takusemba.Spotlight.Shape;
using MafiatorApp.Droid.Services;
using MafiatorApp.Models;
using MafiatorApp.Services;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Button = Android.Widget.Button;
using Platform = Xamarin.Forms.Platform.Android.Platform;
using View = Xamarin.Forms.View;

[assembly: Dependency(typeof(OverlayService))]

namespace MafiatorApp.Droid.Services
{
    public class OverlayService : IOverlayService
    {
        private readonly List<Target> _targets;
        private Spotlight _spotlight;

        public OverlayService()
        {
            _targets = new List<Target>();
        }

        public void AddOverlay(Dictionary<View, string> targets, ShowCaseConfig config)
        {
            _targets.Clear();

            foreach (var target in targets)
            {
                var root = new FrameLayout(Xamarin.Essentials.Platform.CurrentActivity);
                var view = Xamarin.Essentials.Platform.CurrentActivity.LayoutInflater.Inflate(Resource.Layout.targ, root);
                var next = view?.FindViewById<Button>(Resource.Id.spotlight_next);
                var text = view?.FindViewById<TextView>(Resource.Id.spotlight_text);
                text.Text = target.Value;
                next.Click += delegate
                {
                    _spotlight.Next();
                };
                _targets.Add(new Target.Builder()
                    .SetAnchor(GetOrCreateRenderer(target.Key).View)
                    .SetShape(new Circle(100))
                    .SetEffect(new FlickerEffect(20, Resource.Color.primary))
                    .SetOverlay(view)
                    .Build());
            }
            _spotlight = new Spotlight.Builder(Xamarin.Essentials.Platform.CurrentActivity)
                .SetTargets(_targets)
                .SetBackgroundColor(Resource.Color.spotlight)
                .SetDuration(800L)
                .SetAnimation(new AccelerateDecelerateInterpolator())
                .Build();
            _spotlight.Start();
        }


        private static IVisualElementRenderer GetOrCreateRenderer(VisualElement element)
        {
            var renderer = Platform.GetRenderer(element);
            if (renderer != null) return renderer;
            renderer = Platform.CreateRendererWithContext(element, Xamarin.Essentials.Platform.AppContext);
            Platform.SetRenderer(element, renderer);
            return renderer;
        }
    }
}