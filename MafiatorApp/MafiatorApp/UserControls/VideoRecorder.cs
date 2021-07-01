using System;
using Xamarin.Forms;

namespace MafiatorApp.UserControls
{
	public class GetVideoFileNameArgs : EventArgs
	{
		public string Name { get; internal set; }
		public GetVideoFileNameArgs(string Name) { this.Name = Name; }
		public GetVideoFileNameArgs() { }
	}

	public enum CameraOptions
	{
		Rear,
		Front
	}

	public enum OrientationOptions
	{
		Landscape,
		Portrait
	}

	public class VideoRecorder : View
	{
		public static readonly BindableProperty CameraProperty =BindableProperty.Create("Camera",typeof(CameraOptions),typeof(VideoRecorder),CameraOptions.Rear);
		public static readonly BindableProperty OrientationProperty = BindableProperty.Create("Orientation",typeof(OrientationOptions),typeof(VideoRecorder),OrientationOptions.Portrait);
		public string VideoFileName { get; set; }

		public CameraOptions Camera
		{
			get => (CameraOptions)GetValue(CameraProperty);
            set => SetValue(CameraProperty, value);
        }

		public OrientationOptions Orientation
		{
			get => (OrientationOptions)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

		public EventHandler OnStartRecording;
		public EventHandler OnStopRecording;
		public EventHandler OnStartPreviewing;
		public EventHandler OnStopPreviewing;
		public EventHandler OnDoCleanup;

		public bool IsPreviewing { get; set; }
		public bool IsRecording { get; set; }

		//Start recording
		public void StartRecording()
        {
            OnStartRecording?.Invoke(this, new EventArgs());
        }

		//Stop recording
		public void StopRecording()
        {
            OnStopRecording?.Invoke(this, new EventArgs());
        }

		//Start previewing
		public void StartPreviewing()
        {
            OnStartPreviewing?.Invoke(this, new EventArgs());
        }

		//Stop previewing
		public void StopPreviewing()
        {
            OnStopPreviewing?.Invoke(this, new EventArgs());
        }

		//Do cleanup
		public void DoCleanup()
        {
            OnDoCleanup?.Invoke(this, new EventArgs());
        }
	}
}