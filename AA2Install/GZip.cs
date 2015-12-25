using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA2Install
{
    public static class GZip
    {
        /// <summary>
        /// Compresses a string in the UTF-8 format.
        /// </summary>
        /// <param name="str">String to compress.</param>
        /// <returns>Byte array of compressed contents.</returns>
        public static byte[] CompressString(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            byte[] o;

            using (var msi = new MemoryStream(bytes))
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(mso, CompressionMode.Compress))
                        msi.CopyTo(gs);

                    o = mso.ToArray();
                }
            return o;
        }

        /// <summary>
        /// Decompresses a string in the UTF-8 format.
        /// </summary>
        /// <param name="bytes">Array of compressed contents.</param>
        /// <returns>Decompressed string.</returns>
        public static string DecompressString(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                        gs.CopyTo(mso);

                    return Encoding.UTF8.GetString(mso.ToArray());
                }
        }
    }
}
