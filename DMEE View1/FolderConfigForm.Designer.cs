namespace DMEEView1
{
    partial class FolderConfigForm
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
            this.LibraryFolderTextBox = new System.Windows.Forms.TextBox();
            this.labelLibraryFolder = new System.Windows.Forms.Label();
            this.WorkingFolderTextBox = new System.Windows.Forms.TextBox();
            this.labelWorkingFolder = new System.Windows.Forms.Label();
            this.buttonChangeLibraryFolder = new System.Windows.Forms.Button();
            this.buttonChangeWorkingFolder = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SaveButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog2 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // LibraryFolderTextBox
            // 
            this.LibraryFolderTextBox.Location = new System.Drawing.Point(106, 20);
            this.LibraryFolderTextBox.Name = "LibraryFolderTextBox";
            this.LibraryFolderTextBox.ReadOnly = true;
            this.LibraryFolderTextBox.Size = new System.Drawing.Size(356, 20);
            this.LibraryFolderTextBox.TabIndex = 0;
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
            // WorkingFolderTextBox
            // 
            this.WorkingFolderTextBox.Location = new System.Drawing.Point(106, 61);
            this.WorkingFolderTextBox.Name = "WorkingFolderTextBox";
            this.WorkingFolderTextBox.ReadOnly = true;
            this.WorkingFolderTextBox.Size = new System.Drawing.Size(356, 20);
            this.WorkingFolderTextBox.TabIndex = 0;
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
            this.buttonChangeLibraryFolder.Click += new System.EventHandler(this.ChangeLibraryFolderButton_Click);
            // 
            // buttonChangeWorkingFolder
            // 
            this.buttonChangeWorkingFolder.Location = new System.Drawing.Point(468, 56);
            this.buttonChangeWorkingFolder.Name = "buttonChangeWorkingFolder";
            this.buttonChangeWorkingFolder.Size = new System.Drawing.Size(83, 29);
            this.buttonChangeWorkingFolder.TabIndex = 2;
            this.buttonChangeWorkingFolder.Text = "change";
            this.buttonChangeWorkingFolder.UseVisualStyleBackColor = true;
            this.buttonChangeWorkingFolder.Click += new System.EventHandler(this.ChangeWorkingFolderButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(379, 109);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(83, 29);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(468, 109);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(83, 29);
            this.CancelButton.TabIndex = 2;
            this.CancelButton.Text = "cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // FolderConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 152);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.buttonChangeWorkingFolder);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.buttonChangeLibraryFolder);
            this.Controls.Add(this.labelWorkingFolder);
            this.Controls.Add(this.labelLibraryFolder);
            this.Controls.Add(this.WorkingFolderTextBox);
            this.Controls.Add(this.LibraryFolderTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FolderConfigForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Folders";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox LibraryFolderTextBox;
        private System.Windows.Forms.Label labelLibraryFolder;
        private System.Windows.Forms.TextBox WorkingFolderTextBox;
        private System.Windows.Forms.Label labelWorkingFolder;
        private System.Windows.Forms.Button buttonChangeLibraryFolder;
        private System.Windows.Forms.Button buttonChangeWorkingFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog2;
    }
}