using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using SevenZipNET;

namespace AA2Install.Archives
{
    public static class _7z
    {
        public delegate void ProgressUpdatedEventArgs(int progress);
        public static event ProgressUpdatedEventArgs ProgressUpdated;

        /// <summary>
        /// Creates a mod index from a 7Zip file
        /// </summary>
        /// <param name="filename">Location of the 7Zip file.</param>
        /// <param name="miscFiles">Whether or not to include files that aren't related to mod installation. Default is false.</param>
        /// <returns>Structure containing mod info.</returns>
        public static Mod Index(string filename, bool miscFiles = false)
        {
            SevenZipExtractor z = new SevenZipExtractor(filename);
            
            var subfiles = new List<string>();
            foreach (var d in z.Files)
            {
                string s = d.Filename;
                if (!d.Attributes.HasFlag(Attributes.Directory) &&
                    (miscFiles ||
                    (s.StartsWith(@"AA2_MAKE\") ||
                    s.StartsWith(@"AA2_PLAY\"))))
                {
                    subfiles.Add(s);
                }
            }

            return new Mod(filename, z.UnpackedSize, subfiles);
        }
        /// <summary>
        /// Extracts a 7Zip archive to the temp folder
        /// </summary>
        /// <param name="filename">Location of the 7Zip file.</param>
        /// <param name="dest">Destination of extracted files.</param>
        /// <returns>Location of extracted contents.</returns>
        public static void Extract(string filename, string dest = "")
        {
            Extract(filename, "AA2_*", dest);
        }
        /// <summary>
        /// Extracts a 7Zip archive to the temp folder (using wildcard)
        /// </summary>
        /// <param name="filename">Location of the 7Zip file.</param>
        /// <param name="wildcard">Wildcard.</param>
        /// <returns>Location of extracted contents.</returns>
        public static void Extract(string filename, string wildcard, string dest = "")
        {
            if (dest == "")
                dest = Paths.TEMP;

            SevenZipExtractor z = new SevenZipExtractor(filename);

            z.ProgressUpdated += (i) =>
            {
                var invoke = ProgressUpdated;
                if (invoke != null)
                {
                    invoke(i);
                }
            };

            z.ExtractWildcard(dest + "\\", wildcard);
        }
        /// <summary>
        /// Compresses a specified list of files into a 7z archive.
        /// </summary>
        /// <param name="filename">Location to save the 7Z file.</param>
        /// <param name="workingdir">Working directory of 7za.exe.</param>
        /// <param name="directory">Files to compress into the archive.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public static void Compress(string filename, string workingdir, string directory)
        {
            SevenZipCompressor z = new SevenZipCompressor(filename);

            z.CompressDirectory(directory, workingdir);
        }
    }
}
