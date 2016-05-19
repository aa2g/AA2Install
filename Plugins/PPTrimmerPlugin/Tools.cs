using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SB3Utility;
using System.IO;

namespace PPTrimmerPlugin
{
    internal class Tools
    {
        public static Stream GetReadStream(IWriteFile iw)
        {
            Stream str;

            if (iw is MemSubfile)
            {
                MemSubfile m = iw as MemSubfile;
                str = m.CreateReadStream();
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

            return str;
        }
    }
}
