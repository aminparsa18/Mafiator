using System;
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
        }
    }
}