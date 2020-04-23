using DotsAndBoxes.CustomEventArgs;
using DotsAndBoxes.Enums;
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
        public event EventHandler<CustomEventArgs<RectangleStructure>> RectangleEnclosed;
        public event EventHandler RestartDone;
        public event EventHandler<CustomEventArgs<List<LineStructure>>> AITurn;
        public event EventHandler GameEnded;
        public event EventHandler<CustomEventArgs<LineStructure>> LineColored; 

        private GameState _gameState;
        private AI _ai;

        private GameType _gameType;
        private GameMode _gameMode;
        private int _gridSize;

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

        public GameController() { }

        public void Initialize(double canvasHeight, double canvasWidth)
        {
            EllipseSize = 10;
            _pointList = new List<Point>();

            if (GameControllerParameters.IsNewGame)
            {
                StartNewGAme(canvasHeight, canvasWidth);
            }
            else
            {
                ReadPreviousState(canvasWidth, canvasHeight);
            }

        }

        private void StartNewGAme(double canvasHeight, double canvasWidth)
        {
            _gameType = GameControllerParameters.GameType;
            _gameMode = GameControllerParameters.GameMode;
            _gridSize = GameControllerParameters.GridSize;
            _gameState = new GameState()
            {
                GameType = _gameType,
                GameMode = _gameMode,
                GridSize = _gridSize,
                Player1 = GameControllerParameters.Player1,
                Player2 = GameControllerParameters.Player2
            };
            GameWidth = (int)canvasWidth / GameControllerParameters.GridSize;
            GameHeight = GameWidth;
            NumberOfRows = GameControllerParameters.GridSize;
            NumberOfColums = NumberOfRows;

            CreateAi();

            CreateEllipsePositionList();
            CreateLineList(Brushes.White.ToString());
        }

        private void CreateAi()
        {
            if (_gameMode == GameMode.SINGLE)
            {
                _ai = new AI(GameHeight, GameWidth);
                AITurn += _ai.GameController_AITurn;
                _ai.LineChosen += AI_LineChosen;

            }
        }

        private void Restart()
        {
            _gameState = new GameState();
            if (DataProvider.GameStates.Count != 0 &&
                !DataProvider.GameStates[^1].IsEnded)
            {
                DataProvider.RemoveLastElement();
                DataProvider.CommitChanges();
            }
            CreateLineList(Brushes.White.ToString());
            OnRestartDone();
        }

        private void OnRestartDone()
        {
            RestartDone?.Invoke(this, EventArgs.Empty);
        }

        private void ReadPreviousState(double canvasHeight, double canvasWidth)
        {
            GameState gameState = DataProvider.GameStates[^1];
            _gameState = gameState;
            TimeElapsed = gameState.Length;
            _gameType = _gameState.GameType;
            _gameMode = _gameState.GameMode;
            _gridSize = _gameState.GridSize;
            GameWidth = (int)canvasWidth / _gridSize;
            GameHeight = GameWidth;
            EllipseSize = 10;
            NumberOfRows = _gridSize;
            NumberOfColums = NumberOfRows;

            CreateAi();

            CreateEllipsePositionList();

            RestorePlacedRectangles();
        }

        private void AI_LineChosen(object sender, CustomEventArgs<LineStructure> e)
        {
            foreach (LineStructure line in _gameState.LineList)
            {
                if (AreEqualLines(line, e.Content))
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
            if (_gameType == GameType.CLASSIC)
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
            if (_gameType == GameType.CLASSIC)
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
            //ReadPreviousState();
        }

        public void Window_SaveGame(object sender, EventArgs e)
        {
            WriteGameState(false);
        }

        public void Window_LineClicked(object seder, CustomEventArgs<LineStructure> e)
        {
            foreach(LineStructure line in _gameState.LineList)
            {
                if (AreEqualLines(line, e.Content))
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

            TurnId = 1 - TurnId;

            if (_ai != null && TurnId == 1)
            {
                OnAiTurn();
            }

        }

        private void OnAiTurn()
        {
            AITurn?.Invoke(this, new CustomEventArgs<List<LineStructure>>(_gameState.LineList));
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
            LineColored?.Invoke(this, new CustomEventArgs<LineStructure>(line));
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
            RectangleEnclosed?.Invoke(this, new CustomEventArgs<RectangleStructure>(rectangle));
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

            DataProvider.AddElement(_gameState);
            DataProvider.CommitChanges();
        }

        private void OnGameEnded()
        {
            WriteGameState(true);
            GameEnded?.Invoke(this, EventArgs.Empty);
        }
    }
}
