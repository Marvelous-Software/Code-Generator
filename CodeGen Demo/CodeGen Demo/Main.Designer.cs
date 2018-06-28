namespace CodeGen_Demo
{
    partial class frmMain
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
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblOutputDir = new System.Windows.Forms.Label();
            this.txtOutputDir = new System.Windows.Forms.TextBox();
            this.grpLanguage = new System.Windows.Forms.GroupBox();
            this.rdoVB = new System.Windows.Forms.RadioButton();
            this.rdoCS = new System.Windows.Forms.RadioButton();
            this.btnGO = new System.Windows.Forms.Button();
            this.grpOutputVersion = new System.Windows.Forms.GroupBox();
            this.rdo2010 = new System.Windows.Forms.RadioButton();
            this.rdo2008 = new System.Windows.Forms.RadioButton();
            this.grpLanguage.SuspendLayout();
            this.grpOutputVersion.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(425, 148);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 0;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblOutputDir
            // 
            this.lblOutputDir.AutoSize = true;
            this.lblOutputDir.Location = new System.Drawing.Point(12, 134);
            this.lblOutputDir.Name = "lblOutputDir";
            this.lblOutputDir.Size = new System.Drawing.Size(71, 13);
            this.lblOutputDir.TabIndex = 1;
            this.lblOutputDir.Text = "Output Folder";
            // 
            // txtOutputDir
            // 
            this.txtOutputDir.Location = new System.Drawing.Point(12, 150);
            this.txtOutputDir.Name = "txtOutputDir";
            this.txtOutputDir.Size = new System.Drawing.Size(407, 20);
            this.txtOutputDir.TabIndex = 2;
            this.txtOutputDir.Text = "C:\\CodeGen";
            // 
            // grpLanguage
            // 
            this.grpLanguage.Controls.Add(this.rdoVB);
            this.grpLanguage.Controls.Add(this.rdoCS);
            this.grpLanguage.Location = new System.Drawing.Point(15, 26);
            this.grpLanguage.Name = "grpLanguage";
            this.grpLanguage.Size = new System.Drawing.Size(129, 95);
            this.grpLanguage.TabIndex = 3;
            this.grpLanguage.TabStop = false;
            this.grpLanguage.Text = "Output Language";
            // 
            // rdoVB
            // 
            this.rdoVB.AutoSize = true;
            this.rdoVB.Location = new System.Drawing.Point(16, 57);
            this.rdoVB.Name = "rdoVB";
            this.rdoVB.Size = new System.Drawing.Size(39, 17);
            this.rdoVB.TabIndex = 5;
            this.rdoVB.Text = "VB";
            this.rdoVB.UseVisualStyleBackColor = true;
            // 
            // rdoCS
            // 
            this.rdoCS.AutoSize = true;
            this.rdoCS.Checked = true;
            this.rdoCS.Location = new System.Drawing.Point(16, 29);
            this.rdoCS.Name = "rdoCS";
            this.rdoCS.Size = new System.Drawing.Size(39, 17);
            this.rdoCS.TabIndex = 4;
            this.rdoCS.TabStop = true;
            this.rdoCS.Text = "C#";
            this.rdoCS.UseVisualStyleBackColor = true;
            // 
            // btnGO
            // 
            this.btnGO.Location = new System.Drawing.Point(207, 194);
            this.btnGO.Name = "btnGO";
            this.btnGO.Size = new System.Drawing.Size(75, 23);
            this.btnGO.TabIndex = 4;
            this.btnGO.Text = "GO";
            this.btnGO.UseVisualStyleBackColor = true;
            this.btnGO.Click += new System.EventHandler(this.btnGO_Click);
            // 
            // grpOutputVersion
            // 
            this.grpOutputVersion.Controls.Add(this.rdo2010);
            this.grpOutputVersion.Controls.Add(this.rdo2008);
            this.grpOutputVersion.Location = new System.Drawing.Point(194, 26);
            this.grpOutputVersion.Name = "grpOutputVersion";
            this.grpOutputVersion.Size = new System.Drawing.Size(225, 95);
            this.grpOutputVersion.TabIndex = 5;
            this.grpOutputVersion.TabStop = false;
            this.grpOutputVersion.Text = "Output Version";
            // 
            // rdo2010
            // 
            this.rdo2010.AutoSize = true;
            this.rdo2010.Checked = true;
            this.rdo2010.Location = new System.Drawing.Point(16, 57);
            this.rdo2010.Name = "rdo2010";
            this.rdo2010.Size = new System.Drawing.Size(49, 17);
            this.rdo2010.TabIndex = 5;
            this.rdo2010.TabStop = true;
            this.rdo2010.Text = "2010";
            this.rdo2010.UseVisualStyleBackColor = true;
            // 
            // rdo2008
            // 
            this.rdo2008.AutoSize = true;
            this.rdo2008.Location = new System.Drawing.Point(16, 29);
            this.rdo2008.Name = "rdo2008";
            this.rdo2008.Size = new System.Drawing.Size(49, 17);
            this.rdo2008.TabIndex = 4;
            this.rdo2008.Text = "2008";
            this.rdo2008.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 270);
            this.Controls.Add(this.grpOutputVersion);
            this.Controls.Add(this.btnGO);
            this.Controls.Add(this.grpLanguage);
            this.Controls.Add(this.txtOutputDir);
            this.Controls.Add(this.lblOutputDir);
            this.Controls.Add(this.btnBrowse);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmMain";
            this.Text = "Code builder demo";
            this.grpLanguage.ResumeLayout(false);
            this.grpLanguage.PerformLayout();
            this.grpOutputVersion.ResumeLayout(false);
            this.grpOutputVersion.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblOutputDir;
        private System.Windows.Forms.TextBox txtOutputDir;
        private System.Windows.Forms.GroupBox grpLanguage;
        private System.Windows.Forms.RadioButton rdoVB;
        private System.Windows.Forms.RadioButton rdoCS;
        private System.Windows.Forms.Button btnGO;
        private System.Windows.Forms.GroupBox grpOutputVersion;
        private System.Windows.Forms.RadioButton rdo2010;
        private System.Windows.Forms.RadioButton rdo2008;
    }
}

