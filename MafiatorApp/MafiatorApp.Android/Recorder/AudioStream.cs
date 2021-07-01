using Android.Media;
using MafiatorApp.Services;

namespace MafiatorApp.Droid.Recorder
{
    public class AudioStream :IAudioStream
    {
        private readonly int bufferSize;

        /// <summary>
        /// The audio source.
        /// </summary>
        private readonly AudioRecord audioSource;


        /// <summary>
        /// The default device.
        /// </summary>
        public static AudioSource DefaultDevice = AudioSource.Mic;

        /// <summary>
        /// Gets the sample rate.
        /// </summary>
        /// <value>
        /// The sample rate.
        /// </value>
        public int SampleRate => this.audioSource.SampleRate;

        /// <summary>
        /// Gets bits per sample.
        /// </summary>
        public int BitsPerSample => this.audioSource.AudioFormat == Android.Media.Encoding.Pcm16bit ? 16 : 8;

        /// <summary>
        /// Gets the channel count.
        /// </summary>
        /// <value>
        /// The channel count.
        /// </value>        
        public int ChannelCount => this.audioSource.ChannelCount;

        /// <summary>
        /// Gets the average data transfer rate
        /// </summary>
        /// <value>The average data transfer rate in bytes per second.</value>
        public int AverageBytesPerSecond => this.SampleRate * this.BitsPerSample / 8 * this.ChannelCount;

        public bool Active => this.audioSource.RecordingState == RecordState.Recording;

        /// <summary>
        /// Start recording from the hardware audio source.
        /// </summary>
        public bool Start()
        {
            Android.OS.Process.SetThreadPriority(Android.OS.ThreadPriority.UrgentAudio);

            if (this.Active)
            {
                return this.Active;
            }

            this.audioSource.StartRecording();

            Record();

            return this.Active;
        }

        /// <summary>
        /// Stops recording.
        /// </summary>
        public void Stop()
        {
            this.audioSource.Stop();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimplyMobile.Media.AudioStream"/> class.
        /// </summary>
        /// <param name="sampleRate">Sample rate.</param>
        /// <param name="bufferSize">Buffer size.</param>
        public AudioStream(int sampleRate, int bufferSize)
        {
            this.bufferSize = bufferSize;
            this.audioSource = new AudioRecord(
                AudioStream.DefaultDevice,
                sampleRate,
                ChannelIn.Mono,
                Android.Media.Encoding.Pcm16bit,
                this.bufferSize);
        }

        /// <summary>
        /// Record from the microphone and broadcast the buffer.
        /// </summary>
        private void Record()
        {
            var buffer = new byte[this.bufferSize];
            var task = this.audioSource.ReadAsync(buffer, 0, this.bufferSize).ContinueWith(
                (s) =>
            {

                if (this.Active)
                {
                    Record();
                }
            });
        }

    }
}