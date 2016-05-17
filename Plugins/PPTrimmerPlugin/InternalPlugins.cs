using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SB3Utility;
using NAudio.Wave;
using ImageMagick;
using NAudio.FileFormats.Wav;

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

        public readonly int SampleRate = 32000;

        public event ProgressUpdatedEventArgs ProgressUpdated;

        public long AnalyzePP(ppParser pp)
        {
            long total = 0;

            for (int i = 0; i < pp.Subfiles.Count; i++)
            {
                IWriteFile iw = pp.Subfiles[i];

                if (!iw.Name.EndsWith(".wav"))
                    continue;

                Stream str;
                
                if (iw is MemSubfile)
                {
                    MemSubfile m = iw as MemSubfile;
                    str = m.CreateReadStream();

                    str.Position = 0;
                }
                else if (iw is IReadFile)
                {
                    IReadFile p = iw as IReadFile;
                    str = p.CreateReadStream();
                }
                else
                {
                    str = new MemoryStream();
                    iw.WriteTo(str);

                    str.Position = 0;
                }

                if (str.CanSeek && str.Length == 0)
                {
                    str.Close();
                    continue;
                }

                using (str)
                {
                    WaveFileChunkReader wv = new WaveFileChunkReader();
                    WaveFormat f = wv.ReadWaveHeader(str);
                    long length = wv.DataChunkLength;
                    long remaining = length;
                    if (f.Channels > 1) // || wv.WaveFormat.Encoding != WaveFormatEncoding.Adpcm
                    {
                        total += (length / 2);
                        remaining /= 2;
                    }
                    if (f.SampleRate > SampleRate)
                    {
                        total += remaining - (long)(((float)SampleRate / f.SampleRate) * remaining);
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
                        if (wv.WaveFormat.Channels > 1 || wv.WaveFormat.SampleRate > SampleRate) // || wv.WaveFormat.Encoding != WaveFormatEncoding.Adpcm
                        {
                            WaveFormat f = new WaveFormat(SampleRate, 16, 1); //new AdpcmWaveFormat(wv.WaveFormat.SampleRate, 1);

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
                                    pp.Subfiles[i] = new MemSubfile(ToByteArray(o), iw.Name);
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
                            pp.Subfiles[i] = new MemSubfile(rle.ToArray(), iw.Name);
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
