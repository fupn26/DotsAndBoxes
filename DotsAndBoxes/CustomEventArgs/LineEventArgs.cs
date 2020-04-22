using DotsAndBoxes.Structures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;

namespace DotsAndBoxes.CustomEventArgs
{
    public class LineEventArgs : EventArgs
    {
        public LineStructure Line { get; set; }

        public LineEventArgs(LineStructure line)
        {
            this.Line = line;
        }
    }
}
