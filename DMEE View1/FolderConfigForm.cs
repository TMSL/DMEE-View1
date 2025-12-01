using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace DMEEView1
{
    public partial class FolderConfigForm : Form
    {
        public string libraryFolder = "";
        public List<string> libraryFileList = new List<string>();
        public string workingFolder = "";

        public FolderConfigForm()
        {
            InitializeComponent();
            this.ControlBox = false;
            libraryFolder = Properties.Settings.Default.LibFolder;
            if (Directory.Exists(libraryFolder))
            {
                libraryFolder = Properties.Settings.Default.LibFolder;
                IEnumerable<string> libFiles = Directory.EnumerateFiles(libraryFolder, "*.lbr", SearchOption.TopDirectoryOnly);
                libraryFileList.Clear();
                foreach (string ss in libFiles) libraryFileList.Add(ss);
            }
            else
            {
                libraryFolder = Directory.GetCurrentDirectory();
                Properties.Settings.Default.LibFolder = libraryFolder;
            }
            LibraryFolderTextBox.Text = libraryFolder;
            LibraryFolderTextBox.Select(0, 0);

            workingFolder = Properties.Settings.Default.WorkFolder;
            if (Directory.Exists(workingFolder))
            {
                workingFolder = Properties.Settings.Default.WorkFolder;
            }
            else
            {
                workingFolder = Directory.GetCurrentDirectory();
                Properties.Settings.Default.WorkFolder = workingFolder;
            }
            WorkingFolderTextBox.Text = workingFolder;
            WorkingFolderTextBox.Select(0, 0);

            LibraryFolderTextBox.Select();
        }

        private void ChangeLibraryFolderButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowNewFolderButton = false;

            if (Directory.Exists(libraryFolder))
            {
                folderBrowserDialog1.SelectedPath = libraryFolder;
            }
            else
            {
                folderBrowserDialog1.SelectedPath = Directory.GetCurrentDirectory();
            }

            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                libraryFolder = folderBrowserDialog1.SelectedPath;
                LibraryFolderTextBox.Text = libraryFolder;
                IEnumerable<string> libFiles = Directory.EnumerateFiles(libraryFolder, "*.lbr", SearchOption.TopDirectoryOnly);
                libraryFileList.Clear();
                foreach (string ss in libFiles) libraryFileList.Add(ss);
            }
        }


        private void ChangeWorkingFolderButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowNewFolderButton = false;
            if (Directory.Exists(workingFolder))
            {
                folderBrowserDialog1.SelectedPath = workingFolder;
            }
            else
            {
                folderBrowserDialog1.SelectedPath = Directory.GetCurrentDirectory();
            }

            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                workingFolder = folderBrowserDialog1.SelectedPath;
                Properties.Settings.Default.WorkFolder = workingFolder;
                WorkingFolderTextBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string text = LibraryFolderTextBox.Text;
            if (text != "")
            {
                Properties.Settings.Default.LibFolder = text;
            }

            text = WorkingFolderTextBox.Text;
            if (text != "")
            {
                Properties.Settings.Default.WorkFolder = text;
            }
            DialogResult = DialogResult.OK;
            Hide();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
