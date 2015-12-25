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
    public partial class formDelete : Form
    {
        public formDelete(string name)
        {
            InitializeComponent();
            lblName.Text = name + Environment.NewLine + "has been detected to be installed. How would you like to proceed?";
        }
    }
}
