using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;

namespace DotsAndBoxes.CustomEventArgs
{
    class LineEventArgs : EventArgs
    {
        public Line Line { get; set; }

        public LineEventArgs(Line line)
        {
            this.Line = line;
        }
    }
}
