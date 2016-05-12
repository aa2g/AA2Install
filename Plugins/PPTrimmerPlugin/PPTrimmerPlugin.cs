using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginLoader;

namespace PPTrimmerPlugin
{
    public class PPTrimmerPlugin : IPlugin
    {
        public string Name => "PP Trimmer Plugin";

        public Version Version => new Version(1, 0, 0, 0);

        public PluginType Type => PluginType.UserControl;

        public object Payload => new UserControlMethod("PP Trimmer", new ucMain());
    }

    public delegate void ProgressUpdatedEventArgs(int progress);

    public interface ITrimPlugin
    {
        event ProgressUpdatedEventArgs ProgressUpdated;
        void ProcessPP(SB3Utility.ppParser pp);
        long AnalyzePP(SB3Utility.ppParser pp);
        string Name { get; }
        string DisplayName { get; }
        Version Version { get; }
    }
}
