using System.Threading.Tasks;

namespace MafiatorApp.Services
{
    public interface IAudioRecorder
    {
        void Init();
        void StartRecording();
        Task StopRecording();
        bool IsRecording { get; set; }
        void ConvertToFlac(string path);
    }
}