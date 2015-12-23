namespace AA2Install
{
    partial class formCrash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formCrash));
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.linkGithub = new System.Windows.Forms.LinkLabel();
            this.linkaa2g = new System.Windows.Forms.LinkLabel();
            this.linkHongfire = new System.Windows.Forms.LinkLabel();
            this.lsbFiles = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDetails = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(112, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(394, 93);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AA2Install.Properties.Resources.aa2install_error;
            this.pictureBox1.Location = new System.Drawing.Point(3, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(106, 102);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // linkGithub
            // 
            this.linkGithub.AutoSize = true;
            this.linkGithub.Location = new System.Drawing.Point(246, 48);
            this.linkGithub.Name = "linkGithub";
            this.linkGithub.Size = new System.Drawing.Size(63, 13);
            this.linkGithub.TabIndex = 2;
            this.linkGithub.TabStop = true;
            this.linkGithub.Text = "github issue";
            this.linkGithub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkGithub_LinkClicked);
            // 
            // linkaa2g
            // 
            this.linkaa2g.AutoSize = true;
            this.linkaa2g.Location = new System.Drawing.Point(369, 48);
            this.linkaa2g.Name = "linkaa2g";
            this.linkaa2g.Size = new System.Drawing.Size(41, 13);
            this.linkaa2g.TabIndex = 3;
            this.linkaa2g.TabStop = true;
            this.linkaa2g.Text = "/aa2g/";
            this.linkaa2g.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkaa2g_LinkClicked);
            // 
            // linkHongfire
            // 
            this.linkHongfire.AutoSize = true;
            this.linkHongfire.Location = new System.Drawing.Point(418, 48);
            this.linkHongfire.Name = "linkHongfire";
            this.linkHongfire.Size = new System.Drawing.Size(45, 13);
            this.linkHongfire.TabIndex = 4;
            this.linkHongfire.TabStop = true;
            this.linkHongfire.Text = "hongfire";
            this.linkHongfire.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkHongfire_LinkClicked);
            // 
            // lsbFiles
            // 
            this.lsbFiles.FormattingEnabled = true;
            this.lsbFiles.Items.AddRange(new object[] {
            "config.json.gz"});
            this.lsbFiles.Location = new System.Drawing.Point(115, 79);
            this.lsbFiles.Name = "lsbFiles";
            this.lsbFiles.Size = new System.Drawing.Size(391, 30);
            this.lsbFiles.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(494, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "Details:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDetails
            // 
            this.txtDetails.BackColor = System.Drawing.Color.White;
            this.txtDetails.Location = new System.Drawing.Point(12, 134);
            this.txtDetails.Multiline = true;
            this.txtDetails.Name = "txtDetails";
            this.txtDetails.ReadOnly = true;
            this.txtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDetails.Size = new System.Drawing.Size(494, 114);
            this.txtDetails.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(222, 254);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // formCrash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(518, 289);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtDetails);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lsbFiles);
            this.Controls.Add(this.linkHongfire);
            this.Controls.Add(this.linkaa2g);
            this.Controls.Add(this.linkGithub);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formCrash";
            this.Text = "Fatal Error";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel linkGithub;
        private System.Windows.Forms.LinkLabel linkaa2g;
        private System.Windows.Forms.LinkLabel linkHongfire;
        private System.Windows.Forms.ListBox lsbFiles;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDetails;
        private System.Windows.Forms.Button button1;
    }
}