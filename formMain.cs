using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using AA2Install.Archives;
using SB3Utility;

namespace AA2Install
{
    /* TODO:
     * 
     * Nothing
     * 
     */

    [Serializable()] public struct Mod
    {
        public string Name;
        public bool Installed;
        public ulong size;
        public List<string> Filenames;
    }
    public partial class formMain : Form
    {
        public Dictionary<string, Mod> modDict = new Dictionary<string, Mod>();

        #region Console

        string minorProgress = "(0/0)"; //When processing PP files via console

        /// <summary>
        /// Handles process output to console text box
        /// </summary>
        void ProcessOutputHandler(object sender, DataReceivedEventArgs e)
        {
            Trace.WriteLine(e.Data);
            string s = e.Data ?? string.Empty;

            Match match = Regex.Match(s, @"^\((\d+)\/(\d+)\)"); //prgMinor gets updated directly from console output
            

            Console.Log.Add(s);
            this.BeginInvoke(new MethodInvoker(() =>
            {                
                rtbConsole.AppendText((s) + Environment.NewLine);
                if (match.Success)
                {
                    minorProgress = match.Value;
                    prgMinor.Value = int.Parse(match.Groups[1].Value);
                    prgMinor.Maximum = int.Parse(match.Groups[2].Value);
                }
            }));
        }
        #endregion
        #region Preferences

        private void loadUIConfiguration()
        {
            btnAA2PLAY.Enabled = txtAA2PLAY.Enabled = checkAA2PLAY.Checked = Configuration.getBool("AA2PLAY");
            txtAA2PLAY.Text = Configuration.ReadSetting("AA2PLAY_Path") ?? "";
            btnAA2EDIT.Enabled = txtAA2EDIT.Enabled = checkAA2EDIT.Checked = Configuration.getBool("AA2EDIT");
            txtAA2EDIT.Text = Configuration.ReadSetting("AA2EDIT_Path") ?? "";

            //checkRAW.Checked = Configuration.getBool("PPRAW");
        }

        private void btnAA2PLAY_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fold = new FolderBrowserDialog();
            fold.Description = @"Locate the AA2_PLAY\data folder.";
            DialogResult result = fold.ShowDialog();
            if (result == DialogResult.OK)
            {
                Configuration.WriteSetting("AA2PLAY_Path", fold.SelectedPath);
                txtAA2PLAY.Text = fold.SelectedPath;
            }
        }

        private void txtAA2PLAY_TextChanged(object sender, EventArgs e)
        {
            Configuration.WriteSetting("AA2PLAY_Path", txtAA2PLAY.Text);
        }

        private void checkAA2PLAY_CheckedChanged(object sender, EventArgs e)
        {
            if (checkAA2PLAY.Checked)
            {
                txtAA2PLAY.Enabled = true;
                btnAA2PLAY.Enabled = true;
            }
            else
            {
                txtAA2PLAY.Enabled = false;
                btnAA2PLAY.Enabled = false;
            }
            Configuration.WriteSetting("AA2PLAY", checkAA2PLAY.Checked.ToString());
        }

        private void checkAA2EDIT_CheckedChanged(object sender, EventArgs e)
        {
            if (checkAA2EDIT.Checked)
            {
                txtAA2EDIT.Enabled = true;
                btnAA2EDIT.Enabled = true;
            }
            else
            {
                txtAA2EDIT.Enabled = false;
                btnAA2EDIT.Enabled = false;
            }
            Configuration.WriteSetting("AA2EDIT", checkAA2EDIT.Checked.ToString());
        }

        private void txtAA2EDIT_TextChanged(object sender, EventArgs e)
        {
            Configuration.WriteSetting("AA2EDIT_Path", txtAA2EDIT.Text);
        }

        private void btnAA2EDIT_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fold = new FolderBrowserDialog();
            fold.Description = @"Locate the AA2_PLAY\data folder.";
            DialogResult result = fold.ShowDialog();
            if (result == DialogResult.OK)
            {
                Configuration.WriteSetting("AA2EDIT_Path", fold.SelectedPath);
                txtAA2EDIT.Text = fold.SelectedPath;
            }
        }

        #endregion
        #region Methods
        /// <summary>
        /// Refreshes the list from the /mods/ directory
        /// </summary>
        public void refreshModList()
        {
            lsvMods.Enabled = false;
            btnApply.Enabled = false;
            btnRefresh.Enabled = false;
            btnUninstall.Enabled = false;
            Paths.ISBACKUP = false;

            modDict = Configuration.loadMods();
            lsvMods.Items.Clear();
            foreach (Mod m in modDict.Values)
            {
                lsvMods.Items.Add(m.Name.Remove(0, m.Name.LastIndexOf('\\') + 1).Replace(".7z", ""), 0);
                bool backup = File.Exists(Paths.BACKUP + "\\" + m.Name.Remove(0, m.Name.LastIndexOf('\\') + 1));

                if (m.Installed && !File.Exists(m.Name) && !backup)
                {
                    lsvMods.Items[lsvMods.Items.Count - 1].ForeColor = Color.Maroon;
                }
                else if (m.Installed && File.Exists(m.Name) && !backup)
                {
                    lsvMods.Items[lsvMods.Items.Count - 1].ForeColor = Color.Goldenrod;
                }
                else if ((!m.Installed || !File.Exists(m.Name)) && backup)
                {
                    lsvMods.Items[lsvMods.Items.Count - 1].ForeColor = Color.DarkBlue;
                }
                else if (m.Installed && backup)
                {
                    lsvMods.Items[lsvMods.Items.Count - 1].ForeColor = Color.DarkGreen;
                }

                lsvMods.Items[lsvMods.Items.Count - 1].SubItems.Add((m.size / (1024)).ToString("#,## kB"));
                lsvMods.Items[lsvMods.Items.Count - 1].Tag = m;
            }

            foreach(string path in Directory.GetFiles(Paths.MODS, "*.7z", SearchOption.TopDirectoryOnly))
            {
                Mod m = _7z.Index(path);
                if (modDict.ContainsKey(m.Name))
                {
                    continue;
                }
                lsvMods.Items.Add(path.Remove(0, path.LastIndexOf('\\')+1).Replace(".7z", ""), 0);
                bool backup = File.Exists(Paths.BACKUP + "\\" + m.Name.Remove(0, m.Name.LastIndexOf('\\') + 1));

                if (m.Installed && !File.Exists(m.Name) && !backup)
                {
                    lsvMods.Items[lsvMods.Items.Count - 1].ForeColor = Color.Maroon;
                }
                else if (m.Installed && File.Exists(m.Name) && !backup)
                {
                    lsvMods.Items[lsvMods.Items.Count - 1].ForeColor = Color.Goldenrod;
                }
                else if ((!m.Installed || !File.Exists(m.Name)) && backup)
                {
                    lsvMods.Items[lsvMods.Items.Count - 1].ForeColor = Color.DarkBlue;
                }
                else if (m.Installed && backup)
                {
                    lsvMods.Items[lsvMods.Items.Count - 1].ForeColor = Color.DarkGreen;
                }

                lsvMods.Items[lsvMods.Items.Count - 1].SubItems.Add((m.size/(1024)).ToString("#,## kB"));
                lsvMods.Items[lsvMods.Items.Count - 1].Tag = m;
            }

            foreach (string path in Directory.GetFiles(Paths.BACKUP, "*.7z", SearchOption.TopDirectoryOnly))
            {
                Mod m = _7z.Index(path);
                m.Name = Paths.MODS + "\\" + m.Name.Remove(0, m.Name.LastIndexOf('\\') + 1);
                if (modDict.ContainsKey(m.Name) || File.Exists(Paths.MODS + "\\" + m.Name.Remove(0, m.Name.LastIndexOf('\\') + 1)))
                {
                    continue;
                }
                m.Name = path;
                lsvMods.Items.Add(path.Remove(0, path.LastIndexOf('\\') + 1).Replace(".7z", ""), 0);

                if (!File.Exists(m.Name))
                {
                    lsvMods.Items[lsvMods.Items.Count - 1].ForeColor = Color.Purple;
                }
                else if (!m.Installed)
                {
                    lsvMods.Items[lsvMods.Items.Count - 1].ForeColor = Color.DarkBlue;
                }
                else
                {
                    lsvMods.Items[lsvMods.Items.Count - 1].ForeColor = Color.Goldenrod;
                }

                lsvMods.Items[lsvMods.Items.Count - 1].SubItems.Add((m.size / (1024)).ToString("#,## kB"));
                lsvMods.Items[lsvMods.Items.Count - 1].Tag = m;
            }

            lsvMods.Enabled = true;
            btnApply.Enabled = true;
            btnRefresh.Enabled = true;
            btnUninstall.Enabled = true;
        }

        /// <summary>
        /// Copies a directory since C# doesn't have an inbuilt method
        /// </summary>
        /// <param name="source">Source Directory</param>
        /// <param name="target">Target Directory</param>
        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            if (!Directory.Exists(target.FullName)) { Directory.CreateDirectory(target.FullName); }
            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles())
                File.Copy(file.FullName, Path.Combine(target.FullName, file.Name), true);
                //file.CopyTo(Path.Combine(target.FullName, file.Name), true);
        }

        /// <summary>
        /// Deletes a directory where it can fail to do so (large directories like TEMP)
        /// </summary>
        /// <param name="target_dir">Target Directory</param>
        public static void TryDeleteDirectory(string target_dir)
        {
            try
            {
                Directory.Delete(target_dir, true);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Injects mods
        /// </summary>
        public void inject(bool createBackup = true)
        {
            //Reset controls
            btnApply.Enabled = false;
            btnRefresh.Enabled = false;
            btnUninstall.Enabled = false;
            prgMinor.Value = 0;
            prgMajor.Value = 0;
            prgMajor.Style = ProgressBarStyle.Marquee;
            int index = 0;

            //Check if directory exists
            if (!(Directory.Exists(Paths.AA2Play) && Directory.Exists(Paths.AA2Edit)))
            {
                updateStatus("FAILED: AA2Play/AA2Edit is not installed/cannot be found");
                btnApply.Enabled = true;
                btnRefresh.Enabled = true;
                btnUninstall.Enabled = true;
                Paths.ISBACKUP = false;
                return;
            }

            //Clear and create temp
            updateStatus("Clearing TEMP folder...");
            if (Directory.Exists(Paths.TEMP)) { TryDeleteDirectory(Paths.TEMP); }
            if (Directory.Exists(Paths.WORKING)) { TryDeleteDirectory(Paths.WORKING); }
            if (!Directory.Exists(Paths.BACKUP)) { Directory.CreateDirectory(Paths.BACKUP + @"\"); }

            Directory.CreateDirectory(Paths.TEMP + @"\");
            Directory.CreateDirectory(Paths.WORKING + @"\");
            Directory.CreateDirectory(Paths.TEMP + @"\AA2_PLAY\");
            Directory.CreateDirectory(Paths.TEMP + @"\AA2_MAKE\");

            //Check conflicts
            index = 0;
            updateStatus("Checking conflicts...");

            Dictionary<string, string> files = new Dictionary<string, string>();
            foreach (Mod m in modDict.Values)
            {
                foreach (string s in m.Filenames)
                {
                    files[s] = m.Name;
                }
            }

            bool conflict = false;
            foreach (ListViewItem item in lsvMods.Items)
            {
                if (item.Checked)
                {
                    lsvMods.Items[index].ImageIndex = 1; //Ready
                    foreach (string s in ((Mod)item.Tag).Filenames)
                    {
                        if (files.ContainsKey(s))
                        {
                            conflict = true;

                            foreach (ListViewItem i in lsvMods.Items) //Clusterfuck to find the other conflicting mod
                            {
                                Mod m = (Mod)i.Tag;
                                if (m.Filenames.Contains(s))
                                {
                                    lsvMods.Items[i.Index].ImageIndex = 3; //Triangle error
                                }
                            }

                            lsvMods.Items[item.Index].ImageIndex = 3; //Triangle error
                        }
                        files[s] = ((Mod)item.Tag).Name;
                    }
                }
                index++;
            }
            if (conflict && !Paths.ISBACKUP) //Ignore conflicts if uninstalling
            {
                updateStatus("FAILED: The highlighted mods have conflicting files");
                btnApply.Enabled = true;
                btnRefresh.Enabled = true;
                btnUninstall.Enabled = true;
                Paths.ISBACKUP = false;
                prgMinor.Value = 0;
                prgMajor.Value = 0;
                prgMajor.Style = ProgressBarStyle.Continuous;
                return;
            }

            //Extract all mods
            index = 0;
            prgMinor.Maximum = lsvMods.Items.Count;

            foreach (ListViewItem item in lsvMods.Items)
            {
                if (item.Checked)
                {
                    lsvMods.Items[index].ImageIndex = 2; //Processing
                    updateStatus("Extracting " + item.Text + "...");
                    _7z.Extract(Paths.MODS + "\\" + item.Text + ".7z");
                    lsvMods.Items[index].ImageIndex = 4; //Done
                }
                index++;
                prgMinor.Value = index;
            }

            //Build ppQueue
            index = 0;

            Queue<string> ppQueue = new Queue<string>();
            updateStatus("Creating .pp file queue...");
            List<string> tempPLAY = new List<string>(Directory.GetDirectories(Paths.TEMP + @"\AA2_PLAY", "jg2*", SearchOption.TopDirectoryOnly));
            List<string> tempEDIT = new List<string>(Directory.GetDirectories(Paths.TEMP + @"\AA2_MAKE", "jg2*", SearchOption.TopDirectoryOnly));

            foreach (string path in tempPLAY)
            {
                ppQueue.Enqueue(path);
            }
            foreach (string path in tempEDIT)
            {
                ppQueue.Enqueue(path);
            }

            prgMinor.Value = 0;
            prgMajor.Style = ProgressBarStyle.Continuous;

            //Process .pp files
            prgMajor.Maximum = ppQueue.Count;
            index = 0;
            while (ppQueue.Count > 0)
            {
                string ppRAW = ppQueue.Dequeue();
                string ppDir = ppRAW.Remove(0, ppRAW.LastIndexOf('\\') + 1);
                string pp = ppDir + ".pp";

                string destination = "";

                updateStatus("(" + (index + 1).ToString() + "/" + prgMajor.Maximum.ToString() + ") Currently injecting " + pp + "...");

                //Fetch.pp file if it exists

                switch (pp[3]) //jg2[e/p]0x_...
                {
                    case 'e':
                        //AA2EDIT
                        destination = Paths.AA2Edit;
                        break;
                    case 'p':
                        //AA2PLAY
                        destination = Paths.AA2Play;
                        break;
                    default:
                        //Unknown, check both folders
                        if (tempPLAY.Contains(pp))
                        {
                            destination = Paths.AA2Play;
                        }
                        else
                        {
                            destination = Paths.AA2Edit;
                        }
                        break;
                }

                prgMinor.Style = ProgressBarStyle.Continuous;
                prgMinor.Maximum = Directory.GetFiles(ppRAW).Length;
                int i = 1;

                ppParser p = new ppParser(destination + "\\" + pp, new ppFormat_AA2());

                foreach (ListViewItem item in lsvMods.Items)
                {
                    if (item.Checked)
                    {
                        foreach (string s in ((Mod)item.Tag).Filenames)
                        {
                            if (s.Contains(ppDir))
                            {
                                string r = s.Remove(0, 9);
                                string rs = r.Remove(0, r.LastIndexOf('\\') + 1);
                                string workingdir = Paths.WORKING + "\\BACKUP\\" + item.Text + "\\";
                                string directory;
                                if (destination == Paths.AA2Play)
                                {
                                    directory = workingdir + "AA2_PLAY\\" + r.Remove(r.LastIndexOf('\\') + 1);
                                }
                                else
                                {
                                    directory = workingdir + "AA2_MAKE\\" + r.Remove(r.LastIndexOf('\\') + 1);
                                }

                                Directory.CreateDirectory(directory);
                                foreach (IWriteFile iw in p.Subfiles)
                                {
                                    if (iw.Name == rs)
                                    {
                                        using (FileStream fs = new FileStream(directory + rs, FileMode.Create))
                                        {
                                            iw.WriteTo(fs);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (string s in Directory.GetFiles(ppRAW))
                {
                    string fname = s.Remove(0, s.LastIndexOf('\\')+1);
                    foreach (IWriteFile sub in p.Subfiles)
                    {
                        if (fname == sub.Name)
                        {
                            p.Subfiles.Remove(sub);
                            break;
                        }
                    }
                    prgMinor.Value = i;
                    p.Subfiles.Add(new Subfile(s));
                    i++;
                }
                BackgroundWorker b = p.WriteArchive(destination + "\\" + pp, createBackup, "", true);
                b.RunWorkerAsync();
                while (b.IsBusy)
                {
                    Application.DoEvents();
                }
                //Loop complete

                index++;
                prgMajor.Value = index;
            }

            int ind = 0;
            prgMinor.Value = 0;
            //Archive backups
            prgMinor.Value = 0;
            List<string> tempBackup = new List<string>(Directory.GetDirectories(Paths.WORKING + "\\BACKUP\\"));
            prgMinor.Maximum = tempBackup.Count;
            foreach (string s in tempBackup)
            {
                ind++;
                prgMinor.Value = ind;
                updateStatus("(" + ind.ToString() + "/" + tempBackup.Count.ToString() + ") Archiving backup of " + s + "...");

                string item = s.Remove(0, s.LastIndexOf('\\') + 1);
                string archive = Paths.BACKUP + "\\" + item + ".7z";
                if (Directory.Exists(s + "\\AA2_PLAY\\"))
                {
                    foreach (string sub in Directory.GetDirectories(s + "\\AA2_PLAY\\"))
                    {
                        string g = sub.Remove(0, s.Length);
                        _7z.Compress(archive, s, sub.Remove(0, s.Length + 1) + "\\");
                    }
                }
                if (Directory.Exists(s + "\\AA2_MAKE\\"))
                {
                    foreach (string sub in Directory.GetDirectories(s + "\\AA2_MAKE\\"))
                    {
                        string g = sub.Remove(0, s.Length);
                        _7z.Compress(archive, s, sub.Remove(0, s.Length + 1) + "\\");
                    }
                }
            }

            //Finish up
            prgMinor.Style = ProgressBarStyle.Continuous;
            updateStatus("Finishing up...");
            if (Paths.ISBACKUP) //If uninstalling remove all trace of existance
            {
                foreach (ListViewItem item in lsvMods.Items)
                {
                    if (item.Checked)
                    {
                        Mod m = ((Mod)item.Tag);
                        Paths.ISBACKUP = false;
                        string r = Paths.MODS + m.Name.Remove(0, m.Name.LastIndexOf("\\"));
                        Paths.ISBACKUP = true;
                        string s = Paths.MODS + m.Name.Remove(0, m.Name.LastIndexOf("\\"));
                        if (modDict.ContainsKey(r))
                        {
                            modDict.Remove(r);
                        }
                        if (File.Exists(s))
                        {
                            File.Delete(s);
                        }
                        lsvMods.Items[item.Index].Checked = false;
                    }
                }
                Paths.ISBACKUP = false;
            }

            TryDeleteDirectory(Paths.TEMP);
            TryDeleteDirectory(Paths.WORKING);

            //Add installed mods to the modlist

            foreach (ListViewItem item in lsvMods.Items)
            {
                if (item.Checked)
                {
                    Mod m = ((Mod)item.Tag);
                    m.Installed = true;
                    modDict[m.Name] = m;
                }
            }

            Configuration.saveMods(modDict);

            updateStatus("Success!");
            refreshModList();
        }
        
        #endregion
        #region UI

        /// <summary>
        /// Updates status bar
        /// </summary>
        /// <param name="status">Current status</param>
        void updateStatus(string status)
        {
            labelStatus.Text = status;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void clearConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbConsole.Text = "";
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form about = new formAbout();
            about.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            refreshModList();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            //Reset individual statuses
            int count = 0;
            foreach (ListViewItem item in lsvMods.Items)
            {
                int index = item.Index;
                lsvMods.Items[index].ImageIndex = 0; //Standby

                if (lsvMods.Items[index].Checked)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                inject(false);
            }
            else
            {
                updateStatus("ERROR: No mods have been selected (mods that have been already installed / cannot be found have been deselected)");
            }
        }

        #endregion

        public formMain()
        {
            InitializeComponent();
        }


        private void formMain_Shown(object sender, EventArgs e)
        {
            //Check if installed
            if (!(Directory.Exists(Paths.AA2Play) && Directory.Exists(Paths.AA2Edit)))
            {
                MessageBox.Show("You don't seem to have AA2Play and/or AA2Edit (properly) installed.\nPlease install it, use the registry fixer (if you've already installed it) or manually specify the install path in the preferences.", "AA2 Not Installed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            //Set event handlers
            _7z.OutputDataRecieved += new DataReceivedEventHandler(ProcessOutputHandler);

            //Create necessary folders
            if (!Directory.Exists(Paths.BACKUP)) { Directory.CreateDirectory(Paths.BACKUP + @"\"); }
            if (!Directory.Exists(Paths.MODS)) { Directory.CreateDirectory(Paths.MODS + @"\"); }

            //Start program
            loadUIConfiguration();
            refreshModList();

            //Get the damn console to scroll to bottom
            rtbConsole.VisibleChanged += (a, b) =>
            {
                if (rtbConsole.Visible)
                {
                    rtbConsole.SelectionStart = rtbConsole.TextLength;
                    rtbConsole.ScrollToCaret();
                }
            };
        }

        #region Image and Description
        int imageIndex = 1;
        List<string> imageLoop = new List<string>();
        private void listMods_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsvMods.SelectedItems.Count > 0)
            {
                imagePreview.Image = null;
                imageLoop.Clear();
                txtDescription.Text = "Loading...";

                string name = lsvMods.SelectedItems[0].Text;
                _7z.ExtractWildcard(Paths.MODS + "\\" + name + ".7z", name + "*");

                if (File.Exists(Paths.TEMP + "\\" + name + ".txt"))
                {
                    txtDescription.Text = File.ReadAllText(Paths.TEMP + "\\" + name + ".txt");
                }
                else
                {
                    txtDescription.Text = "No description found.";
                }

                if (File.Exists(Paths.TEMP + "\\" + name + ".jpg"))
                {
                    imageLoop.Add(Paths.TEMP + "\\" + name + ".jpg");
                    imagePreview.Image = new Bitmap(imageLoop[0]);
                    int index = 1;
                    string newFile = Paths.TEMP + "\\" + name + index.ToString() + ".jpg";
                    while (File.Exists(newFile))
                    {
                        imageLoop.Add(newFile);
                        index++;
                        newFile = Paths.TEMP + "\\" + name + index.ToString() + ".jpg";
                    }

                }
            
            }           
            
        }
        private void imageTimer_Tick(object sender, EventArgs e)
        {
            if (imageLoop.Count > 0)
            {
                if (File.Exists(imageLoop[(imageIndex % imageLoop.Count)]))
                {
                    imagePreview.Image.Dispose();
                    imagePreview.Image = new Bitmap(imageLoop[(imageIndex % imageLoop.Count)]);
                    GC.Collect();
                    imageIndex++;
                }
                
            }
            else
            {
                imagePreview.Image = null;
            }
        }
        #endregion

        private void colorGuideToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show(@"Not installed w/ no backup: Black
Not installed w/ backup: Dark Blue
No original w/ backup: Purple
Installed w/ backup: Green
Installed w/ no backup: Gold
Installed w/ no backup and no original: Maroon");
        }

        private void btnUninstall_Click(object sender, EventArgs e)
        {
            //Reset individual statuses
            int count = 0;
            foreach (ListViewItem item in lsvMods.Items)
            {
                int index = item.Index;
                lsvMods.Items[index].ImageIndex = 0; //Standby

                string name = ((Mod)lsvMods.Items[index].Tag).Name;
                string path = Paths.BACKUP + name.Remove(0, name.LastIndexOf("\\"));
                if (!File.Exists(path))
                {
                    lsvMods.Items[index].Checked = false; //Ignore already uninstalled mods
                }

                if (lsvMods.Items[index].Checked)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                Paths.ISBACKUP = true;
                inject(false);
            }
            else
            {
                updateStatus("ERROR: No mods have been selected (mods that have been already installed / cannot be found have been deselected)");
            }
        }
    }
}
