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
    static class _7z
    {
        public static event DataReceivedEventHandler OutputDataRecieved;
        /// <summary>
        /// Creates a mod index from a 7Zip file
        /// </summary>
        /// <param name="filename">Location of the 7Zip file</param>
        /// <returns>Structure containing mod info</returns>
        public static Mod Index(string filename)
        {
            Mod m = new Mod();
            List<string> oldlist = Console.Log;
            Console.Log.Clear();
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
            m.Filenames = new List<string>();
            foreach (string s in Console.Log)
            {
                Regex r = new Regex(@"^\d{4}");
                if (r.IsMatch(s))
                {
                    ModItems.Add(s);
                    string[] ss = s.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    if (ss[2] == "....A")
                    {
                        m.Filenames.Add(ss[4]);
                    }
                }
            }
            Console.Log = oldlist;
            m.Name = filename;
            if (ModItems.Count > 3)
            {
                string[] ee = ModItems[ModItems.Count - 1].Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                m.size = ulong.Parse(ee[2]);
            }
            else
            {
                m.size = 0;
            }
            return m;
        }
        /// <summary>
        /// Extracts a 7Zip archive to the temp folder
        /// </summary>
        /// <param name="filename">Location of the 7Zip file</param>
        /// <returns>Location of extracted contents</returns>
        public static string Extract(string filename)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = Paths._7Za;
                p.StartInfo.Arguments = "x \"" + filename + "\" AA2* -aos -mmt -o\"" + Paths.TEMP + "\"";
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
        /// Extracts a 7Zip archive to the temp folder (using wildcard)
        /// </summary>
        /// <param name="filename">Location of the 7Zip file</param>
        /// <param name="wildcard">Wildcard</param>
        /// <returns>Location of extracted contents</returns>
        public static string ExtractWildcard(string filename, string wildcard)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = Paths._7Za;
                p.StartInfo.Arguments = "x \"" + filename + "\" \"" + wildcard + "\" -aos -mmt -o\"" + Paths.TEMP + "\"";
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
    }
}
