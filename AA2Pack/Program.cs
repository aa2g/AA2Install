using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SB3Utility;

namespace AA2Pack
{
    class Program
    {
        static void PrintHelp()
        {
            ConsoleWriter.WriteFormat("Usage: {0}", "aa2pack ");
            ConsoleWriter.WriteLine("[switches] [files and/or folders]");

            ConsoleWriter.WriteLine();

            ConsoleWriter.WriteLine("Switches:");
            ConsoleWriter.WriteSwitch("-h", "Shows this help dialog.");
            ConsoleWriter.WriteSwitch("-uc", "Disables .pp compression.");
            ConsoleWriter.WriteSwitch("-y", "Overwrites files without prompting.");

            ConsoleWriter.WriteLine();
        }

        public static bool PromptUser(string prompt)
        {
            ConsoleWriter.WriteFormat(prompt + " [{0}/{1}]: ", "Y", "N");
            bool result = Console.ReadLine().ToLower().StartsWith("y");
            ConsoleWriter.WriteLine();
            return result;
        }
        
        static void Main(string[] args)
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif

            ConsoleWriter.WriteLineFormat("AA2Pack v{0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            ConsoleWriter.WriteLineFormat("by {0}", "drpavel");
            ConsoleWriter.WriteLine();

            if (args.Length == 0 ||
                args.ContainsSwitch("h"))
            {
                PrintHelp();
                return;
            }

            bool compress = !args.ContainsSwitch("uc");
            bool suppress = args.ContainsSwitch("y");

            foreach (string s in args)
            {

                if (File.Exists(s))
                {
                    //extracting a .pp
                    ConsoleWriter.WriteLineFormat("Processing {0}...", Path.GetFileName(s));

                    ppParser pp = new ppParser(s, new ppFormat_AA2());

                    string dirPath = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(s)), Path.GetFileNameWithoutExtension(s));

                    if (Directory.Exists(dirPath))
                    {
                        if (!suppress &&
                            !PromptUser("Are you sure you want to overwrite \"" + dirPath + "\"?"))
                            continue;
                    } 
                    else
                        Directory.CreateDirectory(dirPath);

                    int i = 0;
                    foreach (IWriteFile iw in pp.Subfiles)
                    {
                        string path = Path.Combine(dirPath, iw.Name);
                        using (FileStream fs = new FileStream(path, FileMode.Create))
                            iw.WriteTo(fs);

                        ConsoleWriter.WriteFormat("[{0}/{1}] Processed {2}... ({3})",
                            (++i).ToString(), //processed subfiles
                            pp.Subfiles.Count.ToString(), //total subfiles
                            iw.Name, //current subfile
                            string.Format("{0}", Math.Round((double)100 * i / pp.Subfiles.Count) + "%"));
                    }

                    ConsoleWriter.WriteLine();
                }
                else if (Directory.Exists(s))
                {
                    //packing into a .pp
                    
                    string localPath = s.TrimEnd('\\') + ".pp";
                    if (localPath.Contains("\\"))
                        localPath = localPath.Remove(0, s.LastIndexOf('\\'));

                    string ppPath = Path.Combine(Path.GetDirectoryName(s.TrimEnd('\\')), localPath);

                    if (File.Exists(ppPath))
                    {
                        if (!suppress &&
                            !PromptUser("Are you sure you want to overwrite \"" + ppPath + "\"?"))
                        {
                            continue;
                        }
                        else
                            File.Delete(ppPath);
                    }
                    
                    ppParser pp = new ppParser(ppPath, new ppFormat_AA2());
                    
                    foreach (string file in Directory.GetFiles(s))
                        pp.Subfiles.Add(new Subfile(file));

                    var bg = pp.WriteArchive(ppPath, false, ".bak", true, compress);

                    bg.ProgressChanged += (sender, e) =>
                    {
                        ConsoleWriter.ClearLine();
                        ConsoleWriter.WriteFormat("Writing " + localPath + "... ({0})", e.ProgressPercentage + "%");
                    };

                    bg.RunWorkerAsync();

                    while (bg.IsBusy)
                        System.Threading.Thread.Sleep(50);

                    ConsoleWriter.ClearLine();
                    ConsoleWriter.WriteLineFormat("Writing " + localPath + "... ({0})", "100%");
                }

            }
        }
    }
}
