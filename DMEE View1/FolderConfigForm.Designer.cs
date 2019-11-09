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
            this.folderBrowserDialog2 = new System.Windows.Forms.FolderBrowserDialog();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
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
            // Cancel_Button
            // 
            this.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Button.Location = new System.Drawing.Point(468, 109);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(83, 29);
            this.Cancel_Button.TabIndex = 2;
            this.Cancel_Button.Text = "cancel";
            this.Cancel_Button.UseVisualStyleBackColor = true;
            this.Cancel_Button.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(12, 8);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(147, 17);
            this.radioButton1.TabIndex = 3;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Search working folder first";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(12, 31);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(137, 17);
            this.radioButton2.TabIndex = 3;
            this.radioButton2.Text = "Search library folder first";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Checked = true;
            this.radioButton3.Location = new System.Drawing.Point(4, 6);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(149, 17);
            this.radioButton3.TabIndex = 3;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Search libraries (.LBR) first";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(4, 29);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(99, 17);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.Text = "Search files first";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButton2);
            this.panel1.Controls.Add(this.radioButton1);
            this.panel1.Enabled = false;
            this.panel1.Location = new System.Drawing.Point(18, 90);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(172, 57);
            this.panel1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.radioButton4);
            this.panel2.Controls.Add(this.radioButton3);
            this.panel2.Enabled = false;
            this.panel2.Location = new System.Drawing.Point(200, 90);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(163, 57);
            this.panel2.TabIndex = 5;
            // 
            // FolderConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel_Button;
            this.ClientSize = new System.Drawing.Size(566, 152);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Cancel_Button);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.buttonChangeWorkingFolder);
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
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
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
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog2;
        private System.Windows.Forms.Button Cancel_Button;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}