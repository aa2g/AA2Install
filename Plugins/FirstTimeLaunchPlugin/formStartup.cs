using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirstTimeLaunchPlugin
{
    public partial class formStartup : Form
    {
        public formStartup()
        {
            InitializeComponent();
        }

        private void btnFAQ_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(@"..\FAQ.txt"))
                System.Diagnostics.Process.Start(@"..\FAQ.txt");
            else
                System.Diagnostics.Process.Start(@"https://raw.githubusercontent.com/aa2g/AA2Install/master/AA2Install/FAQ.txt");
        }
    }
}
