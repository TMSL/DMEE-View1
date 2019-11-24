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
        private float previewAreaW, previewAreaH, offsetW, offsetH;
        private float scaleFactor;
        private float ZoomFactor = 1.0F;
        private PageSettings pgs;
        private bool fitToPage = true;

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
            CalcBlankPage(out previewAreaW, out previewAreaH, out offsetW, out offsetH);
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

        private void FitToPageCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            fitToPage = FitToPageCheckBox.Checked;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void customNumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
        }

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

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics gr = e.Graphics;
            gr.FillRectangle(new Pen(Color.White).Brush, offsetW - 1, offsetH - 1, previewAreaW, previewAreaH);
            // check that topModule exists and is loaded
            DcModule topModule = parentForm.topModuleCommand;
            DcBounds dBounds = parentForm.topModuleCommand.bounds;

            // first give a little margin for page margins
            float LMargin = pgs.Margins.Left * scaleFactor;
            float RMargin = pgs.Margins.Right * scaleFactor;
            float TMargin = pgs.Margins.Top * scaleFactor;
            float BMargin = pgs.Margins.Bottom * scaleFactor;
            float frameX = offsetW-1 + LMargin;
            float frameY = offsetH-1 + TMargin;
            float frameW = previewAreaW - (RMargin + LMargin);
            float frameH = previewAreaH - (TMargin + BMargin);

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
                switch (dAlign)  // handle horizontal
                {
                    case DrawingAlignment.topMiddle:
                    case DrawingAlignment.center:
                    case DrawingAlignment.bottomMiddle:
                        if (dWidth < frameW) dOffsetX = (frameW - dWidth)/ 2.0F;
                        break;
                    case DrawingAlignment.topRight:
                    case DrawingAlignment.middleRight:
                    case DrawingAlignment.bottomRight:
                        if (dWidth < frameW) dOffsetX = (frameW - dWidth);
                        break;
                    default:
                        break;
                }

                switch (dAlign)  // handle vertical
                {
                    case DrawingAlignment.middleLeft:
                    case DrawingAlignment.center:
                    case DrawingAlignment.middleRight:
                        if (dHeight < frameH) dOffsetY = (frameH - dHeight) / 2.0F;
                        break;
                    case DrawingAlignment.bottomLeft:
                    case DrawingAlignment.bottomMiddle:
                    case DrawingAlignment.bottomRight:
                        if (dHeight < frameH) dOffsetY = (frameH - dHeight);
                        break;
                    default:
                        break;
                }

                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                pen.Color = Color.LightGreen;
                gr.DrawRectangle(pen, frameX+dOffsetX, frameY+dOffsetY, dWidth, dHeight);

                // set clip rectangle
                gr.SetClip(new RectangleF(frameX + dOffsetX, frameY + dOffsetY, dWidth, dHeight));

                gr.TranslateTransform(frameX + dOffsetX, frameY + dOffsetY);

                gr.ScaleTransform(scaleFactor * ZoomFactor, scaleFactor * ZoomFactor);
                gr.TranslateTransform(-dBounds.XMin, dBounds.YMax);
                // FLIP Y COORDINATES
                gr.ScaleTransform(1, -1);

                parentForm.PaintDcModule(e.Graphics, topModule);
            }
        }

        private void PageSetupButton_Click(object sender, EventArgs e)
        {
            pageSetupDialog.PageSettings = pdoc.DefaultPageSettings;
            var result = pageSetupDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                CalcBlankPage(out previewAreaW, out previewAreaH, out offsetW, out offsetH);
            }
            Invalidate();
        }
    }
}
