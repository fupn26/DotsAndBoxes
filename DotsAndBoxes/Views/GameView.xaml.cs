using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using DotsAndBoxes.Classes;
using DotsAndBoxes.CustomEventArgs;
using DotsAndBoxes.Structures;
using MaterialDesignThemes.Wpf;

namespace DotsAndBoxes.Views
{
    public partial class GameView
    {
        private GameController _gameController;

        private bool _isCanvasEnabled;

        private Line _lastLine;

        private int _snackShownTime;

        private DispatcherTimer _timer;


        public GameView()
        {
            InitializeComponent();
            LoadComponents();
        }

        public event EventHandler InitScore;

        public event EventHandler<CustomEventArgs<LineStructure>> LineClicked;

        private void LoadComponents()
        {
            _gameController = new GameController();
            _gameController.ScoreChanged += GameController_ScoreChanged;
            _gameController.RectangleEnclosed += GameController_RectangleEnclosed;
            _gameController.RestartDone += GameController_RestartDone;
            _gameController.GameEnded += GameController_GameEnded;
            _gameController.LineColored += GameController_LineColored;
            _gameController.PlayerNameSet += GameController_PlayerNameSet;
            InitScore += _gameController.Window_InitScore;
            RestartGame += _gameController.Windows_RestartGame;
            SaveGame += _gameController.Window_SaveGame;
            LineClicked += _gameController.Window_LineClicked;
            _gameController.Initialize(Canvas.Height, Canvas.Width);

            SaveGameSnack.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));

            _isCanvasEnabled = true;

            InitGame();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        public event EventHandler RestartGame;

        public event EventHandler SaveGame;

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.IsOpen = false;
            _timer.Start();
        }

        private void ColorLine(Line line, string brush)
        {
            line.Stroke = (Brush) new BrushConverter().ConvertFromString(brush);
            line.Cursor = Cursors.Arrow;
            line.MouseEnter -= Line_MouseEnter;
            line.MouseLeave -= Line_MouseLeave;
            line.MouseLeftButtonDown -= Line_MouseLeftButtonDown;
        }

        private bool CompareLineToLineStruct(Line line, LineStructure lineStructure)
        {
            return line.X1 == lineStructure.X1 && line.X2 == lineStructure.X2 && line.Y1 == lineStructure.Y1 &&
                   line.Y2 == lineStructure.Y2;
        }

        private LineStructure ConvertLineToLineStructure(Line line)
        {
            var lineStructure = new LineStructure
            {
                X1 = (int) line.X1,
                Y1 = (int) line.Y1,
                X2 = (int) line.X2,
                Y2 = (int) line.Y2
            };

            return lineStructure;
        }

        private void DisplayPlayersName(ReadOnlyCollection<string> names)
        {
            Player1Name.Icon = names[0][0];
            Player1Name.IconBackground = Brushes.DarkBlue;
            Player1Name.Content = names[0];
            Player2Name.Icon = names[1][0];
            Player2Name.IconBackground = Brushes.DarkRed;
            Player2Name.Content = names[1];
        }

        private void DNoSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void DrawEllipses()
        {
            foreach (var point in _gameController.PointList)
            {
                var ellipse = new Ellipse
                {
                    Width = _gameController.EllipseSize,
                    Height = _gameController.EllipseSize,
                    Fill = Brushes.Black
                };

                Canvas.SetLeft(ellipse, point.X - _gameController.EllipseSize / 2);
                Canvas.SetTop(ellipse, point.Y - _gameController.EllipseSize / 2);
                Canvas.Children.Add(ellipse);
            }
        }

        private void DrawLines()
        {
            foreach (var lineStructure in _gameController.LineList)
            {
                var line = new Line
                {
                    X1 = lineStructure.X1,
                    Y1 = lineStructure.Y1,
                    X2 = lineStructure.X2,
                    Y2 = lineStructure.Y2,
                    Stroke = (Brush) new BrushConverter().ConvertFromString(lineStructure.StrokeColor),
                    StrokeThickness = 8
                };
                if (line.Stroke?.ToString() == Brushes.White.ToString()) line.Cursor = Cursors.Hand;
                line.MouseEnter += Line_MouseEnter;
                line.MouseLeave += Line_MouseLeave;
                line.MouseLeftButtonDown += Line_MouseLeftButtonDown;

                Canvas.Children.Add(line);
            }

            Canvas.UpdateLayout();
        }

        private void DrawRectangles(List<RectangleStructure> rectangles)
        {
            foreach (var rectangle in rectangles)
            {
                var rect = new Rectangle
                {
                    Width = rectangle.Width,
                    Height = rectangle.Height,
                    Fill = (Brush) new BrushConverter().ConvertFromString(rectangle.Fill),
                    RadiusX = rectangle.RadiusX,
                    RadiusY = rectangle.RadiusY
                };
                Canvas.SetTop(rect, rectangle.RefPoint.Y + (_gameController.GameHeight - rect.Height) / 2);
                Canvas.SetLeft(rect, rectangle.RefPoint.X + (_gameController.GameWidth - rect.Width) / 2);
                Canvas.Children.Add(rect);
            }
        }

        private void DSaveButton_Click(object sender, RoutedEventArgs e)
        {
            OnSaveGameState();
            Application.Current.Shutdown();
        }

        private void OnExitButtonClicked()
        {
            DialogHost.IsOpen = true;
            _timer.Stop();

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            OnExitButtonClicked();
        }

        private void GameController_GameEnded(object sender, EventArgs e)
        {
            var resView = new ResultsView();
            NavigationService?.Navigate(resView);
        }

        private void GameController_LineColored(object sender, CustomEventArgs<LineStructure> e)
        {
            foreach (UIElement element in Canvas.Children)
                if (element is Line line)
                    if (CompareLineToLineStruct(line, e.Content))
                        ColorLine(line, e.Content.StrokeColor);
        }

        private void GameController_PlayerNameSet(object sender, CustomEventArgs<ReadOnlyCollection<string>> e)
        {
            DisplayPlayersName(e.Content);
        }

        private void GameController_RectangleEnclosed(object sender,
            CustomEventArgs<List<RectangleStructure>> e)
        {
            DrawRectangles(e.Content);
        }

        private void GameController_RestartDone(object sender, EventArgs e)
        {
            Restart();
        }

        private void GameController_ScoreChanged(object sender, EventArgs e)
        {
            ScorePlayer1.Text = _gameController.Scores[0].ToString();
            ScorePlayer2.Text = _gameController.Scores[1].ToString();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            OnHomeButtonClicked();
        }

        private void InitGame()
        {
            UpdateTimerText();
            OnInitScore();
            DrawLines();
            DrawEllipses();
        }


        private void Line_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Line line)
                if (line.Stroke.ToString() == Brushes.White.ToString())
                    line.Stroke = Brushes.Black;
        }

        private void Line_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Line line)
                if (line.Stroke.ToString() == Brushes.Black.ToString())
                    line.Stroke = Brushes.White;
        }

        private void Line_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Line line)
            {
                _lastLine = line;
                OnLineClicked();
            }
        }

        private void OnHomeButtonClicked()
        {
            NavigationService?.Navigate(new WelcomeView());
        }

        private void OnInitScore()
        {
            InitScore?.Invoke(this, EventArgs.Empty);
        }

        private void OnLineClicked()
        {
            LineClicked?.Invoke(this, new CustomEventArgs<LineStructure>(ConvertLineToLineStructure(_lastLine)));
        }

        private void OnRestartButtonClicked()
        {
            RestartGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnSaveGameState()
        {
            SaveGameSnack.MessageQueue.Enqueue("Game Saved");
            SaveGame?.Invoke(this, EventArgs.Empty);
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            PauseGame();
        }

        private void PauseGame()
        {
            if (_isCanvasEnabled)
            {
                Canvas.IsEnabled = false;
                PauseButtonSign.Kind = PackIconKind.PlayArrow;
                _timer.Stop();
            }
            else
            {
                Canvas.IsEnabled = true;
                PauseButtonSign.Kind = PackIconKind.Pause;
                _timer.Start();
            }

            _isCanvasEnabled = !_isCanvasEnabled;
        }

        private void PopupBox_Closed(object sender, RoutedEventArgs e)
        {
            PopupBoxPanel.Visibility = Visibility.Collapsed;
            if (!_isCanvasEnabled) PauseGame();
        }

        private void PopupBox_Opened(object sender, RoutedEventArgs e)
        {
            PopupBoxPanel.Visibility = Visibility.Visible;
            if (_isCanvasEnabled) PauseGame();
        }

        private void Restart()
        {
            Canvas.Children.Clear();
            _gameController.TimeElapsed = 0;
            InitGame();
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            OnRestartButtonClicked();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            OnSaveGameState();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateTimerText();
            _gameController.TimeElapsed += 1;
        }

        private void UpdateTimerText()
        {
            var time = TimeSpan.FromSeconds(_gameController.TimeElapsed);
            Timer.Text = time.ToString("mm':'ss");
        }

        public void Window_Closing(object sender, EventArgs e)
        {
            OnExitButtonClicked();
        }
    }
}