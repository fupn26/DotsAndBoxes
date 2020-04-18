﻿using DotsAndBoxes.Structures;
using System;

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
