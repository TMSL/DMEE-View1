using System;
using System.Drawing;
using System.Windows.Forms;

namespace DMEEView1
{
    public partial class ColorConfigForm : Form
    {
        private DcColorConfig colorConfigTemp = new DcColorConfig();
        public DcColorConfig settings = new DcColorConfig();

        public ColorConfigForm()
        {
            InitializeComponent();
        }

        public void SetColorConfig(DcColorConfig cfg)
        {
            settings = cfg;
            colorConfigTemp = cfg;
        }

        public class DcColorConfig
        {
            public Color pinsColor = Color.Black;
            public Color textColor = Color.Black;
            public Color wiresColor = Color.Black;
            public Color linesColor = Color.Black;
            public bool showPins = true;
            public bool blackAndWhite = false;
        }

        private void pinsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            colorConfigTemp.showPins = pinsCheckBox.Checked;
            Invalidate();
        }

        private void pinsColorBox_Click(object sender, EventArgs e)
        {
            ChooseColor(ref colorConfigTemp.pinsColor);
            pinsColorBox.Invalidate();
        }

        private void pinsColorBox_Paint(object sender, PaintEventArgs e)
        {
            SetColor(e.Graphics, colorConfigTemp.pinsColor);
        }

        private void textColorBox_Click(object sender, EventArgs e)
        {
            ChooseColor(ref colorConfigTemp.textColor);
            textColorBox.Invalidate();
        }

        private void textColorBox_Paint(object sender, PaintEventArgs e)
        {
            SetColor(e.Graphics, colorConfigTemp.textColor);
        }

        private void linesColorBox_Click(object sender, EventArgs e)
        {
            ChooseColor(ref colorConfigTemp.linesColor);
            linesColorBox.Invalidate();
        }

        private void linesColorBox_Paint(object sender, PaintEventArgs e)
        {
            SetColor(e.Graphics, colorConfigTemp.linesColor);
        }

        private void wiresColorBox_Click(object sender, EventArgs e)
        {
            ChooseColor(ref colorConfigTemp.wiresColor);
            wiresColorBox.Invalidate();
        }

        private void wiresColorBox_Paint(object sender, PaintEventArgs e)
        {
            SetColor(e.Graphics, colorConfigTemp.wiresColor);
        }

        private void busColorBox_Click(object sender, EventArgs e)
        {

        }

        private void busColorBox_Paint(object sender, PaintEventArgs e)
        {

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
            settings = colorConfigTemp;
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

        private void ColorConfigForm_Shown(object sender, EventArgs e)
        {
            pinsCheckBox.Checked = colorConfigTemp.showPins;
            blackAndWhiteRadioButton.Checked = colorConfigTemp.blackAndWhite;
            colorRadioButton.Checked = !colorConfigTemp.blackAndWhite;
            Console.WriteLine("shown");
        }

        private void pinsCheckBox_Paint(object sender, PaintEventArgs e)
        {
            pinsCheckBox.Checked = colorConfigTemp.showPins;
        }

        private void blackAndWhiteRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            colorConfigTemp.blackAndWhite = blackAndWhiteRadioButton.Checked;
        }
    }
}
