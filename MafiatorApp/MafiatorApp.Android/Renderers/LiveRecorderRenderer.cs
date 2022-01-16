using System.ComponentModel;
using System.IO;
using Android.Content;
using Com.Github.Faucamp.Simplertmp;
using Java.Lang;
using Java.Net;
using MafiatorApp.Droid.Renderers;
using MafiatorApp.UserControls;
using Net.Ossrs.Yasea;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using IOException = Java.IO.IOException;

[assembly: ExportRenderer(typeof(LiveRecorder), typeof(LiveRecorderRenderer))]

namespace MafiatorApp.Droid.Renderers
{
    public class LiveRecorderRenderer : ViewRenderer<LiveRecorder, SrsCameraView>, SrsEncodeHandler.ISrsEncodeListener,
        SrsRecordHandler.ISrsRecordListener, RtmpHandler.IRtmpListener
    {
        private SrsCameraView cameraView;
        private SrsPublisher mPublisher;

        public LiveRecorderRenderer(Context context) : base(context)
        {
        }

        void CreateNativeAdControl()
        {
            if (cameraView != null) return;
            cameraView = new SrsCameraView(Xamarin.Essentials.Platform.CurrentActivity);
            mPublisher = new SrsPublisher(cameraView);
            mPublisher.SetEncodeHandler(new SrsEncodeHandler(this));
            mPublisher.SetRtmpHandler(new RtmpHandler(this));
            mPublisher.SetRecordHandler(new SrsRecordHandler(this));
            mPublisher.SetPreviewResolution(640, 360);
            mPublisher.SetOutputResolution(360, 640);
            mPublisher.SetVideoHDMode();
            mPublisher.StartCamera();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<LiveRecorder> e)
        {
            base.OnElementChanged(e);
            if (Control != null) return;
            CreateNativeAdControl();
            SetNativeControl(cameraView);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName.Equals(nameof(LiveRecorder.IsRecording)))
            {
                if (Element.IsRecording)
                    mPublisher.StartRecord(Path.Combine(Path.GetTempPath(),"record.mp4"));
                else
                    mPublisher.StopRecord();
            }
            else if (e.PropertyName.Equals(nameof(LiveRecorder.IsPublishing)))
            {
                if (Element.IsPublishing)
                {
                    mPublisher.StartEncode();
                    mPublisher.StartPublish(
                        "rtmp://test2-mftor-usw22.channel.media.azure.net:1935/live/acf7b6ef8a37425fb8fc51c2d6a5a86a");
                }
                else
                {
                    mPublisher.StopEncode();
                    mPublisher.StopPublish();
                }
            }
        }

        public void OnEncodeIllegalArgumentException(IllegalArgumentException p0)
        {
            throw p0;
        }

        public void OnNetworkResume()
        {
            var ss = 2;
        }

        public void OnNetworkWeak()
        {
            var ss = 2;
        }

        public void OnRecordFinished(string p0)
        {
            var xs = p0;
        }

        public void OnRecordIOException(IOException p0)
        {
           throw p0;
        }

        public void OnRecordIllegalArgumentException(IllegalArgumentException p0)
        {
            throw p0;
        }

        public void OnRecordPause()
        {
        }

        public void OnRecordResume()
        {
        }

        public void OnRecordStarted(string p0)
        {
            var xs = p0;
        }

        public void OnRtmpAudioBitrateChanged(double p0)
        {
            int a = 2;
        }

        public void OnRtmpAudioStreaming()
        {
            var xs = 2;
        }

        public void OnRtmpConnected(string p0)
        {
            var ss = p0;
        }

        public void OnRtmpConnecting(string p0)
        {
            var xs = p0;
        }

        public void OnRtmpDisconnected()
        {
            var xs = 2;
        }

        public void OnRtmpIOException(IOException p0)
        {
            throw p0;
        }

        public void OnRtmpIllegalArgumentException(IllegalArgumentException p0)
        {
            throw p0;
        }

        public void OnRtmpIllegalStateException(IllegalStateException p0)
        {
            throw p0;
        }

        public void OnRtmpSocketException(SocketException p0)
        {
           throw p0;
        }

        public void OnRtmpStopped()
        {
            var xs = 2;
        }

        public void OnRtmpVideoBitrateChanged(double p0)
        {
            var x = 2;
        }

        public void OnRtmpVideoFpsChanged(double p0)
        {
            var x = 2;

        }

        public void OnRtmpVideoStreaming()
        {
            var x = 2;

        }
    }
}