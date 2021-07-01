using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MafiatorApp.UserControls
{
    public class SkeletonView : BoxView
    {
        public SkeletonView()
        {
            Device.StartTimer(TimeSpan.FromSeconds(2), () =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await this.FadeTo(0.9, 800, Easing.Linear);
                    await this.FadeTo(0.2, 800);
                });
                return true;

            });
            //var smoothAnimation = new Animation();

            //smoothAnimation.WithConcurrent((f) => this.Opacity = f, 0.2, 1, Xamarin.Forms.Easing.Linear);
            //smoothAnimation.WithConcurrent((f) => this.Opacity = f, 1, 0.2, Xamarin.Forms.Easing.Linear);

            //this.Animate("FadeInOut", smoothAnimation, 2, 2000, Easing.Linear, null, () => true);
        }

    }
}