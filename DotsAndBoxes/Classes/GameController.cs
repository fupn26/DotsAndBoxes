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
    public class GameController : StateChecker
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
        public event EventHandler<ListEventArgs<LineStructure>> AITurn;
        public event EventHandler GameEnded;
        public event EventHandler<LineEventArgs> LineColored; 

        private GameState _gameState;
        private AI _ai;

        private bool _isClassisView;

        private List<Point> _pointList;

        private List<GameState> _prevGameStates;
        public ReadOnlyCollection<Point> PointList { 
            get
            {
                return new ReadOnlyCollection<Point>(_pointList);
            } 
        }
        public ReadOnlyCollection<LineStructure> LineList { get
            {
                return new ReadOnlyCollection<LineStructure>(_gameState.LineList);
            } 
        }

        public GameController(double canvasHeight, double canvasWidth, int gameWidth, int gameHeight, bool diamondView)
        {
            Init(canvasHeight, canvasWidth, gameWidth, gameHeight, diamondView);
        }

        public void Init(double canvasHeight, double canvasWidth, int gameWidth, int gameHeight, bool isClassicView)
        {
            _isClassisView = isClassicView;
            GameWidth = gameHeight;
            GameHeight = gameWidth;
            EllipseSize = 10;
            NumberOfRows = (int)canvasHeight / GameHeight;
            NumberOfColums = (int)canvasWidth / GameWidth;
            _gameState = new GameState();
            _pointList = new List<Point>();
            _prevGameStates = new List<GameState>();
            _ai = new AI(GameHeight, GameWidth);
            AITurn += _ai.GameController_AITurn;
            _ai.LineChosen += AI_LineChosen;

            CreateEllipsePositionList();
        }

        private void Restart()
        {
            _gameState = new GameState();
            if (!_prevGameStates[^1].IsEnded)
            {
                _prevGameStates.RemoveAt(_prevGameStates.Count - 1);
                DataProvider.WriteJson(_prevGameStates);
            }
            CreateLineList(Brushes.White.ToString());
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
                GameState gameState = _prevGameStates[^1];
                if (gameState.IsEnded)
                {
                    CreateLineList(Brushes.White.ToString());
                    return false;
                }
                _gameState = gameState;
                TimeElapsed = gameState.Length;
                return true;
            }
            else
            {
                _prevGameStates = new List<GameState>();
                CreateLineList(Brushes.White.ToString());
                return false;
            }
        }

        private void AI_LineChosen(object sender, LineEventArgs e)
        {
            foreach (LineStructure line in _gameState.LineList)
            {
                if (AreEqualLines(line, e.Line))
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
            if (_isClassisView)
            {
                CreateEllipsePositionListForClassicView();
            }
            else
            {
                CreateEllipsePositionListForDiamondView();
            }
        }

        private void CreateEllipsePositionListForDiamondView()
        {
            int offset = (NumberOfColums - 1) / 2;
            for (int i = 0; i <= NumberOfRows / 2; ++i)
            {
                for (int j = offset - i; j <= NumberOfColums - (offset - i); ++j)
                {
                    AddPoint(j, i);
                }
            }
            for (int i = NumberOfRows / 2 + 1; i <= NumberOfRows; ++i)
            {
                for (int j = i - offset - 1; j <= NumberOfColums - (i - offset - 1); ++j)
                {
                    AddPoint(j, i);
                }
            }
        }

        private void CreateEllipsePositionListForClassicView()
        {
            for (int i = 0; i <= NumberOfRows; ++i)
            {
                for (int j = 0; j <= NumberOfColums; ++j)
                {
                    AddPoint(j, i);
                }
            }

        }

        private void AddPoint(int positionX, int positionY)
        {
            Point point = new Point(positionX * GameWidth, positionY * GameHeight);
            _pointList.Add(point);

        }

        public void CreateLineList(string brush)
        {
            if (_isClassisView)
            {
                CreateClassicGrid(brush);
            }
            else
            {
                CreateDiamonGrid(brush);
            }
        }

        private void CreateClassicGrid(string brush)
        {
            for (int i = 0; i <= NumberOfRows; ++i)
            {
                for (int j = 0; j < NumberOfColums; ++j)
                {
                    AddHorizontalLine(j, i, brush);
                }
            }

            for (int i = 0; i < NumberOfRows; ++i)
            {
                for (int j = 0; j <= NumberOfColums; ++j)
                {
                    AddVerticalLine(j, i, brush);
                }
            }
        }

        private void CreateDiamonGrid(string brush)
        {
            int offset = (NumberOfColums - 1) / 2;
            for (int i = 0; i <= NumberOfRows / 2; ++i)
            {
                for (int j = offset - i; j < NumberOfColums - (offset - i); ++j)
                {
                    AddHorizontalLine(j, i, brush);
                }
            }

            for (int i = 0; i < NumberOfRows / 2; ++i)
            {
                for (int j = offset - i; j <= NumberOfColums - (offset - i); ++j)
                {
                    AddVerticalLine(j, i, brush);
                }
            }

            for (int i = NumberOfRows / 2 + 1; i <= NumberOfRows; ++i)
            {
                for (int j = i - offset - 1; j < NumberOfColums - (i - offset - 1); ++j)
                {
                    AddHorizontalLine(j, i, brush);
                }
            }
            for (int i = NumberOfRows / 2; i <= NumberOfRows; ++i)
            {
                for (int j = i - offset; j < NumberOfColums - (i - offset - 1); ++j)
                {
                    AddVerticalLine(j, i, brush);
                }
            }

        }


        private void AddHorizontalLine(int positionX, int positionY, string brush)
        {
            int x1 = positionX * GameWidth;
            int y1 = positionY * GameHeight;
            int x2 = x1 + GameWidth;
            int y2 = y1;
            LineStructure line = new LineStructure
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                StrokeColor = brush,
                StrokeThickness = 8
            };
            _gameState.LineList.Add(line);
        }

        private void AddVerticalLine(int positionX, int positionY, string brush)
        {
            int x1 = positionX * GameWidth;
            int y1 = positionY * GameHeight;
            int x2 = x1;
            int y2 = y1 + GameHeight;
            LineStructure line = new LineStructure
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                StrokeColor = brush,
                StrokeThickness = 8
            };
            _gameState.LineList.Add(line);
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
                //RestorePreferencesOfLines();
                RestorePlacedRectangles();
            }
        }

        public void Window_SaveGame(object sender, EventArgs e)
        {
            WriteGameState(false);
        }

        public void Window_LineClicked(object seder, LineEventArgs e)
        {
            foreach(LineStructure line in _gameState.LineList)
            {
                if (AreEqualLines(line, e.Line))
                {
                    ChangeTurn(line);
                    break;
                }
            }
        }

        private void ChangeTurn(LineStructure line)
        {
            if (IsLineColored(line))
            {
                return;
            }
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
            //else
            //{
            //    TurnId = 1 - TurnId;
            //}

            TurnId = 1 - TurnId;

            if (TurnId == 1)
            {
                OnAiTurn();
            }

        }

        private void OnAiTurn()
        {
            AITurn?.Invoke(this, new ListEventArgs<LineStructure>(_gameState.LineList));
        }

        private void ColorChosenLine(LineStructure line)
        {
            if (TurnId == 0)
            {
                line.StrokeColor = Brushes.DarkBlue.ToString();
            }
            else
            {
                line.StrokeColor = Brushes.DarkRed.ToString();
            }

            OnLineColored(line);
        }

        private void OnLineColored(LineStructure line)
        {
            LineColored?.Invoke(this, new LineEventArgs(line));
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
            foreach (LineStructure line in _gameState.LineList)
            {
                if (!IsLineColored(line))
                {
                    return;
                }
            }
            OnGameEnded();
        }

        private void WriteGameState(bool isEnded)
        {
            _gameState.IsEnded = isEnded;
            _gameState.Length = TimeElapsed;
            _prevGameStates.Add(_gameState);
            DataProvider.WriteJson(_prevGameStates);
        }

        private void OnGameEnded()
        {
            WriteGameState(true);
            GameEnded?.Invoke(this, EventArgs.Empty);
        }
    }
}
