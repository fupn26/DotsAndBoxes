using DotsAndBoxes.Structures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DotsAndBoxes.Classes
{
    class GameController
    {
        private const string fileName = "saved_game.json";
        public int GameWidth { get; }
        public int GameHeight { get; }
        public int EllipseSize { get; }

        public int TurnId
        {
            get
            {
                return _gameState.TurnID;
            }
            private set
            {
                _gameState.TurnID = value;
            }
        }

        public ReadOnlyCollection<int> Scores { 
            get
            {
                return new ReadOnlyCollection<int>(_gameState.Scores);
            }
        }

        public int NumberOfRows { get; }
        public int NumberOfColums { get; }

        public event EventHandler ScoreChanged;
        public event EventHandler<RectangleEventArgs> RectangleEnclosed;
        public event EventHandler RestartDone;

        private GameState _gameState;

        private bool _diamondView;

        private List<Point> _pointList;

        private List<GameStateSerializable> _prevGameStates;
        public ReadOnlyCollection<Point> PointList { 
            get
            {
                return new ReadOnlyCollection<Point>(_pointList);
            } 
        }
        public ReadOnlyCollection<Line> LineList { get
            {
                return new ReadOnlyCollection<Line>(_gameState.LineList);
            } 
        }

        public GameController(double canvasHeight, double canvasWidth, int gameWidth, int gameHeight, bool diamondView)
        {
            _diamondView = diamondView;
            GameWidth = gameHeight;
            GameHeight = gameWidth;
            EllipseSize = 10;
            NumberOfRows = (int)canvasHeight / GameHeight;
            NumberOfColums = (int)canvasWidth / GameWidth;
            Init();
        }

        private void Init()
        {
            _gameState = new GameState();
            _pointList = new List<Point>();
            _prevGameStates = new List<GameStateSerializable>();
            CreateEllipsePositionList();
        }

        private void Restart()
        {
            _gameState = new GameState();
            CreateLineList(Brushes.White);
            OnRestartDone();
        }

        private void OnRestartDone()
        {
            RestartDone?.Invoke(this, EventArgs.Empty);
        }

        private bool ReadPreviousState()
        {
            if (File.Exists(fileName))
            {
                string jsonString = File.ReadAllText(fileName);
                _prevGameStates = JsonSerializer.Deserialize<List<GameStateSerializable>>(jsonString);
                GameStateSerializable gameState = _prevGameStates[_prevGameStates.Count - 1];
                if (gameState.isEnded)
                {
                    CreateLineList(Brushes.White);
                    return false;
                }
                _gameState.FromSerializable(gameState);
                return true;
            }
            else
            {
                CreateLineList(Brushes.White);
                return false;
            }
        }

        private void RestorePlacedRectangles()
        {
            foreach (RectangleStructure rectangle in _gameState.PlacedRectangles)
            {
                OnRectangleEnclosed(rectangle);
            }
        }

        private int MyModulo(int a, int b)
        {
            if (a < b)
            {
                return b % a;
            }
            else
            {
                return a % b;
            }
        }
        public void CreateEllipsePositionList()
        {
            if (_diamondView)
            {
                int offset = (NumberOfColums - 1) / 2; 
                for (int i = 0; i <= NumberOfRows/2; ++i)
                {
                    for (int j = offset-i; j <= NumberOfColums - (offset - i); ++j)
                    {
                        Point point = new Point(j * GameWidth, i * GameHeight);

                        _pointList.Add(point);
                    }
                }
                for (int i = NumberOfRows / 2 + 1; i <= NumberOfRows; ++i)
                {
                    for (int j = i - offset - 1; j <= NumberOfColums - (i - offset - 1); ++j)
                    {
                        Point point = new Point(j * GameWidth, i * GameHeight);

                        _pointList.Add(point);
                    }
                }
            }
            else
            {
                for (int i = 0; i <= NumberOfRows; ++i)
                {
                    for (int j = 0; j <= NumberOfColums; ++j)
                    {
                        Point point = new Point(j * GameWidth, i * GameHeight);

                        _pointList.Add(point);
                    }
                }

            }
        }

        public void CreateLineList(Brush brush)
        {
            if (!_diamondView)
            {
                for (int i = 0; i <= NumberOfRows; ++i)
                {
                    for (int j = 0; j < NumberOfColums; ++j)
                    {
                        int x1 = j * GameWidth;
                        int y1 = i * GameHeight;
                        int x2 = x1 + GameWidth;
                        int y2 = y1;
                        Line line = new Line();
                        line.X1 = x1;
                        line.Y1 = y1;
                        line.X2 = x2;
                        line.Y2 = y2;
                        line.Stroke = brush;
                        line.StrokeThickness = 8;
                        line.Cursor = Cursors.Hand;
                        line.MouseEnter += Line_MouseEnter;
                        line.MouseLeave += Line_MouseLeave;
                        line.MouseLeftButtonDown += Line_MouseLeftButtonDown;
                        _gameState.LineList.Add(line);
                    }
                }

                for (int i = 0; i < NumberOfRows; ++i)
                {
                    for (int j = 0; j <= NumberOfColums; ++j)
                    {
                        int x1 = j * GameWidth;
                        int y1 = i * GameHeight;
                        int x2 = x1;
                        int y2 = y1 + GameHeight;
                        Line line = new Line();
                        line.X1 = x1;
                        line.Y1 = y1;
                        line.X2 = x2;
                        line.Y2 = y2;
                        line.Stroke = brush;
                        line.StrokeThickness = 8;
                        line.Cursor = Cursors.Hand;
                        line.MouseEnter += Line_MouseEnter;
                        line.MouseLeave += Line_MouseLeave;
                        line.MouseLeftButtonDown += Line_MouseLeftButtonDown;
                        _gameState.LineList.Add(line);
                    }
                }
            }
            else
            {
                int offset = (NumberOfColums - 1) / 2;
                for (int i = 0; i <= NumberOfRows / 2; ++i)
                {
                    for (int j = offset-i; j < NumberOfColums-(offset-i); ++j)
                    {
                        int x1 = j * GameWidth;
                        int y1 = i * GameHeight;
                        int x2 = x1 + GameWidth;
                        int y2 = y1;
                        Line line = new Line();
                        line.X1 = x1;
                        line.Y1 = y1;
                        line.X2 = x2;
                        line.Y2 = y2;
                        line.Stroke = brush;
                        line.StrokeThickness = 8;
                        line.Cursor = Cursors.Hand;
                        line.MouseEnter += Line_MouseEnter;
                        line.MouseLeave += Line_MouseLeave;
                        line.MouseLeftButtonDown += Line_MouseLeftButtonDown;
                        _gameState.LineList.Add(line);
                    }
                }

                for (int i = 0; i < NumberOfRows/2; ++i)
                {
                    for (int j = offset-i; j <= NumberOfColums - (offset-i); ++j)
                    {
                        int x1 = j * GameWidth;
                        int y1 = i * GameHeight;
                        int x2 = x1;
                        int y2 = y1 + GameHeight;
                        Line line = new Line();
                        line.X1 = x1;
                        line.Y1 = y1;
                        line.X2 = x2;
                        line.Y2 = y2;
                        line.Stroke = brush;
                        line.StrokeThickness = 8;
                        line.Cursor = Cursors.Hand;
                        line.MouseEnter += Line_MouseEnter;
                        line.MouseLeave += Line_MouseLeave;
                        line.MouseLeftButtonDown += Line_MouseLeftButtonDown;
                        _gameState.LineList.Add(line);
                    }
                }

                for (int i = NumberOfRows / 2 + 1; i <= NumberOfRows; ++i)
                {
                    for (int j = i - offset - 1; j < NumberOfColums - (i - offset-1); ++j)
                    {
                        int x1 = j * GameWidth;
                        int y1 = i * GameHeight;
                        int x2 = x1 + GameWidth;
                        int y2 = y1;
                        Line line = new Line();
                        line.X1 = x1;
                        line.Y1 = y1;
                        line.X2 = x2;
                        line.Y2 = y2;
                        line.Stroke = brush;
                        line.StrokeThickness = 8;
                        line.Cursor = Cursors.Hand;
                        line.MouseEnter += Line_MouseEnter;
                        line.MouseLeave += Line_MouseLeave;
                        line.MouseLeftButtonDown += Line_MouseLeftButtonDown;
                        _gameState.LineList.Add(line);


                    }
                }
                for (int i = NumberOfRows / 2; i <= NumberOfRows; ++i)
                {
                    for (int j = i - offset; j < NumberOfColums - (i - offset - 1); ++j)
                    {
                        int x1 = j * GameWidth;
                        int y1 = i * GameHeight;
                        int x2 = x1;
                        int y2 = y1 + GameHeight;
                        Line line = new Line();
                        line.X1 = x1;
                        line.Y1 = y1;
                        line.X2 = x2;
                        line.Y2 = y2;
                        line.Stroke = brush;
                        line.StrokeThickness = 8;
                        line.Cursor = Cursors.Hand;
                        line.MouseEnter += Line_MouseEnter;
                        line.MouseLeave += Line_MouseLeave;
                        line.MouseLeftButtonDown += Line_MouseLeftButtonDown;
                        _gameState.LineList.Add(line);

                    }
                }

            }
        }

        public void Window_InitScore(object sender, EventArgs e)
        {
            OnScoreChanged();
        }

        public void Windows_RestartGame(object sender, EventArgs e)
        {
            Restart();
        }

        public void Window_RestoreState(object sender, EventArgs e)
        {
            if (ReadPreviousState())
            {
                RestorePreferencesOfLines();
                RestorePlacedRectangles();
            }
        }

        private void RestorePreferencesOfLines()
        {
            foreach (Line line in _gameState.LineList)
            {
                line.Cursor = Cursors.Hand;
                line.MouseEnter += Line_MouseEnter;
                line.MouseLeave += Line_MouseLeave;
                line.MouseLeftButtonDown += Line_MouseLeftButtonDown;
            }
        }

        private void Line_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Line)
            {
                Line line = (Line)sender;
                if (line.Stroke != Brushes.Black)
                {
                    return;
                }
                if (TurnId == 0)
                {
                    line.Stroke = Brushes.DarkBlue;
                }
                else
                {
                    line.Stroke = Brushes.DarkRed;
                }
                CheckState(line);

            }
        }

        private void Line_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Line)
            {
                Line line = (Line)sender;
                if (line.Stroke == Brushes.Black)
                {
                    line.Stroke = Brushes.White;
                }
            }
        }

        private void Line_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Line)
            {
                Line line = (Line)sender;
                if (line.Stroke == Brushes.White)
                {
                    line.Stroke = Brushes.Black;

                }
            }
        }

        private void CreateNewRectangleStructure(Point point)
        {
            RectangleStructure rectangle = new RectangleStructure();
            rectangle.RefPoint = point;
            rectangle.Width = GameWidth * 0.85;
            rectangle.Height = GameHeight * 0.85;
            rectangle.Fill = Brushes.DarkBlue.ToString();
            rectangle.RadiusX = 8;
            rectangle.RadiusY = 8;
            if (_gameState.TurnID == 0)
            {
                rectangle.Fill = Brushes.DarkBlue.ToString();

            }
            else
            {
                rectangle.Fill = Brushes.DarkRed.ToString();
            }
            _gameState.PlacedRectangles.Add(rectangle);
            OnRectangleEnclosed(rectangle);
        }

        private void OnScoreChanged()
        {
            ScoreChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnRectangleEnclosed(RectangleStructure rectangle)
        {
            RectangleEnclosed?.Invoke(this, new RectangleEventArgs(rectangle));
        }

        private bool areEqualLines(Line line1, Line line2)
        {
            if (line1.X1 == line2.X1 && line1.Y1 == line2.Y1 && line1.X2 == line2.X2 && line1.Y2 == line2.Y2 ||
                line1.X1 == line2.X2 && line1.Y1 == line2.Y2 && line1.X2 == line2.X1 && line1.Y2 == line2.Y1)
            {
                return true;
            }
            return false;
        }

        private Line moveUp(Line line)
        {
            Line newLine = new Line();
            newLine.X1 = line.X1;
            newLine.X2 = line.X2;
            newLine.Y1 = line.Y1 + GameHeight;
            newLine.Y2 = line.Y2 + GameHeight;
            return newLine;
        }
        private Line moveDown(Line line)
        {
            Line newLine = new Line();
            newLine.X1 = line.X1;
            newLine.X2 = line.X2;
            newLine.Y1 = line.Y1 - GameHeight;
            newLine.Y2 = line.Y2 - GameHeight;
            return newLine;
        }

        private Line moveRight(Line line)
        {
            Line newLine = new Line();
            newLine.X1 = line.X1 + GameWidth;
            newLine.X2 = line.X2 + GameWidth;
            newLine.Y1 = line.Y1;
            newLine.Y2 = line.Y2;
            return newLine;
        }

        private Line moveLeft(Line line)
        {
            Line newLine = new Line();
            newLine.X1 = line.X1 - GameWidth;
            newLine.X2 = line.X2 - GameWidth;
            newLine.Y1 = line.Y1;
            newLine.Y2 = line.Y2;
            return newLine;
        }

        public void CheckState(Line line)
        {
            int counter = 0;
            if (line.X1 == line.X2)
            {
                foreach (Line element in LineList)
                {
                    if (areEqualLines(moveLeft(line), element) &&
                        element.Stroke != Brushes.Black &&
                        element.Stroke != Brushes.White)
                    {
                        counter += isSquare(line, element, true);
                    }
                    else if (areEqualLines(moveRight(line), element) &&
                        element.Stroke != Brushes.Black &&
                        element.Stroke != Brushes.White)
                    {
                        counter += isSquare(line, element, true);
                    }
                }
            }
            else
            {
                foreach (Line element in LineList)
                {
                    if (areEqualLines(moveUp(line), element) &&
                        element.Stroke != Brushes.Black &&
                        element.Stroke != Brushes.White)
                    {
                        counter += isSquare(line, element, false);
                    }
                    else if (areEqualLines(moveDown(line), element) &&
                        element.Stroke != Brushes.Black &&
                        element.Stroke != Brushes.White)
                    {
                        counter += isSquare(line, element, false);
                    }
                }
            }
            _gameState.Scores[TurnId] += counter;
            if (counter == 0)
            {
                TurnId = 1 - TurnId;
                return;
            }
            OnScoreChanged();
            isGameEnded();

        }

        private void isGameEnded()
        {
            foreach (Line line in _gameState.LineList)
            {
                if (line.Stroke == Brushes.White || line.Stroke == Brushes.Black)
                {
                    return;
                }
            }
            OnGameEnded();
        }

        private void OnGameEnded()
        {
            GameStateSerializable stateSerializable = _gameState.ToSerializable();
            stateSerializable.isEnded = true;
            _prevGameStates.Add(stateSerializable);
            string jsonString;
            string v = JsonSerializer.Serialize<List<GameStateSerializable>>(_prevGameStates);
            jsonString = v;
            File.WriteAllText(fileName, jsonString);
        }

        private Point minPoint(Point p1, Point p2, Point p3, Point p4)
        {
            Point minp = p1;
            if (p2.X < minp.X || p2.Y < minp.Y)
            {
                minp = p2;
            }
            if (p3.X < minp.X || p3.Y < minp.Y)
            {
                minp = p3;
            }
            if (p4.X < minp.X || p4.Y < minp.Y)
            {
                minp = p4;
            }
            return minp;
        }

        private bool isPointsOfLine(Line line, Point p1, Point p2)
        {
            Point lineP1 = new Point((int)line.X1, (int)line.Y1);
            Point lineP2 = new Point((int)line.X2, (int)line.Y2);

            if (lineP1.Equals(p1) && lineP2.Equals(p2) ||
                lineP1.Equals(p2) && lineP2.Equals(p1))
            {
                return true;
            }

            return false;
        }

        private int isSquare(Line line1, Line line2, bool isVertical)
        {
            Point point1 = new Point((int)line1.X1, (int)line1.Y1);
            Point point2 = new Point((int)line1.X2, (int)line1.Y2);
            Point point3 = new Point((int)line2.X1, (int)line2.Y1);
            Point point4 = new Point((int)line2.X2, (int)line2.Y2);

            int counter = 0;
            if (isVertical)
            {
                foreach (Line element in LineList)
                {
                    if (element.Y1 == element.Y2)
                    {
                        if (isPointsOfLine(element, point1, point3) &&
                            element.Stroke != Brushes.Black &&
                            element.Stroke != Brushes.White)
                        {
                            counter++;
                        }
                        else if (isPointsOfLine(element, point2, point4) &&
                            element.Stroke != Brushes.Black &&
                            element.Stroke != Brushes.White)
                        {
                            counter++;
                        }
                    }
                }
            }
            else
            {
                foreach (Line element in LineList)
                {
                    if (element.X1 == element.X2)
                    {
                        if (isPointsOfLine(element, point1, point3) &&
                            element.Stroke != Brushes.Black &&
                            element.Stroke != Brushes.White)
                        {
                            counter++;
                        }
                        else if (isPointsOfLine(element, point2, point4) &&
                            element.Stroke != Brushes.Black &&
                            element.Stroke != Brushes.White)
                        {
                            counter++;
                        }
                    }
                }
            }

            if (counter != 2)
            {
                return 0;
            }
            else
            {
                Point point = minPoint(point1, point2, point3, point4);
                CreateNewRectangleStructure(point);
                return 1;
            }
        }
    }
}
