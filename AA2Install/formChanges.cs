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
    public partial class formChanges : Form
    {
        ToolStripMenuItem chk;
        public formChanges(ToolStripMenuItem check)
        {
            InitializeComponent();
            chk = check;
        }

        private void formChanges_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;
            chk.Checked = false;
            Hide();
        }

        private void formChanges_Activated(object sender, EventArgs e)
        {
            this.Opacity = 1.00;
        }

        private void formChanges_Deactivate(object sender, EventArgs e)
        {
            try { this.Opacity = 0.75; }
            catch { }
        }
    }
}
