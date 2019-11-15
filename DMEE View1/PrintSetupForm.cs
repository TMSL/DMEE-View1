using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace DMEEView1
{
    public partial class PrintSetupForm : Form
    {
        private PrintDocument pdoc = new PrintDocument();
        private MainForm parentForm;

        public PrintSetupForm()
        {
            InitializeComponent();
            cancelButton.Select();
        }
        public PrintSetupForm(PrintDocument pd, MainForm parent) : this()
        {
            parentForm = parent;
            pdoc = pd;
        }

        private void PrintSetupForm_Shown(object sender, EventArgs e)
        {
            colorCheckBox.Checked = pdoc.DefaultPageSettings.Color;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //parentForm.PaintDcModule(e.Graphics,...)
        }
    }
}
