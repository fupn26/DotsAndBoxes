using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Shapes;

namespace DotsAndBoxes.Classes
{
    class GameController
    {
        private const int GameWidth = 100;
        private const int GameHeight = 100;
        private const int EllipseSize = 10;


        public readonly int NumberOfRows;
        private readonly int NumberOfColums;


        public List<Point> PointList { get; set; }
        public List<Line> LineList { get; set; }

    }
}
