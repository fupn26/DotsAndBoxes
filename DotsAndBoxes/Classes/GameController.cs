using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Media;
using DotsAndBoxes.CustomEventArgs;
using DotsAndBoxes.Enums;
using DotsAndBoxes.Structures;

namespace DotsAndBoxes.Classes
{
    public class GameController : StateChecker
    {
        private Ai _ai;
        private GameMode _gameMode;

        private GameState _gameState;

        private GameType _gameType;
        private int _gridSize;

        private List<Point> _pointList;
        private Random _random;
        public int EllipseSize { get; private set; }

        public int TurnId
        {
            get => _gameState.TurnId;
            private set => _gameState.TurnId = value;
        }

        public ReadOnlyCollection<int> Scores => new ReadOnlyCollection<int>(_gameState.Scores);

        public int NumberOfRows { get; private set; }
        public int NumberOfColums { get; private set; }
        public int TimeElapsed { get; set; }

        public ReadOnlyCollection<Point> PointList => new ReadOnlyCollection<Point>(_pointList);

        public ReadOnlyCollection<LineStructure> LineList => new ReadOnlyCollection<LineStructure>(_gameState.LineList);

        public event EventHandler<CustomEventArgs<List<LineStructure>>> AiTurn;

        public void CreateEllipsePositionList()
        {
            if (_gameType == GameType.Classic)
                CreateEllipsePositionListForClassicView();
            else
                CreateEllipsePositionListForDiamondView();
        }

        public void CreateLineList(string brush)
        {
            if (_gameType == GameType.Classic)
                CreateClassicGrid(brush);
            else
                CreateDiamonGrid(brush);
        }

        public event EventHandler GameEnded;

        public void Initialize(double canvasHeight, double canvasWidth)
        {
            _random = new Random();
            EllipseSize = 10;
            _pointList = new List<Point>();

            if (GameControllerParameters.IsNewGame)
                StartNewGame(canvasWidth);
            else
                ReadPreviousState(canvasHeight);
            OnPlayerNameSet();
        }

        public event EventHandler<CustomEventArgs<LineStructure>> LineColored;
        public event EventHandler<CustomEventArgs<ReadOnlyCollection<string>>> PlayerNameSet;
        public event EventHandler<CustomEventArgs<List<RectangleStructure>>> RectangleEnclosed;
        public event EventHandler RestartDone;

        public event EventHandler ScoreChanged;

        public void Window_InitScore(object sender, EventArgs e)
        {
            OnScoreChanged();
        }

        public void Window_LineClicked(object seder, CustomEventArgs<LineStructure> e)
        {
            foreach (var line in _gameState.LineList)
                if (AreEqualLines(line, e.Content))
                {
                    ModifyGrid(line);
                    break;
                }
        }

        public void Window_SaveGame(object sender, EventArgs e)
        {
            WriteGameState(false);
        }

        public void Windows_RestartGame(object sender, EventArgs e)
        {
            Restart();
        }


        private void AddHorizontalLine(int positionX, int positionY, string brush)
        {
            var x1 = positionX * GameWidth;
            var y1 = positionY * GameHeight;
            var x2 = x1 + GameWidth;
            var y2 = y1;
            var line = new LineStructure
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

        private void AddPoint(int positionX, int positionY)
        {
            var point = new Point(positionX * GameWidth, positionY * GameHeight);
            _pointList.Add(point);
        }

        private void AddVerticalLine(int positionX, int positionY, string brush)
        {
            var x1 = positionX * GameWidth;
            var y1 = positionY * GameHeight;
            var x2 = x1;
            var y2 = y1 + GameHeight;
            var line = new LineStructure
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

        private void AI_LineChosen(object sender, CustomEventArgs<LineStructure> e)
        {
            foreach (var line in _gameState.LineList)
                if (AreEqualLines(line, e.Content))
                {
                    ModifyGrid(line);
                    break;
                }
        }

        private void ChangeTurn()
        {
            TurnId = 1 - TurnId;
        }

        private void ColorChosenLine(LineStructure line)
        {
            if (TurnId == 0)
                line.StrokeColor = Brushes.DarkBlue.ToString();
            else
                line.StrokeColor = Brushes.DarkRed.ToString();

            OnLineColored(line);
        }

        private void CreateAi()
        {
            if (_gameMode == GameMode.Single)
            {
                _ai = new Ai(GameHeight, GameWidth);
                AiTurn += _ai.GameController_AITurn;
                _ai.LineChosen += AI_LineChosen;
            }
        }

        private void CreateClassicGrid(string brush)
        {
            for (var i = 0; i <= NumberOfRows; ++i)
            for (var j = 0; j < NumberOfColums; ++j)
                AddHorizontalLine(j, i, brush);

            for (var i = 0; i < NumberOfRows; ++i)
            for (var j = 0; j <= NumberOfColums; ++j)
                AddVerticalLine(j, i, brush);
        }

        private void CreateDiamonGrid(string brush)
        {
            var offset = (NumberOfColums - 1) / 2;

            for (var i = 0; i <= NumberOfRows / 2; ++i)
            for (var j = offset - i; j < NumberOfColums - (offset - i); ++j)
                AddHorizontalLine(j, i, brush);

            for (var i = 0; i < NumberOfRows / 2; ++i)
            for (var j = offset - i; j <= NumberOfColums - (offset - i); ++j)
                AddVerticalLine(j, i, brush);

            for (var i = NumberOfRows / 2 + 1; i <= NumberOfRows; ++i)
            for (var j = i - offset - 1; j < NumberOfColums - (i - offset - 1); ++j)
                AddHorizontalLine(j, i, brush);

            for (var i = NumberOfRows / 2; i <= NumberOfRows; ++i)
            for (var j = i - offset; j < NumberOfColums - (i - offset - 1); ++j)
                AddVerticalLine(j, i, brush);
        }

        private void CreateEllipsePositionListForClassicView()
        {
            for (var i = 0; i <= NumberOfRows; ++i)
            for (var j = 0; j <= NumberOfColums; ++j)
                AddPoint(j, i);
        }

        private void CreateEllipsePositionListForDiamondView()
        {
            var offset = (NumberOfColums - 1) / 2;
            for (var i = 0; i <= NumberOfRows / 2; ++i)
            for (var j = offset - i; j <= NumberOfColums - (offset - i); ++j)
                AddPoint(j, i);
            for (var i = NumberOfRows / 2 + 1; i <= NumberOfRows; ++i)
            for (var j = i - offset - 1; j <= NumberOfColums - (i - offset - 1); ++j)
                AddPoint(j, i);
        }

        private void CreateNewGameState()
        {
            _gameState = new GameState
            {
                GameType = _gameType,
                GameMode = _gameMode,
                GridSize = _gridSize,
                TurnId = _random.Next(2),
                Player1 = GameControllerParameters.Player1,
                Player2 = GameControllerParameters.Player2
            };
            if (string.IsNullOrEmpty(_gameState.Player2)) _gameState.Player2 = "AI";
        }

        private RectangleStructure CreateNewRectangleStructure(Point point)
        {
            var rectangle = new RectangleStructure
            {
                RefPoint = point,
                Width = GameWidth * 0.85,
                Height = GameHeight * 0.85,
                Fill = Brushes.DarkBlue.ToString(),
                RadiusX = 8,
                RadiusY = 8
            };
            if (_gameState.TurnId == 0)
                rectangle.Fill = Brushes.DarkBlue.ToString();
            else
                rectangle.Fill = Brushes.DarkRed.ToString();
            _gameState.PlacedRectangles.Add(rectangle);
            return rectangle;
        }

        private void InitTurn()
        {
            if (TurnId == 1 && _ai != null) OnAiTurn();
        }

        private void IsGameEnded()
        {
            foreach (var line in _gameState.LineList)
                if (!IsLineColored(line))
                {
                    if (_ai != null && TurnId == 1) OnAiTurn();
                    return;
                }

            OnGameEnded();
        }

        private void ModifyGrid(LineStructure line)
        {
            ColorChosenLine(line);

            var result = CheckState(line, _gameState.LineList);
            if (result.Item2 != 0)
            {
                _gameState.Scores[TurnId] += result.Item2;
                var rects = new List<RectangleStructure>();
                foreach (var point in result.Item1) rects.Add(CreateNewRectangleStructure(point));

                OnRectangleEnclosed(rects);
                OnScoreChanged();
            }

            ChangeTurn();

            IsGameEnded();
        }

        private void OnAiTurn()
        {
            AiTurn?.Invoke(this, new CustomEventArgs<List<LineStructure>>(_gameState.LineList));
        }

        private void OnGameEnded()
        {
            WriteGameState(true);
            GameEnded?.Invoke(this, EventArgs.Empty);
        }

        private void OnLineColored(LineStructure line)
        {
            LineColored?.Invoke(this, new CustomEventArgs<LineStructure>(line));
        }

        private void OnPlayerNameSet()
        {
            var names = new List<string> {_gameState.Player1, _gameState.Player2};
            PlayerNameSet?.Invoke(this, new CustomEventArgs<ReadOnlyCollection<string>>(names.AsReadOnly()));
        }

        private void OnRectangleEnclosed(List<RectangleStructure> rectangle)
        {
            RectangleEnclosed?.Invoke(this, new CustomEventArgs<List<RectangleStructure>>(rectangle));
        }

        private void OnRestartDone()
        {
            RestartDone?.Invoke(this, EventArgs.Empty);
        }

        private void OnScoreChanged()
        {
            ScoreChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ReadPreviousState(double canvasWidth)
        {
            var gameState = DataProvider.GameStates[^1];
            _gameState = gameState;
            TimeElapsed = gameState.Length;
            _gameType = _gameState.GameType;
            _gameMode = _gameState.GameMode;
            _gridSize = _gameState.GridSize;
            GameWidth = (int) canvasWidth / _gridSize;
            GameHeight = GameWidth;
            EllipseSize = 10;
            NumberOfRows = _gridSize;
            NumberOfColums = NumberOfRows;

            CreateAi();

            CreateEllipsePositionList();

            RestorePlacedRectangles();

            InitTurn();
        }

        private void Restart()
        {
            CreateNewGameState();
            if (DataProvider.GameStates.Count != 0 &&
                !DataProvider.GameStates[^1].IsEnded)
            {
                DataProvider.RemoveLastElement();
                DataProvider.CommitChanges();
            }

            CreateLineList(Brushes.White.ToString());
            OnRestartDone();

            InitTurn();
        }

        private void RestorePlacedRectangles()
        {
            OnRectangleEnclosed(_gameState.PlacedRectangles);
        }

        private void StartNewGame(double canvasWidth)
        {
            _gameType = GameControllerParameters.GameType;
            _gameMode = GameControllerParameters.GameMode;
            _gridSize = GameControllerParameters.GridSize;

            CreateNewGameState();

            GameWidth = (int) canvasWidth / GameControllerParameters.GridSize;
            GameHeight = GameWidth;
            NumberOfRows = GameControllerParameters.GridSize;
            NumberOfColums = NumberOfRows;

            CreateAi();

            CreateEllipsePositionList();
            CreateLineList(Brushes.White.ToString());

            InitTurn();
        }

        private void WriteGameState(bool isEnded)
        {
            if (DataProvider.GameStates.Count != 0 && !DataProvider.GameStates[^1].IsEnded)
                DataProvider.RemoveLastElement();
            _gameState.IsEnded = isEnded;
            _gameState.Length = TimeElapsed;

            DataProvider.AddElement(_gameState);
            DataProvider.CommitChanges();
        }
    }
}