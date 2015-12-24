namespace AA2Install
{
    partial class formMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.flushCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lsvMods = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.modContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnApply = new System.Windows.Forms.ToolStripButton();
            this.btnCancel = new System.Windows.Forms.ToolStripButton();
            this.cmbSorting = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.txtSearch = new System.Windows.Forms.ToolStripTextBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.imagePreview = new System.Windows.Forms.PictureBox();
            this.rtbDescription = new System.Windows.Forms.RichTextBox();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.prgMinor = new System.Windows.Forms.ToolStripProgressBar();
            this.prgMajor = new System.Windows.Forms.ToolStripProgressBar();
            this.labelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.rtbConsole = new System.Windows.Forms.RichTextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkConflicts = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblEditPath = new System.Windows.Forms.Label();
            this.lblPlayPath = new System.Windows.Forms.Label();
            this.btnAA2EDIT = new System.Windows.Forms.Button();
            this.txtAA2EDIT = new System.Windows.Forms.TextBox();
            this.checkAA2EDIT = new System.Windows.Forms.CheckBox();
            this.btnAA2PLAY = new System.Windows.Forms.Button();
            this.txtAA2PLAY = new System.Windows.Forms.TextBox();
            this.checkAA2PLAY = new System.Windows.Forms.CheckBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.txtMigrate = new System.Windows.Forms.TextBox();
            this.btnMigrate = new System.Windows.Forms.Button();
            this.btnBrowseMigrate = new System.Windows.Forms.Button();
            this.txtBrowseMigrate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.lsvLog = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.imageTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.modContextMenu.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imagePreview)).BeginInit();
            this.statusMain.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(977, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearConsoleToolStripMenuItem,
            this.clearLogToolStripMenuItem,
            this.toolStripSeparator2,
            this.flushCacheToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // clearConsoleToolStripMenuItem
            // 
            this.clearConsoleToolStripMenuItem.Name = "clearConsoleToolStripMenuItem";
            this.clearConsoleToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.clearConsoleToolStripMenuItem.Text = "Clear Console";
            this.clearConsoleToolStripMenuItem.Click += new System.EventHandler(this.clearConsoleToolStripMenuItem_Click);
            // 
            // clearLogToolStripMenuItem
            // 
            this.clearLogToolStripMenuItem.Name = "clearLogToolStripMenuItem";
            this.clearLogToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.clearLogToolStripMenuItem.Text = "Clear Log";
            this.clearLogToolStripMenuItem.Click += new System.EventHandler(this.clearLogToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(144, 6);
            // 
            // flushCacheToolStripMenuItem
            // 
            this.flushCacheToolStripMenuItem.Name = "flushCacheToolStripMenuItem";
            this.flushCacheToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.flushCacheToolStripMenuItem.Text = "Flush Cache";
            this.flushCacheToolStripMenuItem.Click += new System.EventHandler(this.flushCacheToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(977, 457);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Controls.Add(this.statusMain);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(969, 431);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Mods";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lsvMods);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(963, 403);
            this.splitContainer1.SplitterDistance = 522;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 3;
            // 
            // lsvMods
            // 
            this.lsvMods.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lsvMods.CheckBoxes = true;
            this.lsvMods.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lsvMods.ContextMenuStrip = this.modContextMenu;
            this.lsvMods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvMods.GridLines = true;
            this.lsvMods.Location = new System.Drawing.Point(0, 25);
            this.lsvMods.Name = "lsvMods";
            this.lsvMods.OwnerDraw = true;
            this.lsvMods.Size = new System.Drawing.Size(522, 378);
            this.lsvMods.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lsvMods.TabIndex = 1;
            this.lsvMods.UseCompatibleStateImageBehavior = false;
            this.lsvMods.View = System.Windows.Forms.View.Details;
            this.lsvMods.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lsvMods_DrawColumnHeader);
            this.lsvMods.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.lsvMods_DrawItem);
            this.lsvMods.SelectedIndexChanged += new System.EventHandler(this.lsvMods_SelectedIndexChanged);
            this.lsvMods.SizeChanged += new System.EventHandler(this.lsvMods_SizeChanged);
            this.lsvMods.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lsvMods_KeyPress);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 428;
            // 
            // modContextMenu
            // 
            this.modContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.modContextMenu.Name = "modContextMenu";
            this.modContextMenu.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Enabled = false;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRefresh,
            this.btnApply,
            this.btnCancel,
            this.cmbSorting,
            this.toolStripLabel1,
            this.toolStripSeparator1,
            this.txtSearch});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(522, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(66, 22);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.ToolTipText = "Refresh Mods";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnApply
            // 
            this.btnApply.Image = ((System.Drawing.Image)(resources.GetObject("btnApply.Image")));
            this.btnApply.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(91, 22);
            this.btnApply.Text = "Synchronize";
            this.btnApply.ToolTipText = "Synchronize checked";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(63, 22);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbSorting
            // 
            this.cmbSorting.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmbSorting.Items.AddRange(new object[] {
            "Name",
            "Date"});
            this.cmbSorting.Name = "cmbSorting";
            this.cmbSorting.Size = new System.Drawing.Size(75, 25);
            this.cmbSorting.Text = "Name";
            this.cmbSorting.SelectedIndexChanged += new System.EventHandler(this.cmbSorting_SelectedIndexChanged);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(47, 22);
            this.toolStripLabel1.Text = "Sort by:";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // txtSearch
            // 
            this.txtSearch.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(100, 25);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.imagePreview);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.rtbDescription);
            this.splitContainer2.Size = new System.Drawing.Size(440, 403);
            this.splitContainer2.SplitterDistance = 263;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 0;
            // 
            // imagePreview
            // 
            this.imagePreview.BackColor = System.Drawing.Color.White;
            this.imagePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imagePreview.Location = new System.Drawing.Point(0, 0);
            this.imagePreview.Name = "imagePreview";
            this.imagePreview.Size = new System.Drawing.Size(440, 263);
            this.imagePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imagePreview.TabIndex = 0;
            this.imagePreview.TabStop = false;
            // 
            // rtbDescription
            // 
            this.rtbDescription.BackColor = System.Drawing.SystemColors.Window;
            this.rtbDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDescription.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbDescription.Location = new System.Drawing.Point(0, 0);
            this.rtbDescription.Name = "rtbDescription";
            this.rtbDescription.ReadOnly = true;
            this.rtbDescription.Size = new System.Drawing.Size(440, 139);
            this.rtbDescription.TabIndex = 0;
            this.rtbDescription.Text = "";
            // 
            // statusMain
            // 
            this.statusMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.prgMinor,
            this.prgMajor,
            this.labelStatus});
            this.statusMain.Location = new System.Drawing.Point(3, 406);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(963, 22);
            this.statusMain.TabIndex = 1;
            this.statusMain.Text = "statusStrip1";
            // 
            // prgMinor
            // 
            this.prgMinor.Name = "prgMinor";
            this.prgMinor.Size = new System.Drawing.Size(100, 16);
            // 
            // prgMajor
            // 
            this.prgMajor.Name = "prgMajor";
            this.prgMajor.Size = new System.Drawing.Size(100, 16);
            // 
            // labelStatus
            // 
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(26, 17);
            this.labelStatus.Text = "Idle";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Black;
            this.tabPage2.Controls.Add(this.rtbConsole);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(969, 431);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Console Output";
            // 
            // rtbConsole
            // 
            this.rtbConsole.BackColor = System.Drawing.Color.Black;
            this.rtbConsole.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbConsole.Cursor = System.Windows.Forms.Cursors.Default;
            this.rtbConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbConsole.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbConsole.ForeColor = System.Drawing.Color.White;
            this.rtbConsole.Location = new System.Drawing.Point(3, 3);
            this.rtbConsole.Name = "rtbConsole";
            this.rtbConsole.ReadOnly = true;
            this.rtbConsole.Size = new System.Drawing.Size(963, 425);
            this.rtbConsole.TabIndex = 0;
            this.rtbConsole.Text = "";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(969, 431);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Preferences";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.checkConflicts);
            this.groupBox2.Location = new System.Drawing.Point(8, 122);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(953, 49);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Installation";
            // 
            // checkConflicts
            // 
            this.checkConflicts.AutoSize = true;
            this.checkConflicts.Location = new System.Drawing.Point(6, 19);
            this.checkConflicts.Name = "checkConflicts";
            this.checkConflicts.Size = new System.Drawing.Size(302, 17);
            this.checkConflicts.TabIndex = 0;
            this.checkConflicts.Text = "Ignore conflicts (Uninstallation results will be unpredictable)";
            this.checkConflicts.UseVisualStyleBackColor = true;
            this.checkConflicts.CheckedChanged += new System.EventHandler(this.checkConflicts_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblEditPath);
            this.groupBox1.Controls.Add(this.lblPlayPath);
            this.groupBox1.Controls.Add(this.btnAA2EDIT);
            this.groupBox1.Controls.Add(this.txtAA2EDIT);
            this.groupBox1.Controls.Add(this.checkAA2EDIT);
            this.groupBox1.Controls.Add(this.btnAA2PLAY);
            this.groupBox1.Controls.Add(this.txtAA2PLAY);
            this.groupBox1.Controls.Add(this.checkAA2PLAY);
            this.groupBox1.Location = new System.Drawing.Point(8, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(953, 113);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Paths";
            // 
            // lblEditPath
            // 
            this.lblEditPath.AutoSize = true;
            this.lblEditPath.Location = new System.Drawing.Point(195, 65);
            this.lblEditPath.Name = "lblEditPath";
            this.lblEditPath.Size = new System.Drawing.Size(125, 13);
            this.lblEditPath.TabIndex = 6;
            this.lblEditPath.Text = "Current AA2_EDIT path: ";
            // 
            // lblPlayPath
            // 
            this.lblPlayPath.AutoSize = true;
            this.lblPlayPath.Location = new System.Drawing.Point(195, 20);
            this.lblPlayPath.Name = "lblPlayPath";
            this.lblPlayPath.Size = new System.Drawing.Size(127, 13);
            this.lblPlayPath.TabIndex = 5;
            this.lblPlayPath.Text = "Current AA2_PLAY path: ";
            // 
            // btnAA2EDIT
            // 
            this.btnAA2EDIT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAA2EDIT.Enabled = false;
            this.btnAA2EDIT.Location = new System.Drawing.Point(923, 81);
            this.btnAA2EDIT.Name = "btnAA2EDIT";
            this.btnAA2EDIT.Size = new System.Drawing.Size(24, 23);
            this.btnAA2EDIT.TabIndex = 4;
            this.btnAA2EDIT.Text = "...";
            this.btnAA2EDIT.UseVisualStyleBackColor = true;
            this.btnAA2EDIT.Click += new System.EventHandler(this.btnAA2EDIT_Click);
            // 
            // txtAA2EDIT
            // 
            this.txtAA2EDIT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAA2EDIT.Enabled = false;
            this.txtAA2EDIT.Location = new System.Drawing.Point(6, 83);
            this.txtAA2EDIT.Name = "txtAA2EDIT";
            this.txtAA2EDIT.Size = new System.Drawing.Size(911, 20);
            this.txtAA2EDIT.TabIndex = 3;
            this.txtAA2EDIT.TextChanged += new System.EventHandler(this.txtAA2EDIT_TextChanged);
            // 
            // checkAA2EDIT
            // 
            this.checkAA2EDIT.AutoSize = true;
            this.checkAA2EDIT.Location = new System.Drawing.Point(6, 64);
            this.checkAA2EDIT.Name = "checkAA2EDIT";
            this.checkAA2EDIT.Size = new System.Drawing.Size(165, 17);
            this.checkAA2EDIT.TabIndex = 2;
            this.checkAA2EDIT.Text = "Custom AA2_EDIT\\data path";
            this.checkAA2EDIT.UseVisualStyleBackColor = true;
            this.checkAA2EDIT.CheckedChanged += new System.EventHandler(this.checkAA2EDIT_CheckedChanged);
            // 
            // btnAA2PLAY
            // 
            this.btnAA2PLAY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAA2PLAY.Enabled = false;
            this.btnAA2PLAY.Location = new System.Drawing.Point(923, 36);
            this.btnAA2PLAY.Name = "btnAA2PLAY";
            this.btnAA2PLAY.Size = new System.Drawing.Size(24, 23);
            this.btnAA2PLAY.TabIndex = 1;
            this.btnAA2PLAY.Text = "...";
            this.btnAA2PLAY.UseVisualStyleBackColor = true;
            this.btnAA2PLAY.Click += new System.EventHandler(this.btnAA2PLAY_Click);
            // 
            // txtAA2PLAY
            // 
            this.txtAA2PLAY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAA2PLAY.Enabled = false;
            this.txtAA2PLAY.Location = new System.Drawing.Point(6, 38);
            this.txtAA2PLAY.Name = "txtAA2PLAY";
            this.txtAA2PLAY.Size = new System.Drawing.Size(911, 20);
            this.txtAA2PLAY.TabIndex = 1;
            this.txtAA2PLAY.TextChanged += new System.EventHandler(this.txtAA2PLAY_TextChanged);
            // 
            // checkAA2PLAY
            // 
            this.checkAA2PLAY.AutoSize = true;
            this.checkAA2PLAY.Location = new System.Drawing.Point(6, 19);
            this.checkAA2PLAY.Name = "checkAA2PLAY";
            this.checkAA2PLAY.Size = new System.Drawing.Size(167, 17);
            this.checkAA2PLAY.TabIndex = 0;
            this.checkAA2PLAY.Text = "Custom AA2_PLAY\\data path";
            this.checkAA2PLAY.UseVisualStyleBackColor = true;
            this.checkAA2PLAY.CheckedChanged += new System.EventHandler(this.checkAA2PLAY_CheckedChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.txtMigrate);
            this.tabPage4.Controls.Add(this.btnMigrate);
            this.tabPage4.Controls.Add(this.btnBrowseMigrate);
            this.tabPage4.Controls.Add(this.txtBrowseMigrate);
            this.tabPage4.Controls.Add(this.label1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(969, 431);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Wizzard Migration";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // txtMigrate
            // 
            this.txtMigrate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMigrate.Location = new System.Drawing.Point(0, 63);
            this.txtMigrate.Multiline = true;
            this.txtMigrate.Name = "txtMigrate";
            this.txtMigrate.ReadOnly = true;
            this.txtMigrate.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMigrate.Size = new System.Drawing.Size(969, 368);
            this.txtMigrate.TabIndex = 12;
            // 
            // btnMigrate
            // 
            this.btnMigrate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMigrate.Location = new System.Drawing.Point(11, 33);
            this.btnMigrate.Name = "btnMigrate";
            this.btnMigrate.Size = new System.Drawing.Size(950, 23);
            this.btnMigrate.TabIndex = 11;
            this.btnMigrate.Text = "Migrate";
            this.btnMigrate.UseVisualStyleBackColor = true;
            this.btnMigrate.Click += new System.EventHandler(this.btnMigrate_Click);
            // 
            // btnBrowseMigrate
            // 
            this.btnBrowseMigrate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseMigrate.Location = new System.Drawing.Point(937, 5);
            this.btnBrowseMigrate.Name = "btnBrowseMigrate";
            this.btnBrowseMigrate.Size = new System.Drawing.Size(24, 23);
            this.btnBrowseMigrate.TabIndex = 10;
            this.btnBrowseMigrate.Text = "...";
            this.btnBrowseMigrate.UseVisualStyleBackColor = true;
            this.btnBrowseMigrate.Click += new System.EventHandler(this.btnBrowseMigrate_Click);
            // 
            // txtBrowseMigrate
            // 
            this.txtBrowseMigrate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBrowseMigrate.Location = new System.Drawing.Point(106, 7);
            this.txtBrowseMigrate.Name = "txtBrowseMigrate";
            this.txtBrowseMigrate.Size = new System.Drawing.Size(825, 20);
            this.txtBrowseMigrate.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Wizzard Location:";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.lsvLog);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(969, 431);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Log";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // lsvLog
            // 
            this.lsvLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3});
            this.lsvLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvLog.GridLines = true;
            this.lsvLog.LargeImageList = this.imageList1;
            this.lsvLog.Location = new System.Drawing.Point(3, 3);
            this.lsvLog.Name = "lsvLog";
            this.lsvLog.Size = new System.Drawing.Size(963, 425);
            this.lsvLog.SmallImageList = this.imageList1;
            this.lsvLog.TabIndex = 0;
            this.lsvLog.UseCompatibleStateImageBehavior = false;
            this.lsvLog.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Entry";
            this.columnHeader2.Width = 716;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Time taken";
            this.columnHeader3.Width = 75;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "bullet_black.png");
            this.imageList1.Images.SetKeyName(1, "bullet_green.png");
            this.imageList1.Images.SetKeyName(2, "bullet_go.png");
            this.imageList1.Images.SetKeyName(3, "bullet_error.png");
            this.imageList1.Images.SetKeyName(4, "accept.png");
            this.imageList1.Images.SetKeyName(5, "cross-circle.png");
            // 
            // imageTimer
            // 
            this.imageTimer.Enabled = true;
            this.imageTimer.Interval = 3000;
            this.imageTimer.Tick += new System.EventHandler(this.imageTimer_Tick);
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 481);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(880, 519);
            this.Name = "formMain";
            this.Text = "AA2Install";
            this.Load += new System.EventHandler(this.formMain_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.modContextMenu.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imagePreview)).EndInit();
            this.statusMain.ResumeLayout(false);
            this.statusMain.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripButton btnApply;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAA2PLAY;
        private System.Windows.Forms.TextBox txtAA2PLAY;
        private System.Windows.Forms.CheckBox checkAA2PLAY;
        private System.Windows.Forms.Button btnAA2EDIT;
        private System.Windows.Forms.TextBox txtAA2EDIT;
        private System.Windows.Forms.CheckBox checkAA2EDIT;
        private System.Windows.Forms.ToolStripProgressBar prgMinor;
        private System.Windows.Forms.ToolStripProgressBar prgMajor;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PictureBox imagePreview;
        private System.Windows.Forms.Timer imageTimer;
        private System.Windows.Forms.Label lblEditPath;
        private System.Windows.Forms.Label lblPlayPath;
        private System.Windows.Forms.RichTextBox rtbDescription;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkConflicts;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox txtMigrate;
        private System.Windows.Forms.Button btnMigrate;
        private System.Windows.Forms.Button btnBrowseMigrate;
        private System.Windows.Forms.TextBox txtBrowseMigrate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ToolStripButton btnCancel;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox txtSearch;
        public System.Windows.Forms.ListView lsvMods;
        public System.Windows.Forms.ListView lsvLog;
        public System.Windows.Forms.ToolStripStatusLabel labelStatus;
        private System.Windows.Forms.ContextMenuStrip modContextMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearConsoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem flushCacheToolStripMenuItem;
        public System.Windows.Forms.RichTextBox rtbConsole;
        public System.Windows.Forms.ToolStripComboBox cmbSorting;
    }
}

