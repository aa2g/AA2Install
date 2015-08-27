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
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lsvMods = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnApply = new System.Windows.Forms.ToolStripButton();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.imagePreview = new System.Windows.Forms.PictureBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.prgMinor = new System.Windows.Forms.ToolStripProgressBar();
            this.prgMajor = new System.Windows.Forms.ToolStripProgressBar();
            this.labelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.rtbConsole = new System.Windows.Forms.RichTextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnRAW = new System.Windows.Forms.Button();
            this.checkRAW = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAA2EDIT = new System.Windows.Forms.Button();
            this.txtAA2EDIT = new System.Windows.Forms.TextBox();
            this.checkAA2EDIT = new System.Windows.Forms.CheckBox();
            this.btnAA2PLAY = new System.Windows.Forms.Button();
            this.txtAA2PLAY = new System.Windows.Forms.TextBox();
            this.checkAA2PLAY = new System.Windows.Forms.CheckBox();
            this.imageTimer = new System.Windows.Forms.Timer(this.components);
            this.colorGuideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnUninstall = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imagePreview)).BeginInit();
            this.statusMain.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.menuStrip1.Size = new System.Drawing.Size(864, 24);
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
            this.clearConsoleToolStripMenuItem});
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
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.colorGuideToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(864, 457);
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
            this.tabPage1.Size = new System.Drawing.Size(856, 431);
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
            this.splitContainer1.Size = new System.Drawing.Size(850, 403);
            this.splitContainer1.SplitterDistance = 461;
            this.splitContainer1.TabIndex = 3;
            // 
            // lsvMods
            // 
            this.lsvMods.CheckBoxes = true;
            this.lsvMods.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lsvMods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvMods.GridLines = true;
            this.lsvMods.LargeImageList = this.imageList1;
            this.lsvMods.Location = new System.Drawing.Point(0, 25);
            this.lsvMods.Name = "lsvMods";
            this.lsvMods.Size = new System.Drawing.Size(461, 378);
            this.lsvMods.SmallImageList = this.imageList1;
            this.lsvMods.TabIndex = 2;
            this.lsvMods.UseCompatibleStateImageBehavior = false;
            this.lsvMods.View = System.Windows.Forms.View.Details;
            this.lsvMods.SelectedIndexChanged += new System.EventHandler(this.listMods_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 308;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Estimated Size";
            this.columnHeader2.Width = 103;
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
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRefresh,
            this.btnApply,
            this.btnUninstall});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(461, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnRefresh
            // 
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(23, 22);
            this.btnRefresh.Text = "Refresh Mods";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnApply
            // 
            this.btnApply.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnApply.Image = ((System.Drawing.Image)(resources.GetObject("btnApply.Image")));
            this.btnApply.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(23, 22);
            this.btnApply.Text = "Install Checked";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
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
            this.splitContainer2.Panel2.Controls.Add(this.txtDescription);
            this.splitContainer2.Size = new System.Drawing.Size(385, 403);
            this.splitContainer2.SplitterDistance = 263;
            this.splitContainer2.TabIndex = 0;
            // 
            // imagePreview
            // 
            this.imagePreview.BackColor = System.Drawing.Color.White;
            this.imagePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imagePreview.Location = new System.Drawing.Point(0, 0);
            this.imagePreview.Name = "imagePreview";
            this.imagePreview.Size = new System.Drawing.Size(385, 263);
            this.imagePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imagePreview.TabIndex = 0;
            this.imagePreview.TabStop = false;
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescription.Location = new System.Drawing.Point(0, 0);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(385, 136);
            this.txtDescription.TabIndex = 0;
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
            this.statusMain.Size = new System.Drawing.Size(850, 22);
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
            this.tabPage2.Size = new System.Drawing.Size(751, 431);
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
            this.rtbConsole.Size = new System.Drawing.Size(745, 425);
            this.rtbConsole.TabIndex = 0;
            this.rtbConsole.Text = "";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnRAW);
            this.tabPage3.Controls.Add(this.checkRAW);
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(751, 431);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Preferences";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnRAW
            // 
            this.btnRAW.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRAW.Enabled = false;
            this.btnRAW.Location = new System.Drawing.Point(388, 118);
            this.btnRAW.Name = "btnRAW";
            this.btnRAW.Size = new System.Drawing.Size(355, 23);
            this.btnRAW.TabIndex = 2;
            this.btnRAW.Text = "Uncompress All Now";
            this.btnRAW.UseVisualStyleBackColor = true;
            // 
            // checkRAW
            // 
            this.checkRAW.AutoSize = true;
            this.checkRAW.Enabled = false;
            this.checkRAW.Location = new System.Drawing.Point(8, 122);
            this.checkRAW.Name = "checkRAW";
            this.checkRAW.Size = new System.Drawing.Size(374, 17);
            this.checkRAW.TabIndex = 1;
            this.checkRAW.Text = "Keep .pp files uncompressed (Faster, but requires 10-30gb of HDD space)";
            this.checkRAW.UseVisualStyleBackColor = true;
            this.checkRAW.CheckedChanged += new System.EventHandler(this.checkRAW_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnAA2EDIT);
            this.groupBox1.Controls.Add(this.txtAA2EDIT);
            this.groupBox1.Controls.Add(this.checkAA2EDIT);
            this.groupBox1.Controls.Add(this.btnAA2PLAY);
            this.groupBox1.Controls.Add(this.txtAA2PLAY);
            this.groupBox1.Controls.Add(this.checkAA2PLAY);
            this.groupBox1.Location = new System.Drawing.Point(8, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(735, 113);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Paths";
            // 
            // btnAA2EDIT
            // 
            this.btnAA2EDIT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAA2EDIT.Enabled = false;
            this.btnAA2EDIT.Location = new System.Drawing.Point(705, 81);
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
            this.txtAA2EDIT.Size = new System.Drawing.Size(693, 20);
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
            this.btnAA2PLAY.Location = new System.Drawing.Point(705, 36);
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
            this.txtAA2PLAY.Size = new System.Drawing.Size(693, 20);
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
            // imageTimer
            // 
            this.imageTimer.Enabled = true;
            this.imageTimer.Interval = 3000;
            this.imageTimer.Tick += new System.EventHandler(this.imageTimer_Tick);
            // 
            // colorGuideToolStripMenuItem
            // 
            this.colorGuideToolStripMenuItem.Name = "colorGuideToolStripMenuItem";
            this.colorGuideToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.colorGuideToolStripMenuItem.Text = "Color Guide";
            this.colorGuideToolStripMenuItem.Click += new System.EventHandler(this.colorGuideToolStripMenuItem_Click_1);
            // 
            // btnUninstall
            // 
            this.btnUninstall.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUninstall.Image = ((System.Drawing.Image)(resources.GetObject("btnUninstall.Image")));
            this.btnUninstall.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.Size = new System.Drawing.Size(23, 22);
            this.btnUninstall.Text = "Uninstall Checked";
            this.btnUninstall.Click += new System.EventHandler(this.btnUninstall_Click);
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 481);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(880, 519);
            this.Name = "formMain";
            this.Text = "AA2Install";
            this.Shown += new System.EventHandler(this.formMain_Shown);
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
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imagePreview)).EndInit();
            this.statusMain.ResumeLayout(false);
            this.statusMain.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox rtbConsole;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ListView lsvMods;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripButton btnApply;
        private System.Windows.Forms.ToolStripStatusLabel labelStatus;
        private System.Windows.Forms.ToolStripMenuItem clearConsoleToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAA2PLAY;
        private System.Windows.Forms.TextBox txtAA2PLAY;
        private System.Windows.Forms.CheckBox checkAA2PLAY;
        private System.Windows.Forms.Button btnAA2EDIT;
        private System.Windows.Forms.TextBox txtAA2EDIT;
        private System.Windows.Forms.CheckBox checkAA2EDIT;
        private System.Windows.Forms.Button btnRAW;
        private System.Windows.Forms.CheckBox checkRAW;
        private System.Windows.Forms.ToolStripProgressBar prgMinor;
        private System.Windows.Forms.ToolStripProgressBar prgMajor;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PictureBox imagePreview;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Timer imageTimer;
        private System.Windows.Forms.ToolStripMenuItem colorGuideToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnUninstall;
    }
}

