using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using AA2Install;
using SB3Utility;

namespace PPTrimmerPlugin
{
    public partial class ucMain : UserControl
    {
        public ucMain()
        {
            InitializeComponent();
        }

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

        public void ReloadPPs()
        {
            lsvPP.Items.Clear();
            List<string> pps = Directory.GetFiles(Paths.AA2Play, "*.pp").Union(
                               Directory.GetFiles(Paths.AA2Edit, "*.pp"))
                               .ToList();

            foreach (string pp in pps)
            {
                FileInfo fi = new FileInfo(pp);
                ListViewItem item = lsvPP.Items.Add(Path.GetFileName(pp));
                item.SubItems.Add(BytesToString(fi.Length));
                item.Tag = fi;
            }
        }

        public void ReloadPlugins()
        {
            panelPlugins.Controls.Clear();

#warning actually load plugins here
            List<ITrimPlugin> plugins = InternalPlugins.AllInternalPlugins; 

            foreach (ITrimPlugin itp in plugins)
            {
                UserControl uc = new ucPlugin(itp);
                uc.Dock = DockStyle.Top;
                panelPlugins.Controls.Add(uc);
                uc.SendToBack();
            }
        }

        private void ucMain_Load(object sender, EventArgs e)
        {
            ReloadPPs();
            ReloadPlugins();
        }

        private void btnTrim_Click(object sender, EventArgs e)
        {
            prgMajor.Value = 0;
            var items = lsvPP.Items.Cast<ListViewItem>().Where(x => x.Checked);
            prgMajor.Maximum = items.Count();
            foreach (ListViewItem item in items)
            {
                FileInfo fi = item.Tag as FileInfo;
                long originalSize = fi.Length;

                ppParser pp = new ppParser(fi.FullName, new ppFormat_AA2());

                BackgroundWorker bb = new BackgroundWorker();
                bb.DoWork += (s, ev) => {
                    foreach (UserControl uc in panelPlugins.Controls)
                    {
                        ucPlugin ucp = uc as ucPlugin;
                        ucp.Invoke(new MethodInvoker(() =>
                        {
                            ucp.Execute(pp);
                        }));
                    }
                };

                bb.RunWorkerAsync();

                while (bb.IsBusy)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(50);
                }

                bb = pp.WriteArchive(pp.FilePath, false, ".bak", true, true);
                bb.ProgressChanged += (s, ev) =>
                {
                    prgMinor.Value = ev.ProgressPercentage;
                };

                bb.RunWorkerAsync();

                while (bb.IsBusy)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(50);
                }

                prgMinor.Value = 100;
                fi = new FileInfo(fi.FullName);
                long offsetSize = originalSize - fi.Length;
                item.SubItems.Add(BytesToString(offsetSize));

                prgMajor.Value++;
            }
        }
    }
}
