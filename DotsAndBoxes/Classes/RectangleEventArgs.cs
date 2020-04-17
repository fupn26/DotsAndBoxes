using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DotsAndBoxes.Classes
{
    class RectangleEventArgs : EventArgs
    {
        public Point RefPoint { get; }

        public RectangleEventArgs(Point refPoint)
        {
            RefPoint = refPoint;
        }
    }
}
