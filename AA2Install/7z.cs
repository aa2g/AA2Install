using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace AA2Install.Archives
{
    public static class _7z
    {
        public static event DataReceivedEventHandler OutputDataRecieved;
        /// <summary>
        /// Creates a mod index from a 7Zip file
        /// </summary>
        /// <param name="filename">Location of the 7Zip file.</param>
        /// <param name="miscFiles">Whether or not to include files that aren't related to mod installation. Default is false.</param>
        /// <returns>Structure containing mod info.</returns>
        public static Mod Index(string filename, bool miscFiles = false)
        {
            List<string> oldlist = Console.ConsoleLog;
            Console.ConsoleLog.Clear();
            using (Process p = new Process())
            {
                p.StartInfo.FileName = Paths._7Za;
                p.StartInfo.Arguments = "l \"" + filename + "\"";
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;

                p.OutputDataReceived += OutputDataRecieved;
                p.Start();
                p.BeginOutputReadLine();

                while (!p.HasExited)
                {
                    Application.DoEvents();
                }
                p.WaitForExit();
            }
            List<string> ModItems = new List<string>();
            var subfiles = new List<string>();
            foreach (string s in Console.ConsoleLog)
            {
                Regex r = new Regex(@"^\d{4}");
                if (r.IsMatch(s))
                {
                    ModItems.Add(s);
                    string[] ss = s.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    if (ss[2] == "....A")
                    {
                        string n = "";
                        if (Regex.IsMatch(ss[4], @"^\d+$"))
                            for (int i = 5; i < ss.Length; i++)
                            {
                                n = n + ss[i] + " ";
                            }
                        else
                            for (int i = 4; i < ss.Length; i++)
                            {
                                n = n + ss[i] + " ";
                            }

                        if (n.StartsWith(@"AA2_MAKE\") || n.StartsWith(@"AA2_PLAY\") || miscFiles)
                            subfiles.Add(ss.Last());
                    }
                }
            }
            Console.ConsoleLog = oldlist;
            var name = filename;
            ulong size;
            if (ModItems.Count > 3)
            {
                string[] ee = ModItems[ModItems.Count - 1].Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                size = ulong.Parse(ee[2]);
            }
            else
            {
                size = 0;
            }
            return new Mod(name, size, subfiles);
        }
        /// <summary>
        /// Extracts a 7Zip archive to the temp folder
        /// </summary>
        /// <param name="filename">Location of the 7Zip file.</param>
        /// <param name="dest">Destination of extracted files.</param>
        /// <returns>Location of extracted contents.</returns>
        public static string Extract(string filename, string dest = "")
        {
            if (string.IsNullOrEmpty(dest))
                dest = Paths.TEMP;

            using (Process p = new Process())
            {
                p.StartInfo.FileName = Paths._7Za;
                p.StartInfo.Arguments = "x \"" + filename + "\" AA2* -aos -mmt -o\"" + dest + "\"";
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;

                p.OutputDataReceived += OutputDataRecieved;
                p.Start();
                p.BeginOutputReadLine();

                while (!p.HasExited)
                {
                    Application.DoEvents();
                }
            }
            return dest;
        }
        /// <summary>
        /// Extracts a 7Zip archive to the temp folder (using wildcard)
        /// </summary>
        /// <param name="filename">Location of the 7Zip file.</param>
        /// <param name="wildcard">Wildcard.</param>
        /// <returns>Location of extracted contents.</returns>
        public static string ExtractWildcard(string filename, string wildcard, string dest = "")
        {
            if (string.IsNullOrEmpty(dest))
                dest = Paths.TEMP;

            using (Process p = new Process())
            {
                p.StartInfo.FileName = Paths._7Za;
                p.StartInfo.Arguments = "x \"" + filename + "\" \"" + wildcard + "\" -aos -mmt -o\"" + dest + "\"";
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;

                p.OutputDataReceived += OutputDataRecieved;
                p.Start();
                p.BeginOutputReadLine();

                while (!p.HasExited)
                {
                    Application.DoEvents();
                }
            }
            return Paths.TEMP;
        }
        /// <summary>
        /// Compresses a specified list of files into a 7z archive.
        /// </summary>
        /// <param name="filename">Location to save the 7Z file.</param>
        /// <param name="workingdir">Working directory of 7za.exe.</param>
        /// <param name="directory">Files to compress into the archive.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public static bool Compress(string filename, string workingdir, string directory)
        {
            using (Process p = new Process())
            {

                p.StartInfo.FileName = Paths._7Za;
                p.StartInfo.WorkingDirectory = workingdir;
                p.StartInfo.Arguments = "a \"" + filename + "\" -mmt " + directory;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;

                p.OutputDataReceived += OutputDataRecieved;
                p.Start();
                p.BeginOutputReadLine();

                while (!p.HasExited)
                {
                    Application.DoEvents();
                }
            }
            return System.IO.File.Exists(filename);
        }
    }
}
