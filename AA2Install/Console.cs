using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AA2Install
{
    public static class Console
    {
        public static List<string> ConsoleLog = new List<string>();
        public static List<LogEntry> ProgramLog = new List<LogEntry>();

        public static void ProcessOutputHandler(object sender, DataReceivedEventArgs e)
        {
            if (e != null)
                ConsoleLog.Add(e.Data ?? string.Empty);
        }

        public static void InitializeOutput()
        {
            Archives._7z.OutputDataRecieved += new DataReceivedEventHandler(ProcessOutputHandler);
        }
    }

    [Serializable()]
    public class LogEntry
    {
        public string log;
        public DateTime time;
        public formMain.LogIcon logicon;

        public LogEntry() { }
        public LogEntry(string entry, formMain.LogIcon icon)
        {
            log = entry;
            time = DateTime.Now;
            logicon = icon;
        }
    }
}
