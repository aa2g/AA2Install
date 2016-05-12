using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SB3Utility;
using NAudio.Wave;
using ImageMagick;

namespace PPTrimmerPlugin
{
    internal static class InternalPlugins
    {
        internal static List<ITrimPlugin> AllInternalPlugins => new List<ITrimPlugin>
        {
            new PPWavAudioTrimmer(),
            new PPTgaImageCompressor(),
        };
    }

    public class PPWavAudioTrimmer : ITrimPlugin
    {
        public string Name => ".wav Resampler & Trimmer";

        public string DisplayName => "Split .wav files to mono and lower quality";

        public Version Version => new Version("1.1.0.0");

        public event ProgressUpdatedEventArgs ProgressUpdated;

        public long AnalyzePP(ppParser pp)
        {
            long total = 0;

            for (int i = 0; i < pp.Subfiles.Count; i++)
            {
                IWriteFile iw = pp.Subfiles[i];

                if (!iw.Name.EndsWith(".wav"))
                    continue;

                using (MemoryStream mem = new MemoryStream())
                {
                    iw.WriteTo(mem);

                    mem.Position = 0;
                    using (WaveFileReader wv = new WaveFileReader(mem))
                    {
                        if (wv.WaveFormat.Channels > 1) // || wv.WaveFormat.Encoding != WaveFormatEncoding.Adpcm
                        {
                            total += (wv.Length / 2);
                        }
                    }

                }

                if (ProgressUpdated != null)
                    ProgressUpdated((int)Math.Floor((double)(100 * i) / pp.Subfiles.Count));
            }

            return total;
        }

        public void ProcessPP(ppParser pp)
        {
            for (int i = 0; i < pp.Subfiles.Count; i++)
            {
                IWriteFile iw = pp.Subfiles[i];

                if (!iw.Name.EndsWith(".wav"))
                    continue;

                using (MemoryStream mem = new MemoryStream())
                {
                    iw.WriteTo(mem);

                    mem.Position = 0;
                    using (WaveFileReader wv = new WaveFileReader(mem))
                    {
                        if (wv.WaveFormat.Channels > 1) // || wv.WaveFormat.Encoding != WaveFormatEncoding.Adpcm
                        {
                            WaveFormat f = new WaveFormat(wv.WaveFormat.SampleRate, 16, 1); //new AdpcmWaveFormat(wv.WaveFormat.SampleRate, 1);

                            using (MediaFoundationResampler resampledAudio = new MediaFoundationResampler(wv, f))
                            {
                                resampledAudio.ResamplerQuality = 60;
                            
                                MemoryStream o = new MemoryStream();
                                using (WaveFileWriter wr = new WaveFileWriter(o, f))
                                {
                                    int count = 0;
                                    byte[] buffer = new byte[2048];
                                    while ((count = resampledAudio.Read(buffer, 0, 2048)) > 0)
                                    {
                                        wr.Write(buffer, 0, count);
                                    }
                                    wr.Flush();
                                    pp.Subfiles[i] = new MemSubfile(new MemoryStream(ToByteArray(o)), iw.Name);
                                }
                            }
                           
                        }                        
                    }
                    
                }

                if (ProgressUpdated != null)
                    ProgressUpdated((int)Math.Floor((double)(100 * i) / pp.Subfiles.Count));
            }

            
        }

        public static byte[] ToByteArray(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(bytes, 0, (int)stream.Length);
            return bytes;
        }

    }

    public class PPTgaImageCompressor : ITrimPlugin
    {
        public string DisplayName => "Enable compression for .tga files";

        public string Name => ".tga File Compressor";

        public Version Version => new Version("1.0.1.0");

        public event ProgressUpdatedEventArgs ProgressUpdated;

        public long AnalyzePP(ppParser pp)
        {
            return 0;
            //probably not possible
            //can't be assed
        }

        public void ProcessPP(ppParser pp)
        {
            for (int i = 0; i < pp.Subfiles.Count; i++)
            {
                IWriteFile iw = pp.Subfiles[i];

                if (!iw.Name.EndsWith(".tga"))
                    continue;

                using (MemoryStream mem = new MemoryStream())
                {
                    try
                    {
                        iw.WriteTo(mem);

                        mem.Position = 0;
                        MagickImage m = new MagickImage(mem);
                        m.CompressionMethod = CompressionMethod.RLE;

                        using (MemoryStream rle = new MemoryStream())
                        {
                            m.Write(rle, MagickFormat.Tga);
                            pp.Subfiles[i] = new MemSubfile(new MemoryStream(rle.ToArray()), iw.Name);
                        }
                    }
                    catch (MagickException) { }
                }

                if (ProgressUpdated != null)
                    ProgressUpdated((int)Math.Floor((double)(100 * i) / pp.Subfiles.Count));
            }
        }
    }
}
