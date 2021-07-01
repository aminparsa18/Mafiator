using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmPhoneView : ContentPage
    {
        private int duration = 30;
        public ConfirmPhoneView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this,false);
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                TimeLbl.Text = TimeSpan.FromSeconds(duration).ToString(@"mm\:ss");
                duration--;
                if (duration != -1) return true;
                ResendBtn.Background = new LinearGradientBrush(new GradientStopCollection()
                {
                    new GradientStop(Color.FromHex("#17161B"),0),
                    new GradientStop(Color.FromHex("#FD5252"),1),
                }, new Point(0, 0), new Point(1, 1));
                ResendBtn.IsEnabled = true;
                return false;
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LazyVideo.LoadViewAsync();
        }
    }
}