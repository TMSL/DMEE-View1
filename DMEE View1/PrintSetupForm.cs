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
        private float previewAreaW, previewAreaH, offsetW, offsetH;
        private float scaleFactor;
        public float ZoomFactor = 1.0F;
        public PageSettings pgs;
        public bool fitToPage = true;
        private int fOffsetX, fOffsetY;
        private Point initialMouseLoc;
        private bool mouseIsDown = false;

        private enum DrawingAlignment
        { topLeft, topMiddle, topRight, middleLeft, center, middleRight, bottomLeft, bottomMiddle, bottomRight };

        DrawingAlignment dAlign = DrawingAlignment.topLeft;
        
        public PrintSetupForm()
        {
            InitializeComponent();
            cancelButton.Select();
        }

        public PrintSetupForm(ref PageSettings pgs, MainForm parent) : this()
        {
            parentForm = parent;
            pageSetupDialog.PageSettings = pgs;
            CalcBlankPage(out previewAreaW, out previewAreaH, out offsetW, out offsetH);
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

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.SizeAll;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            initialMouseLoc = PointToClient(MousePosition);
            mouseIsDown = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseIsDown)
            {
                Point MousePos = PointToClient(MousePosition);
                MousePos.X -= pictureBox1.Location.X;
                MousePos.Y -= pictureBox1.Location.Y;

                if (MousePos.X > initialMouseLoc.X + 10)
                {
                    initialMouseLoc.X += 10;
                    if (initialMouseLoc.X > pictureBox1.Width - 1)
                    {
                        initialMouseLoc.X = pictureBox1.Width - 1;
                    }
                }

                if (MousePos.X < initialMouseLoc.X - 10)
                {
                    initialMouseLoc.X -= 10;
                    if (initialMouseLoc.X < 0)
                    {
                        initialMouseLoc.X = 0;
                    }
                }

                if (MousePos.Y > initialMouseLoc.Y + 10)
                {
                    initialMouseLoc.Y += 10;
                    if (initialMouseLoc.Y > pictureBox1.Location.Y + pictureBox1.Height - 1)
                    {
                        initialMouseLoc.Y = pictureBox1.Location.Y + pictureBox1.Height - 1;
                    }
                }

                if (MousePos.Y < initialMouseLoc.Y - 10)
                {
                    initialMouseLoc.Y -= 10;
                    if (initialMouseLoc.Y < 0)
                    {
                        initialMouseLoc.Y = 0;
                    }
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseIsDown = false;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            mouseIsDown = false;
            fOffsetX = 0;
            fOffsetY = 0;
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

            // calculate 1:1 scale factor (based on page width from print settings)
            float Zoom100Factor = frameW / pgs.Bounds.Width;

            Pen pen = new Pen(Color.LightGray);
            float[] dashValues = { 3, 5, 3, 5};
            pen.DashPattern = dashValues;

            gr.DrawRectangle(pen, frameX, frameY, frameW, frameH);
            if (parentForm.drawingLoaded)
            {
                float scaleDrawing = Zoom100Factor * (float)ZoomUpDown.Value / 100F;
                float drawingHeight = topModule.bounds.YMax - topModule.bounds.YMin;
                float drawingWidth = topModule.bounds.XMax - topModule.bounds.XMin;

                if (fitToPage)
                {
                    // determine scale factor for drawing based on bounds
                    scaleDrawing = frameH / drawingHeight;
                    float scaleW = frameW / drawingWidth;

                    if (scaleW < scaleDrawing) scaleDrawing = scaleW;
                }
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

                // set clip rectangle
                gr.SetClip(new RectangleF(frameX + dOffsetX, frameY + dOffsetY, frameW, frameH));

                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                pen.Color = Color.LightGreen;
                gr.DrawRectangle(pen, frameX + dOffsetX, frameY + dOffsetY, dWidth, dHeight);

                gr.TranslateTransform(frameX + dOffsetX, frameY + dOffsetY);

                gr.ScaleTransform(scaleDrawing * ZoomFactor, scaleDrawing * ZoomFactor);
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
                CalcBlankPage(out previewAreaW, out previewAreaH, out offsetW, out offsetH);
            }
            Invalidate();
        }
    }
}
