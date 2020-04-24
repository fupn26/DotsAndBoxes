using System;

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
