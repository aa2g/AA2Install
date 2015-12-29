namespace AA2Install
{
    partial class formChanges
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formChanges));
            this.label1 = new System.Windows.Forms.Label();
            this.lsbInstall = new System.Windows.Forms.ListBox();
            this.lsbUninstall = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "To be installed:";
            // 
            // lsbInstall
            // 
            this.lsbInstall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsbInstall.FormattingEnabled = true;
            this.lsbInstall.Location = new System.Drawing.Point(6, 25);
            this.lsbInstall.Name = "lsbInstall";
            this.lsbInstall.Size = new System.Drawing.Size(320, 121);
            this.lsbInstall.TabIndex = 1;
            // 
            // lsbUninstall
            // 
            this.lsbUninstall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsbUninstall.FormattingEnabled = true;
            this.lsbUninstall.Location = new System.Drawing.Point(6, 165);
            this.lsbUninstall.Name = "lsbUninstall";
            this.lsbUninstall.Size = new System.Drawing.Size(320, 121);
            this.lsbUninstall.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 149);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "To be uninstalled:";
            // 
            // formChanges
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 293);
            this.Controls.Add(this.lsbUninstall);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lsbInstall);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(2500, 329);
            this.MinimumSize = new System.Drawing.Size(250, 329);
            this.Name = "formChanges";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Pending Changes";
            this.Activated += new System.EventHandler(this.formChanges_Activated);
            this.Deactivate += new System.EventHandler(this.formChanges_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formChanges_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ListBox lsbInstall;
        public System.Windows.Forms.ListBox lsbUninstall;
    }
}