using Microsoft.VisualStudio.TestTools.UnitTesting;
using AA2Install;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SB3Utility;
using System.Windows.Forms;
using AA2Install.Archives;
using System.Diagnostics;

namespace AA2Install.Tests
{
    [TestClass()]
    [DeploymentItem(@"AA2Install\7za.exe")]
    [DeploymentItem(@"mods\", "mods")]
    [DeploymentItem(@"testdir\", "testdir")]
    public class MainTests
    {
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

        formMain form;

        [TestInitialize()]
        public void Initialize()
        {
            Configuration.WriteSetting("AA2PLAY", "true");
            Configuration.WriteSetting("AA2EDIT", "true");
            Configuration.WriteSetting("AA2PLAY_Path", Environment.CurrentDirectory + @"\testdir\");
            Configuration.WriteSetting("AA2EDIT_Path", Environment.CurrentDirectory + @"\testdir\");

            Configuration.saveMods(new ModDictionary());
            form = new formMain();
            form.Show();
        }

        /// <summary>
        /// Tests the finding and showing of available mods.
        /// </summary>
        [TestMethod()]
        public void refreshModListTest()
        {
            int expected;

            //Overall test
            form.refreshModList();
            expected = 2;
            Assert.IsTrue(form.lsvMods.Items.Count == expected, "lsvMods did not show the true amount of mods for the overall test. Expected value: {0}; Actual value: {1}", expected, form.lsvMods.Items.Count);

            //Filter test
            form.refreshModList(true, "dummy");
            expected = 1;
            Assert.IsTrue(form.lsvMods.Items.Count == expected, "lsvMods did not show the true amount of mods for the filter test. Expected value: {0}; Actual value: {1}", expected, form.lsvMods.Items.Count);
            Assert.IsTrue(form.lsvMods.Items[0].Text == "[AA2] DUMMY MOD", "lsvMods did not display the correct name of the mod. Expected value: {0}; Actual value: {1}", "[AA2] DUMMY MOD", form.lsvMods.Items[0].Name);

            //Filter test without result
            form.refreshModList(true, "wew lad");
            expected = 0;
            Assert.IsTrue(form.lsvMods.Items.Count == expected, "lsvMods did not show the true amount of mods for the no result filter test. Expected value: {0}; Actual value: {1}", expected, form.lsvMods.Items.Count);
        }

        /// <summary>
        /// Tests installing and uninstalling mods.
        /// </summary>
        [TestMethod()]
        public void injectTest()
        {
            Assert.IsTrue(File.Exists(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp"), Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp did not deploy");

            form.refreshModList();

            var subfiles = new ppParser(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp", new ppFormat_AA2()).Subfiles;
            Dictionary<string, uint> CRCValues = new Dictionary<string, uint>();

            foreach (IWriteFile kv in subfiles)
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    kv.WriteTo(mem);
                    byte[] buffer = new byte[mem.Length];
                    mem.Position = 0;
                    mem.Read(buffer, 0, (int)mem.Length);

                    var value = DamienG.Security.Cryptography.Crc32.Compute(buffer);
                    CRCValues.Add(kv.Name, value);
                }
            }

            foreach (ListViewItem lv in form.lsvMods.Items)
                lv.Checked = true;

            Assert.IsTrue(form.inject(true, true, true), "Installation injection failed. Log: {0}", form.labelStatus.Text);

            foreach (Mod m in form.modDict.Values)
            {
                Assert.IsTrue(m.Installed, "Backup archive for {0} was not created.", m.Name);
            }
            
            Assert.IsTrue(File.Exists(Environment.CurrentDirectory + @"\testdir\jg2e04_00_TEST.pp"), "jg2e04_00_TEST.pp was not created.");
            Assert.IsTrue(File.Exists(Environment.CurrentDirectory + @"\testdir\jg2p01_00_TEST.pp"), "jg2p01_00_TEST.pp was not created.");

            Dictionary<string, uint> diff = new Dictionary<string, uint>();

            foreach (IWriteFile kv in new ppParser(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp", new ppFormat_AA2()).Subfiles)
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    kv.WriteTo(mem);
                    byte[] buffer = new byte[mem.Length];
                    mem.Position = 0;
                    mem.Read(buffer, 0, (int)mem.Length);

                    var value = DamienG.Security.Cryptography.Crc32.Compute(buffer);
                    if (CRCValues[kv.Name] != value)
                        diff.Add(kv.Name, value);
                }
            }

            Dictionary<string, uint> TrueCRC = new Dictionary<string, uint>()
            {
                { "jg2_01_05_00.lst", 838774076 },
                { "jg2_01_05_01.lst", 1767953068 },
                { "jg2_01_05_02.lst", 1609570759 },
                { "jg2_01_05_03.lst", 4121681499 },
                { "jg2_01_06_05.lst", 32814373 },
                { "jg2_01_06_06.lst", 2454317916 },
                { "jg2_01_06_08.lst", 3999174270 }
            };

            //Assert.IsFalse(subfiles.Count == new ppParser(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp", new ppFormat_AA2()).Subfiles.Count, "Subfiles have not changed.");
            Assert.IsTrue(diff.Count == TrueCRC.Count, "Amount of changes in jg2p00_00_00.pp was incorrect. Expected value: {0}; Actual value: {1}", TrueCRC.Count, diff.Count);

            foreach (KeyValuePair<string, uint> kv in TrueCRC)
                Assert.IsTrue(kv.Value == diff[kv.Key], "CRC check failed after installation. Key: {0}; Expected value: {1}; Actual value: {2}", kv.Key, kv.Value, diff[kv.Key]);

            foreach (ListViewItem lv in form.lsvMods.Items)
                lv.Checked = false;

            Assert.IsTrue(form.inject(false, false, true), "Uninstallation injection failed. Log: {0}", form.labelStatus.Text);

            Assert.IsTrue(!File.Exists(Environment.CurrentDirectory + @"\testdir\jg2e04_00_TEST.pp"), "jg2e04_00_TEST.pp was not deleted.");
            Assert.IsTrue(!File.Exists(Environment.CurrentDirectory + @"\testdir\jg2p01_00_TEST.pp"), "jg2p01_00_TEST.pp was not deleted.");

            foreach (Mod m in form.modDict.Values)
            {
                Assert.IsTrue(!m.Installed, "Backup archive for {0} was not deleted.", m.Name);
            }

            Assert.IsTrue(subfiles.Count == new ppParser(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp", new ppFormat_AA2()).Subfiles.Count, "Amount of restored changes in jg2p00_00_00.pp was incorrect. Expected value: {0}; Actual value: {1}", new object[] { subfiles.Count, new ppParser(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp", new ppFormat_AA2()).Subfiles.Count });

            foreach (IWriteFile kv in new ppParser(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp", new ppFormat_AA2()).Subfiles)
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    kv.WriteTo(mem);
                    byte[] buffer = new byte[mem.Length];
                    mem.Position = 0;
                    mem.Read(buffer, 0, (int)mem.Length);

                    var value = DamienG.Security.Cryptography.Crc32.Compute(buffer);
                    Assert.IsTrue(CRCValues[kv.Name] == value, "CRC check failed after uninstallation. Key: {0}; Expected value: {1}; Actual value: {2}", kv.Name, CRCValues[kv.Name], value);
                }
            }

            Configuration.saveMods(form.modDict);
        }

        /// <summary>
        /// Tests the deletion and uninstallation of mods.
        /// </summary>
        [TestMethod()]
        public void deleteModTest()
        {
            Assert.IsTrue(File.Exists(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp"), Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp did not deploy");

            CopyFilesRecursively(new DirectoryInfo(Environment.CurrentDirectory + @"\mods\"), new DirectoryInfo(Environment.CurrentDirectory + @"\mods2\"));
            form.refreshModList();

            var subfiles = new ppParser(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp", new ppFormat_AA2()).Subfiles;
            Dictionary<string, uint> CRCValues = new Dictionary<string, uint>();

            foreach (IWriteFile kv in subfiles)
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    kv.WriteTo(mem);
                    byte[] buffer = new byte[mem.Length];
                    mem.Position = 0;
                    mem.Read(buffer, 0, (int)mem.Length);

                    var value = DamienG.Security.Cryptography.Crc32.Compute(buffer);
                    CRCValues.Add(kv.Name, value);
                }
            }

            foreach (ListViewItem lv in form.lsvMods.Items)
                lv.Checked = true;

            Assert.IsTrue(form.inject(true, true, true), "Installation injection failed. Log: {0}", form.labelStatus.Text);

            foreach (Mod m in form.modDict.Values)
            {
                Assert.IsTrue(m.Installed, "Backup archive for {0} was not created.", m.Name);
            }

            Assert.IsTrue(File.Exists(Environment.CurrentDirectory + @"\testdir\jg2e04_00_TEST.pp"), "jg2e04_00_TEST.pp was not created.");
            Assert.IsTrue(File.Exists(Environment.CurrentDirectory + @"\testdir\jg2p01_00_TEST.pp"), "jg2p01_00_TEST.pp was not created.");

            Dictionary<string, uint> diff = new Dictionary<string, uint>();

            foreach (IWriteFile kv in new ppParser(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp", new ppFormat_AA2()).Subfiles)
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    kv.WriteTo(mem);
                    byte[] buffer = new byte[mem.Length];
                    mem.Position = 0;
                    mem.Read(buffer, 0, (int)mem.Length);

                    var value = DamienG.Security.Cryptography.Crc32.Compute(buffer);
                    if (CRCValues[kv.Name] != value)
                        diff.Add(kv.Name, value);
                }
            }

            Dictionary<string, uint> TrueCRC = new Dictionary<string, uint>()
            {
                { "jg2_01_05_00.lst", 838774076 },
                { "jg2_01_05_01.lst", 1767953068 },
                { "jg2_01_05_02.lst", 1609570759 },
                { "jg2_01_05_03.lst", 4121681499 },
                { "jg2_01_06_05.lst", 32814373 },
                { "jg2_01_06_06.lst", 2454317916 },
                { "jg2_01_06_08.lst", 3999174270 }
            };

            //Assert.IsFalse(subfiles.Count == new ppParser(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp", new ppFormat_AA2()).Subfiles.Count, "Subfiles have not changed.");
            Assert.IsTrue(diff.Count == TrueCRC.Count, "Amount of changes in jg2p00_00_00.pp was incorrect. Expected value: {0}; Actual value: {1}", TrueCRC.Count, diff.Count);

            foreach (KeyValuePair<string, uint> kv in TrueCRC)
                Assert.IsTrue(kv.Value == diff[kv.Key], "CRC check failed after installation. Key: {0}; Expected value: {1}; Actual value: {2}", kv.Key, kv.Value, diff[kv.Key]);

            foreach (ListViewItem lv in form.lsvMods.Items)
            {
                lv.Checked = false;
                lv.Selected = true;
            }
                

            //Assert.IsTrue(form.inject(false, false, true), "Uninstallation injection failed. Log: {0}", form.labelStatus.Text);
            form.deleteSelectedMods(true);

            Assert.IsTrue(!File.Exists(Environment.CurrentDirectory + @"\testdir\jg2e04_00_TEST.pp"), "jg2e04_00_TEST.pp was not deleted.");
            Assert.IsTrue(!File.Exists(Environment.CurrentDirectory + @"\testdir\jg2p01_00_TEST.pp"), "jg2p01_00_TEST.pp was not deleted.");

            foreach (Mod m in form.modDict.Values)
            {
                Assert.IsTrue(!m.Installed, "Backup archive for {0} was not deleted.", m.Name);
            }

            Assert.IsTrue(subfiles.Count == new ppParser(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp", new ppFormat_AA2()).Subfiles.Count, "Amount of restored changes in jg2p00_00_00.pp was incorrect. Expected value: {0}; Actual value: {1}", new object[] { subfiles.Count, new ppParser(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp", new ppFormat_AA2()).Subfiles.Count });

            foreach (IWriteFile kv in new ppParser(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp", new ppFormat_AA2()).Subfiles)
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    kv.WriteTo(mem);
                    byte[] buffer = new byte[mem.Length];
                    mem.Position = 0;
                    mem.Read(buffer, 0, (int)mem.Length);

                    var value = DamienG.Security.Cryptography.Crc32.Compute(buffer);
                    Assert.IsTrue(CRCValues[kv.Name] == value, "CRC check failed after uninstallation. Key: {0}; Expected value: {1}; Actual value: {2}", kv.Name, CRCValues[kv.Name], value);
                }
            }

            formMain.TryDeleteDirectory(Environment.CurrentDirectory + @"\mods\");
            Directory.Move(Environment.CurrentDirectory + @"\mods2\", Environment.CurrentDirectory + @"\mods\");

            Configuration.saveMods(form.modDict);
        }

        /// <summary>
        /// Tests the forced installation of mods.
        /// </summary>
        [TestMethod()]
        public void forceModTest()
        {
            Assert.IsTrue(File.Exists(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp"), Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp did not deploy");

            form.refreshModList();

            CopyFilesRecursively(new DirectoryInfo(Environment.CurrentDirectory + @"\testdir\"), new DirectoryInfo(Environment.CurrentDirectory + @"\testdir2\"));
            
            var subfiles = new ppParser(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp", new ppFormat_AA2()).Subfiles;
            Dictionary<string, uint> CRCValues = new Dictionary<string, uint>();

            foreach (IWriteFile kv in subfiles)
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    kv.WriteTo(mem);
                    byte[] buffer = new byte[mem.Length];
                    mem.Position = 0;
                    mem.Read(buffer, 0, (int)mem.Length);

                    var value = DamienG.Security.Cryptography.Crc32.Compute(buffer);
                    CRCValues.Add(kv.Name, value);
                }
            }

            foreach (ListViewItem lv in form.lsvMods.Items)
                lv.Checked = true;

            Assert.IsTrue(form.inject(true, true, true), "Installation injection failed. Log: {0}", form.labelStatus.Text);


            Directory.Delete(Environment.CurrentDirectory + @"\testdir\", true);
            CopyFilesRecursively(new DirectoryInfo(Environment.CurrentDirectory + @"\testdir2\"), new DirectoryInfo(Environment.CurrentDirectory + @"\testdir\"));
            Directory.Delete(Environment.CurrentDirectory + @"\testdir2\", true);

            foreach (ListViewItem lsv in form.lsvMods.Items)
                lsv.Selected = true;

            form.forceSelectedMods(true);

            foreach (Mod m in form.modDict.Values)
            {
                Assert.IsTrue(m.Installed, "Backup archive for {0} was not created.", m.Name);
            }

            Assert.IsTrue(File.Exists(Environment.CurrentDirectory + @"\testdir\jg2e04_00_TEST.pp"), "jg2e04_00_TEST.pp was not created.");
            Assert.IsTrue(File.Exists(Environment.CurrentDirectory + @"\testdir\jg2p01_00_TEST.pp"), "jg2p01_00_TEST.pp was not created.");

            Dictionary<string, uint> diff = new Dictionary<string, uint>();

            foreach (IWriteFile kv in new ppParser(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp", new ppFormat_AA2()).Subfiles)
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    kv.WriteTo(mem);
                    byte[] buffer = new byte[mem.Length];
                    mem.Position = 0;
                    mem.Read(buffer, 0, (int)mem.Length);

                    var value = DamienG.Security.Cryptography.Crc32.Compute(buffer);
                    if (CRCValues[kv.Name] != value)
                        diff.Add(kv.Name, value);
                }
            }

            Dictionary<string, uint> TrueCRC = new Dictionary<string, uint>()
            {
                { "jg2_01_05_00.lst", 838774076 },
                { "jg2_01_05_01.lst", 1767953068 },
                { "jg2_01_05_02.lst", 1609570759 },
                { "jg2_01_05_03.lst", 4121681499 },
                { "jg2_01_06_05.lst", 32814373 },
                { "jg2_01_06_06.lst", 2454317916 },
                { "jg2_01_06_08.lst", 3999174270 }
            };

            //Assert.IsFalse(subfiles.Count == new ppParser(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp", new ppFormat_AA2()).Subfiles.Count, "Subfiles have not changed.");
            Assert.IsTrue(diff.Count == TrueCRC.Count, "Amount of changes in jg2p00_00_00.pp was incorrect. Expected value: {0}; Actual value: {1}", TrueCRC.Count, diff.Count);

            foreach (KeyValuePair<string, uint> kv in TrueCRC)
                Assert.IsTrue(kv.Value == diff[kv.Key], "CRC check failed after installation. Key: {0}; Expected value: {1}; Actual value: {2}", kv.Key, kv.Value, diff[kv.Key]);

            foreach (ListViewItem lv in form.lsvMods.Items)
                lv.Checked = false;

            Assert.IsTrue(form.inject(false, false, true), "Uninstallation injection failed. Log: {0}", form.labelStatus.Text);

            Assert.IsTrue(!File.Exists(Environment.CurrentDirectory + @"\testdir\jg2e04_00_TEST.pp"), "jg2e04_00_TEST.pp was not deleted.");
            Assert.IsTrue(!File.Exists(Environment.CurrentDirectory + @"\testdir\jg2p01_00_TEST.pp"), "jg2p01_00_TEST.pp was not deleted.");

            foreach (Mod m in form.modDict.Values)
            {
                Assert.IsTrue(!m.Installed, "Backup archive for {0} was not deleted.", m.Name);
            }

            Assert.IsTrue(subfiles.Count == new ppParser(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp", new ppFormat_AA2()).Subfiles.Count, "Amount of restored changes in jg2p00_00_00.pp was incorrect. Expected value: {0}; Actual value: {1}", new object[] { subfiles.Count, new ppParser(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp", new ppFormat_AA2()).Subfiles.Count });

            foreach (IWriteFile kv in new ppParser(Environment.CurrentDirectory + @"\testdir\jg2p00_00_00.pp", new ppFormat_AA2()).Subfiles)
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    kv.WriteTo(mem);
                    byte[] buffer = new byte[mem.Length];
                    mem.Position = 0;
                    mem.Read(buffer, 0, (int)mem.Length);

                    var value = DamienG.Security.Cryptography.Crc32.Compute(buffer);
                    Assert.IsTrue(CRCValues[kv.Name] == value, "CRC check failed after uninstallation. Key: {0}; Expected value: {1}; Actual value: {2}", kv.Name, CRCValues[kv.Name], value);
                }
            };

            Configuration.saveMods(form.modDict);
        }

        [ClassCleanup()]
        public static void Cleanup()
        {
            foreach (string s in Directory.GetFiles(Environment.CurrentDirectory + @"\testdir\"))
                try { File.Delete(s); }
                catch { }
            foreach (string s in Directory.GetFiles(Environment.CurrentDirectory + @"\backup\"))
                try { File.Delete(s); }
                catch { }
        }
    }
}