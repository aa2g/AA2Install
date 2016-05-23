using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SB3Utility
{
    public class MemSubfile : IReadFile, IWriteFile, IDisposable
    {
        public string Name { get; set; }
        public byte[] data;

        public MemSubfile(byte[] data, string name)
        {
            this.data = data;
            Name = name;
        }

        public void WriteTo(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(CreateReadStream()))
                if (reader.BaseStream.Length > 0)
                {
                    BinaryWriter writer = new BinaryWriter(stream);
                    byte[] buf;
                    int bufsize = Utility.EstBufSize(reader.BaseStream.Length);
                    while ((buf = reader.ReadBytes(bufsize)).Length == bufsize)
                    {
                        writer.Write(buf);
                    }
                    writer.Write(buf);
                }
        }

        public Stream CreateReadStream()
        {
            return new MemoryStream(data);
        }

        public void Dispose()
        {
            data = null;
        }
    }
}
