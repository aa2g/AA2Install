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
        public static List<string> Log = new List<string>();
        
        public static void ProcessOutputHandler(object sender, DataReceivedEventArgs e) => Log.Add(e.Data ?? string.Empty);
    }
}
