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
    public partial class MainForm : Form
    {
        Single biggestX = -100000, biggestY = -100000;
        Single smallestX = 100000, smallestY = 100000;
        Single ZoomFactor = 1;
        private List<DcDrawItem> drawList = new List<DcDrawItem>();
        private List<Single> textScalingList = new List<Single>();

        private bool drawListLoaded = false;

        private class DcDrawItem
        {
            public DcItemType Type = DcItemType.undefined;
        }

        public FolderConfigForm folderConfigForm = new FolderConfigForm();

        public MainForm()
        {
            InitializeComponent();
            menuStrip1.Select();
            textBox1.Text = Properties.Settings.Default.fileName;
            this.Width = Convert.ToInt32(2250 * 0.55);
            this.Height = Convert.ToInt32(1375 * 0.55);
            if (Properties.Settings.Default.ShowInfo == true)
            {
                HideNShowInfoButton.Text = "hide info";
                textBox3.Show();
            }
            else
            {
                HideNShowInfoButton.Text = "show info";
                textBox3.Hide();
            }
        }

        // My B-SIZE DRAWING IS 15-7/8" x 10-15/16", Inner border is ~9-1/2" x ~15-1/2"
        // At present drawing scale 1937 -> 15” = .00774" per unit
        // units = inches / .00774
        // draw a 15.5" line from 3/16", 3/16"
        //Single factor = .00785F;
        //Single x1, y1, x2, y2;
        // draw a line from origin to 16"
        //gs.DrawLine(pen1, 0, 0, 16F / factor, 0);

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics gs = e.Graphics;
           
            //Create brush object
            Brush brush1 = new SolidBrush(Color.Black);

            //Create pen objects
            Pen pen1 = new Pen(Color.Black);

            Single dpiX = gs.DpiX;
            Single dpiY = gs.DpiY;
            int windowWidth = this.Width;
            int windowHeight = this.Height;
            
            gs.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            gs.SmoothingMode = SmoothingMode.AntiAlias;

            if (drawListLoaded)
            {
                Point pt1 = new Point();
                Point pt2 = new Point();
                pen1.Width = 1;
                pen1.Color = Color.Black;

                gs.TranslateTransform(25F-smallestX, biggestY + 80F/ZoomFactor); // Move the origin "down".
                gs.ScaleTransform(ZoomFactor, ZoomFactor, MatrixOrder.Append);

                pen1.Color = Color.LightGray;
                gs.DrawLine(pen1, biggestX - 5, -biggestY, biggestX + 5, -biggestY);
                gs.DrawLine(pen1, biggestX, -biggestY - 5, biggestX, -biggestY + 5);

                // draw crossed lines at origin
                gs.DrawLine(pen1, 0 - 5, 0, 0 + 5, 0);
                gs.DrawLine(pen1, 0, 0 - 5, 0, 0 + 5);
                pen1.Color = Color.Black;

                for (int i=0; i<drawList.Count; i++)
                {
                    switch (drawList[i].Type)
                    {
                        case DcItemType.arc:
                            DcArc dArc = (DcArc)drawList[i];
                            FloatPt arcCenter = new FloatPt(dArc.centerX, -dArc.centerY);
                            FloatPt p1 = new FloatPt(dArc.X1, -dArc.Y1);
                            FloatPt p2 = new FloatPt(dArc.X2, -dArc.Y2);
                            DcDrawArc(gs, pen1, arcCenter, p1, p2);
                            break;
                        case DcItemType.circle:
                            DcCircle dCircle = (DcCircle)drawList[i];
                            pt1.X = Convert.ToInt32(dCircle.X1);
                            pt1.Y = Convert.ToInt32(-dCircle.Y1);
                            pt2.X = Convert.ToInt32(dCircle.X2);
                            Single radius = Math.Abs(pt1.X - pt2.X);
                            Single diameter = 2F * radius;
                            Single center = pt1.X;
                            gs.DrawEllipse(pen1, center - radius, pt1.Y - radius, diameter, diameter);
                            break;
                        case DcItemType.line:
                            DcLine dLine = (DcLine)drawList[i];
                            pt1.X = Convert.ToInt32(dLine.X1);
                            pt1.Y = Convert.ToInt32(-dLine.Y1);
                            pt2.X = Convert.ToInt32(dLine.X2);
                            pt2.Y = Convert.ToInt32(-dLine.Y2);
                            gs.DrawLine(pen1, pt1, pt2);
                            break;
                        case DcItemType.module: break;
                        case DcItemType.pin:
                            DcPin dPin = (DcPin)drawList[i];
                            // draw a small square to identify the pin location.
                            Console.WriteLine(dPin.Type);
                            float width = pen1.Width; // save width
                            pen1.Width = 0.5F;
                            gs.DrawRectangle(pen1, dPin.X1 - 2, -(dPin.Y1 + 2), 4, 4);
                            pen1.Width = width;  // restore width
                            break;
                        case DcItemType.str: break;
                        case DcItemType.text:
                            DcText dct = (DcText)drawList[i];
                            DcDrawText(gs, pen1, dct);
                            break;
                        case DcItemType.wire:
                            DcWire dWire = (DcWire)drawList[i];
                            pt1.X = Convert.ToInt32(dWire.X1);
                            pt1.Y = Convert.ToInt32(-dWire.Y1);
                            pt2.X = Convert.ToInt32(dWire.X2);
                            pt2.Y = Convert.ToInt32(-dWire.Y2);
                            gs.DrawLine(pen1, pt1, pt2);
                            break;
                        case DcItemType.undefined:
                            break;
                        default:
                            break;
                    }
                }
            }
            pen1.Dispose();
            brush1.Dispose();
            //gs.Dispose();
        }

        private void DcDrawText(Graphics gs, Pen pen1, DcText dct)
        {
            PointF pt = new PointF
            {
                X = Convert.ToInt32(dct.X1),
                Y = Convert.ToInt32(-dct.Y1)
            };
            String text = dct.dcStr.strText;
            text = text.TrimStart('#');

            Color color = pen1.Color;

            FontFamily fontFamily = new FontFamily("MS GOTHIC");

            // Calculate font scale factor
            Single fontSize = 10.5F * dct.scale / 0.039063F;

            Font the_font = new Font(fontFamily, fontSize);
            SizeF textSize = gs.MeasureString(text, the_font);

            Single ascent = the_font.FontFamily.GetCellAscent(FontStyle.Regular);
            Single descent = the_font.FontFamily.GetCellDescent(FontStyle.Regular);
            Single emHeight = the_font.FontFamily.GetEmHeight(FontStyle.Regular);
            Single cellHeight = ascent + descent;
            Single cellHeightPixel = the_font.Size * cellHeight / emHeight;
            Single sizeInPoints = the_font.SizeInPoints;  // = emHeight in points

            if (dct.rotation != 0)
            {
                DrawRotatedTextAt(gs, -dct.rotation, text, (int)(pt.X), (int)(pt.Y), the_font, new SolidBrush(color));
            }
            else
            {
                pt.Y -= (cellHeightPixel);
                gs.DrawString(text, the_font, new SolidBrush(color), pt);
            }
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

        private class FloatPt
        {
            public float X = 0;
            public float Y = 0;

            public FloatPt(float x, float y)
            {
                X = x;
                Y = y;
            }
        }

        private static float DcDrawArc(Graphics gr, Pen pen, FloatPt centerPt, FloatPt p1, FloatPt p2)
        {
            // Calculate radius as distance from center to p1
            float radius = (float)Math.Sqrt((centerPt.X - p1.X) * (centerPt.X - p1.X) + (centerPt.Y - p1.Y) * (centerPt.Y - p1.Y));

            // Calculate rectangle for DrawArc
            float xxB = centerPt.X - radius;
            float yyB = centerPt.Y - radius;
            float width = 2 * radius;
            float height = 2 * radius;

            // Determine angles for DrawArc
            float startAngle2 = (float)((180 / Math.PI) * Math.Atan2(p1.Y - centerPt.Y, p1.X - centerPt.X));
            float endAngle2 = (float)((180 / Math.PI) * Math.Atan2(p2.Y - centerPt.Y, p2.X - centerPt.X));

            // Draw the arc
            gr.DrawArc(pen, xxB, yyB, width, height, startAngle2, endAngle2 - startAngle2);

            return radius;
        }

        private static void DrawCross(Graphics gr, Pen pen, FloatPt point)
        {
            gr.DrawLine(pen, point.X - 5, point.Y, point.X + 5, point.Y);
            gr.DrawLine(pen, point.X, point.Y - 5, point.X, point.Y + 5);
        }

        private enum DcItemType { line, wire, circle, arc, module, pin, str, text, undefined };

        DcItemType getDcType(object obj)
        {
            if (obj.GetType() == typeof(DcLine)) return DcItemType.line;
            if (obj.GetType() == typeof(DcWire)) return DcItemType.wire;
            if (obj.GetType() == typeof(DcText)) return DcItemType.text;
            if (obj.GetType() == typeof(DcCircle)) return DcItemType.circle;
            if (obj.GetType() == typeof(DcString)) return DcItemType.str;
            if (obj.GetType() == typeof(DcPin)) return DcItemType.pin;
            return DcItemType.undefined;
        }

        private class DcArc : DcDrawItem
        {
            // Draw arc given coordinates of the center of a circle and two points on the circle that define the arc.
            // Calculating the coordinates of the center given two arbitrary points and a radius is an interesting problem
            // for the software that generates the file. Fortunately, it's fairly straightforward to draw the arc once 
            // the points have been calculated.
            public DcItemType recordType = DcItemType.arc;     //Arc (a) - e.g. "a  6 -38.641014 10 -4 -10 -4 30"
            public int color = 0;
            public Single centerX = 0;
            public Single centerY = 0;
            public Single X1 = 0;
            public Single Y1 = 0;
            public Single X2 = 0;
            public Single Y2 = 0;
        }

        private class DcPin : DcDrawItem
        {   // a pin may have an associated s record. The record defines the pin number, e.g. s 1 0 #2
            public DcItemType recordType = DcItemType.pin;     //Pin (p) - e.g. "p  15 60 10 60 10 1"
            public int color = 0;
            public Single X1 = 0;
            public Single Y1 = 0;
            public Single unk1 = 0;
            public Single unk2 = 0;
            public Single unk3 = 0;
            public String Text = "";
        }

        private class DcLine : DcDrawItem
        {
            public DcItemType recordType = DcItemType.line;    //Line (l)
            public int color = 0;
            public Single X1 = 0;
            public Single Y1 = 0;
            public Single X2 = 0;
            public Single Y2 = 0;
            public int unk1 = 0;        // I've found some 2.10 library modules that have only 6 fields for a line instead of seven (no unk2)
            public int unk2 = 0;
        }

        private class DcWire : DcDrawItem
        {
            public DcItemType recordType = DcItemType.wire;    //Wire (w)
            public int color = 0;
            public Single X1 = 0;
            public Single Y1 = 0;
            public Single X2 = 0;
            public Single Y2 = 0;
            public int unk1 = 0;
            public int unk2 = 0;
            public int net = 0;         //Lines and wires are similar but lines do not include net and unk3 values
            public int unk3 = 0;
            public DcString dcStr = new DcString();
        }

        private class DcText : DcDrawItem
        {
            public DcItemType recordType = DcItemType.text;     //Text (t)
            public int color = 0;
            public Single X1 = 0;
            public Single Y1 = 0;
            public Single scale = 0;     // scaling factor for the font
            public Single rotation = 0;
            public int unk1 = 0;
            public int unk2 = 0;
            public DcString dcStr = new DcString();
        }

        private class DcString : DcDrawItem
        {
        public DcItemType recordType = DcItemType.str;     // String/symbol name (s)
        public int unk1 = 0;
        public int unk2 = 0;
        public int unk3 = 0;
        public String strText="";  // all string text fields in file begin with "#".
                                    // Done for parsing to allow strings to have spaces in them.
                                    // # serves double-duty as start of a comment or comment line
        }

        private class DcCircle : DcDrawItem
        {
            public DcItemType recordType = DcItemType.circle;  //Circle (c)
            public int color = 0;       // color or layer
            public Single X1 = 0;
            public Single Y1 = 0;
            public Single X2 = 0;
            public Single Y2 = 0;
        }

        private class DcNet : DcDrawItem
        {
            public String name = "-unassigned-";
            public int number = 0;
        }

        private class DcModule : DcDrawItem //include Module (m)  -- e.g. m  15 0 0 1.25 0 0 bsize 0 0 0 0 0
        {
            public DcItemType recordType = DcItemType.module; // [0] (m)
            public int color = 0;           // [1] color or layer
            public int unk2 = 0;
            public int unk3 = 0;
            public Single scaleFactor = 0;  // [4]
            public int unk5 = 0;
            public int unk6 = 0;
            public String name = "";        // [7]
            public int unk8 = 0;
            public int unk9 = 0;
            public int unk10 = 0;
            public int unk11 = 0;
            public int unk12 = 0;
        }

        private void DcMakeDrawList()
        {
            String fname = Properties.Settings.Default.fileName;
            String[] fields;
            String fieldStr = "";
            DcItemType recordType = DcItemType.undefined;
            DcItemType prevRecordType = DcItemType.undefined;
            int strIndex = 0;
            int textItemCount = 0;
            int strItemCount = 0;
            int pinItemCount = 0;
            int moduleItemCount = 0;

            drawListLoaded = false;

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
                        case "a": recordType = DcItemType.arc; break;
                        case "c": recordType = DcItemType.circle; break;
                        case "s": recordType = DcItemType.str; break;
                        case "t": recordType = DcItemType.text; break;
                        case "l": recordType = DcItemType.line; break;
                        case "m": recordType = DcItemType.module; break;
                        case "p": recordType = DcItemType.pin; break;
                        case "w": recordType = DcItemType.wire; break;
                        default: recordType = DcItemType.undefined; break;
                    }

                    switch (recordType)  // create a record object for line and add it to draw list
                    {
                        case DcItemType.arc:
                            DcArc dArc = new DcArc()
                            {
                                Type = DcItemType.arc,
                                color = Convert.ToInt16(fields[1]),
                                centerX = Convert.ToSingle(fields[2]),
                                centerY = Convert.ToSingle(fields[3]),
                                X1 = Convert.ToSingle(fields[4]),
                                Y1 = Convert.ToSingle(fields[5]),
                                X2 = Convert.ToSingle(fields[6]),
                                Y2 = Convert.ToSingle(fields[7])
                            };
                            drawList.Add(dArc);
                            break;

                        case DcItemType.circle:
                            DcCircle dcCircle = new DcCircle
                            {
                                Type = DcItemType.circle,
                                color = Convert.ToInt16(fields[1]),
                                X1 = Convert.ToSingle(fields[2]),
                                Y1 = Convert.ToSingle(fields[3]),
                                X2 = Convert.ToSingle(fields[4]),
                                Y2 = Convert.ToSingle(fields[5])
                            };
                            drawList.Add(dcCircle);
                            break;

                        case DcItemType.line:
                            DcLine dcLine = new DcLine
                            {
                                Type = DcItemType.line,
                                color = Convert.ToInt16(fields[1]),
                                X1 = Convert.ToSingle(fields[2]),
                                Y1 = Convert.ToSingle(fields[3]),
                                X2 = Convert.ToSingle(fields[4]),
                                Y2 = Convert.ToSingle(fields[5]),
                                unk1 = Convert.ToInt16(fields[6]),
                                unk2 = Convert.ToInt16(fields[7])
                            };
                            BiggestSmallestX(dcLine.X1);
                            BiggestSmallestX(dcLine.X2);
                            BiggestSmallestY(dcLine.Y1);
                            BiggestSmallestY(dcLine.Y2);
                            drawList.Add(dcLine);
                            break;

                        case DcItemType.module:
                            DcModule dcModule = new DcModule()
                            {
                                Type = DcItemType.module,
                                color = Convert.ToInt16(fields[1]),
                                scaleFactor = Convert.ToSingle(fields[4]),
                                name = Convert.ToString(fields[7])
                            };
                            moduleItemCount++;
                            Console.WriteLine(dcModule.name);
                            break;

                        case DcItemType.pin:
                            DcPin dcPin = new DcPin()
                            {
                                Type = DcItemType.pin,
                                color = Convert.ToInt16(fields[1]),
                                X1 = Convert.ToSingle(fields[2]),
                                Y1 = Convert.ToSingle(fields[3])
                            };
                            BiggestSmallestX(dcPin.X1);
                            BiggestSmallestY(dcPin.Y1);
                            drawList.Add(dcPin);
                            pinItemCount++;
                            break;

                        case DcItemType.str:
                            DcString dcStr = new DcString
                            {
                                Type = DcItemType.str,
                                unk1 = Convert.ToInt16(fields[1]),
                                unk2 = Convert.ToInt16(fields[2]),
                                strText = fieldStr
                            };

                            // I discovered some string commands only have two leading fields. E.g. "s 1 0 #TYPE"
                            // while others have three E.g. "s 1 0 1 #CA31:2
                            if (fields.Length > 3) dcStr.unk3 = Convert.ToInt16(fields[3]);

                            strItemCount++;

                            // Set String in previous record
                            if (prevRecordType == DcItemType.text)
                            {
                                if (drawList.Count > 0)
                                {
                                    DcText dText = (DcText)drawList[drawList.Count - 1];
                                    dText.dcStr = dcStr;
                                }
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

                        case DcItemType.text:
                            DcText dcText = new DcText
                            {
                                Type = DcItemType.text,
                                color = Convert.ToInt16(fields[1]),
                                X1 = Convert.ToSingle(fields[2]),
                                Y1 = Convert.ToSingle(fields[3]),
                                scale = Convert.ToSingle(fields[4]),
                                rotation = Convert.ToSingle(fields[5]),
                                unk1 = Convert.ToInt16(fields[6]),
                                unk2 = Convert.ToInt16(fields[7])
                            };
                            BiggestSmallestX(dcText.X1);
                            BiggestSmallestY(dcText.Y1);
                            textItemCount++;
                            drawList.Add(dcText);
                            if (!textScalingList.Contains(dcText.scale))
                            {
                                textScalingList.Add(dcText.scale);
                            }
                            break;

                        case DcItemType.wire:
                            DcWire dcWire = new DcWire
                            {
                                Type = DcItemType.wire,
                                color = Convert.ToInt16(fields[1]),
                                X1 = Convert.ToSingle(fields[2]),
                                Y1 = Convert.ToSingle(fields[3]),
                                X2 = Convert.ToSingle(fields[4]),
                                Y2 = Convert.ToSingle(fields[5]),
                                unk1 = Convert.ToInt16(fields[6]),
                                unk2 = Convert.ToInt16(fields[7])
                            };
                            if (fields.Length > 8) dcWire.net = Convert.ToInt16(fields[8]);
                            if (fields.Length > 9) dcWire.unk3 = Convert.ToInt16(fields[9]);

                            BiggestSmallestX(dcWire.X1);
                            BiggestSmallestY(dcWire.Y1);
                            BiggestSmallestX(dcWire.X2);
                            BiggestSmallestY(dcWire.Y2);
                            drawList.Add(dcWire);
                            j++;
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
            textBox3.Text += "Module Entries Count: " + moduleItemCount.ToString() + "\r\n";
            textBox3.Text += "Text Entries Count: " + textItemCount.ToString() + "\r\n";
            textBox3.Text += "String Entries Count: " + strItemCount.ToString() + "\r\n";
            textBox3.Text += "Pin Entries Count: " + pinItemCount.ToString() + "\r\n";

            drawListLoaded = true;
            Console.WriteLine(folderConfigForm.libraryFolder);
            foreach(Single scale in textScalingList)
            {
                Console.WriteLine(scale.ToString());
            }
            this.Refresh();
            file.Close();
        }

        private void BiggestSmallestX(float X)
        {
            if (X > biggestX) biggestX = X;
            if (X < smallestX) smallestX = X;
        }

        private void BiggestSmallestY(float Y)
        {
            if (Y > biggestY) biggestY = Y;
            if (Y < smallestY) smallestY = Y;
        }
        
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fullName = Properties.Settings.Default.fileName;
            string fName = "";
            string fDir = "";

            textBox1.Text = Properties.Settings.Default.fileName;
            textBox1.Update();

            fName = fullName.Substring(fullName.LastIndexOf(@"\") + 1);
            if (fDir != "") fDir = fullName.Substring(0, fullName.LastIndexOf(@"\"));

            openFileDialog1.InitialDirectory = fDir;
            openFileDialog1.FileName = fName;
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName != "")
            {
                Properties.Settings.Default.fileName = openFileDialog1.FileName;
                Properties.Settings.Default.Save();
                textBox1.Text = openFileDialog1.FileName;
                textBox1.Update();
                menuStrip1.Select();
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FoldersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderConfigForm.ShowDialog();
        }

        private void ToolStripMenuZoom50_Click(object sender, EventArgs e)
        {
            ZoomFactor = 0.5F;
            DcMakeDrawList();
        }

        private void ToolStripMenuZoom100_Click(object sender, EventArgs e)
        {
            ZoomFactor = 1.0F;
            DcMakeDrawList();
        }

        private void ToolStripMenuZoom150_Click(object sender, EventArgs e)
        {
            ZoomFactor = 1.5F;
            DcMakeDrawList();
        }

        private void ToolStripMenuZoom200_Click(object sender, EventArgs e)
        {
            ZoomFactor = 2.0F;
            DcMakeDrawList();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void DrawFileButton_Click(object sender, EventArgs e)
        {
            DcMakeDrawList();
        }

        private void HideNShowInfoButton_Click(object sender, EventArgs e)
        {
            if (textBox3.Visible == true)
            {
                textBox3.Hide();
                HideNShowInfoButton.Text = "show info";
                Properties.Settings.Default.ShowInfo = false;
            } else
            {
                textBox3.Show();
                HideNShowInfoButton.Text = "hide info";
                Properties.Settings.Default.ShowInfo = true;
            }
            Properties.Settings.Default.Save();
        }
    }
}
