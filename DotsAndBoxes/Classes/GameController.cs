using DotsAndBoxes.CustomEventArgs;
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
    class GameController : StateChecker
    {
        public int EllipseSize { get; private set; }

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

        public int NumberOfRows { get; private set; }
        public int NumberOfColums { get; private set; }
        public int TimeElapsed { get; set; }

        public event EventHandler ScoreChanged;
        public event EventHandler<RectangleEventArgs> RectangleEnclosed;
        public event EventHandler RestartDone;
        public event EventHandler<ListEventArgs<Line>> AITurn;
        public event EventHandler GameEnded;

        private GameState _gameState;
        private AI _ai;

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
            Init(canvasHeight, canvasWidth, gameWidth, gameHeight, diamondView);
        }

        public void Init(double canvasHeight, double canvasWidth, int gameWidth, int gameHeight, bool diamondView)
        {
            _diamondView = diamondView;
            GameWidth = gameHeight;
            GameHeight = gameWidth;
            EllipseSize = 10;
            NumberOfRows = (int)canvasHeight / GameHeight;
            NumberOfColums = (int)canvasWidth / GameWidth;
            _gameState = new GameState();
            _pointList = new List<Point>();
            _prevGameStates = new List<GameStateSerializable>();
            _ai = new AI(GameHeight, GameWidth);
            AITurn += _ai.GameController_AITurn;
            _ai.LineChosen += AI_LineChosen;

            CreateEllipsePositionList();
        }

        private void Restart()
        {
            _gameState = new GameState();
            if (!_prevGameStates[^1].isEnded)
            {
                _prevGameStates.RemoveAt(_prevGameStates.Count - 1);
                DataProvider.WriteJson(_prevGameStates);
            }
            CreateLineList(Brushes.White);
            OnRestartDone();
        }

        private void OnRestartDone()
        {
            RestartDone?.Invoke(this, EventArgs.Empty);
        }

        private bool ReadPreviousState()
        {
            _prevGameStates = DataProvider.ReadJson();
            if (_prevGameStates != null)
            {
                GameStateSerializable gameState = _prevGameStates[^1];
                if (gameState.isEnded)
                {
                    CreateLineList(Brushes.White);
                    return false;
                }
                _gameState.FromSerializable(gameState);
                TimeElapsed = gameState.Length;
                return true;
            }
            else
            {
                CreateLineList(Brushes.White);
                return false;
            }
        }

        private void AI_LineChosen(object sender, LineEventArgs e)
        {
            foreach (Line line in _gameState.LineList)
            {
                if (areEqualLines(line, e.Line))
                {
                    ChangeTurn(line);
                }
            }
        }

        private void RestorePlacedRectangles()
        {
            foreach (RectangleStructure rectangle in _gameState.PlacedRectangles)
            {
                OnRectangleEnclosed(rectangle);
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
                        Line line = new Line
                        {
                            X1 = x1,
                            Y1 = y1,
                            X2 = x2,
                            Y2 = y2,
                            Stroke = brush,
                            StrokeThickness = 8,
                            Cursor = Cursors.Hand
                        };
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
                        Line line = new Line
                        {
                            X1 = x1,
                            Y1 = y1,
                            X2 = x2,
                            Y2 = y2,
                            Stroke = brush,
                            StrokeThickness = 8,
                            Cursor = Cursors.Hand
                        };
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
                        Line line = new Line
                        {
                            X1 = x1,
                            Y1 = y1,
                            X2 = x2,
                            Y2 = y2,
                            Stroke = brush,
                            StrokeThickness = 8,
                            Cursor = Cursors.Hand
                        };
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
                        Line line = new Line
                        {
                            X1 = x1,
                            Y1 = y1,
                            X2 = x2,
                            Y2 = y2,
                            Stroke = brush,
                            StrokeThickness = 8,
                            Cursor = Cursors.Hand
                        };
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
                        Line line = new Line
                        {
                            X1 = x1,
                            Y1 = y1,
                            X2 = x2,
                            Y2 = y2,
                            Stroke = brush,
                            StrokeThickness = 8,
                            Cursor = Cursors.Hand
                        };
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
                        Line line = new Line
                        {
                            X1 = x1,
                            Y1 = y1,
                            X2 = x2,
                            Y2 = y2,
                            Stroke = brush,
                            StrokeThickness = 8,
                            Cursor = Cursors.Hand
                        };
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

        public void Window_SaveGame(object sender, EventArgs e)
        {
            WriteGameState(false);
        }

        private void RestorePreferencesOfLines()
        {
            foreach (Line line in _gameState.LineList)
            {
                line.Cursor = Cursors.Hand;
                line.MouseEnter += this.Line_MouseEnter;
                line.MouseLeave += this.Line_MouseLeave;
                line.MouseLeftButtonDown += this.Line_MouseLeftButtonDown;
            }
        }

        private void ChangeTurn(Line line)
        {
            ColorChosenLine(line);
            Tuple<List<Point>, int> result = CheckState(line, _gameState.LineList);
            _gameState.Scores[TurnId] += result.Item2;
            if (result.Item2 != 0)
            {
                foreach (Point point in result.Item1)
                {
                    CreateNewRectangleStructure(point);
                }

                OnScoreChanged();
                IsGameEnded();
            }
            else
            {
                TurnId = 1 - TurnId;
            }
            if (TurnId == 1)
            {
                OnAiTurn();
            }

        }

        private void OnAiTurn()
        {
            AITurn?.Invoke(this, new ListEventArgs<Line>(_gameState.LineList));
        }

        private void ColorChosenLine(Line line)
        {
            if (!BrushCompare(line.Stroke, Brushes.Black) && !BrushCompare(line.Stroke, Brushes.White))
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
        }

        private void Line_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Line)
            {
                ChangeTurn((Line)sender);
            }
        }

        private void Line_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Line line)
            {
                if (BrushCompare(line.Stroke, Brushes.Black))
                {
                    line.Stroke = Brushes.White;
                }
            }
        }


        private void Line_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Line line)
            {
                if (BrushCompare(line.Stroke, Brushes.White))
                {
                    line.Stroke = Brushes.Black;

                }
            }
        }

        private void CreateNewRectangleStructure(Point point)
        {
            RectangleStructure rectangle = new RectangleStructure
            {
                RefPoint = point,
                Width = GameWidth * 0.85,
                Height = GameHeight * 0.85,
                Fill = Brushes.DarkBlue.ToString(),
                RadiusX = 8,
                RadiusY = 8
            };
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

        private void IsGameEnded()
        {
            foreach (Line line in _gameState.LineList)
            {
                if (BrushCompare(line.Stroke, Brushes.White) || BrushCompare(line.Stroke, Brushes.Black))
                {
                    return;
                }
            }
            OnGameEnded();
        }

        private void WriteGameState(bool isEnded)
        {
            GameStateSerializable stateSerializable = _gameState.ToSerializable();
            stateSerializable.isEnded = isEnded;
            stateSerializable.Length = TimeElapsed;
            _prevGameStates.Add(stateSerializable);
            DataProvider.WriteJson(_prevGameStates);
        }

        private void OnGameEnded()
        {
            WriteGameState(true);
            GameEnded?.Invoke(this, EventArgs.Empty);
        }
    }
}
