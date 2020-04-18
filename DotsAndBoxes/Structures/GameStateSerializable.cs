using System;
using System.Collections.Generic;
using System.Text;

namespace DotsAndBoxes.Structures
{
    struct GameStateSerializable
    {
        public bool isEnded { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public int TurnID { get; set; }
        public int[] Scores { get; set; }
        public List<RectangleStructure> Rectangles { get; set; }
        public List<LineStructure> LineList { get; set; }
    }
}
