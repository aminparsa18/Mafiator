using Android.Media;
using Java.Lang;
using Mafiator.Game.Services;
using Console = System.Console;
using Exception = System.Exception;

namespace Mafiator.Game.Platforms.Android.Services
{
    public class AudioRecorder : IAudioRecorder
    {
        private MediaRecorder _recorder;

        public AudioRecorder()
        {
            _recorder = new MediaRecorder(Platform.AppContext);
            _recorder.SetAudioSource(AudioSource.Mic);
            _recorder.SetOutputFormat(OutputFormat.Mpeg4);
            _recorder.SetAudioEncoder(AudioEncoder.HeAac);
            _recorder.SetOutputFile(Path.Combine(Path.GetTempPath() + "myrecording.aac"));
        }

        public void StartRecording()
        {
            try
            {
                _recorder.Prepare();
                _recorder.Start();
            }
            catch (IllegalStateException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task StopRecording()
        {
            try
            {
                await Task.Delay(300);
                _recorder.Stop();
                _recorder.Reset();
                _recorder.Release();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public bool IsRecording { get; set; }

        //public void ConvertToFlac(string path)
        //{
        //    new FlacEncoder().ConvertToFlac(path);
        //}
    }
}