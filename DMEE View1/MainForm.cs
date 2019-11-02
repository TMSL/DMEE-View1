using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Text;

// TMS - started September 17, 2019

namespace DMEEView1
{
    public partial class MainForm : Form
    {
        string topFileName = "";
        string libraryFolder = "";
        string workingFolder = "";
        float ZoomFactor = 1;
        float DrawPanelScale = 1;
        const string crlf = "\r\n";
        bool modulesLoaded = false;

        private List<ModuleListEntry> moduleList = new List<ModuleListEntry>();
        private DcDictionary dictionary = new DcDictionary();
        private List<DcDictionary> DcDictionaryList = new List<DcDictionary>();
        private DcModule topModuleCommand = new DcModule();

        private class ModuleListEntry
        {
            public bool processed = false;
            public bool fromLibrary = false;
            public string name = "";
            public string fileName = "";
            public List<DcCommand> drawList = new List<DcCommand>();
            public ModuleStats stats = new ModuleStats();
        }

        private class ModuleStats
        {
            public float biggestX = -10000;
            public float biggestY = -10000;
            public float smallestX = 10000;
            public float smallestY = 10000;
            public int textItemCount = 0;
            public int strItemCount = 0;
            public int pinItemCount = 0;
            public int moduleItemCount = 0;
            public int drawingItemCount = 0;
            public List<Single> textScalingList = new List<Single>();
        }

        private class DictionaryEntry
        {
            public String name = "";
            public String extension = "";
            public long filePos = 0;
            public int recordOffset = 0;
            public int recordSize = 0;
        }

        private class DcDictionary
        {
            public String fileName = ""; // full path to the library file from which the dictionary was read.
            public int entryCount = 0;
            public int size = 0;
            public List<DictionaryEntry> entries = new List<DictionaryEntry>();
        }

        public FolderConfigForm folderConfigForm = new FolderConfigForm();

        public MainForm()
        {
            InitializeComponent();
            menuStrip1.Select();

            this.printDocument.PrintPage += new PrintPageEventHandler(this.PrintDocument_PrintPage); // register callback for printing
            topFileName = Properties.Settings.Default.fileName;
            TopFileNameTextBox.Text = topFileName;

            // created a module command on behalf of the top module
            topModuleCommand.parentScaleFactor = 1.0F;
            topModuleCommand.parentRotation = 0F;
            topModuleCommand.scaleFactor = 1.0F;
            topModuleCommand.X1 = 0;
            topModuleCommand.Y1 = 0;
            topModuleCommand.moduleListIndex = 0;
            topModuleCommand.name = topFileName;

            Screen activeScreen = Screen.FromControl(this);
            Console.WriteLine("Screen size: " + activeScreen.Bounds.Width + " x " + activeScreen.Bounds.Height);

            // First, set the height and width of the window to the 11 x 17 aspect ratio
            // where the window height is 95% of the screen height
            this.Height = (int) (activeScreen.Bounds.Height * 0.95F);
            this.Width = (int)(18F / 12F * this.Height);
            this.CenterToScreen();

            // Set the location and size of DrawPanel relative to form
            DrawPanel.Location = new Point(0, menuStrip1.Height);
            DrawPanel.Height = this.Height - menuStrip1.Height-40;
            DrawPanel.Width = this.Width-20;

            Graphics gr = CreateGraphics();

            // Now calculate a scale factor that makes as 12" x 18" area fit in the window and then double it.
            // This should make a 50% zoom factor fit the window nicely
            DrawPictureBox.Location = new Point(0, 0);
            DrawPictureBox.Height = (int)(12 * gr.DpiY);
            DrawPictureBox.Width = (int)(18 * gr.DpiX);

            // calculate scale factor based on fitting a 12" high image into the DrawPanel at 50% Zoom level
            DrawPanelScale = 2F * (DrawPanel.Height / (float)DrawPictureBox.Height);

            if (Properties.Settings.Default.ShowInfo == true)
            {
                HideNShowInfoButton.Text = "hide info";
                InfoTextBox.Show();

            }
            else
            {
                HideNShowInfoButton.Text = "show info";
                InfoTextBox.Hide();
            }

            moduleList.Add(new ModuleListEntry());
            libraryFolder = Properties.Settings.Default.LibFolder;
            workingFolder = Properties.Settings.Default.WorkFolder;

            if (!Directory.Exists(workingFolder))
            {
                workingFolder = "";
                Properties.Settings.Default.WorkFolder = workingFolder;
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ModuleInfoTextBox(ModuleListEntry module)
        {
            InfoTextBox.Clear();
            InfoTextBox.Text += "Library Folder: " + libraryFolder + crlf;
            InfoTextBox.Text += "Working Folder: " + workingFolder + crlf;
            InfoTextBox.Text += "Biggest X: " + module.stats.biggestX.ToString() + crlf;
            InfoTextBox.Text += "Biggest Y: " + module.stats.biggestY.ToString() + crlf;
            InfoTextBox.Text += "Smallest X: " + module.stats.smallestX.ToString() + crlf;
            InfoTextBox.Text += "Smallest Y: " + module.stats.smallestY.ToString() + crlf;
            InfoTextBox.Text += "Module (m) Entries Count: " + module.stats.moduleItemCount + crlf;
            InfoTextBox.Text += "Text (t) Entries Count: " + module.stats.textItemCount + crlf;
            InfoTextBox.Text += "String (s) Entries Count: " + module.stats.strItemCount + crlf;
            InfoTextBox.Text += "Pin (p) Entries Count: " + module.stats.pinItemCount + crlf;
            InfoTextBox.Text += "Drawing/display (d) Entries Count: " + module.stats.drawingItemCount + crlf;
            InfoTextBox.Text += "Module List Entries Count: " + moduleList.Count;
        }

        private void DrawCropMark(Graphics gr, Pen pen, PointF center)
        {
            gr.DrawLine(pen, center.X - 5, center.Y, center.X + 5, center.Y);
            gr.DrawLine(pen, center.X, center.Y - 5, center.X, center.Y + 5);
        }

        private void PaintDcModule(Graphics gr, DcModule moduleCommand)
        {
            Point pt1 = new Point();
            Point pt2 = new Point();
            PointF ptf1 = new Point();
            PointF ptf2 = new Point();
            Pen pen = new Pen(Color.Black);
            Pen savedPen = new Pen(Color.Black);
            PointF location = new PointF(moduleCommand.X1, moduleCommand.Y1);
            List<DcCommand> drawList = moduleList[moduleCommand.moduleListIndex].drawList;
            float scaleFactor = moduleCommand.scaleFactor;

            GraphicsState gsSaved = gr.Save();

            if (scaleFactor > 1.0F) // set scale factor
            {
                gr.ScaleTransform(scaleFactor, scaleFactor);
            }

            // transform location
            // Translate the origin to be at the target x, y position.
            float x = location.X;
            float y = location.Y;

            for (int i = 0; i < drawList.Count; i++)
            {
                GraphicsState grSaved = gr.Save();
                switch (drawList[i].commandType)
                {
                    case DcCommand.CommandType.arc:
                        DcArc dArc = (DcArc)drawList[i];
                        gr.ScaleTransform(1, -1);
                        gr.TranslateTransform(x, y);
                        if (moduleCommand.mirror == 1) gr.ScaleTransform(1, -1);
                        gr.RotateTransform(moduleCommand.rotation);
                        PointF arcCenter = new PointF(dArc.centerX, dArc.centerY);
                        PointF p1 = new PointF(dArc.X1, dArc.Y1);
                        PointF p2 = new PointF(dArc.X2, dArc.Y2);
                        DcDrawArc(gr, pen, arcCenter, p1, p2);
                        break;

                    case DcCommand.CommandType.bus:
                        DcBus dBus = (DcBus)drawList[i];
                        gr.ScaleTransform(1, -1);
                        gr.TranslateTransform(x, y);
                        if (moduleCommand.mirror == 1) gr.ScaleTransform(1, -1);
                        gr.RotateTransform(moduleCommand.rotation);
                        pt1.X = Convert.ToInt32(dBus.X1);
                        pt1.Y = Convert.ToInt32(dBus.Y1);
                        pt2.X = Convert.ToInt32(dBus.X2);
                        pt2.Y = Convert.ToInt32(dBus.Y2);
                        savedPen = (Pen)pen.Clone();
                        pen.Width = dBus.width;
                        gr.DrawLine(pen, pt1, pt2);
                        pen = savedPen;
                        break;

                    case DcCommand.CommandType.circle:
                        DcCircle dCircle = (DcCircle)drawList[i];
                        gr.ScaleTransform(1, -1);
                        gr.TranslateTransform(x, y);
                        if (moduleCommand.mirror == 1) gr.ScaleTransform(1, -1);
                        gr.RotateTransform(moduleCommand.rotation);
                        ptf1.X = Convert.ToSingle(dCircle.X1);
                        ptf1.Y = Convert.ToSingle(dCircle.Y1);
                        ptf2.X = Convert.ToSingle(dCircle.X2);
                        ptf2.Y = Convert.ToSingle(dCircle.Y2);
                        float radius = (float)Math.Sqrt(Math.Pow(ptf1.X - ptf2.X, 2) + Math.Pow(ptf1.Y - ptf2.Y, 2));
                        float diameter = 2F * radius;
                        float center = ptf1.X;
                        gr.DrawEllipse(pen, center - radius, ptf1.Y - radius, diameter, diameter);
                        break;

                    case DcCommand.CommandType.drawing:
                        break;

                    case DcCommand.CommandType.line:
                        DcLine dLine = (DcLine)drawList[i];
                        gr.ScaleTransform(1, -1);
                        gr.TranslateTransform(x, y);
                        if (moduleCommand.mirror == 1) gr.ScaleTransform(1, -1);
                        gr.RotateTransform(moduleCommand.rotation);
                        pt1.X = Convert.ToInt32(dLine.X1);
                        pt1.Y = Convert.ToInt32(dLine.Y1);
                        pt2.X = Convert.ToInt32(dLine.X2);
                        pt2.Y = Convert.ToInt32(dLine.Y2);
                        gr.DrawLine(pen, pt1, pt2);
                        break;

                    case DcCommand.CommandType.module:
                        DcModule dModule = (DcModule)drawList[i];
                        int mIndex = dModule.moduleListIndex;
                        // get drawlist for module from moduleList
                        List<DcCommand> mDrawList;
                        mDrawList = moduleList[mIndex].drawList;
                        // draw the module at specified location with given scalefactor
                        float mScaleFactor = dModule.scaleFactor;
                        PointF mLocation = new PointF(dModule.X1, dModule.Y1);
                        PaintDcModule(gr, dModule);
                        break;

                    case DcCommand.CommandType.pin:
                        DcPin dPin = (DcPin)drawList[i];
                        // draw a small square to identify the pin location.
                        gr.ScaleTransform(1, -1);
                        gr.TranslateTransform(x, y);
                        if (moduleCommand.mirror == 1) gr.ScaleTransform(1, -1);
                        gr.RotateTransform(moduleCommand.rotation);
                        float width = pen.Width; // save width
                        pen.Width = 0.5F;
                        gr.DrawRectangle(pen, dPin.X1 - 2, dPin.Y1 - 2, 4, 4);
                        pen.Width = width;  // restore width
                        break;

                    case DcCommand.CommandType.str:
                        break;

                    case DcCommand.CommandType.text:
                        gr.ScaleTransform(1, -1);
                        gr.TranslateTransform(x, y);
                        gr.RotateTransform(moduleCommand.rotation);
                        DcText dct = (DcText)drawList[i];
                        DcDrawText(gr, pen, dct);
                        break;

                    case DcCommand.CommandType.wire:
                        DcWire dWire = (DcWire)drawList[i];
                        gr.ScaleTransform(1, -1);
                        gr.TranslateTransform(x, y);
                        if (moduleCommand.mirror == 1) gr.ScaleTransform(1, -1);
                        gr.RotateTransform(moduleCommand.rotation);
                        pt1.X = Convert.ToInt32(dWire.X1);
                        pt1.Y = Convert.ToInt32(dWire.Y1);
                        pt2.X = Convert.ToInt32(dWire.X2);
                        pt2.Y = Convert.ToInt32(dWire.Y2);
                        gr.DrawLine(pen, pt1, pt2);
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

        private void DcDrawText(Graphics gr, Pen pen, DcText dct)
        {
            FontFamily fontFamily = new FontFamily("MS GOTHIC");
            string text = dct.dcStr.strText.TrimStart('#');

            // Calculate font scale factor
            float fontSize = 10.5F * dct.scaleFactor / 0.039063F;
            Font the_font = new Font(fontFamily, fontSize);
            GraphicsState grSaved = gr.Save();

            //gr.ScaleTransform(1, -1);
            gr.TranslateTransform(dct.X1, dct.Y1);
            gr.ScaleTransform(1, -1); // undo Y flip

            gr.RotateTransform(-dct.rotation);
            gr.DrawString(text, the_font, pen.Brush, new PointF(0, 0 - fontSize));

            gr.Restore(grSaved);
        }

        private static float DcDrawArc(Graphics gr, Pen pen, PointF centerPt, PointF p1, PointF p2)
        {
            // Calculate radius as distance from center to p1
            float radius = (float)Math.Sqrt((centerPt.X - p1.X) * (centerPt.X - p1.X) + (centerPt.Y - p1.Y) * (centerPt.Y - p1.Y));

            // Calculate rectangle for DrawArc
            float x = centerPt.X - radius;
            float y = centerPt.Y - radius;
            float width = 2 * radius;
            float height = 2 * radius;

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

        // ============ DC OMMAND CLASS DEFINITIONS ===================

        private class DcCommand  // base class for the draw command classes
        {
            public CommandType commandType = CommandType.undefined;
            public enum CommandType { arc, bus, circle, drawing, line, module, net, pin, str, text, wire, undefined };
        }

        //Arc (a) - e.g. "a  6 -38.641014 10 -4 -10 -4 30"
        // Draw arc given coordinates of the center of a circle and two points on the circle that define the arc.
        // Calculating the coordinates of the center given two arbitrary points and a radius is an interesting problem
        // for the software that generates the file. Fortunately, it's fairly straightforward to draw the arc once 
        // the points have been calculated.
        private class DcArc : DcCommand
        {
            public DcArc()
            {
                commandType = CommandType.arc;
            }
            public int color = 0;      // [1]
            public float centerX = 0;  // [2]
            public float centerY = 0;  // [3]
            public float X1 = 0;       // [4]
            public float Y1 = 0;       // [5]
            public float X2 = 0;       // [6]
            public float Y2 = 0;       // [7]
        }

        //Bus (b) - e.g. "b  10 1810 1115 1810 400 0 2"
        private class DcBus : DcCommand
        {
            public DcBus()
            {
                commandType = CommandType.bus;
            }

            public int color = 0;       // [1]
            public float X1 = 0;        // [2]
            public float Y1 = 0;        // [3]
            public float X2 = 0;        // [4]
            public float Y2 = 0;        // [5]
            public int unk1 = 0;        // [6] MIRROR?
            public float width = 0;     // [7] line weight??
        }

        //Pin (p) - e.g. "p  15 60 10 60 10 1"
        // a pin may have an associated s record. The record defines the pin number, e.g. s 1 0 #2
        private class DcPin : DcCommand
        {
            public DcPin()
            {
                commandType = CommandType.pin;
            }
            public int color = 0;       // [1]
            public float X1 = 0;        // [2]
            public float Y1 = 0;        // [3]
            public float unk1 = 0;      // [4]
            public float unk2 = 0;      // [5]
            public float unk3 = 0;      // [6] MIRROR?
            public string Text = "";
        }

        //Line (l)        
        private class DcLine : DcCommand
        {
            public DcLine()
            {
                commandType = CommandType.line;
            }

            public int color = 0;       // [1]
            public float X1 = 0;        // [2]
            public float Y1 = 0;        // [3]
            public float X2 = 0;        // [4]
            public float Y2 = 0;        // [5]
            public int unk6 = 0;        // [6] I've found some 2.10 library modules that have only 7 fields for a line instead of seven (no unk2)
            public int unk7 = 0;        // [7]
        }

        //Wire (w)        
        private class DcWire : DcCommand
        {
            public DcWire()
            {
                commandType = CommandType.wire;
            }

            public int color = 0;       // [1]
            public float X1 = 0;        // [2]
            public float Y1 = 0;        // [3]
            public float X2 = 0;        // [4]
            public float Y2 = 0;        // [5]
            public int unk1 = 0;        // [6]
            public int unk2 = 0;        // [7]
            public int net = 0;         // [8]
            public int unk3 = 0;        // [9]
            public DcString dcStr = new DcString();
        }

        //Text (t)
        private class DcText : DcCommand
        {
            public DcText()
            {
                commandType = CommandType.text;
            }

            public int color = 0;           // [1]
            public float X1 = 0;            // [2]
            public float Y1 = 0;            // [3]
            public float scaleFactor = 0;   // [4] scaling factor for the font
            public float rotation = 0;      // [5]
            public int unk6 = 0;            // [6] MIRROR?
            public int unk7 = 0;            // [7]
            public DcString dcStr = new DcString();
        }

        //String/symbol name (s)
        // String text fields in file begin with "#" immediately followed by the text
        // Strings are allowed to have spaces in them. Thus, encountering a # 'turns off'
        // use of <space> as a field delimiter for the remainder of the line.
        // # serves double-duty as start of a comment or comment line
        private class DcString : DcCommand
        {
            public DcString()
            {
                commandType = CommandType.str;
            }
            public int unk1 = 0;
            public int unk2 = 0;
            public int unk3 = 0;
            public string strText = "";
        }

        //Circle (c)
        private class DcCircle : DcCommand
        {
            public DcCircle()
            {
                commandType = CommandType.circle;
            }

            public int color = 0;       // color or layer
            public float X1 = 0;
            public float Y1 = 0;
            public float X2 = 0;
            public float Y2 = 0;
        }

        private class DcNet : DcCommand
        {
            public DcNet()
            {
                commandType = CommandType.net;
            }
            public string name = "-unassigned-";
            public int number = 0;
        }

        //include Module (m)
        // -- e.g. m  15 0 0 1.25 0 0 bsize 0 0 0 0 0
        //         m  15 1900 725 1 180 1 iowire 0 0 0 1 1 // horizontally flipped
        //         m  15 1675 765 1 180 1 ls125  0 0 0 1 1  // horizontally flipped
        //         m  15 1070 670 1 90  0 r      0 0 0 1 1  // 90 rotated
        private class DcModule : DcCommand
        {
            public DcModule()
            {
                commandType = CommandType.module;
            }
            public int moduleListIndex = -1;    // index to entry for module in moduleList
            public float parentScaleFactor = 1; // Module scale factor from parent module
            public float parentRotation = 0;    // Module rotation from parent module
            public int color = 0;           // [1] color or layer
            public float X1 = 0;            // [2] X coordinate (offset) to place module's origin
            public float Y1 = 0;            // [3] Y coordinate (offset) to place module's origin
            public float scaleFactor = 1;   // [4]
            public float rotation = 0;      // [5] Rotation in degrees
            public int mirror = 0;          // [6] 0 = no mirror, 1 = mirror (horizontal mirror) ??????
            public string name = "";        // [7]
            public int unk8 = 0;            // [8]
            public int unk9 = 0;            // [9]
            public int unk10 = 0;           // [10]
            public int unk11 = 0;           // [11]
            public int unk12 = 0;           // [12]
        }

        private class DcDrawing : DcCommand    // (d) drawing / display -- e.g.
                                                //                   D2BLKDIA:   d  4.09 1  1751  588  1        0 0 0 0 0   5 0
        {                                       //                   CONN62.100: d  3.00 1 -1514 -131  0.291667 0 0 0 0 0 100 0
            public float version = 0;       // [1]
            public int unk2 = 0;            // [2]
            public float X1 = 0;            // [3]
            public float Y1 = 0;            // [4]
            public float scaleFactor = 0;   // [5]
            public int unk6 = 0;            // [6]
            public int unk7 = 0;            // [7]
            public int unk8 = 0;            // [8]
            public int unk9 = 0;            // [9]
            public int unk10 = 0;           // [10]
            public int unk11 = 0;           // [11]
            public int unk12 = 0;           // [12]
        }

        private class DcLibEntry
        {
            public string moduleName = "";
        }

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

        // CREATE LIST CONTAINING LIBRARY DIRECTORIES IN LIBRARY FOLDER AND A LIST OF FILES IN WORKING DIRECTORY
        // Add top module entry to module list and mark as unprocessed.
        // 1. get unprocessed module entry from module list. If none found then done. (since top module is initially the only
        //    module in the list, processing will always start with top module.)
        // 2. search for module in working directory, then in library directories (top module name comes from "open file" so
        //    top module will actually just respond to a "file exists" check.)
        // 3. If module found, extract display list for module else mark module as processed-not-found and back to (1)
        // 4. Search module display list for sub modules
        // 5. for each submodule
        // 5.1      Add sub module entries to module list if not already present in list (marked as unprocessed)
        // 5.5      Set index for "m" drawItem in drawlist to point to module entry in list
        // 6. mark module as processed
        // back to (1)

        enum DrawListStatus { OK, FileNotFound };

        private DrawListStatus DcMakeDrawListFromFile(ref ModuleListEntry module, long startPos, int drawListSize)
        {
            string fname = module.fileName;
            DcCommand.CommandType prevCommandType = DcCommand.CommandType.undefined;
            DcModule parentModuleCommand = new DcModule();
            parentModuleCommand.commandType = DcCommand.CommandType.module;
            string line;
            FileStream file;
            long endPos = 0;

            module.stats = new ModuleStats();  // clears all module stats

            if (File.Exists(fname))
            {
                file = new FileStream(fname, FileMode.Open, FileAccess.Read);
            }
            else
            {
                module.drawList.Clear();
                return DrawListStatus.FileNotFound;
            }

            file.Position = startPos;
            endPos = startPos + drawListSize;

            for (int i = 0; i < 20000; i++) // 20,000 line limit.  TBD: Bad form. Add an 'abort'/cancel function in
                                            // case someone loads a huge file that might be mistaken for a DMEE file.
            {
                if (file.Position == file.Length) break;  // reached end of file
                if (endPos > 0 && file.Position >= endPos) break; // reached end of space reserved for drawlist in dictionary

                long filePos = file.Position;

                line = DcReadASCIILine(file, ref filePos);

                if (line != null)
                {
                    ParseDcCommandLine(ref prevCommandType, ref line, ref module, parentModuleCommand);
                }
                else break;
            }

            module.processed = true;   // module has been processed for sub-modules

            this.Invalidate();
            file.Close();
            return DrawListStatus.OK;
        }

        private DcCommand.CommandType ParseDcCommandLine(ref DcCommand.CommandType prevCommandType, ref string line,
                                              ref ModuleListEntry module, DcModule parentModule)
        {
            DcCommand.CommandType commandType = DcCommand.CommandType.undefined;         
            string[] fields;
            string fieldStr = "";
            string rawLine = line;
            List<DcCommand> drawList = module.drawList;
            ModuleStats stats = module.stats;

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
                case "w": commandType = DcCommand.CommandType.wire; break;
                default: commandType = DcCommand.CommandType.undefined; break;
            }

            switch (commandType)  // create a record object for line and add it to draw list
            {
                case DcCommand.CommandType.arc:
                    DcArc dArc = new DcArc()
                    {
                        commandType = DcCommand.CommandType.arc,
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

                case DcCommand.CommandType.bus:
                    DcBus dBus = new DcBus()
                    {
                        commandType = DcCommand.CommandType.bus,
                        color = Convert.ToInt16(fields[1]),
                        X1 = Convert.ToSingle(fields[2]),
                        Y1 = Convert.ToSingle(fields[3]),
                        X2 = Convert.ToSingle(fields[4]),
                        Y2 = Convert.ToSingle(fields[5]),
                        width = Convert.ToSingle(fields[7])
                    };
                    drawList.Add(dBus);
                    break;

                case DcCommand.CommandType.circle:
                    DcCircle dcCircle = new DcCircle
                    {
                        commandType = DcCommand.CommandType.circle,
                        color = Convert.ToInt16(fields[1]),
                        X1 = Convert.ToSingle(fields[2]),
                        Y1 = Convert.ToSingle(fields[3]),
                        X2 = Convert.ToSingle(fields[4]),
                        Y2 = Convert.ToSingle(fields[5])
                    };
                    drawList.Add(dcCircle);
                    break;

                case DcCommand.CommandType.drawing:
                    InfoTextBox.Text += rawLine + crlf;
                    DcDrawing dcDrawing = new DcDrawing()
                    {
                        commandType = DcCommand.CommandType.drawing,
                        version = Convert.ToSingle(fields[1]),
                        X1 = Convert.ToSingle(fields[3]),
                        Y1 = Convert.ToSingle(fields[4]),
                        scaleFactor = Convert.ToSingle(fields[5])
                    };
                    module.stats.drawingItemCount++;
                    drawList.Add(dcDrawing);
                    break;

                case DcCommand.CommandType.line:
                    DcLine dcLine = new DcLine
                    {
                        commandType = DcCommand.CommandType.line,
                        color = Convert.ToInt16(fields[1]),
                        X1 = Convert.ToSingle(fields[2]),
                        Y1 = Convert.ToSingle(fields[3]),
                        X2 = Convert.ToSingle(fields[4]),
                        Y2 = Convert.ToSingle(fields[5])
                    };
                    if (fields.Length > 6) dcLine.unk6 = Convert.ToInt16(fields[6]);
                    if (fields.Length > 7) dcLine.unk7 = Convert.ToInt16(fields[7]);
                    BiggestSmallestXY(dcLine.X1, dcLine.Y1, ref stats);
                    BiggestSmallestXY(dcLine.X2, dcLine.Y2, ref stats);
                    drawList.Add(dcLine);
                    break;

                case DcCommand.CommandType.module:
                    InfoTextBox.Text += (rawLine + crlf);
                    DcModule dcModule = new DcModule()  // CREATE COMMAND OBJECT FOR MODULE
                    {
                        commandType = DcCommand.CommandType.module,
                        color = Convert.ToInt16(fields[1]),
                        X1 = Convert.ToSingle(fields[2]),
                        Y1 = Convert.ToSingle(fields[3]),
                        scaleFactor = Convert.ToSingle(fields[4]),
                        rotation = Convert.ToSingle(fields[5]),
                        mirror = Convert.ToInt16(fields[6]),
                        name = Convert.ToString(fields[7])
                    };
                    module.stats.moduleItemCount++;
                    BiggestSmallestXY(dcModule.X1, dcModule.Y1, ref stats);

                    // Search to see if an entry for the module is already in the module list.
                    ModuleListEntry result = moduleList.Find(x => x.name == dcModule.name);

                    if (result != null)  // Link module command to existing entry in module list
                    {
                        dcModule.moduleListIndex = moduleList.IndexOf(result);  // set module command to point to entry
                    }
                    else    // if not in list, create a new entry, point the module command to it, and add it to the list.
                    {
                        ModuleListEntry mm = new ModuleListEntry() // create entry that will hold drawlist for module
                        {
                            processed = false,   // module has not been processed for sub-modules
                            name = dcModule.name,
                            fromLibrary = false,
                            fileName = "",
                        };
                        moduleList.Add(mm);     // add new entry to module list
                        dcModule.moduleListIndex = moduleList.IndexOf(mm);     // set module command to point to new entry
                    }

                    // Lastly, add module command to present drawlist
                    drawList.Add(dcModule);
                    break;

                case DcCommand.CommandType.pin:
                    DcPin dcPin = new DcPin()
                    {
                        commandType = DcCommand.CommandType.pin,
                        color = Convert.ToInt16(fields[1]),
                        X1 = Convert.ToSingle(fields[2]),
                        Y1 = Convert.ToSingle(fields[3])
                    };
                    BiggestSmallestXY(dcPin.X1, dcPin.Y1, ref stats);
                    drawList.Add(dcPin);
                    module.stats.pinItemCount++;
                    break;

                case DcCommand.CommandType.str:
                    DcString dcStr = new DcString
                    {
                        commandType = DcCommand.CommandType.str,
                        unk1 = Convert.ToInt16(fields[1]),
                        unk2 = Convert.ToInt16(fields[2]),
                        strText = fieldStr
                    };

                    // I discovered some string commands only have two leading fields. E.g. "s 1 0 #TYPE"
                    // while others have three E.g. "s 1 0 1 #CA31:2
                    if (fields.Length > 3) dcStr.unk3 = Convert.ToInt16(fields[3]);

                    module.stats.strItemCount++;

                    // Set String in previous record
                    if (prevCommandType == DcCommand.CommandType.text)
                    {
                        if (drawList.Count > 0)
                        {
                            DcText dText = (DcText)drawList[drawList.Count - 1];
                            dText.dcStr = dcStr;
                        }
                    }

                    if (prevCommandType == DcCommand.CommandType.wire)
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

                case DcCommand.CommandType.text:
                    DcText dcText = new DcText
                    {
                        commandType = DcCommand.CommandType.text,
                        color = Convert.ToInt16(fields[1]),
                        X1 = Convert.ToSingle(fields[2]),
                        Y1 = Convert.ToSingle(fields[3]),
                        scaleFactor = Convert.ToSingle(fields[4]),
                        rotation = Convert.ToSingle(fields[5]),
                        unk6 = Convert.ToInt16(fields[6]),
                        unk7 = Convert.ToInt16(fields[7])
                    };
                    BiggestSmallestXY(dcText.X1, dcText.Y1, ref stats);
                    module.stats.textItemCount++;
                    drawList.Add(dcText);
                    if (!module.stats.textScalingList.Contains(dcText.scaleFactor))
                    {
                        module.stats.textScalingList.Add(dcText.scaleFactor);
                    }
                    break;

                case DcCommand.CommandType.wire:
                    DcWire dcWire = new DcWire
                    {
                        commandType = DcCommand.CommandType.wire,
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

                    BiggestSmallestXY(dcWire.X1, dcWire.Y1, ref stats);
                    BiggestSmallestXY(dcWire.X2, dcWire.Y2, ref stats);
                    drawList.Add(dcWire);
                    break;

                default: break;
            }
            prevCommandType = commandType;
            return commandType;
        }

        private void BiggestSmallestXY(float X, float Y, ref ModuleStats stats)
        {
            if (X > stats.biggestX) stats.biggestX = X;
            if (X < stats.smallestX) stats.smallestX = X;
            if (Y > stats.biggestY) stats.biggestY = Y;
            if (Y < stats.smallestY) stats.smallestY = Y;
        }

        private int GetDictionary(FileStream inFile, DcDictionary dictionary)
        {
            Byte[] buffer = new Byte[100];
            String str = "";
            int entryCount = 0;
            int recordStartOffset = 0;
            int recordSizeInBytes = 0;

            dictionary.entries.Clear();

            inFile.Read(buffer, 0, 32);

            // Assumes that two-byte binary values in the dictionary portion of the library are 16-bit and little endian.
            // All records in the library start and end on 128-byte boundaries, including the dictionary itself.
            // buffer[14] and [15] are the file offset (in multiples of 128 bytes) to the NEXT record in the library.
            // The first occurrence of the offset thus corresponds to the amount of space taken by the dictionary itself.
            int recordsStart = 128 * Convert.ToInt16(buffer[14] + buffer[15] * 256);

            for (int i = 32; i < recordsStart; i += 32)
            {
                DictionaryEntry entry = new DictionaryEntry();
                long filePos = inFile.Position;
                inFile.Read(buffer, 0, 32);
                if (buffer[0] == 0xFF) break; // no more dictionary entries

                recordStartOffset = 128 * Convert.ToInt16(buffer[12] + buffer[13] * 256);
                recordSizeInBytes = 128 * Convert.ToInt16(buffer[14] + buffer[15] * 256);

                str = Encoding.ASCII.GetString(buffer, 1, 11);
                entry.name = str;
                entry.extension = str.Substring(8, 3).TrimEnd(' '); // extension is last 3 alphanumeric characters, if any.
                entry.recordOffset = recordStartOffset;
                entry.recordSize = recordSizeInBytes;
                entry.filePos = filePos;
                dictionary.entries.Add(entry);

                entryCount++;
            }
            dictionary.fileName = inFile.Name;
            dictionary.size = recordsStart;
            dictionary.entryCount = entryCount;
            return entryCount;
        }

        public enum MatchType { partial, full };

        // search dictionary entries for name (case insensitive) starting from startIndex in dictionary and returning
        // index of entry if match found. Return -1 if match not found.
        // Setting startIndex to 0 will return index of first match.
        private int FindModuleInDictionary(DcDictionary dictionary, String moduleName, int startIndex, MatchType matchType)
        {
            int index;
            bool found = false;
            moduleName = moduleName.ToUpper();  // do all comparisons in uppercase

            for (index = startIndex; index < dictionary.entries.Count; index++)
            {
                string name = dictionary.entries[index].name.ToUpper();
                if (matchType == MatchType.full && name == moduleName)
                {
                    found = true;
                    break;
                }
                else if (matchType == MatchType.partial && name.Contains(moduleName))
                {
                    found = true;
                    break;
                }
            }
            if (!found) return -1;
            return index;
        }

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
            }
            menuStrip1.Select();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FoldersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderConfigForm.ShowDialog();
            libraryFolder = folderConfigForm.libraryFolder;
            workingFolder = folderConfigForm.workingFolder;
        }

        private void ToolStripMenuZoom25_Click(object sender, EventArgs e)
        {
            ZoomFactor = 0.25F;
            this.Invalidate();
        }

        private void ToolStripMenuZoom50_Click(object sender, EventArgs e)
        {
            ZoomFactor = 0.5F;
            this.Invalidate();
        }

        private void ToolStripMenuZoom100_Click(object sender, EventArgs e)
        {
            ZoomFactor = 1.0F;
            this.Invalidate();
        }

        private void ToolStripMenuZoom150_Click(object sender, EventArgs e)
        {
            ZoomFactor = 1.5F;
            this.Invalidate();
        }

        private void ToolStripMenuZoom200_Click(object sender, EventArgs e)
        {
            ZoomFactor = 2.0F;
            this.Refresh();
        }

        private void DrawFileButton_Click(object sender, EventArgs e)
        {
            moduleList.Clear();
            ModuleListEntry module = new ModuleListEntry  // create entry for top module
            {
                fileName = topFileName,
                stats = new ModuleStats()
            };
            module.name = topFileName.Substring(topFileName.LastIndexOf("\\") + 1);
            module.fromLibrary = false;
            moduleList.Add(module);
            DrawListStatus status = DcMakeDrawListFromFile(ref module, 0, 0);
            if (status == DrawListStatus.FileNotFound)
            {
                topFileName = "";
                TopFileNameTextBox.Text = "";
                TopFileNameTextBox.Update();
                Properties.Settings.Default.fileName = "";
                MessageBox.Show("Please select another file using Open under File menu", "File not found");
                return;
            }

            // read dictionaries from each library (.LBR) file in library folder
            if (Directory.Exists(libraryFolder))
            {
                DcDictionaryList.Clear();
                int entryCount = 0;
                string[] libFiles = Directory.GetFiles(libraryFolder, "*.lbr");
                foreach (string fName in libFiles)
                {
                    FileStream libFile = new FileStream(fName, FileMode.Open, FileAccess.Read);
                    DcDictionary dictionary = new DcDictionary();
                    entryCount = GetDictionary(libFile, dictionary);
                    DcDictionaryList.Add(dictionary);
                    libFile.Close();
                }
            }

            foreach (ModuleListEntry m in moduleList)
            {
                module = m;
                Console.WriteLine("moduleList: " + m.name);
                if (!module.processed)
                {
                    // first check for name in working directory
                    string[] s = Directory.GetFiles(workingFolder, m.name);

                    if (s.Length > 0)  // file name found in working folder
                    {
                        module.fromLibrary = false;
                        module.fileName = s[0];
                        DcMakeDrawListFromFile(ref module, 0, 0);
                    }
                    else  // else check for name in libraries
                    {
                        // convert name to 11 character format used in .LBR files
                        string name = module.name;
                        int n = name.IndexOf('.');
                        if (n > 0)
                        {
                            string ext = name.Substring(n+1);
                            name = name.Substring(0, name.IndexOf('.'));
                            name = name.PadRight(8, ' '); // pad to 8 characters
                            name += ext;
                        }
                        name = name.PadRight(11, ' '); // pad to 11 total characters
                        int index = -1;
                        DcDictionary dictionary = new DcDictionary();

                        // find name among dictionaries in dictionary list
                        foreach (DcDictionary d in DcDictionaryList)
                        {
                            index = FindModuleInDictionary(d, name, 0, MatchType.full);
                            if (index >= 0)
                            {
                                dictionary = d;
                                break;
                            }
                        }
                        // load draw commands from file if module found
                        if (index >= 0)
                        {
                            module.fromLibrary = true;
                            module.fileName = dictionary.fileName;
                            DictionaryEntry entry = dictionary.entries[index];
                            DcMakeDrawListFromFile(ref module, entry.recordOffset, entry.recordSize);
                        }
                        else Console.WriteLine("Module \"" + m.name + "\" not found in directory or libraries. ");
                    }
                }
            }
            modulesLoaded = true;
        }

        // The PrintPage event is raised for each page to be printed.
        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Console.WriteLine("Print Page");
            MessageBox.Show("Printing not yet implemented.", "Print Page Event");
        }

        private void ToolStripMenuPrint_Click(object sender, EventArgs e)
        {
            DialogResult result = printDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        private void ToolStripMenuPageSetup_Click(object sender, EventArgs e)
        {
            PageSettings settings = new PageSettings();
            pageSetupDialog1.PageSettings = settings;
            pageSetupDialog1.ShowDialog();
            MessageBox.Show("Not yet implemented.", "Page Setup");
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void DrawPictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics gr = e.Graphics;
            ModuleListEntry module = moduleList[0];
            float biggestX = module.stats.biggestX;
            float biggestY = module.stats.biggestY;
            float smallestX = module.stats.smallestX;
            float smallestY = module.stats.smallestY;
            float dpiX = gr.DpiX;
            float dpiY = gr.DpiY;
            int windowWidth = this.Width;
            int windowHeight = this.Height;

            //Create pen objects
            Pen pen = new Pen(Color.Black);

            gr.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            if (ZoomFactor >= 1.0F)
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
            }

            if (modulesLoaded)
            {
                //DrawPictureBox.Width = (int)(ZoomFactor * (250 + moduleList[0].stats.biggestX - moduleList[0].stats.smallestX));
                //DrawPictureBox.Height = (int)(110 + ZoomFactor * (moduleList[0].stats.biggestY - moduleList[0].stats.smallestY));

                // move origin down
                float shiftY = 12F * gr.DpiY;

                gr.DrawLine(pen, 50, 50, 200, 50);
                gr.TranslateTransform(0, ZoomFactor * (shiftY)); // Move the origin "down".
                Console.WriteLine(shiftY);
                //gr.ScaleTransform(DrawPanelScale, DrawPanelScale);
                gr.ScaleTransform(0.90F, 0.90F);
                gr.ScaleTransform(ZoomFactor, ZoomFactor);

                pen.Width = 1;

                // draw crossed lines at origin
                pen.Color = Color.Red;
                DrawCropMark(gr, pen, new PointF(topModuleCommand.X1, topModuleCommand.Y1));
                DrawCropMark(gr, pen, new PointF(biggestX, -biggestY));

                PaintDcModule(gr, topModuleCommand);
                ModuleInfoTextBox(moduleList[0]);
            }
            pen.Dispose();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            DrawPanel.Location = new Point(0, menuStrip1.Height);
            DrawPanel.Height = this.Height - menuStrip1.Height - 40;
            DrawPanel.Width = this.Width - 20;
        }
    }
}
