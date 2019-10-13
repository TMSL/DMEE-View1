using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Text;
using System.Windows.Forms;

// TMS - started September 17, 2019

namespace DMEEView1
{
    public partial class Form1 : Form
    {
        Single biggestX = 0, biggestY = 0;
        Single smallestX = 10000, smallestY = 10000;

        public Form1()
        {
            InitializeComponent();
            menuStrip1.Select();
            textBox1.Text = Properties.Settings.Default.FNAME;
            this.Width = Convert.ToInt32(2250 * 0.55);
            this.Height = Convert.ToInt32(1375 * 0.55);
            if (Properties.Settings.Default.ShowInfo == true)
            {
                button2.Text = "hide info";
                textBox3.Show();
            }
            else
            {
                button2.Text = "show info";
                textBox3.Hide();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //Create graphic object for the current form
            //Graphics gs = this.CreateGraphics();
            Graphics gs = e.Graphics;
           
            DcRenderer dcr = new DcRenderer(ref gs);
            
            //Create brush object
            Brush brush1 = new SolidBrush(Color.Black);

            //Create pen objects
            Pen pen1 = new Pen(Color.Black);

            Single dpiX = gs.DpiX;
            Single dpiY = gs.DpiY;
            int windowWidth = this.Width;
            int windowHeight = this.Height;
            
            gs.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            
            if (drawListLoaded)
            {
                Point pt1 = new Point();
                Point pt2 = new Point();
                DcDrawItem dcDrawItem = new DcDrawItem();
                pen1.Width = 1;
                pen1.Color = Color.Black;

                gs.TranslateTransform(25F+smallestX, biggestY + 125F); // Move the origin "down".
                gs.ScaleTransform(0.5F, 0.5F, MatrixOrder.Append);

                gs.DrawLine(pen1, biggestX - 5, -biggestY, biggestX + 5, -biggestY);
                gs.DrawLine(pen1, biggestX, -biggestY - 5, biggestX, -biggestY + 5);

                // draw crossed lines at origin
                gs.DrawLine(pen1, 0 - 5, 0, 0 + 5, 0);
                gs.DrawLine(pen1, 0, 0 - 5, 0, 0 + 5);

                // My B-SIZE DRAWING IS 15-7/8" x 10-15/16", Inner border is ~9-1/2" x ~15-1/2"
                // At present drawing scale 1937 -> 15” = .00774" per unit
                // units = inches / .00774
                // draw a 15.5" line from 3/16", 3/16"
                Single factor = .00785F;
                Single x1, y1, x2, y2;

                // draw a line from origin to 16"
                gs.DrawLine(pen1, 0, 0, 16F / factor, 0);

                x1 = 5F / 16F / factor;
                y1 = 5F / 16F / factor;
                x2 = x1 + (15.5F / factor);
                y2 = y1;

                gs.DrawLine(pen1, x1, -y1, x2, -y2);
                if (!debugWritten) Console.WriteLine("l {0} {1} {2} {3}", x1, y1, x2, y2);

                gs.DrawLine(pen1, x1, -(y1 + (9.5F / factor)), x1, -y1);
                if (!debugWritten) Console.WriteLine("l {0} {1} {2} {3}", x1, (y1 + (9.5F / factor)), x1, y1);

                gs.DrawLine(pen1, x1, -(y1 + (9.5F / factor)), x2, -(y1 + (9.5F / factor)));
                if (!debugWritten) Console.WriteLine("l {0} {1} {2} {3} dx={4}", x1, y1 + (9.5F / factor), x2, y1 + (9.5F / factor), x2-x1);

                gs.DrawLine(pen1, x2, -y1, x2, -(y1 + (9.5F / factor)));
                if (!debugWritten) Console.WriteLine("l {0} {1} {2} {3}", x2, y1, x2, y1 + (9.5F / factor));
                
                x1 = x2 - (3F + 3F / 8F) / factor;
                gs.DrawLine(pen1, x1, -y1, x2, -y1);
                if (!debugWritten) Console.WriteLine("l {0} {1} {2} {3} dx={4}", x1, y1, x2, y1, x2-x1);

                y1 += (3F / 16F) / factor;
                gs.DrawLine(pen1, x1, -y1, x2, -y1);
                if (!debugWritten) Console.WriteLine("l {0} {1} {2} {3}", x1, y1, x2, y1);

                y1 += (0.5F) / factor;
                gs.DrawLine(pen1, x1, -y1, x2, -y1);
                if (!debugWritten) Console.WriteLine("l {0} {1} {2} {3}", x1, y1, x2, y1);

                y1 += (0.5F) / factor;
                gs.DrawLine(pen1, x1, -y1, x2, -y1);
                if (!debugWritten) Console.WriteLine("l {0} {1} {2} {3}", x1, y1, x2, y1);

                y1 += (0.5F) / factor;
                gs.DrawLine(pen1, x1, -y1, x2, -y1);
                if (!debugWritten) Console.WriteLine("l {0} {1} {2} {3}", x1, y1, x2, y1);

                y2 = y1;
                y1 = 5F / 16F / factor;
                gs.DrawLine(pen1, x1, -y1, x1, -y2);
                if (!debugWritten) Console.WriteLine("l {0} {1} {2} {3}", x1, y1, x2, y2);

                x1 = x2 - (1.4F / factor);
                y1 = 5F / 16F / factor;
                y2 = y1 + (3F / 16F / factor);
                gs.DrawLine(pen1, x1, -y1, x1, -y2);
                if (!debugWritten) Console.WriteLine("l {0} {1} {2} {3}", x1, y1, x2, y2);

                x1 = x2 - (0.5F / factor);
                y1 += 3F / 16F / factor;
                y2 += 0.5F / factor;
                gs.DrawLine(pen1, x1, -y1, x1, -y2);
                if (!debugWritten) Console.WriteLine("l {0} {1} {2} {3}", x1, y1, x2, y2);

                y1 = 5F / 16F / factor;
                y2 = y1 + (3F / 16F / factor);
                x1 = x2 - ((2F+3F/8F) / factor);
                y1 += 3F / 16F / factor;
                y2 += 0.5F / factor;
                gs.DrawLine(pen1, x1, -y1, x1, -y2);
                if (!debugWritten) Console.WriteLine("l {0} {1} {2} {3}", x1, y1, x2, y2);

                y1 = 5F / 16F / factor;
                y2 = y1 + (3F / 16F / factor);
                x1 = x2 - ((2F + 7F / 8F) / factor);
                y1 += 3F / 16F / factor;
                y2 += 0.5F / factor;
                gs.DrawLine(pen1, x1, -y1, x1, -y2);
                if (!debugWritten) Console.WriteLine("l {0} {1} {2} {3}", x1, y1, x2, y2);

                debugWritten = true;

                for (int i=0; i<drawList.Count; i++)
                {
                    if (drawList[i].GetType() == typeof(DcDrawItem))
                    {
                        dcDrawItem = (DcDrawItem)drawList[i];

                        if (dcDrawItem.itemType == DcItemType.line)
                        {
                            pt1.X = Convert.ToInt32(dcDrawItem.X1);
                            pt1.Y = Convert.ToInt32( - dcDrawItem.Y1);
                            pt2.X = Convert.ToInt32(dcDrawItem.X2);
                            pt2.Y = Convert.ToInt32( - dcDrawItem.Y2);
                            gs.DrawLine(pen1, pt1, pt2);
                        }

                        if (dcDrawItem.itemType == DcItemType.circle)
                        {
                            pt1.X = Convert.ToInt32(dcDrawItem.X1);
                            pt1.Y = Convert.ToInt32( - dcDrawItem.Y1);
                            pt2.X = Convert.ToInt32(dcDrawItem.X2);

                            Single radius = Math.Abs(pt1.X - pt2.X);
                            Single diameter = 2F * radius;
                            Single center = pt1.X;

                            gs.DrawEllipse(pen1, center - radius, pt1.Y - radius, diameter, diameter);
                        }
                    }
                    else
                    {
                        if (drawList[i].GetType() == typeof(DcText))
                        {
                            DcText dct = (DcText)drawList[i];
                            PointF pt = new PointF
                            {
                                X = Convert.ToInt32(dct.X1),
                                Y = Convert.ToInt32(- dct.Y1)
                            };
                            String text = dct.dcStr.strText;
                            text = text.TrimStart('#');

                            Color color = pen1.Color;

                            FontFamily fontFamily = new FontFamily("MS GOTHIC");

                            // Calculate font scale factor
                            Single fontSize = 10.5F * dct.scale / 0.039063F;

                            Font the_font = new Font(fontFamily, fontSize) ;
                            SizeF textSize = gs.MeasureString(text, the_font);

                            Single ascent = the_font.FontFamily.GetCellAscent(FontStyle.Regular);
                            Single descent = the_font.FontFamily.GetCellDescent(FontStyle.Regular);
                            Single emHeight = the_font.FontFamily.GetEmHeight(FontStyle.Regular);
                            Single cellHeight = ascent + descent;
                            Single cellHeightPixel = the_font.Size * cellHeight / emHeight;
                            Single sizeInPoints = the_font.SizeInPoints;  // = emHeight in points

                            if (dct.rotation != 0)
                            {
                                DrawRotatedTextAt(e.Graphics, -dct.rotation, text, (int)(pt.X), (int)(pt.Y), the_font, new SolidBrush (color));
                                //gs.DrawLine(pen1, pt.X - 5, pt.Y, pt.X + 5, pt.Y);
                                //gs.DrawLine(pen1, pt.X, pt.Y - 5, pt.X, pt.Y + 5);
                            }
                            else
                            {
                                pt.Y -= (cellHeightPixel);
                                gs.DrawString(text, the_font, new SolidBrush(color), pt);
                            }
                        }
                    }
                }
            }
            pen1.Dispose();
            brush1.Dispose();
            //gs.Dispose();
        }

        private Single DegreesToRadians(Single degrees)
        {
            return (degrees * (Single)Math.PI / 180F);
        }

        private Single RadiansToDegrees(Single radians)
        {
            return (180F * radians / (Single)Math.PI);
        }

        // Draw a rotated string at specified location for the text origin.
        // This routine assume the text's origin is specified at the lower lefthand corner of the text's box
        // instead of the Microsoft way of having the origin in the upper left corner.
        private void DrawRotatedTextAt(Graphics gr, float angle,
            string txt, Single x, Single y, Font the_font, Brush the_brush)
        {
            Pen pen = new Pen(the_brush);

            Single ascent = the_font.FontFamily.GetCellAscent(FontStyle.Regular);
            Single descent = the_font.FontFamily.GetCellDescent(FontStyle.Regular);
            Single emHeight = the_font.FontFamily.GetEmHeight(FontStyle.Regular);
            Single cellHeight = ascent + descent;
            Single cellHeightPixel = the_font.Size * cellHeight / emHeight;

            // Save the graphics state.
            GraphicsState state = gr.Save();

            Matrix transMatrix = gr.Transform;  // get the scaling factors from the transform matrix
            Single scaleX = transMatrix.Elements[0];
            Single scaleY = transMatrix.Elements[3];
            transMatrix.Dispose();

            y -= cellHeightPixel;   // adjust the y position to put the text origin at the lower lefthand corner

            // Translate the origin to be at the target x, y position.
            // Append the translations so they occur after the rotation.
            // The translation needs to have the input parameters 'manually' scaled since the scaling
            // factors in the translation matrix are not applied to the TranslateTransform
            // input parameters themselves.
            gr.TranslateTransform(scaleX * x, scaleY * y, MatrixOrder.Append);

            if (angle != 0)
            {
                Single CosAngle = (Single)Math.Cos(DegreesToRadians(angle));
                Single SinAngle = (Single)Math.Sin(DegreesToRadians(angle));
                gr.TranslateTransform(SinAngle * cellHeightPixel, (1 - CosAngle) * cellHeightPixel);
            }

            // Rotate.
            gr.RotateTransform(angle);

            gr.DrawString(txt, the_font, the_brush, 0, 0);

            // Restore the graphics state.
            gr.Restore(state);
        }

        private enum DcItemType { line, wire, circle, arc, pin, str, text, undefined };

        DcItemType getDcType(object obj)
        {
            if (obj.GetType() == typeof(DcLine)) return DcItemType.line;
            if (obj.GetType() == typeof(DcWire)) return DcItemType.wire;
            if (obj.GetType() == typeof(DcText)) return DcItemType.text;
            if (obj.GetType() == typeof(DcCircle)) return DcItemType.circle;
            if (obj.GetType() == typeof(DcString)) return DcItemType.str;
            return DcItemType.undefined;
        }

        private class DcRenderer
        {
            Graphics gs;
            Brush brush;
            Pen pen;
            Single YOffset = 1200F;

            public DcRenderer(ref Graphics g)
            {
                gs = g;
                brush = new SolidBrush(Color.CadetBlue);
                pen = new Pen(brush) { Width = 2F };
            }

            public void Draw (DcLine dcLine)
            {
                Point pt1 = new Point();
                Point pt2 = new Point();

                pt1.X = Convert.ToInt32(dcLine.X1);
                pt1.Y = Convert.ToInt32(YOffset - dcLine.Y1);
                pt2.X = Convert.ToInt32(dcLine.X2);
                pt2.Y = Convert.ToInt32(YOffset - dcLine.Y2);
                gs.DrawLine(pen, pt1, pt2);
            }

            public void Draw(DcWire dcWire)
            {
                Point pt1 = new Point();
                Point pt2 = new Point();

                pt1.X = Convert.ToInt32(dcWire.X1);
                pt1.Y = Convert.ToInt32(YOffset - dcWire.Y1);
                pt2.X = Convert.ToInt32(dcWire.X2);
                pt2.Y = Convert.ToInt32(YOffset - dcWire.Y2);
                gs.DrawLine(pen, pt1, pt2);
            }

            public void Draw (DcCircle dcCircle)
            {
                Point pt1 = new Point();
                Point pt2 = new Point();

                pt1.X = Convert.ToInt32(dcCircle.X1);
                pt1.Y = Convert.ToInt32(YOffset - dcCircle.Y1);
                pt2.X = Convert.ToInt32(dcCircle.X2);

                Single radius = Math.Abs(pt1.X - pt2.X);
                Single diameter = 2F * radius;
                Single center = pt1.X;

                gs.DrawEllipse(pen, center - radius, pt1.Y - radius, diameter, diameter);
            }
            
            public void Draw(DcText dcText)
            {

            }
        }

        private class DcDrawItem
        {
            public int color = 0;
            public DcItemType itemType = DcItemType.line;
            public Single X1 = 0;
            public Single Y1 = 0;
            public Single X2 = 0;
            public Single Y2 = 0;
            public Single textSize = 0;  // text size in points
            public String text = "";
        }

        private List<Object> drawList = new List<Object>();
        private List<Single> textScalingList = new List<Single>();         
        private List<Object> drawList2 = new List<object>();

        private bool drawListLoaded = false;
        private bool debugWritten = false;

        private class DcLine
        {
            public DcItemType recordType = DcItemType.line;    //Line (l)
            public int color=0;
            public Single X1=0;
            public Single Y1=0;
            public Single X2=0;
            public Single Y2=0;
            public int unk1=0;
            public int unk2=0;
        }

        private class DcWire
        {
            public DcItemType recordType = DcItemType.wire;    //Wire (w)
            public int color = 0;
            public Single X1 = 0;
            public Single Y1 = 0;
            public Single X2 = 0;
            public Single Y2 = 0;
            public int unk1 = 0;
            public int unk2 = 0;
            public int net = 0;         //Lines do not include net and unk3 values
            public int unk3 = 0;
            public DcString dcStr = new DcString();
        }

        private class DcText
        {
            public DcItemType recordType = DcItemType.text;     //Text (t)
            public int color=0;
            public Single X1=0;
            public Single Y1=0;
            public Single scale=0;     // scaling factor for the font
            public Single rotation=0;
            public int unk1=0;
            public int unk2=0;
            public DcString dcStr = new DcString();
        }

         private class DcString
        {
            public DcItemType recordType = DcItemType.str;     // String/symbol name (s)
            public int unk1=0;
            public int unk2=0;
            public int unk3=0;
            public String strText="";  // all string text fields in file begin with "#". Done for parsing to allow strings to have spaces in them.
                                       // # serves double-duty as start of a comment line
        }

        private class DcCircle
        {
            public DcItemType recordType = DcItemType.circle;  //Circle (c)
            public int color = 0;
            public Single X1 = 0;
            public Single Y1 = 0;
            public Single X2 = 0;
            public Single Y2 = 0;
        }

        private class DcNet
        {
            public String name = "-unassigned-";
            public int number = 0;
        }

        private void parseFile()
        {
            String fname = Properties.Settings.Default.FNAME;
            String[] fields;
            String fieldStr = "";
            DcItemType recordType = DcItemType.undefined;
            DcItemType prevRecordType = DcItemType.undefined;
            int strIndex = 0;
            int textItemCount = 0;
            int strItemCount = 0;

            drawListLoaded = false;
            debugWritten = false;

            drawList.Clear();
            biggestX = -10000;
            biggestY = -10000;
            smallestX = 10000;
            smallestY = 10000;
            textScalingList.Clear();
            String line;
            textBox3.Clear();
            System.IO.StreamReader file;

            if (File.Exists(fname))
            {
                file = new System.IO.StreamReader(fname);
            }
            else {
                MessageBox.Show("File not found. Please select a file using Open from the File menu");
                return;
            }

            int j = 0;
            for (int i = 0; i < 2000; i++)
            {
                if (file.EndOfStream) break;
                line = file.ReadLine();
                if (line != null)
                {
                    line = line.Replace("  ", " ");

                    strIndex = line.IndexOf('#');
                    if (strIndex >= 0)
                    {
                        fieldStr = line.Substring(strIndex);
                        line = line.Substring(0, strIndex);
                    }

                    line = line.TrimEnd(' ');

                    fields = line.Split(' ');
                    
                    switch (fields[0])
                    {
                        case "t": recordType = DcItemType.text; break;
                        case "s": recordType = DcItemType.str; break;
                        case "w": recordType = DcItemType.wire; break;
                        case "l": recordType = DcItemType.line; break;
                        case "c": recordType = DcItemType.circle; break;
                        default: recordType = DcItemType.undefined; break;
                    }

                    switch (recordType)  // create a record object for line and add it to draw list
                    {
                        case DcItemType.text:
                            DcText dcText = new DcText
                            {
                                color = Convert.ToInt16(fields[1]),
                                X1 = Convert.ToSingle(fields[2]),
                                Y1 = Convert.ToSingle(fields[3]),
                                scale = Convert.ToSingle(fields[4]),
                                rotation = Convert.ToSingle(fields[5]),
                                unk1 = Convert.ToInt16(fields[6]),
                                unk2 = Convert.ToInt16(fields[7])
                            };
                            textItemCount++;
                            drawList.Add(dcText);
                            if (!textScalingList.Contains(dcText.scale))
                            {
                                textScalingList.Add(dcText.scale);
                            }
                            break;

                        case DcItemType.str:
                            DcString dcStr = new DcString
                            {
                                unk1 = Convert.ToInt16(fields[1]),
                                unk2 = Convert.ToInt16(fields[2]),
                                unk3 = Convert.ToInt16(fields[3]),
                                strText = fieldStr
                            };
                            strItemCount++;

                            // Set String in previous record
                            if (prevRecordType == DcItemType.text)
                            {
                                if (drawList.Count > 0) ((DcText)drawList[drawList.Count-1]).dcStr = dcStr;
                            }

                            if (prevRecordType == DcItemType.wire)
                            {
                                if (drawList.Count > 0)
                                {
                                    if (drawList[drawList.Count - 1].GetType() == typeof(DcWire))
                                    {
                                        DcWire dcw = (DcWire)drawList[drawList.Count - 1];
                                        dcw.dcStr = dcStr;
                                    }
                                }
                            }
                            break;

                        case DcItemType.wire:
                            DcWire dcWire = new DcWire
                            {
                                color = Convert.ToInt16(fields[1]),
                                X1 = Convert.ToSingle(fields[2]),
                                Y1 = Convert.ToSingle(fields[3]),
                                X2 = Convert.ToSingle(fields[4]),
                                Y2 = Convert.ToSingle(fields[5]),
                                unk1 = Convert.ToInt16(fields[6]),
                                unk2 = Convert.ToInt16(fields[7]),
                                net = Convert.ToInt16(fields[8]),
                                unk3 = Convert.ToInt16(fields[9])
                            };

                            if (dcWire.X1 > biggestX) biggestX = dcWire.X1;
                            if (dcWire.X1 < smallestX) smallestX = dcWire.X1;

                            if (dcWire.Y1 > biggestY) biggestY = dcWire.Y1;
                            if (dcWire.Y1 < smallestY) smallestY = dcWire.Y1;

                            if (dcWire.X2 > biggestX) biggestX = dcWire.X2;
                            if (dcWire.X2 < smallestX) smallestX = dcWire.X2;

                            if (dcWire.Y2 > biggestY) biggestY = dcWire.Y2;
                            if (dcWire.Y2 < smallestY) smallestY = dcWire.Y2;

                            DcDrawItem dWire = new DcDrawItem
                            {
                                itemType = DcItemType.line,
                                X1 = dcWire.X1,
                                Y1 = dcWire.Y1,
                                X2 = dcWire.X2,
                                Y2 = dcWire.Y2
                            };
                            drawList.Add(dWire);
                            j++;
                            break;

                        case DcItemType.line:
                            DcLine dcLine = new DcLine
                            {
                                color = Convert.ToInt16(fields[1]),
                                X1 = Convert.ToSingle(fields[2]),
                                Y1 = Convert.ToSingle(fields[3]),
                                X2 = Convert.ToSingle(fields[4]),
                                Y2 = Convert.ToSingle(fields[5]),
                                unk1 = Convert.ToInt16(fields[6]),
                                unk2 = Convert.ToInt16(fields[7])
                            };

                            if (dcLine.X1 > biggestX) biggestX = dcLine.X1;
                            if (dcLine.X1 < smallestX) smallestX = dcLine.X1;

                            if (dcLine.Y1 > biggestY) biggestY = dcLine.Y1;
                            if (dcLine.Y2 < smallestY) smallestY = dcLine.Y1;

                            if (dcLine.X2 > biggestX) biggestX = dcLine.X2;
                            if (dcLine.X2 < smallestX) smallestX = dcLine.X2;

                            if (dcLine.Y2 > biggestY) biggestY = dcLine.Y2;
                            if (dcLine.Y2 < smallestY) smallestY = dcLine.Y2;

                            DcDrawItem dLine = new DcDrawItem
                            {
                                itemType = DcItemType.line,
                                color = dcLine.color,
                                X1 = dcLine.X1,
                                Y1 = dcLine.Y1,
                                X2 = dcLine.X2,
                                Y2 = dcLine.Y2
                            };

                            drawList.Add(dLine);
                            break;

                        case DcItemType.circle:
                            DcCircle dcCircle = new DcCircle
                            {
                                color = Convert.ToInt16(fields[1]),
                                X1 = Convert.ToSingle(fields[2]),
                                Y1 = Convert.ToSingle(fields[3]),
                                X2 = Convert.ToSingle(fields[4]),
                                Y2 = Convert.ToSingle(fields[5])
                            };

                            DcDrawItem dCircle = new DcDrawItem
                            {
                                itemType = DcItemType.circle,
                                color = dcCircle.color,
                                X1 = dcCircle.X1,
                                Y1 = dcCircle.Y1,
                                X2 = dcCircle.X2,
                                Y2 = dcCircle.Y2
                            };
                            drawList.Add(dCircle);
                            break;

                        default: break;
                    }
                    prevRecordType = recordType;
                }
                else break;
            }
            textBox3.Text += "Biggest X: " + biggestX.ToString() + "\r\n";
            textBox3.Text += "Biggest Y: " + biggestY.ToString() + "\r\n";
            textBox3.Text += "Smallest X: " + smallestX.ToString() + "\r\n";
            textBox3.Text += "Smallest Y: " + smallestY.ToString() + "\r\n";
            textBox3.Text += "Text Item Count: " + textItemCount.ToString() + "\r\n";
            textBox3.Text += "String Item Count: " + strItemCount.ToString() + "\r\n";
            textBox3.Text += "Draw List segments: " + drawList.Count.ToString() + "\r\n";

            drawListLoaded = true;
            foreach(Single scale in textScalingList)
            {
                Console.WriteLine(scale.ToString());
            }
            this.Refresh();
            file.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = Properties.Settings.Default.FNAME;
            openFileDialog1.ShowDialog();
            textBox1.Text = Properties.Settings.Default.FNAME;
            textBox1.Update();
            if (openFileDialog1.FileName != "")
            {
                Properties.Settings.Default.FNAME = openFileDialog1.FileName;
                Properties.Settings.Default.Save();
                textBox1.Text = openFileDialog1.FileName;
                textBox1.Update();
                menuStrip1.Select();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parseFile();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Visible == true)
            {
                textBox3.Hide();
                button2.Text = "show info";
                Properties.Settings.Default.ShowInfo = false;
            } else
            {
                textBox3.Show();
                button2.Text = "hide info";
                Properties.Settings.Default.ShowInfo = true;
            }
            Properties.Settings.Default.Save();
        }
    }
}
