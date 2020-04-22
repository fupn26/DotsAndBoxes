﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DotsAndBoxes.CustomEventArgs
{
    public class ListEventArgs<T> : EventArgs
    {
        public List<T> RefList { get; set; }

        public ListEventArgs(List<T> refList)
        {
            RefList = refList;
        }
    }
}