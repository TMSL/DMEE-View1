using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DMEEView1
{
    public partial class ColorConfigForm : Form
    {
        private DcColorConfig colorConfigTemp = new DcColorConfig();
        public DcColorConfig colorConfig = new DcColorConfig();

        public ColorConfigForm()
        {
            InitializeComponent();
        }

        public void SetColorConfig(DcColorConfig cfg)
        {
            colorConfig = cfg;
            colorConfigTemp = cfg;
        }

        public class DcColorConfig
        {
            public Color pinsColorTemp = Color.Black;
            public Color textColorTemp = Color.Black;
            public Color wiresColorTemp = Color.Black;
            public Color linesColorTemp = Color.Black;
        }

        private void pinsCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void pinsColorBox_Click(object sender, EventArgs e)
        {
            ChooseColor(ref colorConfigTemp.pinsColorTemp);
            pinsColorBox.Invalidate();
        }

        private void pinsColorBox_Paint(object sender, PaintEventArgs e)
        {
            SetColor(e.Graphics, colorConfigTemp.pinsColorTemp);
        }

        private void textColorBox_Click(object sender, EventArgs e)
        {
            ChooseColor(ref colorConfigTemp.textColorTemp);
            textColorBox.Invalidate();
        }

        private void textColorBox_Paint(object sender, PaintEventArgs e)
        {
            SetColor(e.Graphics, colorConfigTemp.textColorTemp);
        }

        private void linesColorBox_Click(object sender, EventArgs e)
        {
            ChooseColor(ref colorConfigTemp.linesColorTemp);
            linesColorBox.Invalidate();
        }

        private void linesColorBox_Paint(object sender, PaintEventArgs e)
        {
            SetColor(e.Graphics, colorConfigTemp.linesColorTemp);
        }

        private void wiresColorBox_Click(object sender, EventArgs e)
        {
            ChooseColor(ref colorConfigTemp.wiresColorTemp);
            wiresColorBox.Invalidate();
        }

        private void wiresColorBox_Paint(object sender, PaintEventArgs e)
        {
            SetColor(e.Graphics, colorConfigTemp.wiresColorTemp);
        }

        private void SetColor(Graphics gr, Color color)
        {
            gr.FillRectangle(new Pen(color).Brush, 0F, 0F, Width - 2, Height - 2);
        }

        private void ChooseColor(ref Color color)
        {
            var result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                color = colorDialog1.Color;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            colorConfig = colorConfigTemp;
            DialogResult = DialogResult.OK;
            Hide();
        }

        private void ColorConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
