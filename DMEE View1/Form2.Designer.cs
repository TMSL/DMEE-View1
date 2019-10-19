namespace DMEEView1
{
    partial class Form2
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
            this.textBoxLibraryFolder = new System.Windows.Forms.TextBox();
            this.labelLibraryFolder = new System.Windows.Forms.Label();
            this.textBoxWorkingFolder = new System.Windows.Forms.TextBox();
            this.labelWorkingFolder = new System.Windows.Forms.Label();
            this.buttonChangeLibraryFolder = new System.Windows.Forms.Button();
            this.buttonChangeWorkingFolder = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // textBoxLibraryFolder
            // 
            this.textBoxLibraryFolder.Location = new System.Drawing.Point(106, 20);
            this.textBoxLibraryFolder.Name = "textBoxLibraryFolder";
            this.textBoxLibraryFolder.Size = new System.Drawing.Size(356, 20);
            this.textBoxLibraryFolder.TabIndex = 0;
            // 
            // labelLibraryFolder
            // 
            this.labelLibraryFolder.AutoSize = true;
            this.labelLibraryFolder.Location = new System.Drawing.Point(27, 23);
            this.labelLibraryFolder.Name = "labelLibraryFolder";
            this.labelLibraryFolder.Size = new System.Drawing.Size(73, 13);
            this.labelLibraryFolder.TabIndex = 1;
            this.labelLibraryFolder.Text = "Library Folder:";
            // 
            // textBoxWorkingFolder
            // 
            this.textBoxWorkingFolder.Location = new System.Drawing.Point(106, 61);
            this.textBoxWorkingFolder.Name = "textBoxWorkingFolder";
            this.textBoxWorkingFolder.Size = new System.Drawing.Size(356, 20);
            this.textBoxWorkingFolder.TabIndex = 0;
            // 
            // labelWorkingFolder
            // 
            this.labelWorkingFolder.AutoSize = true;
            this.labelWorkingFolder.Location = new System.Drawing.Point(18, 64);
            this.labelWorkingFolder.Name = "labelWorkingFolder";
            this.labelWorkingFolder.Size = new System.Drawing.Size(82, 13);
            this.labelWorkingFolder.TabIndex = 1;
            this.labelWorkingFolder.Text = "Working Folder:";
            // 
            // buttonChangeLibraryFolder
            // 
            this.buttonChangeLibraryFolder.Location = new System.Drawing.Point(468, 15);
            this.buttonChangeLibraryFolder.Name = "buttonChangeLibraryFolder";
            this.buttonChangeLibraryFolder.Size = new System.Drawing.Size(83, 29);
            this.buttonChangeLibraryFolder.TabIndex = 2;
            this.buttonChangeLibraryFolder.Text = "change";
            this.buttonChangeLibraryFolder.UseVisualStyleBackColor = true;
            this.buttonChangeLibraryFolder.Click += new System.EventHandler(this.buttonChangeLibraryFolder_Click);
            // 
            // buttonChangeWorkingFolder
            // 
            this.buttonChangeWorkingFolder.Location = new System.Drawing.Point(468, 56);
            this.buttonChangeWorkingFolder.Name = "buttonChangeWorkingFolder";
            this.buttonChangeWorkingFolder.Size = new System.Drawing.Size(83, 29);
            this.buttonChangeWorkingFolder.TabIndex = 2;
            this.buttonChangeWorkingFolder.Text = "change";
            this.buttonChangeWorkingFolder.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 108);
            this.Controls.Add(this.buttonChangeWorkingFolder);
            this.Controls.Add(this.buttonChangeLibraryFolder);
            this.Controls.Add(this.labelWorkingFolder);
            this.Controls.Add(this.labelLibraryFolder);
            this.Controls.Add(this.textBoxWorkingFolder);
            this.Controls.Add(this.textBoxLibraryFolder);
            this.Name = "Form2";
            this.Text = "Folders";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxLibraryFolder;
        private System.Windows.Forms.Label labelLibraryFolder;
        private System.Windows.Forms.TextBox textBoxWorkingFolder;
        private System.Windows.Forms.Label labelWorkingFolder;
        private System.Windows.Forms.Button buttonChangeLibraryFolder;
        private System.Windows.Forms.Button buttonChangeWorkingFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}