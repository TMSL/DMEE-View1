using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using DcClasses;

namespace DMEEView1
{
    public partial class PrintSetupForm : Form
    {
        private PrintDocument pdoc = new PrintDocument();
        private MainForm parentForm;
        private float blankPageW, blankPageH, blankPageX, blankPageY;
        private float scaleFactor;
        public float ZoomFactor = 1.0F;
        public PageSettings pgs;
        public bool fitToPage = true;
        public Alignment dAlign = Alignment.topLeft;

        public enum Alignment
        { topLeft, topMiddle, topRight, middleLeft, center, middleRight, bottomLeft, bottomMiddle, bottomRight };
                
        public PrintSetupForm()
        {
            InitializeComponent();
            cancelButton.Select();
        }

        public PrintSetupForm(ref PageSettings pgs, MainForm parent) : this()
        {
            parentForm = parent;
            pageSetupDialog.PageSettings = pgs;
            CalcBlankPage(out blankPageW, out blankPageH, out blankPageX, out blankPageY);
            pictureBox1.BackColor = Color.Transparent;
            colorCheckBox.Checked = pgs.Color;

            foreach (string s in PrinterSettings.InstalledPrinters)
            {
                comboBox1.Items.Add(s);
            }
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf(pgs.PrinterSettings.PrinterName);
        }


        private void AlignTLButton_Click(object sender, EventArgs e)
        {
            dAlign = Alignment.topLeft;
            Invalidate();
        }

        private void AlignTMButton_Click(object sender, EventArgs e)
        {
            dAlign = Alignment.topMiddle;
            Invalidate();
        }

        private void AlignTRButton_Click(object sender, EventArgs e)
        {
            dAlign = Alignment.topRight;
            Invalidate();
        }

        private void AlignMLButton_Click(object sender, EventArgs e)
        {
            dAlign = Alignment.middleLeft;
            Invalidate();
        }

        private void AlignCenterButton_Click(object sender, EventArgs e)
        {
            dAlign = Alignment.center;
            Invalidate();
        }

        private void AlignMRButton_Click(object sender, EventArgs e)
        {
            dAlign = Alignment.middleRight;
            Invalidate();
        }

        private void AlignBLButton_Click(object sender, EventArgs e)
        {
            dAlign = Alignment.bottomLeft;
            Invalidate();
        }

        private void buttonBMButton_Click(object sender, EventArgs e)
        {
            dAlign = Alignment.bottomMiddle;
            Invalidate();
        }

        private void AlignBRButton_Click(object sender, EventArgs e)
        {
            dAlign = Alignment.bottomRight;
            Invalidate();
        }

        private void FitToPageCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            fitToPage = FitToPageCheckBox.Checked;
            dAlign = Alignment.center;
            Invalidate();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            pgs.PrinterSettings.PrinterName = (string)comboBox1.SelectedItem;
            Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pgs.PrinterSettings.PrinterName = (string)comboBox1.SelectedItem;
        }

        private void colorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            pgs.Color = colorCheckBox.Checked;
        }

        private void ZoomUpDown_ValueChanged(object sender, EventArgs e)
        {
            fitToPage = false;
            FitToPageCheckBox.Checked = false;
            Invalidate();
        }

        // Take the page size from the page settings and scales it down to create a "blank page" area that fits
        // within pictureBox. The routine also creates a scaleFactor that corresponds to the scale for drawing
        // inside the blank page where scalefactor * 100 = 1 inch. This factor is also the factor for drawing
        // at a zoom factor of 1:1.
        private void CalcBlankPage(out float blankPgW, out float blankPgH, out float blankPgX, out float blankPgY)
        {
            pgs = pageSetupDialog.PageSettings;

            int pBoxHeight = pictureBox1.Height;
            int pBoxWidth = pictureBox1.Width;

            const int pictureBoxMargin = 2;

            // get page dimensions in 1/100 inch
            int paperHeight = pgs.PaperSize.Height;
            int paperWidth = pgs.PaperSize.Width;
            if (pgs.Landscape)
            {
                paperHeight = pgs.PaperSize.Width;
                paperWidth = pgs.PaperSize.Height;
            }

            // scale blank page to fit the Picture box
            scaleFactor = (pBoxWidth - pictureBoxMargin) / (float)(paperWidth);
            float sf2 = (pBoxHeight - pictureBoxMargin) / (float)(paperHeight);
            if (sf2 < scaleFactor) scaleFactor = sf2;

            blankPgW = scaleFactor * paperWidth;
            blankPgH = scaleFactor * paperHeight;

            // center in Picture box
            blankPgX = (pBoxWidth - blankPgW) / 2.0F;
            blankPgY = (pBoxHeight - blankPgH) / 2.0F;
        }

        // Calculate offsets for aligning drawing on page per given alignment choice (e.g. top left, bottom right, etc.)
        // This routine doesn't care about what units are used for the print area and the drawing size as long as the
        // units between the two are the same.
        public void CalcAlignmentOffsets(Alignment dAlign, float printAreaW, float printAreaH,
                                          float dWidth, float dHeight,
                                          out float dOffsetX, out float dOffsetY)
        {
            // Position according to drawing alignment
            dOffsetX = 0;
            dOffsetY = 0;
            switch (dAlign)  // handle horizontal
            {
                case Alignment.topMiddle:
                case Alignment.center:
                case Alignment.bottomMiddle:
                    if (dWidth < printAreaW) dOffsetX = (printAreaW - dWidth) / 2.0F;
                    if (dWidth > printAreaW) dOffsetX = -(dWidth - printAreaW) / 2.0F;
                    break;
                case Alignment.topRight:
                case Alignment.middleRight:
                case Alignment.bottomRight:
                    if (dWidth < printAreaW) dOffsetX = (printAreaW - dWidth);
                    if (dWidth > printAreaW) dOffsetX = -(dWidth - printAreaW);
                    break;
                default:
                    break;
            }

            switch (dAlign)  // handle vertical
            {
                case Alignment.middleLeft:
                case Alignment.center:
                case Alignment.middleRight:
                    if (dHeight < printAreaH) dOffsetY = (printAreaH - dHeight) / 2.0F;
                    if (dHeight > printAreaH) dOffsetY = -(dHeight - printAreaH) / 2.0F;
                    break;
                case Alignment.bottomLeft:
                case Alignment.bottomMiddle:
                case Alignment.bottomRight:
                    if (dHeight < printAreaH) dOffsetY = (printAreaH - dHeight);
                    if (dHeight > printAreaH) dOffsetY = -(dHeight - printAreaH);
                    break;
                default:
                    break;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics gr = e.Graphics;
            gr.FillRectangle(new Pen(Color.White).Brush, blankPageX - 1, blankPageY - 1, blankPageW, blankPageH);
            // check that topModule exists and is loaded
            DcModule topModule = parentForm.topModuleCommand;
            DcBounds dBounds = parentForm.topModuleCommand.bounds;

            // first give a little margin for page margins
            float LMargin = pgs.Margins.Left * scaleFactor;
            float RMargin = pgs.Margins.Right * scaleFactor;
            float TMargin = pgs.Margins.Top * scaleFactor;
            float BMargin = pgs.Margins.Bottom * scaleFactor;

            // printArea is the area between the current margins on the blank page
            float printAreaX = blankPageX-1 + LMargin;
            float printAreaY = blankPageY-1 + TMargin;
            float printAreaW = blankPageW - (RMargin + LMargin);
            float printAreaH = blankPageH - (TMargin + BMargin);

            Pen pen = new Pen(Color.LightGray);
            float[] dashValues = { 3, 5, 3, 5};
            pen.DashPattern = dashValues;

            gr.DrawRectangle(pen, printAreaX, printAreaY, printAreaW, printAreaH);
            if (parentForm.drawingLoaded)
            {
                ZoomFactor = (float)ZoomUpDown.Value / 100F;
                float scaleDrawing = scaleFactor * ZoomFactor;  // represent 1:1 scale when drawing onto blankPage
                float drawingHeight = topModule.bounds.YMax - topModule.bounds.YMin;
                float drawingWidth = topModule.bounds.XMax - topModule.bounds.XMin;

                if (fitToPage)
                {
                    // determine scale factor to fit the drawing within the page margins
                    scaleDrawing = printAreaH / drawingHeight;
                    float scaleW = printAreaW / drawingWidth;
                    if (scaleW < scaleDrawing) scaleDrawing = scaleW;
                }

                float dWidth = drawingWidth * scaleDrawing;
                float dHeight = drawingHeight * scaleDrawing;

                CalcAlignmentOffsets(dAlign, printAreaW, printAreaH, dWidth, dHeight, out float dOffsetX, out float dOffsetY);

                // set clip rectangle
                gr.SetClip(new RectangleF(printAreaX, printAreaY, printAreaW, printAreaH));

                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                pen.Color = Color.LightGreen;
                gr.DrawRectangle(pen, printAreaX + dOffsetX, printAreaY + dOffsetY, dWidth, dHeight);

                gr.TranslateTransform(printAreaX + dOffsetX, printAreaY + dOffsetY);

                gr.ScaleTransform(scaleDrawing, scaleDrawing);
                gr.TranslateTransform(-dBounds.XMin, dBounds.YMax);
                // FLIP Y COORDINATES
                gr.ScaleTransform(1, -1);

                parentForm.PaintDcModule(e.Graphics, topModule);
            }
        }

        private void PageSetupButton_Click(object sender, EventArgs e)
        {
            pageSetupDialog.PageSettings = pgs;
            var result = pageSetupDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                CalcBlankPage(out blankPageW, out blankPageH, out blankPageX, out blankPageY);
            }
            Invalidate();
        }
    }
}
