using DotsAndBoxes.Structures;
using System;

namespace DotsAndBoxes.CustomEventArgs
{
    public class RectangleEventArgs : EventArgs
    {
        public RectangleStructure  rectangleStructure { get; }
        public RectangleEventArgs(RectangleStructure rectangle)
        {
            rectangleStructure = rectangle;
        }
    }
}
