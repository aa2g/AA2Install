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

namespace AA2Install
{
    #warning Unfinished features still remain
    /* TODO:
     * 
     * Raw PP handling (Extract all of them and don't delete them when done with them)
     * Partial backups (Create a 7z archive of all modified files so they can be replaced later)
     * Check conflicting mods 
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
        #region Console

        string minorProgress = "(0/0)"; //When processing PP files

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

        private void loadConfiguration()
        {
            btnAA2PLAY.Enabled = txtAA2PLAY.Enabled = checkAA2PLAY.Checked = bool.Parse(Configuration.ReadSetting("AA2PLAY") ?? "False");
            txtAA2PLAY.Text = Configuration.ReadSetting("AA2PLAY_Path") ?? "";
            btnAA2EDIT.Enabled = txtAA2EDIT.Enabled = checkAA2EDIT.Checked = bool.Parse(Configuration.ReadSetting("AA2EDIT") ?? "False");
            txtAA2EDIT.Text = Configuration.ReadSetting("AA2EDIT_Path") ?? "";

            checkRAW.Checked = Configuration.isPPRAW;
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

        private void checkRAW_CheckedChanged(object sender, EventArgs e)
        {
            if (checkRAW.Checked)
            {
                btnRAW.Enabled = true;
            }
            else
            {
                btnRAW.Enabled = false;
            }
            Configuration.WriteSetting("PPRAW", checkRAW.Checked.ToString());
        }

        #endregion
        #region Methods
        /// <summary>
        /// Refreshes the list from the /mods/ directory
        /// </summary>
        public void refreshModList()
        {
            listMods.Enabled = false;
            btnApply.Enabled = false;
            btnRefresh.Enabled = false;

            listMods.Items.Clear();
            foreach(string path in Directory.GetFiles(Paths.MODS, "*.7z", SearchOption.TopDirectoryOnly))
            {
                Mod m = _7z.Index(path);
                string p = path.Remove(0, path.LastIndexOf('\\')+1);
                listMods.Items.Add(p.Replace(".7z", ""), 0);
                listMods.Items[listMods.Items.Count - 1].SubItems.Add((m.size/(1024)).ToString("#,## kB"));
            }

            listMods.Enabled = true;
            btnApply.Enabled = true;
            btnRefresh.Enabled = true;
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
        /// Applies mods
        /// </summary>
        public void install()
        {
            //Reset controls
            btnApply.Enabled = false;
            btnRefresh.Enabled = false;
            prgMinor.Value = 0;
            prgMajor.Value = 0;
            prgMajor.Style = ProgressBarStyle.Marquee;

            //Check if directory exists
            if (!(Directory.Exists(Paths.AA2Play) && Directory.Exists(Paths.AA2Edit)))
            {
                updateStatus("FAILED: AA2Play/AA2Edit is not installed/cannot be found");
                btnApply.Enabled = true;
                btnRefresh.Enabled = true;
                return;
            }

            //Build ppIndex
            int index = 0;

            List<string> ppPlayIndex = new List<string>();
            List<string> ppEditIndex = new List<string>();

            updateStatus("Creating .pp file index...");
            foreach (string path in Directory.GetFiles(Paths.AA2Play, "*.pp", SearchOption.TopDirectoryOnly))
            {
                ppPlayIndex.Add(path.Remove(0, path.LastIndexOf('\\') + 1));
            }

            foreach (string path in Directory.GetFiles(Paths.AA2Edit, "*.pp", SearchOption.TopDirectoryOnly))
            {
                ppEditIndex.Add(path.Remove(0, path.LastIndexOf('\\') + 1));
            }

            //Clear and create temp
            updateStatus("Clearing TEMP folder...");
            if (Directory.Exists(Paths.TEMP)) { Directory.Delete(Paths.TEMP, true); }
            if (Directory.Exists(Paths.WORKING)) { Directory.Delete(Paths.WORKING, true); }

            Directory.CreateDirectory(Paths.PP);
            Directory.CreateDirectory(Paths.TEMP);
            Directory.CreateDirectory(Paths.WORKING);
            Directory.CreateDirectory(Paths.TEMP + @"\AA2_PLAY");
            Directory.CreateDirectory(Paths.TEMP + @"\AA2_MAKE");

            //Reset individual statuses
            foreach (ListViewItem item in listMods.Items)
            {
                listMods.Items[index].ImageIndex = 0; //Standby
                index++;
            }

            //Check conflicts (placeholder)
            index = 0;
            updateStatus("Checking conflicts...");

            foreach (ListViewItem item in listMods.Items)
            {
                if (item.Checked)
                {
                    listMods.Items[index].ImageIndex = 1; //Ready
                }
                index++;
            }

            //Extract all mods
            index = 0;
            prgMinor.Maximum = listMods.Items.Count;

            foreach (ListViewItem item in listMods.Items)
            {
                if (item.Checked)
                {
                    listMods.Items[index].ImageIndex = 2; //Processing
                    updateStatus("Extracting " + item.Text + "...");
                    _7z.Extract(Paths.MODS + "\\" + item.Text + ".7z");
                    listMods.Items[index].ImageIndex = 4; //Done
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
            do
            {
                string ppRAW = ppQueue.Dequeue();
                string ppDir = ppRAW.Remove(0, ppRAW.LastIndexOf('\\') + 1);
                string pp = ppDir + ".pp";

                string destination = "";

                updateStatus("(0/0) (" + (index + 1).ToString() + "/" + prgMajor.Maximum.ToString() + ") Currently unpacking " + pp + "...");

                //Fetch.pp file if it exists

                switch (pp[3]) //jg2[e/p]01_...
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

                updateStatus("(0/0) (" + (index + 1).ToString() + "/" + prgMajor.Maximum.ToString() + ") Currently merging " + pp + "...");
                prgMinor.Style = ProgressBarStyle.Marquee;

                if (File.Exists(destination + "\\" + pp)) { //If original exists then move/copy it

                    if (!Directory.Exists(Paths.PP + "\\" + ppDir)) //Don't overwrite if rawPP is enabled
                    {  
                        File.Copy(destination + "\\" + pp, Paths.PP + "\\" + pp);
                        PP.Extract(Paths.PP + "\\" + pp);
                        File.Delete(Paths.PP + "\\" + pp);
                    }

                    Directory.Move(Paths.PP + "\\" + ppDir, Paths.WORKING + "\\" + ppDir);                 
                    
                }

                //Overwrite .pp with contents of mod

                CopyFilesRecursively(new DirectoryInfo(ppRAW), new DirectoryInfo(Paths.WORKING + "\\" + ppDir)); //since Directory.Move doesn't overwrite
                Directory.Delete(ppRAW, true);

                if (Configuration.isPPRAW) //If ppRAW is enabled copy back the directory
                {
                    CopyFilesRecursively(new DirectoryInfo(Paths.WORKING + "\\" + ppDir), new DirectoryInfo(Paths.PP + "\\" + ppDir));
                }

                //Repackage .pp

                updateStatus("(0/0) (" + (index + 1).ToString() + "/" + prgMajor.Maximum.ToString() + ") Currently re-packing " + pp + "...");
                prgMinor.Style = ProgressBarStyle.Continuous;

                string newPP = PP.Create(Paths.WORKING + "\\" + ppDir);
                Directory.Delete(Paths.WORKING + "\\" + ppDir, true);

                //Patch the new .pp back to the game files

                updateStatus("(0/0) (" + (index + 1).ToString() + "/" + prgMajor.Maximum.ToString() + ") Currently installing " + pp + "...");
                prgMinor.Style = ProgressBarStyle.Marquee;

                if (File.Exists(destination + "\\" + pp)) { File.Delete(destination + "\\" + pp); }
                File.Move(newPP, destination + "\\" + pp);

                //Loop complete

                index++;
                prgMajor.Value = index;
            }
            while (ppQueue.Count > 0);

            prgMinor.Style = ProgressBarStyle.Continuous;
            updateStatus("Finishing up...");
            if (!Configuration.isPPRAW && Directory.Exists(Paths.PP))
            {
                TryDeleteDirectory(Paths.PP);
            }

            TryDeleteDirectory(Paths.TEMP);
            TryDeleteDirectory(Paths.WORKING);

            updateStatus("Success!");
            btnApply.Enabled = true;
            btnRefresh.Enabled = true;
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
            install();
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
            PP.OutputDataRecieved += new DataReceivedEventHandler(ProcessOutputHandler);

            //Start program
            loadConfiguration();
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
            if (listMods.SelectedItems.Count > 0)
            {
                imagePreview.Image = null;
                imageLoop.Clear();
                txtDescription.Text = "Loading...";

                string name = listMods.SelectedItems[0].Text;
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
                imagePreview.Image = new Bitmap(imageLoop[(imageIndex % imageLoop.Count)]);
                imageIndex++;
            }
            else
            {
                imagePreview.Image = null;
            }
        }
        #endregion

    }
}
