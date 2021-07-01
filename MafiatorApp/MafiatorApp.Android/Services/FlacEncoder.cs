using System.IO;
using Java.Nio;
using Net.Sourceforge.Javaflacencoder;
using File = Java.IO.File;
using IOException = Java.IO.IOException;

namespace MafiatorApp.Droid.Services
{
    public class FlacEncoder
    {
        /**
    * Converts a wave file to a FLAC file(in order to POST the data to Google and retrieve a response) <br>
    * Sample Rate is 8000 by default
    *
    * @param inputFile  Input wave file
    * @param outputFile Output FLAC file
    */
        //public void ConvertWaveToFlac(File inputFile)
        //{

        //    AndroidAudioConverter.With(CrossCurrentActivity.Current.AppContext)
        //        .SetFile(inputFile)
        //        .SetFormat(AudioFormat.Flac)
        //        .SetCallback(new ConvertCallback())
        //        .Convert();
        //}
        public void ConvertToFlac(string recordedPath)
        {
            try
            {
         
                var file = new File(Path.Combine(Path.GetTempPath(),"test.flac"));
                // if file doesn't exists, then create it
                if (!file.Exists())
                {
                    file.CreateNewFile();
                }

                var inputStream = new FileStream(recordedPath, FileMode.Open, FileAccess.Read);
                var flacEncoder = new FLACEncoder();
                var streamConfiguration = new StreamConfiguration();
                streamConfiguration.SetSampleRate(8000);
                streamConfiguration.SetBitsPerSample(16);
                streamConfiguration.SetChannelCount(1);
                var flacOut = new FLACFileOutputStream(file);
                flacEncoder.SetStreamConfiguration(streamConfiguration);
                flacEncoder.SetOutputStream(flacOut);
                flacEncoder.OpenFLACStream();
                var sampleData = new int[inputStream.Length];
                var samplesIn = new byte[2];
                var i = 0;
                while (inputStream.Read(samplesIn, 0, 2) > 0)
                {
                    var bb = ByteBuffer.Wrap(samplesIn);
                    bb.Order(ByteOrder.LittleEndian);
                    var shortVal = bb.Short;
                    sampleData[i] = shortVal;

                    i++;
                }

                sampleData = TruncateNullDataInts(sampleData, i);
                flacEncoder.AddSamples(sampleData, i);
                flacEncoder.EncodeSamples(i, false);
                flacEncoder.EncodeSamples(flacEncoder.SamplesAvailableToEncode(), true);

                inputStream.Close();
                flacOut.Close();
                //var totalSamples = 0;
            }
            catch (Java.Lang.Exception ex)
            {
                ex.PrintStackTrace();
            }
            catch (System.Exception ex)
            {
                var a = 2;
            }
        }

        private int[] TruncateNullDataInts(int[] sampleData, int index)
        {
            if (index == sampleData.Length) return sampleData;
            var outt = new int[index];
            for (var i = 0; i < index; i++)
            {
                outt[i] = sampleData[i];
            }

            return outt;
        }

        private int[] short2int(short[] sData)
        {
            var length = sData.Length;
            var iData = new int[length];
            for (var i = 0; i < length; i++)
            {
                iData[i] = sData[i];
            }

            return iData;
        }
    }

    //public class ConvertCallback:Java.Lang.Object,IConvertCallback
    //{
    //    public void OnFailure(Exception p0)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void OnSuccess(File p0)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}