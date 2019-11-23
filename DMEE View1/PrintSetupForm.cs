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
using DcClasses;

namespace DMEEView1
{
    public partial class PrintSetupForm : Form
    {
        private PrintDocument pdoc = new PrintDocument();
        private MainForm parentForm;
        private float scaledW, scaledH, offsetW, offsetH;
        private float scaleFactor;
        private PageSettings pgs;

        private enum DrawingAlignment
        { topLeft, topMiddle, topRight, middleLeft, center, middleRight, bottomLeft, bottomMiddle, bottomRight };

        DrawingAlignment dAlign = DrawingAlignment.topLeft;
        
        public PrintSetupForm()
        {
            InitializeComponent();
            cancelButton.Select();
        }

        public PrintSetupForm(PrintDocument pd, MainForm parent) : this()
        {
            parentForm = parent;
            pdoc = pd;
            pageSetupDialog.PageSettings = pdoc.DefaultPageSettings;
            CalcBlankPage(out scaledW, out scaledH, out offsetW, out offsetH);
            pictureBox1.BackColor = Color.Transparent;
            colorCheckBox.Checked = pdoc.DefaultPageSettings.Color;
        }


        private void AlignTLButton_Click(object sender, EventArgs e)
        {
            dAlign = DrawingAlignment.topLeft;
            Invalidate();
        }

        private void AlignTMButton_Click(object sender, EventArgs e)
        {
            dAlign = DrawingAlignment.topMiddle;
            Invalidate();
        }

        private void AlignTRButton_Click(object sender, EventArgs e)
        {
            dAlign = DrawingAlignment.topRight;
            Invalidate();
        }

        private void AlignMLButton_Click(object sender, EventArgs e)
        {
            dAlign = DrawingAlignment.middleLeft;
            Invalidate();
        }

        private void AlignCenterButton_Click(object sender, EventArgs e)
        {
            dAlign = DrawingAlignment.center;
            Invalidate();
        }

        private void AlignMRButton_Click(object sender, EventArgs e)
        {
            dAlign = DrawingAlignment.middleRight;
            Invalidate();
        }

        private void AlignBLButton_Click(object sender, EventArgs e)
        {
            dAlign = DrawingAlignment.bottomLeft;
            Invalidate();
        }

        private void buttonBMButton_Click(object sender, EventArgs e)
        {
            dAlign = DrawingAlignment.bottomMiddle;
            Invalidate();
        }

        private void AlignBRButton_Click(object sender, EventArgs e)
        {
            dAlign = DrawingAlignment.bottomRight;
            Invalidate();
        }

        private void CalcBlankPage(out float scaledW, out float scaledH, out float offsetW, out float offsetH)
        {
            pgs = pageSetupDialog.PageSettings;

            int height = pictureBox1.Height;
            int width = pictureBox1.Width;

            const int pictureBoxMargin = 2;

            // get page dimensions in 1/100 inch
            int pgHeight = pgs.PaperSize.Height;
            int pgWidth = pgs.PaperSize.Width;
            if (pgs.Landscape)
            {
                pgHeight = pgs.PaperSize.Width;
                pgWidth = pgs.PaperSize.Height;
            }

            // scale blank page to fit the Picture box
            scaleFactor = (width - pictureBoxMargin) / (float)(pgWidth);
            float sf2 = (height - pictureBoxMargin) / (float)(pgHeight);
            if (sf2 < scaleFactor) scaleFactor = sf2;

            scaledW = scaleFactor * pgWidth;
            scaledH = scaleFactor * pgHeight;

            // center in Picture box
            offsetW = (width - scaledW) / 2.0F;
            offsetH = (height - scaledH) / 2.0F;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics gr = e.Graphics;
            gr.FillRectangle(new Pen(Color.White).Brush, offsetW - 1, offsetH - 1, scaledW, scaledH);
            // check that topModule exists and is loaded
            DcModule topModule = parentForm.topModuleCommand;

            // first give a little margin for page margins
            float LMargin = pgs.Margins.Left * scaleFactor;
            float RMargin = pgs.Margins.Right * scaleFactor;
            float TMargin = pgs.Margins.Top * scaleFactor;
            float BMargin = pgs.Margins.Bottom * scaleFactor;
            float frameX = offsetW-1 + LMargin;
            float frameY = offsetH-1 + TMargin;
            float frameW = scaledW - (RMargin + LMargin);
            float frameH = scaledH - (TMargin + BMargin);

            Pen pen = new Pen(Color.LightGray);
            float[] dashValues = { 3, 5, 3, 5};
            pen.DashPattern = dashValues;

            gr.DrawRectangle(pen, frameX, frameY, frameW, frameH);
            if (parentForm.drawingLoaded)
            {
                // determine scale factor for drawing based on bounds
                float drawingHeight = topModule.bounds.YMax - topModule.bounds.YMin;
                float drawingWidth = topModule.bounds.XMax - topModule.bounds.XMin;

                float scaleDrawing = frameH / drawingHeight;
                float scaleW = frameW / drawingWidth;

                if (scaleW < scaleDrawing) scaleDrawing = scaleW;

                float dOffsetX = 0;
                float dOffsetY = 0;
                float dWidth = drawingWidth * scaleDrawing;
                float dHeight = drawingHeight * scaleDrawing;

                // Position according to drawing alignment
                switch (dAlign)
                {
                    case DrawingAlignment.center:
                        if (dWidth < frameW) dOffsetX = (frameW - dWidth)/ 2.0F;
                        if (dHeight < frameH) dOffsetY = (frameH - dHeight) / 2.0F;
                        break;
                    default:
                        break;
                }

                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                pen.Color = Color.LightGreen;
                gr.DrawRectangle(pen, frameX+dOffsetX, frameY+dOffsetY, dWidth, dHeight);

                //parentForm.PaintDcModule(e.Graphics, )
            }
        }

        private void PageSetupButton_Click(object sender, EventArgs e)
        {
            pageSetupDialog.PageSettings = pdoc.DefaultPageSettings;
            var result = pageSetupDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                CalcBlankPage(out scaledW, out scaledH, out offsetW, out offsetH);
            }
            Invalidate();
        }
    }
}
