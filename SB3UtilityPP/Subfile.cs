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
            return new FileStream(path, FileMode.Open);
        }
    }
}
