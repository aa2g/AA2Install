using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AA2Install
{
    public partial class formCrash : Form
    {
        public formCrash(string details, string dumpfile = "")
        {
            InitializeComponent();
            txtDetails.Text = details;
            if (dumpfile != "")
            {
                lsbFiles.Items.Add(dumpfile);
            }
        }

        public void launchLink(string link)
        {
            System.Diagnostics.Process.Start(link);
        }

        private void linkGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            launchLink(@"https://github.com/aa2g/AA2Install/issues/new");
        }

        private void linkaa2g_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            launchLink(@"https://boards.4chan.org/vg/catalog#s=aa2g");
        }

        private void linkHongfire_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            launchLink(@"http://www.hongfire.com/forum/showthread.php/447075-AA2Install-Open-source-mod-installer-Prerelease-5-2-2");
        }
    }
}
