using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Text;
using DcClasses;

// TMS - started September 17, 2019

// Module loading and bounds processing:
// BUILD INTERNAL LIBRARY
//  CREATE LIST CONTAINING LIBRARY CATALOGS FROM LIBRARY FOLDER AND A LIST OF FILES IN WORKING DIRECTORY
//     Add top module entry to internal library and mark as unprocessed.
// 1. get unprocessed module entry from internal library. If none found then done. (since top module is initially the only
//    module in the list, processing will always start with top module.)
// 1.5 this module is the working 'parent' module
// 2. search for module file in working directory and library catalogs (top module name comes from "open file" so
//    top module will actually just respond to a "file exists" check.)
// 3. If module not found mark module entry as processed with bounds set to 0,0,0,0
// 4. If module found, extract draw list for module 
// 5. Scan draw list for module commands, for each module command ("submodule") found:
// 5.1      Create entry for internal library if not already present in list (marked as unprocessed)
// 5.2      Set module command in drawlist to point to internal library entry.
// 6. Mark internal library entry as processed indicating all module commands in drawlist for present entry were scanned
// 7. Back to (1)

namespace DMEEView1
{
    public partial class MainForm : Form
    {
        string topFileName = "";
        string libraryFolder = "";
        string workingFolder = "";
        bool showInfo = false;
        float ZoomFactor = 1;
        const string crlf = "\r\n";
        bool drawingLoaded = false;
        bool fitToWindow = false;

        private List<InternalLBREntry> internalLBR = new List<InternalLBREntry>();
        List<DcExternalLBRCatalog> externalLBRCatalogs = new List<DcExternalLBRCatalog>();
        private DcModule topModuleCommand = new DcModule();
        private PrinterSettings printerSettings = new PrinterSettings();
        private PrintSetupForm printSetupForm = new PrintSetupForm();
        public FolderConfigForm folderConfigForm = new FolderConfigForm();
        public ColorConfigForm colorConfigForm = new ColorConfigForm();
        public ColorConfigForm.DcColorConfig dcColorSettings = new ColorConfigForm.DcColorConfig();

        public MainForm()
        {
            InitializeComponent();
            menuStrip1.Select();
            RestoreAppSettings();  // Restore saved settings

            // Set the initial height and width of the form's window to a 12 x 18 Height to Width ratio
            // where the window height is 95% of the screen height
            Height = (int)(Screen.FromControl(this).Bounds.Height * 0.95F);
            Width = (int)(18F / 12F * Height);
            CenterToScreen();

            // Set the initial location and size of DrawPanel relative to form
            DrawPanel.Location = new Point(0, menuStrip1.Height);
            DrawPanel.Height = Height - menuStrip1.Height - 40;
            DrawPanel.Width = Width - 20;

            // Set location of DrawPictureBox.
            // (Height and Width are set later based on drawing's bounds)
            DrawPictureBox.Location = new Point(0, 0);

            // Set initial text of file name based on previous setting
            TopFileNameTextBox.Text = topFileName;
            TopFileNameTextBox.Show();

            // Set info button based on previous setting of showInfo
            if (showInfo == true)
            {
                HideNShowInfoButton.Text = "hide info";
                InfoTextBox.Show();
            }
            else
            {
                HideNShowInfoButton.Text = "show info";
                InfoTextBox.Hide();
            }
        }

        private void RestoreAppSettings()
        {
            // restore previous setting of topFileName if valid
            topFileName = Properties.Settings.Default.fileName;
            if (!File.Exists(Properties.Settings.Default.fileName))
            {
                topFileName = "";
                Properties.Settings.Default.fileName = topFileName;
            };

            // restore setting of showInfo
            if (Properties.Settings.Default.ShowInfo == true)
            {
                showInfo = true;
            }
            else showInfo = false;

            // restore previous working folder if still valid
            workingFolder = Properties.Settings.Default.WorkFolder;
            if (!Directory.Exists(workingFolder)) // clear if not valid
            {
                workingFolder = "";
                Properties.Settings.Default.WorkFolder = workingFolder;
            }

            // restore previous library folder if still valid
            libraryFolder = Properties.Settings.Default.LibFolder;
            if (!Directory.Exists(libraryFolder))
            {
                libraryFolder = "";
                Properties.Settings.Default.LibFolder = libraryFolder;
            }

            // restore previous page settings
            // Aside: Apparent bug in how MSFT handles properties in settings. If I name the paper
            // size setting "paperSize" the debugger reports a "FileNotFound" exception in mscorlib.dll
            ///when accessing the setting even though the correct values get loaded.
            // Trying to "catch" the exception doesn't catch anything! Changing the property
            // name to something else, e.g. PSize, makes the error go away. Hmmmm....
            printDocument.DefaultPageSettings.PaperSize = Properties.Settings.Default.PSize;
            printDocument.DefaultPageSettings.Margins = Properties.Settings.Default.margins;
            printDocument.DefaultPageSettings.Landscape = Properties.Settings.Default.landscape;

            // restore color settings
            dcColorSettings.pinsColor = Properties.Settings.Default.pinsColor;
            dcColorSettings.textColor = Properties.Settings.Default.textColor;
            dcColorSettings.wiresColor = Properties.Settings.Default.wiresColor;
            dcColorSettings.linesColor = Properties.Settings.Default.linesColor;
            dcColorSettings.showPins = Properties.Settings.Default.showPins;
            dcColorSettings.blackAndWhite = Properties.Settings.Default.blackAndWhite;
        }

        private void ModuleInfoTextBox(InternalLBREntry entry)
        {
            InfoTextBox.Clear();
            InfoTextBox.Text += "Library Folder: " + libraryFolder + crlf;
            InfoTextBox.Text += "Working Folder: " + workingFolder + crlf;
            InfoTextBox.Text += "Drawing bounds: XMax = " + topModuleCommand.bounds.XMax + ", YMax = " + topModuleCommand.bounds.YMax;
            InfoTextBox.Text += "  XMin = " + topModuleCommand.bounds.XMin + ", YMin = " + topModuleCommand.bounds.YMin + crlf;
            InfoTextBox.Text += "Text (t) Entries Count: " + entry.stats.textItemCount + crlf;
            InfoTextBox.Text += "String (s) Entries Count: " + entry.stats.strItemCount + crlf;
            InfoTextBox.Text += "Pin (p) Entries Count: " + entry.stats.pinItemCount + crlf;
            InfoTextBox.Text += "Drawing/display (d) Entries Count: " + entry.stats.drawingItemCount + crlf;
            InfoTextBox.Text += "Module (m) Entries Count (modules in top drawing): " + entry.stats.moduleItemCount + crlf;
            InfoTextBox.Text += "Internal Library Entries Count (unique modules used in drawing & sub-modules): "
                                + internalLBR.Count;
        }

        private void DrawCropMark(Graphics gr, Pen pen, PointF center)
        {
            gr.DrawLine(pen, center.X - 5, center.Y, center.X + 5, center.Y);
            gr.DrawLine(pen, center.X, center.Y - 5, center.X, center.Y + 5);
        }

        // =======================================================
        //           PAINT DC MODULE
        // =======================================================
        public void PaintDcModule(Graphics gr, DcModule moduleCommand)
        {
            PointF ptf1 = new Point();
            PointF ptf2 = new Point();

            GraphicsState gsSaved = gr.Save();

            // scale coordinates from this point on based on module's location, scale factor, and rotation
            gr.TranslateTransform(moduleCommand.X1, moduleCommand.Y1);
            gr.ScaleTransform(moduleCommand.scaleFactor, moduleCommand.scaleFactor);
            if (moduleCommand.mirror == 1) gr.ScaleTransform(1, -1);
            gr.RotateTransform(moduleCommand.rotation);

            Pen pen = new Pen(Color.Black);
            pen.Width = 1F;
            var drawList = internalLBR[moduleCommand.internalLBRIndex].drawList;
            gr.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            if (ZoomFactor > 0.5) gr.SmoothingMode = SmoothingMode.AntiAlias;

            Pen savedPen = (Pen)pen.Clone();

            for (int i = 0; i < drawList.Count; i++)
            {
                GraphicsState grSaved = gr.Save();
                pen = (Pen)savedPen.Clone();

                switch (drawList[i].CmdType)
                {
                    case DcCommand.CommandType.arc:
                        DcArc dArc = (DcArc)drawList[i];
                        PointF arcCenter = new PointF(dArc.centerX, dArc.centerY);
                        ptf1 = new PointF(dArc.X1, dArc.Y1);
                        ptf2 = new PointF(dArc.X2, dArc.Y2);
                        if (!dcColorSettings.blackAndWhite) pen.Color = dcColorSettings.linesColor;
                        DcDrawArc(gr, pen, arcCenter, ptf1, ptf2);
                        break;

                    case DcCommand.CommandType.bus:
                        DcBus dBus = (DcBus)drawList[i];
                        ptf1.X = Convert.ToSingle(dBus.X1);
                        ptf1.Y = Convert.ToSingle(dBus.Y1);
                        ptf2.X = Convert.ToSingle(dBus.X2);
                        ptf2.Y = Convert.ToSingle(dBus.Y2);
                        pen.Width = dBus.width;
                        gr.DrawLine(pen, ptf1, ptf2);
                        break;

                    case DcCommand.CommandType.circle:
                        DcCircle dCircle = (DcCircle)drawList[i];
                        ptf1.X = Convert.ToSingle(dCircle.X1);
                        ptf1.Y = Convert.ToSingle(dCircle.Y1);
                        ptf2.X = Convert.ToSingle(dCircle.X2);
                        ptf2.Y = Convert.ToSingle(dCircle.Y2);
                        float radius = (float)Math.Sqrt(Math.Pow(ptf1.X - ptf2.X, 2) + Math.Pow(ptf1.Y - ptf2.Y, 2));
                        float diameter = 2F * radius;
                        float center = ptf1.X;
                        if (!dcColorSettings.blackAndWhite) pen.Color = dcColorSettings.linesColor;
                        gr.DrawEllipse(pen, center - radius, ptf1.Y - radius, diameter, diameter);
                        break;

                    case DcCommand.CommandType.drawing:
                        break;

                    case DcCommand.CommandType.line:
                        DcLine dLine = (DcLine)drawList[i];
                        ptf1.X = Convert.ToSingle(dLine.X1);
                        ptf1.Y = Convert.ToSingle(dLine.Y1);
                        ptf2.X = Convert.ToSingle(dLine.X2);
                        ptf2.Y = Convert.ToSingle(dLine.Y2);
                        if (!dcColorSettings.blackAndWhite) pen.Color = dcColorSettings.linesColor;
                        gr.DrawLine(pen, ptf1, ptf2);
                        break;

                    case DcCommand.CommandType.module:
                        DcModule dModule = (DcModule)drawList[i];
                        int mIndex = dModule.internalLBRIndex;
                        // get drawlist for module from moduleList
                        List<DcCommand> mDrawList;
                        mDrawList = internalLBR[mIndex].drawList;
                        // draw the module at specified location with given scalefactor
                        float mScaleFactor = dModule.scaleFactor;
                        PointF mLocation = new PointF(dModule.X1, dModule.Y1);
                        savedPen = (Pen)pen.Clone();
                        PaintDcModule(gr, dModule);
                        break;

                    case DcCommand.CommandType.pin:
                        DcPin dPin = (DcPin)drawList[i];
                        // draw a small square to identify the pin location.
                        pen.Width = 0.5F;
                        if (!dcColorSettings.blackAndWhite) pen.Color = dcColorSettings.pinsColor;
                        if (dcColorSettings.showPins) gr.DrawRectangle(pen, dPin.X1 - 2, dPin.Y1 - 2, 4, 4);
                        break;

                    case DcCommand.CommandType.str:
                        break;

                    case DcCommand.CommandType.text:
                        DcText dct = (DcText)drawList[i];
                        if (!dcColorSettings.blackAndWhite) pen.Color = dcColorSettings.textColor;
                        DcDrawText(gr, pen, dct, moduleCommand.mirror == 1);
                        break;

                    case DcCommand.CommandType.wire:
                        DcWire dWire = (DcWire)drawList[i];
                        ptf1.X = Convert.ToSingle(dWire.X1);
                        ptf1.Y = Convert.ToSingle(dWire.Y1);
                        ptf2.X = Convert.ToSingle(dWire.X2);
                        ptf2.Y = Convert.ToSingle(dWire.Y2);
                        if (!dcColorSettings.blackAndWhite) pen.Color = dcColorSettings.wiresColor;
                        gr.DrawLine(pen, ptf1, ptf2);
                        break;

                    case DcCommand.CommandType.undefined:
                        break;

                    default:
                        break;
                }
                gr.Restore(grSaved);
            }
            pen.Dispose();
            gr.Restore(gsSaved);
        }

        // =======================================================
        //          DC TEXT BOUNDS
        // =======================================================
        private void DcTextBounds(DcText dct, DcBounds bounds)
        {
            FontFamily fontFamily = new FontFamily("MS GOTHIC");
            string text = dct.dcStr.strText.TrimStart('#');

            // Calculate font scale factor
            float fontSize = 10.5F * dct.scaleFactor / 0.039063F;
            Font the_font = new Font(fontFamily, fontSize);

            // Get the overall dimensions
            var textSize = TextRenderer.MeasureText(text, the_font);

            // Set the bounds relative to the 'center' of the font being at the font's baseline
            bounds.YMax = fontSize;
            bounds.YMin = fontSize - textSize.Height;
            bounds.XMin = 0;
            bounds.XMax = textSize.Width;
        }

        // =======================================================
        //           DC DRAW TEXT
        // =======================================================
        private void DcDrawText(Graphics gr, Pen pen, DcText dct, bool moduleMirrored)
        {
            FontFamily fontFamily = new FontFamily("MS GOTHIC");
            string text = dct.dcStr.strText.TrimStart('#');

            // Calculate font scale factor
            float fontSize = 10.5F * dct.scaleFactor / 0.039063F;
            Font the_font = new Font(fontFamily, fontSize);
            GraphicsState grSaved = gr.Save();

            if (moduleMirrored && dct.upright == 1)
            {
                gr.ScaleTransform(-1, 1);  // mirror X coordinates so text isn't backwards
                SizeF size = gr.MeasureString(text, the_font); // move origin by text length
                float x = size.Width - gr.MeasureString(" ", the_font).Width * 0.75F; 
                gr.TranslateTransform(-(dct.X1+x), dct.Y1);
            }
            else
            {
                gr.TranslateTransform(dct.X1, dct.Y1);
            }

            if (dct.mirror == 1 && dct.upright == 1)
            {
                SizeF size = gr.MeasureString(text, the_font); // move origin by text length
                float x = size.Width - gr.MeasureString(" ", the_font).Width * 0.75F;
                gr.TranslateTransform(-x, 0);
                gr.ScaleTransform(-1, -1);
            }

            if (dct.upright == 1) gr.ScaleTransform(1, -1); // undo Y flip

            gr.RotateTransform(-dct.rotation);
            gr.DrawString(text, the_font, pen.Brush, new PointF(0, 0 - fontSize));

            gr.Restore(grSaved);
        }

        // =======================================================
        //           DC DRAW ARC
        // =======================================================
        private static float DcDrawArc(Graphics gr, Pen pen, PointF centerPt, PointF p1, PointF p2)
        {
            // Calculate radius as distance from center to p1
            float radius = (float)Math.Sqrt((centerPt.X - p1.X) * (centerPt.X - p1.X) + (centerPt.Y - p1.Y) * (centerPt.Y - p1.Y));

            // Calculate rectangle for DrawArc
            float x = centerPt.X - radius;
            float y = centerPt.Y - radius;
            float width = 2F * radius;
            float height = 2F * radius;

            // Determine angles for DrawArc
            float startAngle2 = (float)((180 / Math.PI) * Math.Atan2(p1.Y - centerPt.Y, p1.X - centerPt.X));
            float endAngle2 = (float)((180 / Math.PI) * Math.Atan2(p2.Y - centerPt.Y, p2.X - centerPt.X));

            // Draw the arc
            if (endAngle2 - startAngle2 > 0)
            {
                gr.DrawArc(pen, x, y, width, height, startAngle2, endAngle2 - startAngle2);
            }
            else
            {
                gr.DrawArc(pen, x, y, width, height, startAngle2, startAngle2 - endAngle2);
            }
            return radius;
        }

        // =======================================================
        //           DC READ ASCII LINE
        // =======================================================
        private String DcReadASCIILine(FileStream file, ref long filePos)
        {
            file.Position = filePos;

            string str = "";
            byte ch;
            while ((file.Position != file.Length) && (ch = (byte)file.ReadByte()) != 0x0D)
            {
                if (ch == 0x1A)
                {
                    str += (char)0x1A;
                    break;
                }
                else
                {
                    str += Convert.ToChar(ch);
                }
            }
            //skip past linefeed, if any
            if (file.Position != file.Length)
            {
                ch = (byte)file.ReadByte();
                if (ch != 0x0A) file.Position -= 1;
            }

            filePos = file.Position;
            return str;
        }

        enum DrawListStatus { OK, FileNotFound, Empty };  // TBD: Make a drawlist class?

        // =======================================================
        //           DC MAKE DRAW LIST FROM FILE
        // =======================================================
        private DrawListStatus DcMakeDrawListFromFile(ref InternalLBREntry internalLBREntry, long startPos, int drawListSize)
        {
            string fname = internalLBREntry.fileName;
            DcCommand.CommandType prevCommandType = DcCommand.CommandType.undefined;
            DcModule parentModuleCommand = new DcModule();
            string line;
            FileStream file;
            long endPos = 0;

            internalLBREntry.stats = new DrawListStats();  // clears all module stats

            if (File.Exists(fname))
            {
                file = new FileStream(fname, FileMode.Open, FileAccess.Read);
            }
            else
            {
                internalLBREntry.drawList.Clear();
                return DrawListStatus.FileNotFound;
            }

            file.Position = startPos;
            endPos = startPos + drawListSize;

            for (int i = 0; i < 20000; i++) // 20,000 line limit.  TBD: This is pretty bad form. Really should
                                            // add an 'abort'/cancel function in case someone loads a huge file
                                            // that might be mistaken for a DMEE file. Maybe do this all in
                                            // a separate thread?
            {
                if (file.Position == file.Length) break;  // reached end of file
                if (endPos > 0 && file.Position >= endPos) break; // reached end of space reserved for drawlist in catalog

                long filePos = file.Position;

                line = DcReadASCIILine(file, ref filePos);

                if (line != null)
                {
                    ParseDcCommandLine(ref prevCommandType, ref line, ref internalLBREntry, parentModuleCommand);
                }
                else break;
            }

            Invalidate();
            file.Close();

            if (internalLBREntry.drawList.Count == 0)
            {
                // set dummy bounds for empty drawing file
                internalLBREntry.bounds.XMin = 0F; internalLBREntry.bounds.YMin = 0F;
                internalLBREntry.bounds.XMax = 0F; internalLBREntry.bounds.YMax = 0F;
                return DrawListStatus.Empty;
            }
            return DrawListStatus.OK;
        }

        // =======================================================
        //           PARSE DC COMMAND LINE
        // =======================================================
        private DcCommand.CommandType ParseDcCommandLine(ref DcCommand.CommandType prevCommandType, ref string line,
                                              ref InternalLBREntry internalLBREntry, DcModule parentModuleCommand)
        {
            DcCommand.CommandType commandType = DcCommand.CommandType.undefined;         
            string[] fields;
            string fieldStr = "";
            string rawLine = line;
            List<DcCommand> drawList = internalLBREntry.drawList;
            DrawListStats drawListStats = internalLBREntry.stats;
            DcBounds drawListBounds = internalLBREntry.bounds;

            // extract comment / string field from the line, if any
            int strIndex = line.IndexOf('#');
            if (strIndex >= 0)
            {
                fieldStr = line.Substring(strIndex);
                line = line.Substring(0, strIndex);
            }

            // get rid of excess <space> characters so single space can be consistently treated as a delimiter
            // for splitting the remainder of the string into individual fields
            line = line.TrimEnd(' ');
            while (line.Contains("  ")) line = line.Replace("  ", " ");  // replace two spaces with single space until all are single

            fields = line.Split(' '); // split the remainder of the line into fields

            switch (fields[0])
            {
                case "a": commandType = DcCommand.CommandType.arc; break;
                case "b": commandType = DcCommand.CommandType.bus; break;
                case "c": commandType = DcCommand.CommandType.circle; break;
                case "d": commandType = DcCommand.CommandType.drawing; break;
                case "s": commandType = DcCommand.CommandType.str; break;
                case "t": commandType = DcCommand.CommandType.text; break;
                case "l": commandType = DcCommand.CommandType.line; break;
                case "m": commandType = DcCommand.CommandType.module; break;
                case "p": commandType = DcCommand.CommandType.pin; break;
                case "r": commandType = DcCommand.CommandType.route; break;
                case "w": commandType = DcCommand.CommandType.wire; break;
                default: commandType = DcCommand.CommandType.undefined; break;
            }

            switch (commandType)  // create a record object for line and add it to draw list
            {
                case DcCommand.CommandType.arc:
                    DcArc dArc = new DcArc()
                    {
                        layer = Convert.ToInt16(fields[1]),
                        centerX = Convert.ToSingle(fields[2]),
                        centerY = Convert.ToSingle(fields[3]),
                        X1 = Convert.ToSingle(fields[4]),
                        Y1 = Convert.ToSingle(fields[5]),
                        X2 = Convert.ToSingle(fields[6]),
                        Y2 = Convert.ToSingle(fields[7])
                    };

                    DcBounds arcBounds = new DcBounds();
                    ArcBounds(dArc, arcBounds);

                    UpdateMinMaxBounds(arcBounds.XMax, arcBounds.YMax, ref drawListBounds);
                    UpdateMinMaxBounds(arcBounds.XMin, arcBounds.YMin, ref drawListBounds);
                    drawList.Add(dArc);
                    break;

                case DcCommand.CommandType.bus:
                    DcBus dBus = new DcBus()
                    {
                        layer = Convert.ToInt16(fields[1]),
                        X1 = Convert.ToSingle(fields[2]),
                        Y1 = Convert.ToSingle(fields[3]),
                        X2 = Convert.ToSingle(fields[4]),
                        Y2 = Convert.ToSingle(fields[5]),
                        width = Convert.ToSingle(fields[7])
                    };
                    UpdateMinMaxBounds(dBus.X1, dBus.Y1, ref drawListBounds);
                    UpdateMinMaxBounds(dBus.X2, dBus.Y2, ref drawListBounds);
                    drawList.Add(dBus);
                    break;

                case DcCommand.CommandType.circle:
                    DcCircle dCircle = new DcCircle
                    {
                        layer = Convert.ToInt16(fields[1]),
                        X1 = Convert.ToSingle(fields[2]),   // center X
                        Y1 = Convert.ToSingle(fields[3]),   // center Y
                        X2 = Convert.ToSingle(fields[4]),   // point on circle, X
                        Y2 = Convert.ToSingle(fields[5])    // point on circle, Y
                    };
                    if (fields.Length > 6) dCircle.unk6 = Convert.ToSingle(fields[6]);
                    // bounds of square holding circle
                    float radius = (float)Math.Sqrt(Math.Pow(dCircle.X1 - dCircle.X2, 2) + Math.Pow(dCircle.Y1 - dCircle.Y2, 2));
                    UpdateMinMaxBounds(dCircle.X1-radius, dCircle.Y1-radius, ref drawListBounds);
                    UpdateMinMaxBounds(dCircle.X1+radius, dCircle.Y1+radius, ref drawListBounds);
                    drawList.Add(dCircle);
                    break;

                case DcCommand.CommandType.drawing:
                    DcDrawing dcDrawing = new DcDrawing()
                    {
                        version = Convert.ToSingle(fields[1]),
                        X1 = Convert.ToSingle(fields[3]),
                        Y1 = Convert.ToSingle(fields[4]),
                        scaleFactor = Convert.ToSingle(fields[5])
                    };
                    internalLBREntry.stats.drawingItemCount++;
                    drawList.Add(dcDrawing);
                    break;

                case DcCommand.CommandType.line:
                    DcLine dcLine = new DcLine
                    {
                        layer = Convert.ToInt16(fields[1]),
                        X1 = Convert.ToSingle(fields[2]),
                        Y1 = Convert.ToSingle(fields[3]),
                        X2 = Convert.ToSingle(fields[4]),
                        Y2 = Convert.ToSingle(fields[5])
                    };
                    if (fields.Length > 6) dcLine.unk6 = Convert.ToSingle(fields[6]);
                    if (fields.Length > 7) dcLine.unk7 = Convert.ToSingle(fields[7]);
                    UpdateMinMaxBounds(dcLine.X1, dcLine.Y1, ref drawListBounds);
                    UpdateMinMaxBounds(dcLine.X2, dcLine.Y2, ref drawListBounds);
                    drawList.Add(dcLine);
                    break;

                case DcCommand.CommandType.module:
                    DcModule dcModule = new DcModule()  // CREATE COMMAND OBJECT FOR MODULE
                    {
                        layer = Convert.ToInt16(fields[1]),
                        X1 = Convert.ToSingle(fields[2]),
                        Y1 = Convert.ToSingle(fields[3]),
                        scaleFactor = Convert.ToSingle(fields[4]),
                        rotation = Convert.ToSingle(fields[5]),
                        mirror = Convert.ToInt16(fields[6]),
                        name = Convert.ToString(fields[7]),
                    };
                    // skip modules with color value >= 50. Those are "pad" module commands that
                    // have a different field syntax than 'drawing' module commands.
                    if (dcModule.layer >= 50) break;
                    if (fields.Length > 8) dcModule.unk8 = Convert.ToInt16(fields[8]);
                    if (fields.Length > 9) dcModule.unk9 = Convert.ToInt16(fields[9]);
                    if (fields.Length > 10) dcModule.unk10 = Convert.ToInt16(fields[10]);
                    if (fields.Length > 11) dcModule.unk11 = Convert.ToInt16(fields[11]);
                    if (fields.Length > 12) dcModule.unk12 = Convert.ToInt16(fields[12]);

                    internalLBREntry.stats.moduleItemCount++;
                    UpdateMinMaxBounds(dcModule.X1, dcModule.Y1, ref drawListBounds);

                    // Search to see if an entry for the module is already in the module list.
                    InternalLBREntry result = internalLBR.Find(x => x.name == dcModule.name);
                    if (result != null)  // existing entry found. Link module command to existing entry in module list
                    {
                        dcModule.internalLBRIndex = internalLBR.IndexOf(result);  // set module command to point to entry
                    }
                    else  // create and add a new internal library entry then point the module command to it
                    {
                        InternalLBREntry intLibEntry = new InternalLBREntry() // create entry that will hold drawlist for module
                        {
                            name = dcModule.name,
                            fromLibrary = false,
                            fileName = "",
                        };
                        internalLBR.Add(intLibEntry);     // add new entry to module list
                        dcModule.internalLBRIndex = internalLBR.IndexOf(intLibEntry);     // set module command to point to new entry
                    }
                    drawList.Add(dcModule);   // add module command to present drawlist
                    break;

                case DcCommand.CommandType.pin:
                    DcPin dcPin = new DcPin()
                    {
                        layer = Convert.ToInt16(fields[1]),
                        X1 = Convert.ToSingle(fields[2]),
                        Y1 = Convert.ToSingle(fields[3])
                    };
                    UpdateMinMaxBounds(dcPin.X1, dcPin.Y1, ref drawListBounds);
                    drawList.Add(dcPin);
                    internalLBREntry.stats.pinItemCount++;
                    break;

                case DcCommand.CommandType.str:
                    DcString dcStr = new DcString
                    {
                        unk1 = Convert.ToInt16(fields[1]),
                        unk2 = Convert.ToInt16(fields[2]),
                        strText = fieldStr
                    };
                    // I discovered some string commands only have two leading fields. E.g. "s 1 0 #TYPE"
                    // while others have three E.g. "s 1 0 1 #CA31:2
                    if (fields.Length > 3) dcStr.unk3 = Convert.ToInt16(fields[3]);

                    internalLBREntry.stats.strItemCount++;
                    if (drawList.Count == 0) break; // handle error condition of possible s command at start of file

                    // Set String for previous command
                    switch(prevCommandType)
                    {
                        case DcCommand.CommandType.text:
                            DcText dText = (DcText)drawList[drawList.Count - 1];
                            dText.dcStr = dcStr;
                            DcBounds srcBounds = new DcBounds();
                            DcBounds dstBounds = dText.textBounds;
                            DcTextBounds(dText, srcBounds);
                            // Set scalefactor to 1.0F because the text was scaled when calculating the
                            // bounds using DcTextBounds
                            TransformBounds(srcBounds, dstBounds, dText.X1, dText.Y1, dText.rotation, 1.0F);
                            UpdateMinMaxBounds(dText.textBounds.XMax, dText.textBounds.YMax, ref drawListBounds);
                            UpdateMinMaxBounds(dText.textBounds.XMin, dText.textBounds.YMin, ref drawListBounds);
                            break;

                        case DcCommand.CommandType.wire:
                            DcWire dcw = (DcWire)drawList[drawList.Count - 1];
                            dcw.dcStr = dcStr;
                            break;

                        case DcCommand.CommandType.drawing:
                            DcDrawing dcd = (DcDrawing)drawList[drawList.Count - 1];
                            dcd.dcStr = dcStr;
                            break;

                        default:
                            break;
                    }
                    break; // end of processing for str (s) command

                case DcCommand.CommandType.text:
                    DcText dcText = new DcText
                    {
                        layer = Convert.ToInt16(fields[1]),
                        X1 = Convert.ToSingle(fields[2]),
                        Y1 = Convert.ToSingle(fields[3]),
                        scaleFactor = Convert.ToSingle(fields[4]),
                        rotation = Convert.ToSingle(fields[5]),
                        mirror = Convert.ToInt16(fields[6]),
                        upright = Convert.ToInt16(fields[7])
                    };
                    // Set initial text bounds. Drawing bounds will be updated if a string gets attached to the module.
                    UpdateMinMaxBounds(dcText.X1, dcText.Y1, ref drawListBounds);
                    internalLBREntry.stats.textItemCount++;
                    drawList.Add(dcText);
                    if (!internalLBREntry.stats.textScalingList.Contains(dcText.scaleFactor))
                    {
                        internalLBREntry.stats.textScalingList.Add(dcText.scaleFactor);
                    }
                    break;

                case DcCommand.CommandType.wire:
                    DcWire dcWire = new DcWire
                    {
                        layer = Convert.ToInt16(fields[1]),
                        X1 = Convert.ToSingle(fields[2]),
                        Y1 = Convert.ToSingle(fields[3]),
                        X2 = Convert.ToSingle(fields[4]),
                        Y2 = Convert.ToSingle(fields[5]),
                        unk1 = Convert.ToInt16(fields[6]),
                        unk2 = Convert.ToInt16(fields[7])
                    };
                    if (fields.Length > 8) dcWire.net = Convert.ToInt16(fields[8]);
                    if (fields.Length > 9) dcWire.unk3 = Convert.ToInt16(fields[9]);
                    UpdateMinMaxBounds(dcWire.X1, dcWire.Y1, ref drawListBounds);
                    UpdateMinMaxBounds(dcWire.X2, dcWire.Y2, ref drawListBounds);
                    drawList.Add(dcWire);
                    break;

                default: break;
            }
            prevCommandType = commandType;
            return commandType;
        }

        // =======================================================
        //           CALCULATE ARC BOUNDS
        // =======================================================
        private static void ArcBounds(DcArc dArc, DcBounds bounds)
        {
            float X1 = dArc.X1, Y1 = dArc.Y1, X2 = dArc.X2, Y2 = dArc.Y2;
            float centerX = dArc.centerX, centerY = dArc.centerY;
            float XMax = 0, YMax = 0, XMin = 0, YMin = 0;
            bool xFlipped = false;
            bool yFlipped = false;

            // Algorithm:
            // set P1, P2 = (dArc.X1, dArc.Y1) (dArc.X2, dArc.Y2), respectively
            // translate arc coordinates to put center at 0,0.
            // transform coordinates to put P1 into first quadrant.
            // set min,max based on which quadrant P2 lands after transformation
            // transform back
            // make sure XMin, XMax, YMin, YMax values are in correct magnitude order
            // translate coordinates back based on original location of center
            // assign to bounds and return

            // translate points to put center at 0,0
            X1 -= dArc.centerX; Y1 -= dArc.centerY; X2 -= dArc.centerX; Y2 -= dArc.centerY;
            centerX = 0; centerY = 0;

            float radius = (float)Math.Sqrt(Math.Pow(X1 - 0, 2) + Math.Pow(Y1 - 0, 2));

            // Transform coordinates to move P1 to quadrant 1 (Q1)
            if (X1 < 0) // P1 is in Q2 or Q3;
            {
                X1 *= -1; X2 *= -1; // flip X coordinates
                xFlipped = true;
            }
            if (Y1 < 0) // P1 is in Q3 or Q4;
            {
                Y1 *= -1; Y2 *= -1; // flip Y coordinates
                yFlipped = true;
            }

            // P1 should now be in Q1.
            // Process according to which quadrant holds transformed P2.
            if (X2 >= centerX && Y2 >= centerY) // P2 is in Q1
            {
                XMax = X1; XMin = X2; YMax = Y2; YMin = Y1;
            }
            if (X2 < 0 && Y2 >= 0) // P2 is in Q2
            {
                XMax = X1; XMin = X2; YMax = radius;
            }
            if (X2 < 0 && Y2 < 0 ) // P2 is in Q3
            {
                XMax = X1; XMin = - radius; YMax = radius; YMin = Y2;
            }
            if (X2 >= 0 && Y2 < 0) // P2 is in Q4
            {
                XMax = X1; XMin = -radius; YMax = radius; YMin = -radius;
                if (X2 > X1) XMax = X2;
            }

            // Transform coordinates back based. 
            // Having a flipped Y is like a 180 degree rotation of the arc around the X-axis
            // that flips the X direction of the arc relative to the Y axis.
            // Thus, the X direction needs to be flipped back.
            if (yFlipped) 
            {
                XMax *= -1; XMin *= -1;
            }
            // Similarly, flipping X is like a 180 degree rotation around Y
            if (xFlipped)
            {
                YMax *= -1; YMin *= -1;
            }

            // make sure bounds are in correct order
            if (XMax < XMin)
            {
                float temp = XMax;
                XMax = XMin; XMin = temp;
            }
            if (YMax < YMin)
            {
                float temp = YMax;
                YMax = YMin; YMin = temp;
            }

            // translate points back
            XMax += dArc.centerX; YMax += dArc.centerY; XMin += dArc.centerX; YMin += dArc.centerY;
            bounds.XMax = XMax; bounds.YMax = YMax; bounds.XMin = XMin; bounds.YMin = YMin;
        }

        // =======================================================
        //           UPDATE MIN AND MAX FOR BOUNDS
        // =======================================================
        private void UpdateMinMaxBounds(float X, float Y, ref DcBounds bounds)
        {
            if (X > bounds.XMax) bounds.XMax = X;
            if (X < bounds.XMin) bounds.XMin = X;
            if (Y > bounds.YMax) bounds.YMax = Y;
            if (Y < bounds.YMin) bounds.YMin = Y;
        }

        // =======================================================
        //           DC Load CATALOG FROM FILE
        // =======================================================
        private int DcLoadCatalog(FileStream inFile, DcExternalLBRCatalog catalog)
        {
            Byte[] buffer = new Byte[100];
            String str = "";
            int entryCount = 0;
            int recordStartOffset = 0;
            int recordSizeInBytes = 0;

            catalog.entries.Clear();

            inFile.Read(buffer, 0, 32);

            // Assumes that two-byte binary values in the catalog portion of the library are 16-bit and little endian.
            // All records in the library start and end on 128-byte boundaries, including the catalog itself.
            // buffer[14] and [15] are the file offset (in multiples of 128 bytes) to the NEXT record in the library.
            // The first occurrence of the offset thus corresponds to the amount of space taken by the catalog itself.
            int recordsStart = 128 * Convert.ToInt16(buffer[14] + buffer[15] * 256);

            for (int i = 32; i < recordsStart; i += 32)
            {
                ExtLBRCatEntry entry = new ExtLBRCatEntry();
                long filePos = inFile.Position;
                inFile.Read(buffer, 0, 32);
                if (buffer[0] == 0xFF) break; // no more catalog entries

                recordStartOffset = 128 * Convert.ToInt16(buffer[12] + buffer[13] * 256);
                recordSizeInBytes = 128 * Convert.ToInt16(buffer[14] + buffer[15] * 256);

                str = Encoding.ASCII.GetString(buffer, 1, 11);
                entry.name = str;
                entry.extension = str.Substring(8, 3).TrimEnd(' '); // extension is last 3 alphanumeric characters, if any.
                entry.recordOffset = recordStartOffset;
                entry.recordSize = recordSizeInBytes;
                entry.filePos = filePos;
                catalog.entries.Add(entry);

                entryCount++;
            }
            catalog.fileName = inFile.Name;
            catalog.size = recordsStart;
            catalog.entryCount = entryCount;
            return entryCount;
        }


        // =======================================================
        //           FIND MODULE IN CATALOG
        // =======================================================
        // search loaded catalog's entries for name (case insensitive) starting from startIndex
        // in catalog and returning index of entry if match found.
        // Return -1 if match not found. Setting startIndex to 0 will return index of first match.
        private int DcFindModuleInCatalog(DcExternalLBRCatalog catalog, String moduleName, int startIndex,
                                        DcExternalLBRCatalog.MatchType matchType)
        {
            int index;
            bool found = false;
            moduleName = moduleName.ToUpper();  // do all comparisons in uppercase

            for (index = startIndex; index < catalog.entries.Count; index++)
            {
                string name = catalog.entries[index].name.ToUpper();
                if (matchType == DcExternalLBRCatalog.MatchType.full && name == moduleName)
                {
                    found = true;
                    break;
                }
                else if (matchType == DcExternalLBRCatalog.MatchType.partial && name.Contains(moduleName))
                {
                    found = true;
                    break;
                }
            }
            if (!found) return -1;
            return index;
        }


        // =======================================================
        //           HANDLE FILE MENU CLICK TO OPEN A DRAWING FILE
        // =======================================================
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fullName = Properties.Settings.Default.fileName;
            string fName = "";
            string fDir = "";

            if (fullName != @"\")
            {
                fName = fullName.Substring(fullName.LastIndexOf(@"\") + 1);
                fDir = fullName.Substring(0, fullName.LastIndexOf(@"\"));
            }
            else
            {
                fullName = "";
            }

            TopFileNameTextBox.Text = fullName;
            TopFileNameTextBox.Update();

            openFileDialog1.InitialDirectory = fDir;
            openFileDialog1.FileName = fName;
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                topFileName = openFileDialog1.FileName;
                TopFileNameTextBox.Text = topFileName;
                TopFileNameTextBox.Update();
                Properties.Settings.Default.fileName = topFileName;
                DcLoadDrawing();

                // Set size of Picture Box for screen painting. Dimensions need to cover a non-zero area for paint event to occur.
                DrawPictureBox.Height = (int)((topModuleCommand.bounds.YMax - topModuleCommand.bounds.YMin) * ZoomFactor + 30);
                DrawPictureBox.Width = (int)((topModuleCommand.bounds.XMax - topModuleCommand.bounds.XMin) * ZoomFactor + 30);
                DrawPictureBox.Invalidate();
            }
            menuStrip1.Select();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FoldersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderConfigForm.ShowDialog();
            libraryFolder = folderConfigForm.libraryFolder;
            workingFolder = folderConfigForm.workingFolder;
        }

        private void ToolStripMenuZoom25_Click(object sender, EventArgs e)
        {
            ZoomIt(0.25F);
        }

        private void ToolStripMenuZoom50_Click(object sender, EventArgs e)
        {
            ZoomIt(0.50F);
        }

        private void ToolStripMenuZoom100_Click(object sender, EventArgs e)
        {
            ZoomIt(1.0F);
        }

        private void ToolStripMenuZoom150_Click(object sender, EventArgs e)
        {
            ZoomIt(1.5F);
        }

        private void ToolStripMenuZoom200_Click(object sender, EventArgs e)
        {
            ZoomIt(2.0F);
        }

        private void ZoomIt(float factor)
        {
            ZoomFactor = factor;
            fitToWindow = false;
            Invalidate();
        }

        // =======================================================
        //           DRAW FILE BUTTON CLICK
        // =======================================================
        private void DrawFileButton_Click(object sender, EventArgs e)
        {
            drawingLoaded = false;
            DcLoadDrawing();

            // Set a default size of Picture Box for screen painting.
            // (Dimensions need to cover a non-zero area for paint event to occur).
            DrawPictureBox.Height = (int)((topModuleCommand.bounds.YMax - topModuleCommand.bounds.YMin) * ZoomFactor + 30);
            DrawPictureBox.Width = (int)((topModuleCommand.bounds.XMax - topModuleCommand.bounds.XMin) * ZoomFactor + 30);
            DrawPictureBox.Invalidate();  // Paint event for picture box handles actual drawing
        }


        // =======================================================
        //           DC LOAD DRAWING
        // Load the top drawing file and create internal library
        // entries for all referenced modules and sub-modules in
        // the drawing.
        // =======================================================
        private void DcLoadDrawing()
        {
            InitializeForLoad();

            InternalLBREntry internalLBREntry = internalLBR[0];

            // At first the top module is the only entry in the internal library. However, as
            // the drawlist for the module is processed, more modules are added.
            DrawListStatus status = DcMakeDrawListFromFile(ref internalLBREntry, 0, 0);  // load drawlist for top module
            if (status == DrawListStatus.FileNotFound)
            {
                topFileName = "";
                TopFileNameTextBox.Text = "";
                TopFileNameTextBox.Update();
                Properties.Settings.Default.fileName = "";
                MessageBox.Show("Please select another file using Open under File menu", "File not found");
                return;
            }

            // Load the internal library (internalLBR) list with all modules used in the drawing along with their drawlists.
            // This also creates a list (moduleList) of all the instances of module commands used in the drawing.
            LoadInternalLibrary();

            // --------------------- BOUNDS PROCESSING -----------------------------------
            // Process bounds for modules in the internal library and topModuleCommand
            // ---------------------------------------------------------------------------
            BoundsProcessInternalLibraryEntries();

            ModuleInfoTextBox(internalLBR[0]);
            WriteDebugDataToConsole();

            if (status == DrawListStatus.Empty)
            {
                MessageBox.Show("File does not contain any drawing commands", "Empty File");
                return;
            }

            drawingLoaded = true;
        }


        // =======================================================
        //          BOUNDS PROCESS INTERNAL LIBRARY COMPOUND MODULES
        // =======================================================
        //HANDLE COMPOUND MODULES THAT MAY ALSO CONTAIN COMPOUND MODULES:
        // For each entry in the internal library, scan all child modules in its drawlist. Update the library entry's
        // bounds for each child module that is already marked as bounds processed.Mark the parent module
        // as boundsProcessed if no unprocessed child modules are detected. Since child modules may themselves be
        // compound modules this may not happen on the first pass through the list. 
        // Thus, repeat scanning and processing the moduleList until no unprocessed modules found or scanDepth limit reached.
        private void BoundsProcessInternalLibraryEntries()
        {
            // --------
            // PROCESS SIMPLE MODULES in the internal library.
            // These modules won't need more bounds processing since the processing was done
            // when the drawlist was created based on the bounds of the lines, arcs, circles, etc. in the drawList.
            foreach (InternalLBREntry ile in internalLBR)
            {
                if (ile.stats.moduleItemCount == 0) ile.bounds.boundsProcessed = true;
            }
            const int scanDepth = 10;   // Maximum number of passes through moduleList. TBD: Make this a configuration option ???
            for (int i = 1; i < scanDepth; i++)
            {
                bool foundUnprocessedModules = false;
                foreach (InternalLBREntry ile in internalLBR)
                {
                    if (!ile.bounds.boundsProcessed) // unprocessed modules still in the list
                    {
                        foundUnprocessedModules = true;
                        int mdlCount = 0; int subProcessedCount = 0;
                        List<DcCommand> ileDrawList = ile.drawList; // get drawlist for this module

                        // scan drawlist for module commands with processed bounds and, if found,
                        // update parent's bounds accordingly
                        foreach (DcCommand cmd in ileDrawList)
                        {
                            if (cmd.CmdType == DcCommand.CommandType.module)
                            {
                                DcModule mCmd = (DcModule)cmd;
                                InternalLBREntry mLBR = internalLBR[mCmd.internalLBRIndex];
                                mdlCount++; // count the number of child modules in the drawList
                                if (mLBR.bounds.boundsProcessed)  // update parent's bounds based on bounds for child in internal library
                                {
                                    DcBounds mCmdBounds = mCmd.bounds;
                                    DcBounds mLBRBounds = mLBR.bounds;

                                    // Transform child's bounds for this module command
                                    TransformBounds(mLBRBounds, mCmdBounds, mCmd.X1, mCmd.Y1, mCmd.rotation, mCmd.scaleFactor);

                                    // Update bounds for the drawList
                                    UpdateMinMaxBounds(mCmdBounds.XMax, mCmdBounds.YMax, ref ile.bounds);
                                    UpdateMinMaxBounds(mCmdBounds.XMin, mCmdBounds.YMin, ref ile.bounds);
                                    mCmdBounds.boundsProcessed = true;
                                    subProcessedCount++; // count the number of child modules that have had their bounds processed
                                }
                            }
                        }
                        if (subProcessedCount == mdlCount)  // no unprocessed sub modules left in this module instance
                        {
                            ile.bounds.boundsProcessed = true;
                        }
                    }
                }
                if (!foundUnprocessedModules) break;  // all modules instances in list have had their bounds processed
            }
            // copy drawlist and bounds to top module
            topModuleCommand.bounds = internalLBR[0].bounds;
        }


        // ==================================================================
        //           TRANSFORM BOUNDS
        // Update destination bounds by transforming source bounds
        // using destination location, rotation, and scale values
        // ==================================================================
        private static void TransformBounds(DcBounds srcBounds, DcBounds dstBounds,
                                               float dstX, float dstY, float dstRot, float dstScale)
        {
            PointF[] pts = new PointF[]
            {
                        new PointF(srcBounds.XMax, srcBounds.YMax),
                        new PointF(srcBounds.XMin, srcBounds.YMin)
            };
            Matrix matrix = new Matrix();

            // Create destination bounds by transforming source bounds using location, rotation, and scale values
            matrix.Translate(dstX, dstY);
            matrix.Rotate(dstRot);
            matrix.Scale(dstScale, dstScale);
            matrix.TransformPoints(pts);
            for (int i = 0; i < pts.Length; i++)  // Round results
            {
                pts[i].X = (float)Math.Round(pts[i].X, 5);
                pts[i].Y = (float)Math.Round(pts[i].Y, 5);
            }
            dstBounds.XMax = pts[0].X;
            dstBounds.YMax = pts[0].Y;
            dstBounds.XMin = pts[1].X;
            dstBounds.YMin = pts[1].Y;
        }

        // =======================================================
        //          LOAD INTERNAL LIBRARY FROM EXTERNAL FILES
        // =======================================================
        private void LoadInternalLibrary()
        {
            // Only one pass is needed since unprocessed modules are added to the end of the list
            // and will never appear before a processed module
            foreach (InternalLBREntry ile in internalLBR)
            {
                var internalLBREntry = ile;

                // first check for name in working directory
                string[] s = Directory.GetFiles(workingFolder, internalLBREntry.name);
                if (s.Length > 0)  // file name found in working folder
                {
                    internalLBREntry.fromLibrary = false;
                    internalLBREntry.fileName = s[0];
                    // More library entries will be added if found while loading the drawlist
                    DcMakeDrawListFromFile(ref internalLBREntry, 0, 0);
                }
                else  // else check for name among catalogs in external catalog list
                {
                    // convert name to 11 character format used in .LBR files
                    string name = MakeDcName(internalLBREntry.name);

                    int index = -1;
                    foreach (DcExternalLBRCatalog cat in externalLBRCatalogs)
                    {
                        index = DcFindModuleInCatalog(cat, name, 0, DcExternalLBRCatalog.MatchType.full);
                        if (index >= 0) // load draw commands from file if module found
                        {
                            internalLBREntry.fromLibrary = true;
                            internalLBREntry.fileName = cat.fileName;
                            ExtLBRCatEntry entry = cat.entries[index];
                            // More library entries will be added if found while loading the drawlist
                            DcMakeDrawListFromFile(ref internalLBREntry, entry.recordOffset, entry.recordSize);
                            break;  // break out of loop if module found
                        }
                    }
                    if (index < 0) // handle missing module
                    {
                        Console.WriteLine("Module \"" + ile.name + "\" not found in directory or libraries. ");
                        // set the bounds to all 0's for empty drawlist
                        ile.bounds.XMax = ile.bounds.YMax = 0;
                        ile.bounds.XMin = ile.bounds.YMin = 0;
                    }
                }
            }
        }

        private void WriteDebugDataToConsole()
        {
            Console.WriteLine("Internal Library: =================================");
            foreach (InternalLBREntry ile in internalLBR)
            {
                Console.Write("moduleList: " + ile.name + " \t");
                if (ile.name.Length < 7) Console.Write("\t");
                if (ile.name.Length < 3) Console.Write("\t");
                Console.Write("sub module count: " + ile.stats.moduleItemCount + " ");
                Console.Write("XMax: " + ile.bounds.XMax + " YMax: " + ile.bounds.YMax);
                Console.Write(" XMin: " + ile.bounds.XMin + " YMin: " + ile.bounds.YMin);
                Console.WriteLine(" Bounds processed: " + ile.bounds.boundsProcessed);
            }
        }

        private static string MakeDcName(string name)
        {
            // This routine converts an input 8.3 name to the
            // 11 character format used by the catalogs in the external
            // libraries. E.g.  FOOBAR.MA becomes "FOOBAR<sp><sp>MA<sp>"
            int n = name.IndexOf('.');
            if (n > 0)
            {
                string ext = name.Substring(n + 1);
                name = name.Substring(0, name.IndexOf('.'));
                name = name.PadRight(8, ' '); // pad to 8 characters
                name += ext;
            }
            name = name.PadRight(11, ' '); // pad to 11 total characters
            return name;
        }

        private void InitializeForLoad()
        {
            // create and initialize entry for top module in internal library
            internalLBR.Clear();
            InternalLBREntry internalLBREntry = new InternalLBREntry
            {
                fileName = topFileName,
                stats = new DrawListStats()
            };
            internalLBREntry.name = topFileName.Substring(topFileName.LastIndexOf("\\") + 1);
            internalLBREntry.fromLibrary = false;
            internalLBR.Add(internalLBREntry);

            // create entry for top module in module instance list
            topModuleCommand = new DcModule
            {
                name = internalLBREntry.name,
                internalLBRIndex = 0
            };

            // read catalogs from each library (.LBR) file in library folder
            if (Directory.Exists(libraryFolder))
            {
                externalLBRCatalogs.Clear();
                int entryCount = 0;
                string[] libFiles = Directory.GetFiles(libraryFolder, "*.lbr");
                foreach (string fName in libFiles)
                {
                    FileStream libFile = new FileStream(fName, FileMode.Open, FileAccess.Read);
                    DcExternalLBRCatalog catalog = new DcExternalLBRCatalog();
                    entryCount = DcLoadCatalog(libFile, catalog);
                    externalLBRCatalogs.Add(catalog);
                    libFile.Close();
                }
            }
        }


        // ==================================================================
        //           PRINT PAGE EVENT HANDLER
        // The PrintPage event is raised for each page to be printed
        // ================================================================== 
        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Console.WriteLine("Print Page");
            Graphics gr = e.Graphics;
            // set clip for printing to the margin bounds that were set up for the page
            gr.Clip = new Region(e.MarginBounds);
            
            if (!drawingLoaded)
            {
                e.HasMorePages = false;
                return;
            }

            DcBounds dBounds = topModuleCommand.bounds;
            bool centerPrint = true;  // TBD: center document horizontally & vertically within margins
            Matrix matrix = new Matrix();

            // Figure out a scale factor to fit drawing within the page margins
            float dWidth = (topModuleCommand.bounds.XMax - topModuleCommand.bounds.XMin);
            float dHeight = (topModuleCommand.bounds.YMax - topModuleCommand.bounds.YMin);
            float scale = e.MarginBounds.Width / dWidth;
            if (scale > e.MarginBounds.Height / dHeight)  // choose smallest scale factor
            {
                scale = e.MarginBounds.Height/ dHeight ;
            }

            // shift print origin based on page margin
            // FYI: Theres also a printdocument property for "OriginAtMargins" but I'm doing it manually here
            gr.TranslateTransform(e.MarginBounds.Left, e.MarginBounds.Top);
            // scale the graphics to the printer by scale factor
            gr.ScaleTransform(scale, scale);
            // shift location of print origin based on drawing bounds
            gr.TranslateTransform(-dBounds.XMin, dBounds.YMin+dHeight);
            // FLIP Y COORDINATES
            gr.ScaleTransform(1, -1);

            // center drawing on page
            if (centerPrint)
            {
                float vDiff = (float)Math.Round((e.MarginBounds.Height / scale - dHeight) / 2.0F, 1);
                float hDiff = (float)Math.Round((e.MarginBounds.Width / scale - dWidth) / 2.0F, 1);
                gr.TranslateTransform(0F, -vDiff);
                gr.TranslateTransform(hDiff, 0);
            }

            //gr.DrawRectangle(new Pen(Color.LightGray), dBounds.XMin, dBounds.YMin, dWidth, dHeight);
            PaintDcModule(gr, topModuleCommand);

            // If more pages exist, print another page.
            e.HasMorePages = false;
        }

        private void ToolStripMenuPrint_Click(object sender, EventArgs e)
        {
            if (!drawingLoaded)
            {
                MessageBox.Show("Need to open or draw a file before printing", "Nothing to print");
                return;
            }
            DialogResult result = printDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        private void ToolStripMenuPageSetup_Click(object sender, EventArgs e)
        {
            pageSetupDialog1.PageSettings = printDocument.DefaultPageSettings;
            var result = pageSetupDialog1.ShowDialog();
            if (result == DialogResult.OK)  // set properties for later restoration
            {
                Properties.Settings.Default.margins = printDocument.DefaultPageSettings.Margins;
                Properties.Settings.Default.PSize = printDocument.DefaultPageSettings.PaperSize;
                Properties.Settings.Default.landscape = printDocument.DefaultPageSettings.Landscape;
            }
        }

        private void HideNShowInfoButton_Click(object sender, EventArgs e)
        {
            if (InfoTextBox.Visible == true)
            {
                InfoTextBox.Hide();
                HideNShowInfoButton.Text = "show info";
                Properties.Settings.Default.ShowInfo = false;
            } else
            {
                InfoTextBox.Show();
                HideNShowInfoButton.Text = "hide info";
                Properties.Settings.Default.ShowInfo = true;
            }
            Properties.Settings.Default.Save();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void DrawPictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics gr = e.Graphics;
            int windowWidth = Width;
            int windowHeight = Height;

            //Create pen objects
            Pen pen = new Pen(Color.Black);

            if (drawingLoaded)
            {
                float dHeight = topModuleCommand.bounds.YMax - topModuleCommand.bounds.YMin;
                float dWidth = topModuleCommand.bounds.XMax - topModuleCommand.bounds.XMin;
                float vDiff = (float)Math.Round((DrawPanel.Height - dHeight)/2.0, 1);
                float hDiff = (float)Math.Round((DrawPanel.Width - dWidth)/2.0, 1);
                float centerShiftX = 0;
                float centerShiftY = 0;

                // calculate zoom factor for fitting drawing to window
                float fitScaleFactor = (DrawPanel.Width - 30) / dWidth;
                if (fitScaleFactor > (DrawPanel.Height - 30) / dHeight) fitScaleFactor = (DrawPanel.Height - 30) / dHeight;

                if (fitToWindow)
                {
                    centerShiftX = (DrawPanel.Width - 30 - dWidth * fitScaleFactor) / 2.0F  ;
                    centerShiftY = (DrawPanel.Height - 30 - dHeight * fitScaleFactor) / 2.0F;
                    ZoomFactor = fitScaleFactor;
                    DrawPictureBox.Height = DrawPanel.Height;
                    DrawPictureBox.Width = DrawPanel.Width;
                }

                // Set origin for display based on drawing bounds
                gr.TranslateTransform(-topModuleCommand.bounds.XMin * ZoomFactor + 15 + centerShiftX,
                                       topModuleCommand.bounds.YMax * ZoomFactor + 15 + centerShiftY);
                
                gr.ScaleTransform(1, -1);  // Set Y direction to be from bottom to top rather than Windows top to bottom
                gr.ScaleTransform(ZoomFactor, ZoomFactor);

                // draw crossed lines at origin
                DrawCropMark(gr, new Pen(Color.Red), new PointF(0F, 0F));
                DrawCropMark(gr, new Pen(Color.Green), new PointF(topModuleCommand.bounds.XMin, - topModuleCommand.bounds.YMin));

                gr.DrawRectangle(new Pen(Color.LightGray), topModuleCommand.bounds.XMin, topModuleCommand.bounds.YMin, dWidth, dHeight);

                PaintDcModule(gr, topModuleCommand);
            }
            pen.Dispose();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            DrawPanel.Location = new Point(0, menuStrip1.Height);
            DrawPanel.Height = Height - menuStrip1.Height - 40;
            DrawPanel.Width = Width - 20;
            Invalidate();
        }

        private void colorPaletteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorConfigForm.SetColorConfig(dcColorSettings);
            var result = colorConfigForm.ShowDialog();
            if(result == DialogResult.OK)
            {
                dcColorSettings = colorConfigForm.settings;
            }
            Invalidate();
            Properties.Settings.Default.pinsColor = dcColorSettings.pinsColor;
            Properties.Settings.Default.textColor = dcColorSettings.textColor;
            Properties.Settings.Default.wiresColor = dcColorSettings.wiresColor;
            Properties.Settings.Default.linesColor = dcColorSettings.linesColor;
            Properties.Settings.Default.showPins = dcColorSettings.showPins;
            Properties.Settings.Default.blackAndWhite = dcColorSettings.blackAndWhite;
        }

        private void FitToWindowButton_Click(object sender, EventArgs e)
        {
            fitToWindow = true;
            Invalidate();
        }

        private void PrintSetupMenuItem_Click(object sender, EventArgs e)
        {
            PrintSetupForm psf = new PrintSetupForm(printDocument, this);
            DialogResult result = psf.ShowDialog();
        }
    }
}
