using System;
using MafiatorApp.Cache;
using MediaManager;
using Xamarin.CommunityToolkit.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views.LazyViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LazyVideoView : ContentView
    {
        public LazyVideoView()
        {
            InitializeComponent();
            VideoElement.Source = Application.Current.RequestedTheme == OSAppTheme.Light
                ? MediaSource.FromUri("ms-appx:///day.mp4")
                : MediaSource.FromUri("ms-appx:///night.mp4");
        }

        private async void VideoElement_OnMediaOpened(object sender, EventArgs e)
        {
            if (!Barrel.Current.Exists("PlayMusic") || Barrel.Current.Get<bool>("PlayMusic"))
                await CrossMediaManager.Current.Play();
        }
    }
}