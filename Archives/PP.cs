using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace AA2Install.Archives
{
    static class PP
    {
        public static event DataReceivedEventHandler OutputDataRecieved;
        /// <summary>
        /// Extracts a .pp file
        /// </summary>
        /// <param name="filename">Location of .pp file</param>
        /// <returns>Location of extracted files</returns>
        public static string Extract(string filename)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = Paths.AA2Decrypt;
                p.StartInfo.Arguments = "\"" + filename + "\"";
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
            return filename.Replace(".pp", "");
        }
        /// <summary>
        /// Creates a .pp file
        /// </summary>
        /// <param name="foldername">Folder to turn into a .pp file</param>
        /// <returns>Location of .pp file</returns>
        public static string Create(string foldername)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = Paths.AA2Decrypt;
                p.StartInfo.Arguments = "\"" + foldername + "\"";
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
            return foldername + ".pp";
        }
    }
}
