using System.Collections.Generic;

namespace DotsAndBoxes.Structures
{
    public struct GameStateSerializable
    {
        public bool isEnded { get; set; }
        public int Length { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public int TurnID { get; set; }
        public int[] Scores { get; set; }
        public List<RectangleStructure> Rectangles { get; set; }
        public List<LineStructure> LineList { get; set; }
    }
}
