using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SB3Utility;

namespace PPTrimmerPlugin
{
    public partial class ucPlugin : UserControl
    {
        public readonly ITrimPlugin plugin;

        public ucPlugin()
        {
            InitializeComponent();
        }

        public ucPlugin(ITrimPlugin plugin)
        {
            InitializeComponent();
            this.plugin = plugin;

            checkEnable.Text = plugin.DisplayName;

            plugin.ProgressUpdated += (p) => { prgProgress.Value = p; };
        }

        public void Execute(ppParser pp)
        {
            if (checkEnable.Checked)
            {
                plugin.ProcessPP(pp);
            }
            prgProgress.Value = 100;
        }

        public void Reload()
        {
            prgProgress.Value = 0;
        }
    }
}
