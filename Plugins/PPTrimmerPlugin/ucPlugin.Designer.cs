namespace PPTrimmerPlugin
{
    partial class ucPlugin
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
            this.checkEnable = new System.Windows.Forms.CheckBox();
            this.prgProgress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // checkEnable
            // 
            this.checkEnable.AutoSize = true;
            this.checkEnable.Location = new System.Drawing.Point(3, 3);
            this.checkEnable.Name = "checkEnable";
            this.checkEnable.Size = new System.Drawing.Size(66, 17);
            this.checkEnable.TabIndex = 0;
            this.checkEnable.Text = "<plugin>";
            this.checkEnable.UseVisualStyleBackColor = true;
            // 
            // prgProgress
            // 
            this.prgProgress.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.prgProgress.Location = new System.Drawing.Point(165, 3);
            this.prgProgress.Name = "prgProgress";
            this.prgProgress.Size = new System.Drawing.Size(146, 17);
            this.prgProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgProgress.TabIndex = 1;
            // 
            // ucPlugin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.prgProgress);
            this.Controls.Add(this.checkEnable);
            this.Name = "ucPlugin";
            this.Size = new System.Drawing.Size(314, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkEnable;
        private System.Windows.Forms.ProgressBar prgProgress;
    }
}
