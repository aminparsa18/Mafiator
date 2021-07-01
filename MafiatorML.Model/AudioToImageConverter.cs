using System.IO;
using Spectrogram;

namespace MafiatorML.Model
{
    public class AudioToImageConverter
    {
        public static void CreateSpectrogram(string path)
        {
            var (sampleRate, audio) = WavFile.ReadMono(path);
            var spec = new Spectrogram.Spectrogram(sampleRate, fftSize: 4096, stepSize: 500, maxFreq: 3000);
            spec.Add(audio);
           // var code = Path.GetFileName(path).Substring(6, 2);
            spec.SaveImage("kir.jpg", intensity: 1);
        }

        private static string ParseCode(string code)
        {
            switch (code)
            {
                case "01": return "neutral";
                case "02": return "calm";
                case "03": return "happy";
                case "04": return "sad";
                case "05": return "angry";
                case "06": return "fearful";
                case "07": return "disgust";
                case "08": return "surprised";
                default: return "not categorized";
            }
        }
    }
}
