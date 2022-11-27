namespace Mafiator.Game.Services
{
    public interface IAudioRecorder
    {
        void StartRecording();
        Task StopRecording();
        bool IsRecording { get; set; }
       // void ConvertToFlac(string path);
    }
}