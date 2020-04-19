using System;
using System.Collections.Generic;
using System.Text;

namespace DotsAndBoxes.CustomEventArgs
{
    public class IntEventArgs : EventArgs
    {
        public int ActualTime { get; set; }

        public IntEventArgs(int actualTime)
        {
            ActualTime = actualTime;
        }
    }
}
