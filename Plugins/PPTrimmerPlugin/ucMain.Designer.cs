namespace PPTrimmerPlugin
{
    partial class ucMain
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lsvPP = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnRefreshPP = new System.Windows.Forms.Button();
            this.prgMajor = new System.Windows.Forms.ProgressBar();
            this.prgMinor = new System.Windows.Forms.ProgressBar();
            this.btnTrim = new System.Windows.Forms.Button();
            this.panelPlugins = new System.Windows.Forms.Panel();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnAnalyze = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lsvPP);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(633, 340);
            this.splitContainer1.SplitterDistance = 364;
            this.splitContainer1.TabIndex = 0;
            // 
            // lsvPP
            // 
            this.lsvPP.CheckBoxes = true;
            this.lsvPP.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lsvPP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvPP.GridLines = true;
            this.lsvPP.Location = new System.Drawing.Point(0, 0);
            this.lsvPP.Name = "lsvPP";
            this.lsvPP.Size = new System.Drawing.Size(364, 340);
            this.lsvPP.TabIndex = 0;
            this.lsvPP.UseCompatibleStateImageBehavior = false;
            this.lsvPP.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 156;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Original Size";
            this.columnHeader2.Width = 117;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Savings";
            this.columnHeader3.Width = 82;
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
            this.splitContainer2.Panel1.Controls.Add(this.btnAnalyze);
            this.splitContainer2.Panel1.Controls.Add(this.btnSelectAll);
            this.splitContainer2.Panel1.Controls.Add(this.btnRefreshPP);
            this.splitContainer2.Panel1.Controls.Add(this.prgMajor);
            this.splitContainer2.Panel1.Controls.Add(this.prgMinor);
            this.splitContainer2.Panel1.Controls.Add(this.btnTrim);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panelPlugins);
            this.splitContainer2.Size = new System.Drawing.Size(265, 340);
            this.splitContainer2.SplitterDistance = 186;
            this.splitContainer2.TabIndex = 0;
            // 
            // btnRefreshPP
            // 
            this.btnRefreshPP.Location = new System.Drawing.Point(12, 98);
            this.btnRefreshPP.Name = "btnRefreshPP";
            this.btnRefreshPP.Size = new System.Drawing.Size(94, 23);
            this.btnRefreshPP.TabIndex = 2;
            this.btnRefreshPP.Text = "Refresh PP files";
            this.btnRefreshPP.UseVisualStyleBackColor = true;
            this.btnRefreshPP.Click += new System.EventHandler(this.btnRefreshPP_Click);
            // 
            // prgMajor
            // 
            this.prgMajor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prgMajor.Location = new System.Drawing.Point(12, 163);
            this.prgMajor.Name = "prgMajor";
            this.prgMajor.Size = new System.Drawing.Size(241, 20);
            this.prgMajor.TabIndex = 1;
            // 
            // prgMinor
            // 
            this.prgMinor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prgMinor.Location = new System.Drawing.Point(12, 137);
            this.prgMinor.Name = "prgMinor";
            this.prgMinor.Size = new System.Drawing.Size(241, 20);
            this.prgMinor.TabIndex = 1;
            // 
            // btnTrim
            // 
            this.btnTrim.Location = new System.Drawing.Point(12, 15);
            this.btnTrim.Name = "btnTrim";
            this.btnTrim.Size = new System.Drawing.Size(94, 23);
            this.btnTrim.TabIndex = 0;
            this.btnTrim.Text = "Trim";
            this.btnTrim.UseVisualStyleBackColor = true;
            this.btnTrim.Click += new System.EventHandler(this.btnTrim_Click);
            // 
            // panelPlugins
            // 
            this.panelPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPlugins.Location = new System.Drawing.Point(0, 0);
            this.panelPlugins.Name = "panelPlugins";
            this.panelPlugins.Size = new System.Drawing.Size(265, 150);
            this.panelPlugins.TabIndex = 0;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(112, 15);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(94, 23);
            this.btnSelectAll.TabIndex = 3;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Location = new System.Drawing.Point(12, 44);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(94, 23);
            this.btnAnalyze.TabIndex = 4;
            this.btnAnalyze.Text = "Analyze";
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // ucMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ucMain";
            this.Size = new System.Drawing.Size(633, 340);
            this.Load += new System.EventHandler(this.ucMain_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView lsvPP;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panelPlugins;
        private System.Windows.Forms.Button btnTrim;
        private System.Windows.Forms.ProgressBar prgMajor;
        private System.Windows.Forms.ProgressBar prgMinor;
        private System.Windows.Forms.Button btnRefreshPP;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnAnalyze;
    }
}
