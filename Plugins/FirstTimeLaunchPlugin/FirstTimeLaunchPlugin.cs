using PluginLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstTimeLaunchPlugin
{
    public class FirstTimeLaunchPlugin : IPlugin
    {
        public string Name => "First Time Launch Plugin";

        public object Payload => new Method(Startup);

        public PluginType Type => PluginType.StartupScript;

        public Version Version => new Version(1, 0, 0, 0);

        public void Startup()
        {
            if (AA2Install.Configuration.ReadSetting("FIRSTTIME") != "True")
            {
                using (formStartup start = new formStartup())
                    AA2Install.Program.MainForm.currentOwner.Invoke(new Action(() => start.ShowDialog(AA2Install.Program.MainForm.currentOwner)));
                AA2Install.Configuration.WriteSetting("FIRSTTIME", "True");
            }
        }
    }
}
