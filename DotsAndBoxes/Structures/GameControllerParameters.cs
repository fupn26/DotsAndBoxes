using DotsAndBoxes.Enums;

namespace DotsAndBoxes.Structures
{
    public static class GameControllerParameters
    {
        public static GameType GameType { get; set; }
        public static GameMode GameMode { get; set; }
        public static int GridSize { get; set; }
        public static string Player1 { get; set; }
        public static string Player2 { get; set; }
        public static bool IsNewGame { get; set; }
    }
}