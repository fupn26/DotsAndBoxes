using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;

namespace DotsAndBoxes.Classes
{
    class GameState
    {
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public int TurnID { get; set; }
        public int[] Scores { get; set; }

        public List<Line> Lines { get; set; }
        //public List<Rectangle> Rectangles { get; set; }
        public List<Line> LineList { get; set; }

        public GameState()
        {
            TurnID = 0;
            Scores = new int[2];
            LineList = new List<Line>();
        }
    }
}
