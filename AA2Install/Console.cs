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
        public static List<LogEntry> ProgramLog = new List<LogEntry>();
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
