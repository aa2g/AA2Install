using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using SevenZip;
using System.IO;

namespace AA2Install.Archives
{
    public static class _7z
    {
        /// <summary>
        /// Creates a mod index from a 7Zip file
        /// </summary>
        /// <param name="filename">Location of the 7Zip file.</param>
        /// <param name="miscFiles">Whether or not to include files that aren't related to mod installation. Default is false.</param>
        /// <returns>Structure containing mod info.</returns>
        public static Mod Index(string filename, bool miscFiles = false)
        {
            SevenZipBase.SetLibraryPath(Paths._7Za);
            Mod m;
            using (SevenZipExtractor z = new SevenZipExtractor(filename))
            {
                List<string> ModItems = new List<string>();
                var subfiles = new List<string>();
                foreach (var d in z.ArchiveFileData)
                {
                    string s = d.FileName;
                    if (!d.FileName.EndsWith(".empty") && !d.IsDirectory && (s.StartsWith(@"AA2_MAKE\") || s.StartsWith(@"AA2_PLAY\") || miscFiles))
                        subfiles.Add(s);
                }
                var name = filename;
                ulong size = (ulong)z.UnpackedSize;
                m = new Mod(name, size, subfiles);
            }
            return m;
        }
        /// <summary>
        /// Extracts a 7Zip archive to the temp folder
        /// </summary>
        /// <param name="filename">Location of the 7Zip file.</param>
        /// <param name="dest">Destination of extracted files.</param>
        /// <returns>Location of extracted contents.</returns>
        public static void Extract(string filename, string dest = "")
        {
            Extract(filename, "^AA2_.+", dest);
        }
        /// <summary>
        /// Extracts a 7Zip archive to the temp folder (using wildcard)
        /// </summary>
        /// <param name="filename">Location of the 7Zip file.</param>
        /// <param name="wildcard">Wildcard.</param>
        /// <returns>Location of extracted contents.</returns>
        public static void Extract(string filename, string regex, string dest = "")
        {
            if (dest == "")
                dest = Paths.TEMP;

            SevenZipBase.SetLibraryPath(Paths._7Za);
            using (SevenZipExtractor z = new SevenZipExtractor(filename))
            {
                Regex r = new Regex(regex.Replace("[", "\\["));
                List<int> indicies = new List<int>();
                foreach (var d in z.ArchiveFileData)
                {
                    if (!d.FileName.EndsWith(".empty") && r.IsMatch(d.FileName))
                        indicies.Add(d.Index);
                    else if (d.FileName.EndsWith(".empty"))
                    {
                        Directory.CreateDirectory(Path.Combine(dest, d.FileName.Remove(d.FileName.Length - 7)));
                    }
                }

                z.ExtractFiles(dest, indicies.ToArray());       
            }
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
            SevenZipBase.SetLibraryPath(Paths._7Za);

            SevenZipCompressor z = new SevenZipCompressor();

            z.CompressionLevel = CompressionLevel.Fast;
            z.CompressionMethod = CompressionMethod.Lzma2;
            if (File.Exists(filename))
                z.CompressionMode = CompressionMode.Append;
            else
                z.CompressionMode = CompressionMode.Create;

            string fulldir = Path.Combine(workingdir, directory);

            File.WriteAllText(fulldir + "\\.empty", "");

            Dictionary<string, string> files = new Dictionary<string, string>();

            foreach (string f in Directory.GetFiles(fulldir, "*", SearchOption.AllDirectories))
            {
                files[f.Remove(0, workingdir.Length + 1).Replace('\\', '/')] = f;
            }
            
            z.CompressFileDictionary(files, filename);
            /*Dictionary<int, string> ToDelete = new Dictionary<int, string>();

            using (SevenZipExtractor e = new SevenZipExtractor(filename))
                foreach (var f in e.ArchiveFileData)
                    if (f.FileName.EndsWith(".empty"))
                        ToDelete[f.Index] = "";

            z.ModifyArchive(filename, ToDelete);*/
        }
    }
}
