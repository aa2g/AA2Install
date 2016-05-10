using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SB3Utility
{
    public class MemSubfile : IReadFile, IWriteFile
    {
        public string Name { get; set; }
        public MemoryStream data;

        public MemSubfile(MemoryStream mem, string name)
        {
            data = mem;
            Name = name;
        }

        public void WriteTo(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(CreateReadStream()))
            {
                BinaryWriter writer = new BinaryWriter(stream);
                byte[] buf;
                while ((buf = reader.ReadBytes(Utility.BufSize)).Length == Utility.BufSize)
                {
                    writer.Write(buf);
                }
                writer.Write(buf);
            }
        }

        public Stream CreateReadStream()
        {
            MemoryStream mem = new MemoryStream();
            data.Position = 0;
            data.CopyTo(mem);
            mem.Position = 0;
            return mem;
        }
    }
}
