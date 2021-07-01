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
    }
}