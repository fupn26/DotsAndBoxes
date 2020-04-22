using System;
using System.Collections.Generic;
using System.Text;

namespace DotsAndBoxes.CustomEventArgs
{
    public class StringEventArgs : EventArgs
    {
        public string text { get; set; }

        public StringEventArgs(string text)
        {
            this.text = text;
        }
    }
}
