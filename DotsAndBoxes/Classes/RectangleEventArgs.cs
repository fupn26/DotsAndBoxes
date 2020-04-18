using DotsAndBoxes.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;

namespace DotsAndBoxes.Classes
{
    class RectangleEventArgs : EventArgs
    {
        public RectangleStructure  rectangleStructure { get; }
        public RectangleEventArgs(RectangleStructure rectangle)
        {
            rectangleStructure = rectangle;
        }
    }
}
