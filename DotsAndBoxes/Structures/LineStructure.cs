using DotsAndBoxes.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DotsAndBoxes.Structures
{
    struct LineStructure
    {
        public Point Point1 { get; set; }
        public Point Point2 { get; set; }
        public string StrokeColor { get; set; }
        public double StrokeThickness { get; set; }
    }
}
