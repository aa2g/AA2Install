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
        public string Name => ".wav ADPCM Channel Trimmer";

        public string DisplayName => "Split .wav files to mono and compress";

        public Version Version => new Version("1.1.0.0");

        public event ProgressUpdatedEventArgs ProgressUpdated;

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
                        if (wv.WaveFormat.Channels > 1 || wv.WaveFormat.Encoding != WaveFormatEncoding.Adpcm)
                        {
                            WaveFormat f = new AdpcmWaveFormat(wv.WaveFormat.SampleRate, 1);//new WaveFormat(wv.WaveFormat.SampleRate, wv.WaveFormat.BitsPerSample, 1);
                            
                            using (WaveFormatConversionStream str = new WaveFormatConversionStream(f, wv))
                            {
                                pp.Subfiles[i] = new MemSubfile(new MemoryStream(ToByteArray(str)), iw.Name);
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
