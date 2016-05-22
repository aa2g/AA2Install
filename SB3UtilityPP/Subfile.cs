using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SB3Utility
{
    public class Subfile: IReadFile, IWriteFile
    {
        public string Name { get; set; }
        public string path;

        public Subfile(string filepath)
        {
            path = filepath;
            Name = path.Remove(0, path.LastIndexOf('\\')+1);
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
            return new FileStream(path, FileMode.Open);
        }
    }
}
