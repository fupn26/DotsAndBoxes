using System;
using System.Collections.Generic;
using System.Text;

namespace DotsAndBoxes.CustomEventArgs
{
    public class CustomEventArgs<T> : EventArgs
    {
        public T Content { get; private set; }

        public CustomEventArgs(T content)
        {
            Content = content;
        }
    }
}
