using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace AA2ModpackCreator
{
    public partial class formMain : Form
    {
        public static string Version => "1";

        public formMain()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (formAddMod add = new formAddMod())
            {
                if (add.ShowDialog() == DialogResult.OK)
                {
                    Mod m = new Mod(add.txtName.Text, new Uri(add.txtUrl.Text));

                    var item = new ListViewItem(m.Name);
                    item.SubItems.Add(m.URL.AbsoluteUri);
                    item.Tag = m;

                    lsvMods.Items.Add(item);
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lsvMods.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in lsvMods.SelectedItems)
                    item.Remove();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            string path;
            using (SaveFileDialog save = new SaveFileDialog())
            {
                save.Filter = "AA2 Modpack file (*.a2m)|*.a2m|All Files (*.*)|*.*";

                if (save.ShowDialog() != DialogResult.OK)
                    return;

                path = save.FileName;
            }

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            XmlWriter xml = XmlWriter.Create(path, settings);

            xml.WriteStartElement("AA2Modpack");

            xml.WriteAttributeString("revision", Version);
            xml.WriteElementString("title", txtTitle.Text);
            xml.WriteElementString("description", txtDescription.Text);
            xml.WriteElementString("authors", txtAuthors.Text);
            xml.WriteElementString("version", numVersion.Value.ToString());

            foreach (ListViewItem item in lsvMods.Items)
            {
                Mod m = item.Tag as Mod;

                xml.WriteStartElement("mod");
                xml.WriteElementString("name", m.Name);
                xml.WriteElementString("url", m.URL.AbsoluteUri);
                xml.WriteEndElement();
            }

            xml.WriteEndElement();

            xml.Close();

            MessageBox.Show("Created succesfully.");
        }
    }
    
    public class Mod
    {
        public string Name;
        public Uri URL;
        public Mod(string name, Uri uri)
        {
            Name = name;
            URL = uri;
        }
    }
}
