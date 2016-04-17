using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AA2ModpackCreator
{
    public partial class formAddMod : Form
    {
        public formAddMod()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                Uri temp = new Uri(txtUrl.Text);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (UriFormatException)
            {
                MessageBox.Show("The provided URL is not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
