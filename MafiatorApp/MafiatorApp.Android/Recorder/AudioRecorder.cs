using System.Threading.Tasks;
using Android.Media;
using Java.Lang;
using MafiatorApp.Droid.Services;
using MafiatorApp.Services;
using Console = System.Console;
using Exception = System.Exception;

namespace MafiatorApp.Droid.Recorder
{
	public class AudioRecorder:IAudioRecorder
	{
        private MediaRecorder recorder;
        public void Init()
        {
            recorder=new MediaRecorder();
            recorder.SetAudioSource(AudioSource.Mic);
            recorder.SetOutputFormat(OutputFormat.Mpeg4);
            recorder.SetAudioEncoder(AudioEncoder.HeAac);
            recorder.SetOutputFile(System.IO.Path.Combine(System.IO.Path.GetTempPath() + "myrecording.aac"));
        }

  

        public void StartRecording()
        {
            try
            {
                recorder.Prepare();
                recorder.Start();
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
                recorder.Stop();
                recorder.Reset();
                recorder.Release();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public bool IsRecording { get; set; }
        public void ConvertToFlac(string path)
        {
            new FlacEncoder().ConvertToFlac(path);
        }
    }
}
