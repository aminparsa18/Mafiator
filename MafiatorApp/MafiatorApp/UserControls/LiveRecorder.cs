using Xamarin.Forms;

namespace MafiatorApp.UserControls
{
    public class LiveRecorder:View
    {
        public static readonly BindableProperty IsRecordingProperty =
            BindableProperty.Create(nameof(IsRecording), typeof(bool), typeof(LiveRecorder), false);

        public bool IsRecording
        {
            get => (bool)GetValue(IsRecordingProperty);
            set => SetValue(IsRecordingProperty, value);
        }

        public static readonly BindableProperty IsPublishingProperty =
            BindableProperty.Create(nameof(IsPublishing), typeof(bool), typeof(LiveRecorder), false);

        public bool IsPublishing
        {
            get => (bool)GetValue(IsPublishingProperty);
            set => SetValue(IsPublishingProperty, value);
        }

        public static readonly BindableProperty RtmpAddressProperty =
            BindableProperty.Create(nameof(RtmpAddress), typeof(string), typeof(LiveRecorder), "");

        public string RtmpAddress
        {
            get => (string)GetValue(RtmpAddressProperty);
            set => SetValue(RtmpAddressProperty, value);
        }
    }
}
