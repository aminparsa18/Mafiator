using Android.Content;
using System;
using MafiatorApp.Droid.Recorder;
using MafiatorApp.Droid.Renderers;
using MafiatorApp.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(VideoRecorder), typeof(AndroidVideoRecorderRenderer))]
namespace MafiatorApp.Droid.Renderers
{
	public class AndroidVideoRecorderRenderer : ViewRenderer<VideoRecorder, AndroidVideoRecorder>
	{
        private AndroidVideoRecorder recorder;
        public AndroidVideoRecorderRenderer(Context context) : base(context)
        {
        }

		protected override void OnElementChanged(ElementChangedEventArgs<VideoRecorder> e)
		{
			base.OnElementChanged(e);
			if (Control == null)
			{
				recorder = new AndroidVideoRecorder(Context, e.NewElement, e.NewElement.Camera, e.NewElement.Orientation);
				SetNativeControl(recorder);
			}

			if (e.OldElement != null)
			{
				// Unsubscribe
				e.OldElement.OnStartRecording -= OnStartRecording; //unsubscribe from start recording event in xamarin.forms control
				e.OldElement.OnStopRecording -= OnStopRecording; //unsubscribe from stop recording event in xamarin.forms control
				e.OldElement.OnStopPreviewing -= OnStopPreviewing;
				e.OldElement.OnStartPreviewing -= OnStartPreviewing;
				e.OldElement.OnDoCleanup -= OnDoCleanup;

			}
			if (e.NewElement != null)
			{
				// Subscribe
				e.NewElement.OnStartRecording += OnStartRecording; //subscribe from start recording event in xamarin.forms control
				e.NewElement.OnStopRecording += OnStopRecording;//subscribe from stop recording event in xamarin.forms control
				e.NewElement.OnStartPreviewing += OnStartPreviewing;
				e.NewElement.OnStopPreviewing += OnStopPreviewing;
				e.NewElement.OnDoCleanup += OnDoCleanup;

				//cameraPreview.Click += OnCameraPreviewClicked;
			}
		}

        private void OnStartRecording(object sender, EventArgs e)
		{
			recorder.StartRecording(sender, e);
		}

        private void OnStopRecording(object sender, EventArgs e)
		{
			recorder.StopRecording(sender, e);
		}

        private void OnStartPreviewing(object sender, EventArgs e)
		{
			recorder.StartPreviewing(sender, e);
		}

        private void OnStopPreviewing(object sender, EventArgs e)
		{
			recorder.StopPreviewing(sender, e);
		}

        private void OnDoCleanup(object sender, EventArgs e)
		{
			recorder.DoCleanup(sender, e);
		}



		protected override void Dispose(bool disposing)
		{
            base.Dispose(disposing);
		}
	}
}