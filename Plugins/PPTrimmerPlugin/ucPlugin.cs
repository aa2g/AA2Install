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

        public void updateProgress(int progress)
        {
            if (prgProgress.InvokeRequired)
                prgProgress.Invoke(new MethodInvoker(() =>
                {
                    prgProgress.Value = progress;
                }));
            else
                prgProgress.Value = progress;
        }

        public ucPlugin(ITrimPlugin plugin)
        {
            InitializeComponent();
            this.plugin = plugin;

            checkEnable.Text = plugin.DisplayName;

            plugin.ProgressUpdated += (p) => 
            {
                updateProgress(p);
            };
        }

        public void Execute(ppParser pp)
        {
            if (checkEnable.Checked)
            {
                plugin.ProcessPP(pp);
            }
            prgProgress.Value = 100;
        }

        public long Analyze(ppParser pp)
        {
            long savings = 0;
            if (checkEnable.Checked)
            {
                savings = plugin.AnalyzePP(pp);
            }
            updateProgress(100);
            return savings;
        }

        public void Reload()
        {
            prgProgress.Value = 0;
        }
    }
}
