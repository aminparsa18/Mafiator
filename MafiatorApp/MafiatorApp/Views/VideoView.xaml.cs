using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoView : ContentPage
    {
        public VideoView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
           
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            RtmpLiveRecorder.IsRecording = !RtmpLiveRecorder.IsRecording;
        }

        private void Button_OnClicked2(object sender, EventArgs e)
        {
            RtmpLiveRecorder.IsPublishing = !RtmpLiveRecorder.IsPublishing;

        }
    }
}