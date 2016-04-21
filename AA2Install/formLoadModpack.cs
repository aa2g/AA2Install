using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CG.Web.MegaApiClient;

namespace AA2Install
{
    public partial class formLoadModpack : Form
    {
        private string BytesToString(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }

            return string.Format("{0:0.##} {1}", len, sizes[order]);
        }

        public formLoadModpack()
        {
            InitializeComponent();
        }

        public formLoadModpack(Modpack m) : this()
        {
            var mega = new MegaApiClient();
            mega.LoginAnonymous();

            foreach (ModDL d in m.Mods)
            {
                ListViewItem lv = new ListViewItem();
                lv.Text = d.Name;
                lv.Tag = d;

                var node = mega.GetNodeFromLink(d.URL);

                lv.SubItems.Add(BytesToString(node.Size));

                lsvModDLs.Items.Add(lv);
            }

            Font temp = rtbDescription.SelectionFont;
            rtbDescription.SelectionFont = new Font(temp, FontStyle.Bold);

            rtbDescription.AppendText(m.Name + "\r\n");
            rtbDescription.AppendText("by " + m.Authors + "\r\n\r\n");

            rtbDescription.SelectionFont = temp;

            rtbDescription.AppendText(m.Description);
        }
    }
}
