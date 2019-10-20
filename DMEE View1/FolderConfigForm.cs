using System;
using System.IO;
using System.Windows.Forms;

namespace DMEEView1
{
    public partial class FolderConfigForm : Form
    {
        public String libraryFolder = "";
        public String workingFolder = "";

        public FolderConfigForm()
        {
            InitializeComponent();
            this.ControlBox = false;
            string folder = Properties.Settings.Default.LibFolder;
            if (Directory.Exists(folder))
            {
                libraryFolder = Properties.Settings.Default.LibFolder;
                LibraryFolderTextBox.Text = libraryFolder;
                LibraryFolderTextBox.Select(0, 0);
            }
            else
            {
                Properties.Settings.Default.LibFolder = "";
                libraryFolder = "";
            }

            folder = Properties.Settings.Default.WorkFolder;
            if (Directory.Exists(folder))
            {
                workingFolder = Properties.Settings.Default.WorkFolder;
                WorkingFolderTextBox.Text = workingFolder;
                WorkingFolderTextBox.Select(0, 0);
            }
            else
            {
                Properties.Settings.Default.WorkFolder = "";
                workingFolder = "";
            }
            LibraryFolderTextBox.Select();
        }
               
        private void ChangeLibraryFolderButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowNewFolderButton = false;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (Directory.Exists(libraryFolder))
            {
                folderBrowserDialog1.SelectedPath = libraryFolder;
            }

            if (result == DialogResult.OK)
            {
                libraryFolder = folderBrowserDialog1.SelectedPath;
                LibraryFolderTextBox.Text = folderBrowserDialog1.SelectedPath;
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
