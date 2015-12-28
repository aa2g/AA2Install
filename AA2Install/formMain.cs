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
using System.Drawing.Drawing2D;
using System.Collections;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Threading;

namespace AA2Install
{
    public partial class formMain : Form
    {
        public ModDictionary modDict = new ModDictionary();

        #region Preferences

        public void loadUIConfiguration()
        {
            btnAA2PLAY.Enabled = txtAA2PLAY.Enabled = checkAA2PLAY.Checked = Configuration.getBool("AA2PLAY");
            txtAA2PLAY.Text = Configuration.ReadSetting("AA2PLAY_Path") ?? "";
            btnAA2EDIT.Enabled = txtAA2EDIT.Enabled = checkAA2EDIT.Checked = Configuration.getBool("AA2EDIT");
            txtAA2EDIT.Text = Configuration.ReadSetting("AA2EDIT_Path") ?? "";
            checkConflicts.Checked = Configuration.getBool("CONFLICTS");

            lblEditPath.Text = "Current AA2_EDIT path: " + Paths.AA2Edit;
            lblPlayPath.Text = "Current AA2_PLAY path: " + Paths.AA2Play;

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

            lblEditPath.Text = "Current AA2_EDIT path: " + Paths.AA2Edit;
            lblPlayPath.Text = "Current AA2_PLAY path: " + Paths.AA2Play;
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

            lblEditPath.Text = "Current AA2_EDIT path: " + Paths.AA2Edit;
            lblPlayPath.Text = "Current AA2_PLAY path: " + Paths.AA2Play;
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

            lblEditPath.Text = "Current AA2_EDIT path: " + Paths.AA2Edit;
            lblPlayPath.Text = "Current AA2_PLAY path: " + Paths.AA2Play;
        }

        private void txtAA2EDIT_TextChanged(object sender, EventArgs e)
        {
            Configuration.WriteSetting("AA2EDIT_Path", txtAA2EDIT.Text);

            lblEditPath.Text = "Current AA2_EDIT path: " + Paths.AA2Edit;
            lblPlayPath.Text = "Current AA2_PLAY path: " + Paths.AA2Play;
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

        private void checkConflicts_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.WriteSetting("CONFLICTS", checkConflicts.Checked.ToString());
        }

        #endregion
        #region Methods
        /// <summary>
        /// Sets enabled status of essential controls.
        /// </summary>
        /// <param name="enabled">Sets enabled status to this.</param>
        public void setEnabled(bool enabled)
        {
            lsvMods.Enabled = btnApply.Enabled = btnRefresh.Enabled =
                checkAA2EDIT.Enabled = txtAA2EDIT.Enabled = btnAA2EDIT.Enabled =
                checkAA2PLAY.Enabled = txtAA2PLAY.Enabled = btnAA2PLAY.Enabled =
                checkConflicts.Enabled = btnMigrate.Enabled = btnBrowseMigrate.Enabled =
                txtMigrate.Enabled = txtBrowseMigrate.Enabled = btnCancel.Enabled = 
                cmbSorting.Enabled = enabled;
        }

        /// <summary>
        /// Returns a list of files in a directory.
        /// </summary>
        /// <param name="SourceFolder">Directory to search.</param>
        /// <param name="Filter">Filter; can have multiple filters split by '|'.</param>
        /// <param name="searchOption">Option to search by.</param>
        /// <returns>List of files in directory.</returns>
        public string[] getFiles(string SourceFolder, string Filter, System.IO.SearchOption searchOption)
        {
            ArrayList alFiles = new ArrayList();
            string[] MultipleFilters = Filter.Split('|');
            foreach (string FileFilter in MultipleFilters)
            {
                alFiles.AddRange(Directory.GetFiles(SourceFolder, FileFilter, searchOption));
            }
            return (string[])alFiles.ToArray(typeof(string));
        }

        /// <summary>
        /// Flag for if cancellation is pending.
        /// </summary>
        public bool cancelPending = false;
        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancelPending = true;
            updateStatus("Pending cancellation...", LogIcon.Warning);
            updateStatus("Pending cancellation...", LogIcon.Warning, false, true);
        }
        /// <summary>
        /// Cancels if cancelPending is set to true.
        /// </summary>
        /// <returns></returns>
        public bool tryCancel()
        {
            if (cancelPending)
            {
                setEnabled(true);
                updateStatus("User cancelled operation.", LogIcon.Error);
                updateStatus("User cancelled operation.", LogIcon.Error, false, true);
                refreshModList();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to delete file, and prompts user if the file is being accessed.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public TryDeleteResult tryDelete(string filename)
        {
            bool tryAgain = false;

            do
            {

                try
                {
                    if (File.Exists(filename))
                    {
                        using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite)) { }
                        File.Delete(filename);
                    }
                    tryAgain = false;
                }
                catch (IOException)
                {
                    switch (MessageBox.Show("Failed to delete file " + filename + "; is it being accessed?", "Failed", MessageBoxButtons.AbortRetryIgnore))
                    {
                        case DialogResult.Retry:
                            tryAgain = true;
                            break;
                        case DialogResult.Ignore:
                            return TryDeleteResult.Ignored;
                        case DialogResult.Abort:
                            return TryDeleteResult.Cancelled;
                    }
                }

            } while (tryAgain);

            return TryDeleteResult.OK;
        }

        /// <summary>
        /// Refreshes the list from the /mods/ directory.
        /// </summary>
        public void refreshModList(bool skipReload = false, string filter = "")
        {
            lsvMods.Items.Clear();
            modDict = Configuration.loadMods();
            if (!skipReload)
            {
                setEnabled(false);
                initializeBench();
                
                string[] paths = getFiles(Paths.MODS, "*.7z|*.zip", SearchOption.TopDirectoryOnly);
                prgMajor.Maximum = paths.Length;
                prgMinor.Style = ProgressBarStyle.Marquee;
            
                int i = 0;
            
                foreach (string path in paths)
                {
                    i++;
                    updateStatus("(" + i.ToString() + "/" + prgMajor.Maximum.ToString() + ") Processing " + path.Remove(0, path.LastIndexOf('\\') + 1) + "...");

                    bool flag = false;
                    foreach (Mod m in modDict.Values)
                    {
                        if (path == m.Filename)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        prgMajor.Value = i;
                        continue;
                    }

                    modDict[path] = _7z.Index(path);

                    prgMajor.Value = i;
                }
            }

            foreach (Mod m in modDict.Values.ToList())
            {
                //bool backup = File.Exists(Paths.BACKUP + "\\" + m.Name.Remove(0, m.Name.LastIndexOf('\\') + 1).Replace(".zip", ".7z"));

                if (!File.Exists(m.Filename) && !m.Installed)
                {
                    modDict.Remove(m.Name);
                    continue;
                }

                if (!m.Name.Replace(".7z", "").Replace(".zip", "").ToLower().Contains(filter.ToLower()) && filter != "")
                    continue;

                lsvMods.Items.Add(m.Name, m.Name.Replace(".7z", "").Replace(".zip", ""), 0);
                int index = lsvMods.Items.IndexOfKey(m.Name);

                if (m.Installed && !m.Exists)
                {
                    //lsvMods.Items[index].BackColor = Color.DarkBlue;
                }
                else if (m.Installed)
                {
                    lsvMods.Items[index].BackColor = Color.LightGreen;
                    lsvMods.Items[index].Checked = true;
                }

                m.Properties["Estimated Size"] = (m.Size / (1024)).ToString("#,## kB");

                if (m.Installed && m.InstallTime.Year > 2000)
                    m.Properties["Installed on"] = m.InstallTime.ToString();
                else
                    m.Properties.Remove("Installed on");

                lsvMods.Items[index].Tag = m;
                
                //modDict[m.Name] = m;
            }

            if (!skipReload)
            {
                Configuration.saveMods(modDict);

                prgMinor.Style = ProgressBarStyle.Continuous;

                prgMinor.Value = prgMinor.Maximum;
                updateStatus("Ready.", LogIcon.OK, false);


                setEnabled(true);
            }            
        }

        /// <summary>
        /// Deletes a directory where it can fail to do so (large directories like TEMP).
        /// </summary>
        /// <param name="target_dir">Target directory.</param>
        public static void TryDeleteDirectory(string target_dir)
        {
            try
            {
                Directory.Delete(target_dir, true);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Injects mods as per selected in lsvMods.
        /// </summary>
        /// <param name="createBackup">Creates a backup of modified .pp files if true. Default is false.</param>
        /// <param name="checkConflicts">Checks for conflicts in pending mods. Default is true.</param>
        /// <param name="suppressPopups">If true, does not generate message boxes. Default is false.</param>
        public bool inject(bool createBackup = false, bool checkConflicts = true, bool suppressPopups = false)
        {
            initializeBench();
            cancelPending = false;
            updateStatus("Compiling changes...", LogIcon.Processing);
            //Compile changes
            Queue<string> prearc = new Queue<string>();
            Queue<string> postarc = new Queue<string>();
            List<string> rsub = new List<string>();
            //List<string> arc = new List<string>();
            List<Mod> mods = new List<Mod>();
            List<Mod> unmods = new List<Mod>();
            foreach (ListViewItem l in lsvMods.Items)
            {
                Mod m = (Mod)l.Tag;
                if (l.Checked ^ (m.Installed))
                {
                    if (l.Checked)
                    {
                        postarc.Enqueue(m.Filename);
                        mods.Add(m);
                    }
                    else
                    {
                        prearc.Enqueue(Paths.BACKUP + "\\" + m.Name.Replace(".zip", ".7z"));
                        unmods.Add(m);
                        rsub.AddRange(m.SubFilenames);
                    }
                }
            }

            if (prearc.Count + postarc.Count == 0)
            {
                updateStatus("No changes in selected mods found.", LogIcon.Error, false);
                updateStatus("FAILED: No changes in selected mods", LogIcon.Error, false, true);
                return false;
            }

            //while (prearc.Count > 0)
            //arc.Add(prearc.Dequeue());
            //while (postarc.Count > 0)
            //arc.Add(postarc.Dequeue());

            //Reset controls
            setEnabled(false);
            btnCancel.Enabled = true;
            prgMinor.Value = 0;
            prgMajor.Value = 0;
            prgMajor.Style = ProgressBarStyle.Marquee;
            int index = 0;

            //Check if directory exists
            if (!(Directory.Exists(Paths.AA2Play) && Directory.Exists(Paths.AA2Edit)))
            {
                updateStatus("AA2Play/AA2Edit is not installed/cannot be found", LogIcon.Error, false);
                updateStatus("FAILED: AA2Play/AA2Edit is not installed/cannot be found", LogIcon.Error, false, true);
                btnApply.Enabled = true;
                btnRefresh.Enabled = true;
                return false;
            }

            refreshModList(true);

            foreach (ListViewItem l in lsvMods.Items)
            {
                Mod m = (Mod)l.Tag;
                foreach (Mod n in mods)
                    if (n.Filename == m.Filename)
                        l.Checked = true;
                
                foreach (Mod n in unmods)
                    if (n.Filename == m.Filename)
                        l.Checked = false;
            }

                //Clear and create temp
            updateStatus("Clearing TEMP folder...");
            updateStatus("Clearing temporary folders...");

            if (Directory.Exists(Paths.TEMP))
                TryDeleteDirectory(Paths.TEMP);

            if (Directory.Exists(Paths.WORKING))
                TryDeleteDirectory(Paths.WORKING); 

            if (!Directory.Exists(Paths.BACKUP))
                Directory.CreateDirectory(Paths.BACKUP + @"\"); 

            
            Directory.CreateDirectory(Paths.TEMP + @"\");
            Directory.CreateDirectory(Paths.WORKING + @"\");
            Directory.CreateDirectory(Paths.TEMP + @"\AA2_PLAY\");
            Directory.CreateDirectory(Paths.TEMP + @"\AA2_MAKE\");
            Directory.CreateDirectory(Paths.TEMP + @"\BACKUP\");
            Directory.CreateDirectory(Paths.TEMP + @"\BACKUP\AA2_PLAY\");
            Directory.CreateDirectory(Paths.TEMP + @"\BACKUP\AA2_MAKE\");

            //Check conflicts
            if (checkConflicts) { 
                index = 0;
                updateStatus("Checking conflicts...");

                Dictionary<string, string> files = new Dictionary<string, string>();
                List<string> conflicts = new List<string>();
                foreach (Mod m in modDict.Values)
                {
                    if (!m.Installed || 
                        !lsvMods.Items[lsvMods.Items.IndexOfKey(m.Name)].Checked)
                        continue;

                    foreach (string s in m.SubFilenames)
                    {
                        files[s] = m.Name;
                    }
                }

                foreach (ListViewItem item in lsvMods.Items)
                {
                    lsvMods.Items[item.Index].BackColor = Color.Transparent;
                }

                bool conflict = false;
                foreach (ListViewItem item in lsvMods.Items)
                {
                    if (item.Checked)
                    {
                        lsvMods.Items[index].ImageIndex = 1; //Ready
                        foreach (string s in ((Mod)item.Tag).SubFilenames)
                        {
                            if (files.ContainsKey(s))
                            {
                                if (files[s] == ((Mod)item.Tag).Name)
                                    break;
                                conflict = true;

                                foreach (ListViewItem i in lsvMods.Items) //Clusterfuck to find the other conflicting mod
                                {
                                    Mod m = (Mod)i.Tag;
                                    if (m.SubFilenames.Contains(s) && i.Checked)
                                    {
                                        //lsvMods.Items[i.Index].ImageIndex = 3; //Triangle error
                                        //updateStatus(i.Text + ": " + s, LogIcon.Error);
                                        string temp = i.Text + ": " + s;
                                        if (!conflicts.Contains(temp))
                                            conflicts.Add(temp);
                                        lsvMods.Items[i.Index].BackColor = Color.FromArgb(255, 255, 111);
                                    }
                                }

                                //lsvMods.Items[item.Index].ImageIndex = 3; //Triangle error
                                //updateStatus(item.Text + ": " + s, LogIcon.Error);
                                string temp2 = item.Text + ": " + s;
                                if (!conflicts.Contains(temp2))
                                    conflicts.Add(temp2);
                                lsvMods.Items[item.Index].BackColor = Color.FromArgb(255, 255, 111);
                            }
                            files[s] = ((Mod)item.Tag).Name;
                        }
                    }
                    index++;
                }
                if (conflict)
                {
                    foreach (string c in conflicts)
                        updateStatus(c, LogIcon.Error, false);
                    updateStatus("Collision detected.", LogIcon.Error, false);
                    updateStatus("FAILED: The highlighted mods have conflicting files", LogIcon.Ready, false, true);
                    MessageBox.Show("Some mods have been detected to have conflicting files.\nYou can use the log to manually fix the conflicting files in the mods (if they can be fixed) or you can proceed anyway by changing the relevant setting in the preferences.\nNote: if you proceed anyway, to uninstall you must uninstall mods in the reverse order you installed them to ensure that wrong files are not left behind.", "Collision detected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    setEnabled(true);
                    prgMinor.Value = 0;
                    prgMajor.Value = 0;
                    prgMajor.Style = ProgressBarStyle.Continuous;
                    return false;
                }
            }


            //Extract all mods
            
            index = 0;
            prgMinor.Maximum = prearc.Count + postarc.Count;

            foreach (string item in prearc)
            {
                updateStatus("(" + (index + 1).ToString() + "/" + prgMinor.Maximum.ToString() + ") Extracting " + item.Remove(0, item.LastIndexOf('\\') + 1) + "...");
                _7z.Extract(item, Paths.TEMP + @"\BACKUP\");
                index++;
                prgMinor.Value = index;
                if (tryCancel())
                    return false;
            }

            foreach (string item in postarc)
            {
                updateStatus("(" + (index + 1).ToString() + "/" + prgMinor.Maximum.ToString() + ") Extracting " + item.Remove(0, item.LastIndexOf('\\') + 1) + "...");
                _7z.Extract(item);
                index++;
                prgMinor.Value = index;
                if (tryCancel())
                    return false;
            }

            if (tryCancel())
                return false;

            //Reached point of no return.
            btnCancel.Enabled = false;

            //Build ppQueue
            index = 0;
           
            Queue<basePP> ppQueue = new Queue<basePP>();
            List<basePP> ppList = new List<basePP>();
            updateStatus("Creating .pp file queue...");


            List<string> tempPLAY = new List<string>(Directory.GetDirectories(Paths.TEMP + @"\BACKUP\AA2_PLAY", "jg2*", SearchOption.TopDirectoryOnly));
            List<string> tempEDIT = new List<string>(Directory.GetDirectories(Paths.TEMP + @"\BACKUP\AA2_MAKE", "jg2*", SearchOption.TopDirectoryOnly));

            foreach (string path in tempPLAY)
            {
                ppQueue.Enqueue(new basePP(path, Paths.AA2Play));
            }
            foreach (string path in tempEDIT)
            {
                ppQueue.Enqueue(new basePP(path, Paths.AA2Edit));
            }

            while (ppQueue.Count > 0)
            {
                basePP bp = ppQueue.Dequeue();

                var r = rsub.ToArray();
                foreach (string s in r)
                {
                    foreach (IWriteFile iw in bp.pp.Subfiles)
                        if (bp.ppDir + "\\" + iw.Name == s.Remove(0, 9))
                        {
                            rsub.Remove(s);
                            bp.pp.Subfiles.Remove(iw);
                            break;
                        }
                }

                prgMinor.Style = ProgressBarStyle.Continuous;
                prgMinor.Maximum = Directory.GetFiles(bp.ppRAW).Length;
                int i = 1;

                foreach (string s in Directory.GetFiles(bp.ppRAW))
                {
                    string fname = s.Remove(0, s.LastIndexOf('\\') + 1);
                    foreach (IWriteFile sub in bp.pp.Subfiles)
                    {
                        if (fname == sub.Name)
                        {
                            bp.pp.Subfiles.Remove(sub);
                            break;
                        }
                    }
                    prgMinor.Value = i;
                    bp.pp.Subfiles.Add(new Subfile(s));
                    i++;
                }

                ppList.Add(bp);
            }

            tempPLAY = new List<string>(Directory.GetDirectories(Paths.TEMP + @"\AA2_PLAY", "jg2*", SearchOption.TopDirectoryOnly));
            tempEDIT = new List<string>(Directory.GetDirectories(Paths.TEMP + @"\AA2_MAKE", "jg2*", SearchOption.TopDirectoryOnly));


            foreach (string path in tempPLAY)
            {
                var p = new basePP(path, Paths.AA2Play);
                var o = ppList.Find(x => x.ppDir == p.ppDir);
                if (o != null)
                {
                    p.pp = o.pp;
                    ppList.Remove(o);
                }
                ppQueue.Enqueue(p);
            }
            foreach (string path in tempEDIT)
            {
                var p = new basePP(path, Paths.AA2Edit);
                var o = ppList.Find(x => x.ppDir == p.ppDir);
                if (o != null)
                {
                    p.pp = o.pp;
                    ppList.Remove(o);
                }
                ppQueue.Enqueue(p);
            }

            int ii = 0;
            prgMinor.Maximum = ppList.Count();
            foreach (basePP b in ppList)
            {
                updateStatus("(" + (ii + 1).ToString() + "/" + prgMinor.Maximum.ToString() + ") Reverting " + b.ppFile + "...");
                if (b.pp.Subfiles.Count > 0)
                {
                    BackgroundWorker bb = b.pp.WriteArchive(b.pp.FilePath, createBackup, "", true);
                    bb.RunWorkerAsync();
                    while (bb.IsBusy)
                    {
                        Application.DoEvents();
                    }
                }
                else
                {
                    File.Delete(b.pp.FilePath);
                }
            }

            prgMinor.Value = 0;
            prgMajor.Style = ProgressBarStyle.Continuous;

            //Process .pp files
            prgMajor.Maximum = ppQueue.Count;
            updateTaskbarProgress();
            index = 0;
            while (ppQueue.Count > 0)
            {
                basePP b = ppQueue.Dequeue();

                updateStatus("(" + (index + 1).ToString() + "/" + prgMajor.Maximum.ToString() + ") Injecting " + b.ppFile + "...");

                prgMinor.Style = ProgressBarStyle.Continuous;
                prgMinor.Maximum = Directory.GetFiles(b.ppRAW).Length;
                int i = 1;

                foreach (Mod m in mods)
                {
                    foreach (string s in m.SubFilenames)
                    {
                        if (s.Contains(b.ppDir))
                        {
                            string r = s.Remove(0, 9);
                            string rs = r.Remove(0, r.LastIndexOf('\\') + 1);
                            string workingdir = Paths.WORKING + "\\BACKUP\\" + m.Name.Replace(".7z", "").Replace(".zip", "") + "\\";
                            string directory;
                            if (tempPLAY.Contains(b.ppRAW))
                            {
                                directory = workingdir + "AA2_PLAY\\" + r.Remove(r.LastIndexOf('\\') + 1);
                            }
                            else
                            {
                                directory = workingdir + "AA2_MAKE\\" + r.Remove(r.LastIndexOf('\\') + 1);
                            }

                            Directory.CreateDirectory(directory);
                            foreach (IWriteFile iw in b.pp.Subfiles)
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

                foreach (string s in rsub)
                {
                    foreach (IWriteFile iw in b.pp.Subfiles)
                        if (b.ppDir + "\\" + iw.Name == s.Remove(0, 9))
                        {
                            b.pp.Subfiles.Remove(iw);
                            break;
                        }
                }

                foreach (string s in Directory.GetFiles(b.ppRAW))
                {
                    string fname = s.Remove(0, s.LastIndexOf('\\')+1);
                    foreach (IWriteFile sub in b.pp.Subfiles)
                    {
                        if (fname == sub.Name)
                        {
                            b.pp.Subfiles.Remove(sub);
                            break;
                        }
                    }
                    prgMinor.Value = i;
                    b.pp.Subfiles.Add(new Subfile(s));
                    i++;
                }
                if (b.pp.Subfiles.Count > 0)
                {
                    BackgroundWorker bb = b.pp.WriteArchive(b.pp.FilePath, createBackup, "", true);
                    bb.RunWorkerAsync();
                    while (bb.IsBusy)
                    {
                        Application.DoEvents();
                    }
                }
                else
                {
                    File.Delete(b.pp.FilePath);
                }
                //Loop complete

                TryDeleteDirectory(b.ppRAW + "\\");

                index++;
                prgMajor.Value = index;
                updateTaskbarProgress();
            }

            int ind = 0;
            prgMinor.Value = 0;
            //Archive backups
            prgMinor.Style = ProgressBarStyle.Marquee;
            prgMajor.Value = 0;
            if (!Directory.Exists(Paths.WORKING + "\\BACKUP\\")) { Directory.CreateDirectory(Paths.WORKING + "\\BACKUP\\"); }
            List<string> tempBackup = new List<string>(Directory.GetDirectories(Paths.WORKING + "\\BACKUP\\"));
            prgMajor.Maximum = tempBackup.Count;
            foreach (string s in tempBackup)
            {
                ind++;
                prgMajor.Value = ind;
                updateStatus("(" + ind.ToString() + "/" + tempBackup.Count.ToString() + ") Archiving backup of " + s + "...");

                string item = s.Remove(0, s.LastIndexOf('\\') + 1);
                string archive = Paths.BACKUP + "\\" + item + ".7z";
                if (Directory.Exists(s + "\\AA2_PLAY\\"))
                {
                    foreach (string sub in Directory.GetDirectories(s + "\\AA2_PLAY\\"))
                    {
                        string g = sub.Remove(0, s.Length + 1) + "\\";
                        _7z.Compress(archive, s, g);
                    }
                }
                if (Directory.Exists(s + "\\AA2_MAKE\\"))
                {
                    foreach (string sub in Directory.GetDirectories(s + "\\AA2_MAKE\\"))
                    {
                        string g = sub.Remove(0, s.Length + 1) + "\\";
                        _7z.Compress(archive, s, g);
                    }
                }
            }

            //Finish up
            prgMinor.Style = ProgressBarStyle.Continuous;
            updateStatus("Finishing up...");
            mods.AddRange(unmods);

            foreach (Mod m in unmods)
            {
                string s = Paths.BACKUP + "\\" + m.Name.Replace(".zip", ".7z");
                if (File.Exists(s))
                {
                    tryDelete(s);
                }
            }

            Configuration.saveMods(modDict);

            TryDeleteDirectory(Paths.TEMP);
            TryDeleteDirectory(Paths.WORKING);

            updateStatus("Success!", LogIcon.OK, false);
            updateTaskbarProgress(TaskbarProgress.TaskbarStates.NoProgress);
            if (!suppressPopups)
                MessageBox.Show("Mods successfully synced.");
            refreshModList();
            return true;
        }
        
        #endregion
        #region UI

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

        private void lsvMods_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }
        private void lsvMods_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
            if (e.Item.Index != -1)
            {
                if (lsvMods.Enabled)
                    e.DrawBackground();

                if ((e.State & ListViewItemStates.Selected) > 0)
                {
                    Color c = Color.FromKnownColor(KnownColor.Highlight);
                    Brush brush = new LinearGradientBrush(e.Bounds, c, c, LinearGradientMode.Horizontal);
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }
            }
        }

        private void lsvMods_SizeChanged(object sender, EventArgs e)
        {
            lsvMods.Columns[0].Width = lsvMods.Width - 5;
        }

        private void flushCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageTimer.Enabled = false;

            if (imagePreview.Image != null)
                imagePreview.Image.Dispose();
            imagePreview.Image = null;

            imageLoop.Clear();
            if (Directory.Exists(Paths.CACHE))
                TryDeleteDirectory(Paths.CACHE);
            Directory.CreateDirectory(Paths.CACHE + "\\");

            var iter = modDict.ToDictionary(entry => entry.Key,
                entry => entry.Value);
            /*
            foreach (Mod m in iter.Values)
                if (!m.Installed)
                    modDict.Remove(m.Name);
           */
            modDict = new ModDictionary();

            Configuration.saveMods(modDict);
            refreshModList();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            inject(false, !checkConflicts.Checked);
        }

        private void cmbSorting_SelectedIndexChanged(object sender, EventArgs e)
        {
            Configuration.WriteSetting("SORTMODE", cmbSorting.SelectedIndex.ToString());
            lsvMods.ListViewItemSorter = new CustomListViewSorter(cmbSorting.SelectedIndex);
            lsvMods.Sort();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            refreshModList(true, txtSearch.Text);
        }

        private void updateTaskbarProgress(TaskbarProgress.TaskbarStates state = TaskbarProgress.TaskbarStates.Normal)
        {
            updateTaskbarProgress(prgMajor.Value, prgMajor.Maximum, state);
        }

        private void updateTaskbarProgress(int value, int maximum, TaskbarProgress.TaskbarStates state = TaskbarProgress.TaskbarStates.Normal)
        {
            try
            {
                TaskbarProgress.SetState(this.Handle, state);
                TaskbarProgress.SetValue(this.Handle, value, maximum);
            }
            catch (System.Runtime.InteropServices.InvalidComObjectException) { }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteSelectedMods();
        }

        public void deleteSelectedMods(bool suppressDialogs = false)
        {
            if (lsvMods.SelectedItems.Count > 0)
            {
                List<Mod> mods = new List<Mod>(Enumerable.Range(0, lsvMods.SelectedItems.Count)
                    .Select(index => (Mod)lsvMods.SelectedItems[index].Tag));

                DialogResult result = DialogResult.No;
                if (!suppressDialogs)
                    result = MessageBox.Show("Are you sure you want to delete mod(s): " + Environment.NewLine + mods.Select(m => m.Name).Aggregate((i, j) => i + Environment.NewLine + j), "Delete mods?", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes || suppressDialogs)
                {
                    foreach (Mod m in mods)
                    {
                        if (m.Installed)
                            if (!suppressDialogs)
                                using (formDelete del = new formDelete(m.Name))
                                    result = del.ShowDialog();
                            else
                                result = DialogResult.Yes;

                            switch (result)
                            {
                                case DialogResult.OK: //delete only
                                    tryDelete(Paths.BACKUP + "\\" + m.Name.Replace(".zip", ".7z"));
                                    break;
                                case DialogResult.Yes: //uninstall + delete
                                    refreshModList(true);
                                    lsvMods.Items[lsvMods.Items.IndexOfKey(m.Name)].Checked = false;
                                    inject(false, false, true);
                                    break;
                                case DialogResult.Cancel:
                                    return; //exit entire loop
                            }
                            
                        tryDelete(m.Filename);
                    }
                    refreshModList();
                }
            }
        }

        private void lsvMods_KeyPress(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    deleteSelectedMods();
                    break;
            }
        }

        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lsvLog.Items.Clear();
        }
        #endregion
        #region Form Events
        public formMain()
        {
            InitializeComponent();
        }


        public void formMain_Shown(object sender, EventArgs ev)
        {
            //Hide();
            bool done = false;

            //Change title
            this.Text = "AA2Install v" + formAbout.AssemblyVersion;

            //Show splash
            ThreadPool.QueueUserWorkItem((x) =>
            {
                using (var splashForm = new formSplash())
                {
                    splashForm.lblVer.Text = "AA2Install v" + formAbout.AssemblyVersion;
                    statusUpdated += (s) =>
                    {
                        splashForm.BeginInvoke(new MethodInvoker(() =>
                        {
                            splashForm.lblStatus.Text = s;
                        }));
                    };
                    splashForm.Show();
                    while (!done)
                        Application.DoEvents();

                    statusUpdated = null;
                    splashForm.Close();
                    this.BeginInvoke(new MethodInvoker((() => { this.Activate(); })));
                }
            });

            //Resize the column
            lsvMods_SizeChanged(null, null);

            //Check if installed
            if (!(Directory.Exists(Paths.AA2Play) && Directory.Exists(Paths.AA2Edit)))
            {
                MessageBox.Show("You don't seem to have AA2Play and/or AA2Edit (properly) installed.\nPlease install it, use the registry fixer (if you've already installed it) or manually specify the install path in the preferences.", "AA2 Not Installed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            //Set event handlers
            Console.InitializeOutput();
            _7z.OutputDataRecieved += new DataReceivedEventHandler((s, e) =>
            {
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    rtbConsole.AppendText((e.Data ?? string.Empty) + Environment.NewLine);
                }));
            });

            //Create necessary folders
            if (!Directory.Exists(Paths.BACKUP)) { Directory.CreateDirectory(Paths.BACKUP + @"\"); }
            if (!Directory.Exists(Paths.MODS)) { Directory.CreateDirectory(Paths.MODS + @"\"); }

            //Setup sorting
            lsvMods.ListViewItemSorter = new CustomListViewSorter(int.Parse(Configuration.ReadSetting("SORTMODE") ?? "0"));
            cmbSorting.SelectedIndex = int.Parse(Configuration.ReadSetting("SORTMODE") ?? "0");

            //Start program
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

            //Hide splash
            done = true;
            loadUIConfiguration();
            Show();
        }
        #endregion
        #region Image and Description
        int imageIndex = 1;
        List<string> imageLoop = new List<string>();
        private void lsvMods_SelectedIndexChanged(object sender, EventArgs e)
        {
            deleteToolStripMenuItem.Enabled = lsvMods.SelectedItems.Count > 0;

            if (lsvMods.SelectedItems.Count > 0)
            {
                if (imagePreview.Image != null) 
                    imagePreview.Image.Dispose();
                imagePreview.Image = null;
                imageLoop.Clear();
                rtbDescription.Clear();
                rtbDescription.AppendText("Loading...");

                string name = lsvMods.SelectedItems[0].Text;
                if (!File.Exists(Paths.CACHE + "\\" + name + ".txt") && !File.Exists(Paths.CACHE + "\\" + name + ".jpg"))
                    _7z.ExtractWildcard(Paths.MODS + "\\" + name + ".7z", name + "*", Paths.CACHE);

                rtbDescription.Clear();
                Font temp = rtbDescription.SelectionFont;
                StringBuilder str = new StringBuilder();
                StringBuilder ustr = new StringBuilder();
                rtbDescription.SelectionFont = new Font(temp, FontStyle.Bold);
                str.AppendLine(name);
                
                foreach (KeyValuePair<string, string> kv in ((Mod)lsvMods.SelectedItems[0].Tag).Properties)
                {
                    str.AppendLine(System.Threading.Thread.CurrentThread
           .CurrentCulture.TextInfo.ToTitleCase(kv.Key.ToLower())
           + ": " + kv.Value);
                }
                rtbDescription.AppendText(str.ToString());
                str.Clear();
                rtbDescription.SelectionFont = temp;

                if (File.Exists(Paths.CACHE + "\\" + name + ".txt"))
                {
                    //Fix for font changing when foreign characters are read
                    string text = File.ReadAllText(Paths.CACHE + "\\" + name + ".txt");
                    foreach (char c in text)
                    {
                        if (c < 127)
                        {
                            str.Append(c);
                            rtbDescription.AppendText(ustr.ToString());
                            ustr.Clear();
                            continue;
                        }
                        else
                        {
                            ustr.Append(c);
                            rtbDescription.SelectionFont = temp;
                            rtbDescription.AppendText(str.ToString());
                            str.Clear();
                        }
                    }
                    rtbDescription.SelectionFont = temp;
                    rtbDescription.AppendText(str.ToString());
                    rtbDescription.AppendText(ustr.ToString());
                }
                else
                {
                    rtbDescription.SelectionFont = new Font(temp, FontStyle.Italic);
                    rtbDescription.AppendText("[ No description found. ]");
                    rtbDescription.SelectionFont = temp;
                }

                if (File.Exists(Paths.CACHE + "\\" + name + ".jpg"))
                {
                    imageLoop.Add(Paths.CACHE + "\\" + name + ".jpg");
                    using (Stream s = new FileStream(imageLoop[0], FileMode.Open))
                        imagePreview.Image = new Bitmap(s);
                    int index = 1;
                    string newFile = Paths.CACHE + "\\" + name + index.ToString() + ".jpg";
                    while (File.Exists(newFile))
                    {
                        imageLoop.Add(newFile);
                        index++;
                        newFile = Paths.CACHE + "\\" + name + index.ToString() + ".jpg";
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

        #region Wizzard Migrate
        void updateLog(string l)
        {
            txtMigrate.Text = txtMigrate.Text + l + Environment.NewLine;
        }

        private void btnBrowseMigrate_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fold = new FolderBrowserDialog();
            fold.Description = @"Locate the Wizzard install folder (where Illusion_Wizzard.exe is)";
            DialogResult result = fold.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtBrowseMigrate.Text = fold.SelectedPath;
            }
        }

        private void btnMigrate_Click(object sender, EventArgs e)
        {
            setEnabled(false);

            var mods = Configuration.loadMods();

            txtMigrate.Clear();

            if (!Directory.Exists(txtBrowseMigrate.Text + @"\AA2_PLAY\"))
            {
                updateLog("ERROR: " + txtBrowseMigrate.Text + @"\AA2_PLAY\ not found!");
                setEnabled(true);
                return;
            }
            updateLog("Starting migration..");
            List<string> smods = new List<string>(Directory.GetFiles(txtBrowseMigrate.Text + @"\AA2_PLAY\mods\"));
            List<string> backups = new List<string>(Directory.GetFiles(txtBrowseMigrate.Text + @"\AA2_PLAY\backups\"));

            int i = 0;
            foreach (string m in smods)
            {
                i++;
                string r = m.Remove(0, m.LastIndexOf('\\') + 1);
                updateLog("(" + i.ToString() + "/" + smods.Count.ToString() + ") Processing " + r);
                File.Copy(m, Paths.MODS + "\\" + r, true);
                bool ins = File.Exists(txtBrowseMigrate.Text + @"\AA2_PLAY\backups\" + r);
                updateLog("Installed status: " + ins.ToString());
                if (ins)
                    File.Copy(txtBrowseMigrate.Text + @"\AA2_PLAY\backups\" + r, Paths.BACKUP + "\\" + r.Replace(".zip", ".7z"), true);
                Mod mm = _7z.Index(Paths.MODS + "\\" + r);
                mods[Paths.MODS + "\\" + r] = mm;
            }

            Configuration.saveMods(mods);
            //setEnabled(true);

            updateLog("Done!");

            refreshModList();
        }
        #endregion

        #region Log
        public enum LogIcon
        {
            Idle = 0,
            Ready = 1,
            Processing = 2,
            Warning = 3,
            OK = 4,
            Error = 5
        }
        public DateTime bench = new DateTime(1991, 9, 8);
        public void initializeBench()
        {
            bench = DateTime.Now;
        }
        public TimeSpan getTimeSinceLastCheck()
        {
            var diff = DateTime.Now - bench;
            bench = DateTime.Now;
            return diff;
        }

        public delegate void statusUpdatedEventHandler(string status);
        public event statusUpdatedEventHandler statusUpdated;

        private bool showTime = false;
        public void updateStatus(string entry, LogIcon icon = LogIcon.Ready, bool displayTime = true, bool onlyStatusBar = false)
        {
            if (statusUpdated != null)
                statusUpdated(entry);
            if (onlyStatusBar)
            {
                labelStatus.Text = entry;
            }
            else
            {
                Console.ProgramLog.Add(new LogEntry(entry, icon));
                        if (lsvLog.Items.Count > 0 && showTime)
                        {
                            lsvLog.Items[lsvLog.Items.Count - 1].SubItems.Add((getTimeSinceLastCheck().TotalMilliseconds / 1000).ToString("F2") + "s");
                        }
                        showTime = displayTime;
                switch (icon)
                {
                    case LogIcon.Error:
                    case LogIcon.Warning:
                        lsvLog.Items.Add(entry, (int)icon);
                        break;
                    default:
                        lsvLog.Items.Add(entry, (int)icon);
                        labelStatus.Text = entry;
                        break;
                }
            }
            
        }
        #endregion
    }
    #region Structures
    [XmlRoot("mod")]
    [DebuggerDisplay("{Name}")]
    public class Mod : ISerializable, IXmlSerializable
    {
        #region Serialization
        public Mod()
        {
            Name = "<default>";
            Size = 0;
            SubFilenames = new List<string>();
        }
        public Mod(SerializationInfo info, StreamingContext ctxt)
        {
            
            Name = (string)info.GetValue("Name", typeof(string));
            Installed = (bool)info.GetValue("Installed", typeof(bool));
            Exists = (bool)info.GetValue("Exists", typeof(bool));
            Size = (ulong)info.GetValue("Size", typeof(ulong));
            SubFilenames = (List<string>)info.GetValue("SubFilenames", typeof(List<string>));
            InstallTime = (DateTime)info.GetValue("InstallTime", typeof(DateTime));
            Properties = (SerializableDictionary<string, string>)info.GetValue("Properties", typeof(SerializableDictionary<string, string>));
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("Installed", Installed);
            info.AddValue("Exists", Exists);
            info.AddValue("Size", Size);
            info.AddValue("SubFilenames", SubFilenames);
            info.AddValue("InstallTime", InstallTime);
            info.AddValue("Properties", Properties);
        }

        XmlSchema IXmlSerializable.GetSchema() => null;

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.ReadStartElement();

            reader.ReadStartElement();
            Name = reader.ReadElementString();
            Exists = reader.ReadElementString().DeserializeObject<bool>();
            Size = reader.ReadElementString().DeserializeObject<ulong>();
            SubFilenames = reader.ReadElementString().DeserializeObject<List<string>>();
            InstallTime = reader.ReadElementString().DeserializeObject<DateTime>();
            Properties = reader.ReadElementString().DeserializeObject<SerializableDictionary<string, string>>();
            reader.ReadEndElement();

            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Mod");
            //writer.WriteAttributeString("Assembly Version", FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion);
            //writer.WriteAttributeString("Config Version", "2");
            
            writer.WriteElementString("Name", Name);
            writer.WriteElementString("Exists", _Exists.SerializeObject());
            writer.WriteElementString("Size", Size.SerializeObject());
            writer.WriteElementString("SubFilenames", SubFilenames.SerializeObject());
            writer.WriteElementString("InstallTime", _InstallTime.SerializeObject());
            writer.WriteElementString("Properties", Properties.SerializeObject());

            writer.WriteEndElement();
        }
        #endregion

        public Mod(string name)
        {
            setName(name);
            Size = 0;
            SubFilenames = new List<string>();
        }

        public Mod(string name, ulong size, List<string> subfilenames)
        {
            setName(name);
            Size = size;
            SubFilenames = subfilenames;
        }
        
        protected void setName(string newname)
        {
            if (newname.Contains('\\'))
                Name = newname.Remove(0, newname.LastIndexOf('\\') + 1);
            else
                Name = newname;
        }
        
        public string Name
        {
            get;
            protected set;
        }

        public string Filename => Paths.MODS + "\\" + Name;
        
        private bool _Installed = false;
        
        public bool Installed
        {
            get
            {
                var value = File.Exists(Paths.BACKUP + "\\" + Name.Replace(".zip", ".7z"));
                _Installed = value;
                return value;
            }
            protected set
            {
                _Installed = value;
            }
        }
        
        private bool _Exists = false;
        
        public bool Exists
        {
            get
            {
                var value = File.Exists(Filename);
                _Exists = value;
                return value;
            }
            protected set
            {
                _Exists = value;
            }
        }
        
        public ulong Size
        {
            get;
            protected set;
        }
        
        public List<string> SubFilenames
        {
            get;
            protected set;
        }
        
        private DateTime _InstallTime = new DateTime(1991, 9, 8);
        
        public DateTime InstallTime
        {
            get
            {
                DateTime value = _InstallTime;
                if (Installed)
                    value = File.GetLastWriteTime(Paths.BACKUP + "\\" + Name.Replace(".zip", ".7z"));
                _InstallTime = value;
                return value;
            }
            protected set
            {
                _InstallTime = value;
            }
        }
        
        //This is user-generated data so it can be left alone
        public SerializableDictionary<string, string> Properties = new SerializableDictionary<string, string>();
    }
    public class basePP
    {
        public string ppRAW;
        public ppParser pp;

        public basePP(string file, string destination)
        {
            ppRAW = file;
            pp = new ppParser(destination + "\\" + ppFile, new ppFormat_AA2());
        }

        public string ppDir => ppRAW.Remove(0, ppRAW.LastIndexOf('\\') + 1);

        public string ppFile => ppDir + ".pp";
    }
    public class CustomListViewSorter : IComparer
    {
        private int mode = 0;
        public CustomListViewSorter(int _mode = 0)
        {
            mode = _mode;
        }
        public int Compare(object x, object y)
        {
            var Lx = x as ListViewItem;
            var Ly = y as ListViewItem;

            var Mx = Lx.Tag as Mod;
            var My = Ly.Tag as Mod;

            if (x == y)
                return 0;

            if (x == null)
                return 1;
            if (y == null)
                return -1;

            switch (mode)
            {
                case 0: //Text sorting
                default:
                    return string.Compare(Lx.Text, Ly.Text);
                case 1: //Install date sorting
                    if (Lx.Tag == null || !My.Installed)
                        return 1;
                    if (Ly.Tag == null || !Mx.Installed)
                        return -1;
                    return (int)(My.InstallTime - Mx.InstallTime).TotalSeconds;
            }
        }
    }
    public enum TryDeleteResult
    {
        OK,
        Ignored,
        Cancelled
    }
    #endregion
}

