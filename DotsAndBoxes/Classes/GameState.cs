using DotsAndBoxes.Converters;
using DotsAndBoxes.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DotsAndBoxes.Classes
{
    //[JsonConverter(typeof(GameStateConverter))]
    class GameState
    {
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public int TurnID { get; set; }
        public int[] Scores { get; set; }

        public List<RectangleStructure> PlacedRectangles { get; set; }
        public List<Line> LineList { get; set; }

        public GameState()
        {
            Player1 = "p1";
            Player2 = "p2";
            TurnID = 0;
            Scores = new int[2];
            PlacedRectangles = new List<RectangleStructure>();
            LineList = new List<Line>();
        }

        public GameStateSerializable ToSerializable()
        {
            GameStateSerializable gameState = new GameStateSerializable();
            gameState.Player1 = Player1;
            gameState.Player2 = Player2;
            gameState.Scores = Scores;
            gameState.Rectangles = PlacedRectangles;
            gameState.LineList = new List<LineStructure>();
            foreach (Line line in LineList)
            {
                LineStructure lineStructure = new LineStructure();
                lineStructure.Point1 = new Point((int)line.X1, (int)line.Y1);
                lineStructure.Point2 = new Point((int)line.X2, (int)line.Y2);
                lineStructure.StrokeColor = line.Stroke.ToString();
                lineStructure.StrokeThickness = line.StrokeThickness;
                gameState.LineList.Add(lineStructure);
            }

            return gameState;
        }

        public void FromSerializable(GameStateSerializable gameState)
        {
            Player1 = gameState.Player1;
            Player2 = gameState.Player2;
            Scores = gameState.Scores;
            foreach (RectangleStructure rectangle in gameState.Rectangles)
            {
                PlacedRectangles.Add(rectangle);
            }
            foreach (LineStructure lineStructure in gameState.LineList)
            {
                Line line = new Line();
                line.X1 = lineStructure.Point1.X;
                line.Y1 = lineStructure.Point1.Y;
                line.X2 = lineStructure.Point2.X;
                line.Y2 = lineStructure.Point2.Y;
                line.Stroke = (Brush)new BrushConverter().ConvertFromString(lineStructure.StrokeColor);
                line.StrokeThickness = lineStructure.StrokeThickness;
                LineList.Add(line);
            }
        }
    }
}
