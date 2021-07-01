using System.IO;
using Spectrogram;

namespace Mafiator.Ai
{
    public class AudioToImageConverter
    {
        public static void CreateSpectrogram(string fileName)
        {
            var (sampleRate, audio) = WavFile.ReadMono(fileName);
            var spec = new Spectrogram.Spectrogram(sampleRate, fftSize: 4096, stepSize: 500, maxFreq: 3000);
            spec.Add(audio);
            var code = Path.GetFileName(fileName).Substring(6, 2);
            spec.SaveImage(ParseCode(code) + Path.GetFileName(fileName)[..^4] + ".jpg", intensity: 1);
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
