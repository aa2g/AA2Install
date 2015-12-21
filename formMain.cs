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

namespace AA2Install
{
    public partial class formMain : Form
    {
        public SerializableDictionary<Mod> modDict = new SerializableDictionary<Mod>();

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
        /// Sets enabled status of essential controls to variable
        /// </summary>
        /// <param name="enabled"></param>
        public void setEnabled(bool enabled)
        {
            lsvMods.Enabled = btnApply.Enabled = btnRefresh.Enabled =
                checkAA2EDIT.Enabled = txtAA2EDIT.Enabled = btnAA2EDIT.Enabled =
                checkAA2PLAY.Enabled = txtAA2PLAY.Enabled = btnAA2PLAY.Enabled =
                checkConflicts.Enabled = btnMigrate.Enabled = btnBrowseMigrate.Enabled =
                txtMigrate.Enabled = txtBrowseMigrate.Enabled = btnCancel.Enabled = 
                cmbSorting.Enabled = enabled;
        }

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

        public bool cancelPending = false;
        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancelPending = true;
            updateStatus("Pending cancellation...", LogIcon.Warning);
            updateStatus("Pending cancellation...", LogIcon.Warning, false, true);
        }
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
        /// Refreshes the list from the /mods/ directory
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
                        if (path == m.Name)
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

            foreach (Mod mn in modDict.Values.ToList())
            {
                Mod m = mn;
                bool backup = File.Exists(Paths.BACKUP + "\\" + m.Name.Remove(0, m.Name.LastIndexOf('\\') + 1).Replace(".zip", ".7z"));
                string shortName = m.Name.Remove(0, m.Name.LastIndexOf('\\') + 1);

                if (!File.Exists(m.Name) && !(m.Installed && backup))
                {
                    continue;
                }

                m.Installed = backup;

                if (!shortName.ToLower().Contains(filter.ToLower()) && filter != "")
                    continue;

                lsvMods.Items.Add(m.Name, m.Name.Remove(0, m.Name.LastIndexOf('\\') + 1).Replace(".7z", "").Replace(".zip", ""), 0);
                int index = lsvMods.Items.IndexOfKey(m.Name);

                if (m.Installed && !File.Exists(m.Name) && !backup)
                {
                    //lsvMods.Items[index].BackColor = Color.Maroon;
                }
                else if (m.Installed && File.Exists(m.Name) && !backup)
                {
                    //lsvMods.Items[index].BackColor = Color.Goldenrod;
                }
                else if ((!m.Installed || !File.Exists(m.Name)) && backup)
                {
                    //lsvMods.Items[index].BackColor = Color.DarkBlue;
                }
                else if (m.Installed && backup)
                {
                    lsvMods.Items[index].BackColor = Color.LightGreen;
                    lsvMods.Items[index].Checked = true;
                }

                m.Properties["Estimated Size"] = (m.size / (1024)).ToString("#,## kB");

                if (m.Installed && m.InstallTime.Year > 2000)
                    m.Properties["Installed on"] = m.InstallTime.ToString();
                else if (backup)
                {
                    m.InstallTime = (new FileInfo(m.Name)).LastWriteTime;
                    m.Properties["Installed on"] = m.InstallTime.ToString();
                }
                else
                    m.Properties.Remove("Installed on");

                lsvMods.Items[index].Tag = m;
                
                modDict[m.Name] = m;
            }

            if (!skipReload)
            {
                Configuration.saveMods(modDict);

                prgMinor.Style = ProgressBarStyle.Continuous;

                prgMinor.Value = prgMinor.Maximum;
                updateStatus("Ready.", LogIcon.OK);


                setEnabled(true);
            }            
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
        public bool inject(bool createBackup = false, bool checkConflicts = true)
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
                bool backup = File.Exists(Paths.BACKUP + "\\" + m.Name.Remove(0, m.Name.LastIndexOf('\\') + 1).Replace(".zip", ".7z"));
                if (l.Checked ^ (m.Installed && backup))
                {
                    if (l.Checked)
                    {
                        postarc.Enqueue(m.Name);
                        mods.Add(m);
                    }
                    else
                    {
                        prearc.Enqueue(Paths.BACKUP + "\\" + m.Name.Remove(0, m.Name.LastIndexOf('\\') + 1).Replace(".zip", ".7z"));
                        unmods.Add(m);
                        rsub.AddRange(m.Filenames);
                    }
                }
            }

            if (prearc.Count + postarc.Count == 0)
            {
                updateStatus("FAILED: No changes in selected mods");
                updateStatus("No changes in selected mods found.", LogIcon.Error);
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
                updateStatus("FAILED: AA2Play/AA2Edit is not installed/cannot be found");
                btnApply.Enabled = true;
                btnRefresh.Enabled = true;
                return false;
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

                    foreach (string s in m.Filenames)
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
                        foreach (string s in ((Mod)item.Tag).Filenames)
                        {
                            if (files.ContainsKey(s))
                            {
                                if (files[s] == ((Mod)item.Tag).Name)
                                    break;
                                conflict = true;

                                foreach (ListViewItem i in lsvMods.Items) //Clusterfuck to find the other conflicting mod
                                {
                                    Mod m = (Mod)i.Tag;
                                    if (m.Filenames.Contains(s) && i.Checked)
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
                        updateStatus(c, LogIcon.Error);
                    updateStatus("FAILED: The highlighted mods have conflicting files", LogIcon.Ready, false, true);
                    updateStatus("Collision detected.", LogIcon.Error);
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
                    foreach (string s in m.Filenames)
                    {
                        if (s.Contains(b.ppDir))
                        {
                            string r = s.Remove(0, 9);
                            string rs = r.Remove(0, r.LastIndexOf('\\') + 1);
                            string workingdir = Paths.WORKING + "\\BACKUP\\" + m.Name.Remove(0, m.Name.LastIndexOf('\\') + 1).Replace(".7z", "").Replace(".zip", "") + "\\";
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
            mods.AddRange(unmods);

            foreach (Mod m in mods)
            {
                if (modDict.ContainsKey(m.Name))
                {
                    Mod mm = modDict[m.Name];
                    if (m.Installed)
                    {
                        mm.Installed = false;
                        string s = Paths.BACKUP + m.Name.Remove(0, m.Name.LastIndexOf("\\"));
                        if (File.Exists(s))
                        {
                            File.Delete(s);
                        }
                        mm.InstallTime = new DateTime(1991, 9, 8);
                        modDict[m.Name] = mm;
                    }
                    else
                    {
                        mm.Installed = true;
                        mm.InstallTime = DateTime.Now;
                        modDict[m.Name] = mm;
                    }
                }
            }

            Configuration.saveMods(modDict);

            TryDeleteDirectory(Paths.TEMP);
            TryDeleteDirectory(Paths.WORKING);

            updateStatus("Success!", LogIcon.OK);
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
            foreach (Mod m in iter.Values)
                if (!m.Installed)
                    modDict.Remove(m.Name);

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
        #endregion
        #region Form Events
        public formMain()
        {
            InitializeComponent();
        }


        private void formMain_Shown(object sender, EventArgs e)
        {
            //Resize the column
            lsvMods_SizeChanged(null, null);

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

            //Setup sorting
            lsvMods.ListViewItemSorter = new CustomListViewSorter(int.Parse(Configuration.ReadSetting("SORTMODE") ?? "0"));
            cmbSorting.SelectedIndex = int.Parse(Configuration.ReadSetting("SORTMODE") ?? "0");

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
        #endregion
        #region Image and Description
        int imageIndex = 1;
        List<string> imageLoop = new List<string>();
        private void lsvMods_SelectedIndexChanged(object sender, EventArgs e)
        {
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
                mm.Installed = ins;
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
        public void updateStatus(string entry, LogIcon icon = LogIcon.Ready, bool displayTime = true, bool onlyStatusBar = false)
        {
            if (onlyStatusBar)
            {
                labelStatus.Text = entry;
            }
            else
            {
                switch (icon)
                {
                    case LogIcon.Error:
                    case LogIcon.Warning:
                        lsvLog.Items.Add(entry, (int)icon);
                        break;
                    default:
                        lsvLog.Items.Add("(" + Math.Round(getTimeSinceLastCheck().TotalMilliseconds / 1000, 2).ToString() + "s) " + entry, (int)icon);
                        labelStatus.Text = entry;
                        break;
                }
            }
            
        }
        #endregion
    }
    #region Structures
    [Serializable()]
    [DebuggerDisplay("{Name}")]
    public struct Mod
    {
        public string Name;
        public bool Installed;
        public ulong size;
        public List<string> Filenames;
        public SerializableDictionary<string> Properties;
        public DateTime InstallTime;
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
                    return string.Compare(((ListViewItem)x).Text, ((ListViewItem)y).Text);
                case 1: //Install date sorting
                    if (((ListViewItem)x).Tag == null || !((Mod)((ListViewItem)x).Tag).Installed)
                        return 1;
                    if (((ListViewItem)y).Tag == null || !((Mod)((ListViewItem)y).Tag).Installed)
                        return -1;
                    return (int)(((Mod)((ListViewItem)y).Tag).InstallTime - ((Mod)((ListViewItem)x).Tag).InstallTime).TotalSeconds;
            }
        }
    }
    #endregion
}

