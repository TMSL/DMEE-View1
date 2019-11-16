using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcClasses
{
    public class DcBounds
    {
        public bool boundsProcessed = false; // flag indicating bounds processing is done for this module instance
        public float XMax = -32000;
        public float YMax = -32000;
        public float XMin = 32000;
        public float YMin = 32000;
    }

    public class DcLibEntry
    {
        public string moduleName = "";
    }

    public class InternalLBREntry
    {
        public bool fromLibrary = false;
        public string name = "";
        public string fileName = "";
        public List<DcCommand> drawList = new List<DcCommand>();
        public DrawListStats stats = new DrawListStats();
        public DcBounds bounds = new DcBounds();
    }

    public class DrawListStats
    {
        public int textItemCount = 0;
        public int strItemCount = 0;
        public int pinItemCount = 0;
        public int moduleItemCount = 0;
        public int drawingItemCount = 0;
        public List<Single> textScalingList = new List<Single>();
    }

    public class ExtLBRCatEntry
    {
        public String name = "";
        public String extension = "";
        public long filePos = 0;
        public int recordOffset = 0;
        public int recordSize = 0;
    }

    public class DcExternalLBRCatalog
    {
        public enum MatchType { partial, full };
        public String fileName = ""; // full path to the library file from which the catalog was read.
        public int entryCount = 0;
        public int size = 0;
        public List<ExtLBRCatEntry> entries = new List<ExtLBRCatEntry>();
    }

    // ==================================================================
    // ================= DC COMMAND CLASS DEFINITIONS ===================
    // ==================================================================
    public class DcCommand  // base class for the draw command classes
    {
        public enum CommandType
        {
            arc, bus, circle, drawing, fill, line,
            module, net, pin, route, str, text, wire,
            undefined
        };
        protected CommandType _cmdType = CommandType.undefined;
        public CommandType CmdType
        {
            get => _cmdType;
        }
    }

    //Arc (a) - e.g. "a  6 -38.641014 10 -4 -10 -4 30"
    // Command to draw an arc using coordinates of the center of a circle and two points on the circle to define the arc.
    //      Aside: Calculating the coordinates of the center given two arbitrary points and a radius is an interesting problem
    //      for the software that generates the file. Fortunately, it's fairly straightforward to draw the arc once 
    //      the points have been calculated.
    public class DcArc : DcCommand
    {
        public DcArc()
        {
            _cmdType = CommandType.arc;
        }
        public int layer = 0;      // [1]
        public float centerX = 0;  // [2]
        public float centerY = 0;  // [3]
        public float X1 = 0;       // [4]
        public float Y1 = 0;       // [5]
        public float X2 = 0;       // [6]
        public float Y2 = 0;       // [7]
    }

    //Bus (b) - e.g. "b  10 1810 1115 1810 400 0 2"
    public class DcBus : DcCommand
    {
        public DcBus()
        {
            _cmdType = CommandType.bus;
        }

        public int layer = 0;       // [1]
        public float X1 = 0;        // [2]
        public float Y1 = 0;        // [3]
        public float X2 = 0;        // [4]
        public float Y2 = 0;        // [5]
        public int unk1 = 0;        // [6] MIRROR?
        public float width = 0;     // [7] line weight??
    }

    //Circle (c)
    // e.g. c  15 35 0 30 0 = Circle with radius 5 (diameter = 10) centered at 35,0
    public class DcCircle : DcCommand
    {
        public DcCircle()
        {
            _cmdType = CommandType.circle;
        }

        public int layer = 0;           // [1] color or layer
        public float X1 = 0;            // [2]
        public float Y1 = 0;            // [3]
        public float X2 = 0;            // [4]
        public float Y2 = 0;            // [5]
        public float unk6 = 0;          // [6]
    }

    //Drawing (d) -- e.g. D2BLKDIA:   d  4.09 1  1751  588  1        0 0 0 0 0   5 0
    public class DcDrawing : DcCommand
    {
        public DcDrawing()
        {
            _cmdType = CommandType.drawing;
        }
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
        public int grid = 0;            // [11] units per grid / snap
        public int units = 0;           // [12] 0 = 1 mil per unit, 1 = 5 mils per unit
        public DcString dcStr = new DcString();
    }

    //Line (l)        
    public class DcLine : DcCommand
    {
        public DcLine()
        {
            _cmdType = CommandType.line;
        }

        public int layer = 0;       // [1]
        public float X1 = 0;        // [2]
        public float Y1 = 0;        // [3]
        public float X2 = 0;        // [4]
        public float Y2 = 0;        // [5]
        public float unk6 = 0;      // [6] found some 2.10 library modules that have only 6 fields for a line instead of seven
        public float unk7 = 0;      // [7] found this is a float, perhaps line weight/width ??
    }

    //Module (m)
    // -- e.g. m  15  0    0  1.25   0  0 bsize   0  0  0  0  0  // just scaled
    //         m  15 1900 725  1   180  1 iowire  0  0  0  1  1  // horizontally flipped
    //         m  15 1675 765  1   180  1 ls125   0  0  0  1  1  // horizontally flipped
    //         m  15 1070 670  1    90  0   r     0  0  0  1  1  // 90 rotated
    //         m  15 175  585 0.75 270  0 ls04a   1  0  0  1  0  // 270 rotated and scaled
    //         0   1  2    3   4    5   6   7     8  9 10 11 12
    public class DcModule : DcCommand
    {
        public DcModule()
        {
            _cmdType = CommandType.module;
        }
        public int internalLBRIndex = -1; // index to entry for module in internal library
        public DcBounds bounds = new DcBounds(); //
        public int layer = 0;           // [1]
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

    //Pin (p) - e.g. "p  15 60 10 60 10 1"
    // a pin may have an associated s record. The record defines the pin number, e.g. s 1 0 #2
    public class DcPin : DcCommand
    {
        public DcPin()
        {
            _cmdType = CommandType.pin;
        }
        public int layer = 0;       // [1]
        public float X1 = 0;        // [2]
        public float Y1 = 0;        // [3]
        public float unk1 = 0;      // [4]
        public float unk2 = 0;      // [5]
        public float unk3 = 0;      // [6] MIRROR?
        public string Text = "";
    }

    //String/symbol name (s)
    // String text fields in file begin with "#" immediately followed by the text
    // Strings are allowed to have spaces in them. Thus, encountering a # 'turns off'
    // use of <space> as a field delimiter for the remainder of the line.
    // # serves double-duty as start of a comment or comment line
    public class DcString : DcCommand
    {
        public DcString()
        {
            _cmdType = CommandType.str;
        }
        public int unk1 = 0;
        public int unk2 = 0;
        public int unk3 = 0;
        public string strText = "";
    }

    //Text (t)
    public class DcText : DcCommand
    {
        public DcText()
        {
            _cmdType = CommandType.text;
        }
        public DcBounds textBounds = new DcBounds();
        public int layer = 0;           // [1]
        public float X1 = 0;            // [2]
        public float Y1 = 0;            // [3]
        public float scaleFactor = 0;   // [4] scaling factor for the font
        public float rotation = 0;      // [5]
        public int mirror = 0;          // [6] MIRROR = 1?
        public int upright = 0;         // [7] KEEP UPRIGHT = 1?
        public DcString dcStr = new DcString();
    }

    //Wire (w)        
    public class DcWire : DcCommand
    {
        public DcWire()
        {
            _cmdType = CommandType.wire;
        }

        public int layer = 0;           // [1]
        public float X1 = 0;            // [2]
        public float Y1 = 0;            // [3]
        public float X2 = 0;            // [4]
        public float Y2 = 0;            // [5]
        public int unk1 = 0;            // [6]
        public int unk2 = 0;            // [7]
        public int net = 0;             // [8]
        public int unk3 = 0;            // [9]
        public DcString dcStr = new DcString();
    }

    //Net (x)
    public class DcNet : DcCommand
    {
        public DcNet()
        {
            _cmdType = CommandType.net;
        }
        public string name = "-unassigned-";
        public int number = 0;
    }
}
