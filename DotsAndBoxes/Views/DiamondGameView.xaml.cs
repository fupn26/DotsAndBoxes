using DotsAndBoxes.Classes;
using DotsAndBoxes.CustomEventArgs;
using DotsAndBoxes.Structures;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Point = System.Drawing.Point;

namespace DotsAndBoxes.Views
{
    public partial class DiamondGameView : UserControl
    {
        private readonly GameController gameController;

        public event EventHandler InitScore;

        public event EventHandler RestoreState;

        public event EventHandler RestartGame;

        public event EventHandler SaveGame;

        private bool isCanvasEnabled;

        private readonly DispatcherTimer _timer;

        public DiamondGameView()
        {
            InitializeComponent();
            gameController = new GameController(canvas.Height, canvas.Width, 80, 80, true);
            gameController.ScoreChanged += GameController_ScoreChanged;
            gameController.RectangleEnclosed += GameController_RectangleEnclosed;
            gameController.RestartDone += GameController_RestartDone;
            gameController.GameEnded += GameController_GameEnded;
            InitScore += gameController.Window_InitScore;
            RestoreState += gameController.Window_RestoreState;
            RestartGame += gameController.Windows_RestartGame;
            SaveGame += gameController.Window_SaveGame;
            isCanvasEnabled = true;
            OnRestoreState();
            InitGame();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void GameController_GameEnded(object sender, EventArgs e)
        {
            OnRestartButtonClicked();
        }

        private void GameController_RestartDone(object sender, EventArgs e)
        {
            Restart();
        }

        private void UpdateTimerText()
        {
            int sec = gameController.TimeElapsed % 60;
            int min = gameController.TimeElapsed / 60;
            if (sec < 10 && min == 0)
            {
                Timer.Text = string.Format("00:0{0}", sec);
            }
            else if (sec >= 10 && min < 10)
            {
                Timer.Text = string.Format("0{0}:{1}", min, sec);
            }
            else if (sec < 10 && min >= 10)
            {
                Timer.Text = string.Format("{0}:0{1}", min, sec);
            }
            else if (sec < 10 && min < 10)
            {
                Timer.Text = string.Format("0{0}:0{1}", min, sec);
            }
            else
            {
                Timer.Text = string.Format("{0}:{1}", min, sec);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateTimerText();
            gameController.TimeElapsed += 1;
        }

        private void GameController_RectangleEnclosed(object sender, RectangleEventArgs e)
        {
            DrawRectangle(e.rectangleStructure);
        }

        private void GameController_ScoreChanged(object sender, EventArgs e)
        {
            ScorePlayer1.Text = gameController.Scores[0].ToString();
            ScorePlayer2.Text = gameController.Scores[1].ToString();
        }

        private void InitGame()
        {
            gameController.TimeElapsed = 0;
            UpdateTimerText();
            OnInitScore();
            DrawLines();
            DrawEllipses();
        }

        private void Restart()
        {
            canvas.Children.Clear();
            InitGame();
        }

        private void OnRestoreState()
        {
            RestoreState?.Invoke(this, EventArgs.Empty);
        }

        private void OnInitScore()
        {
            InitScore?.Invoke(this, EventArgs.Empty);
        }

        private void DrawEllipses()
        {
            foreach (Point point in gameController.PointList)
            {
                Ellipse ellipse = new Ellipse();
                ellipse.Width = gameController.EllipseSize;
                ellipse.Height = gameController.EllipseSize;
                ellipse.Fill = Brushes.Black;

                Canvas.SetLeft(ellipse, point.X - gameController.EllipseSize / 2);
                Canvas.SetTop(ellipse, point.Y - gameController.EllipseSize / 2);
                canvas.Children.Add(ellipse);

            }

        }

        private void DrawRectangle(RectangleStructure rectangle)
        {
            Rectangle rect = new Rectangle();
            rect.Width = rectangle.Width;
            rect.Height = rectangle.Height;
            rect.Fill = (Brush)new BrushConverter().ConvertFromString(rectangle.Fill);
            rect.RadiusX = rectangle.RadiusX;
            rect.RadiusY = rectangle.RadiusY;
            Canvas.SetTop(rect, rectangle.RefPoint.Y + (gameController.GameHeight - rect.Height) / 2);
            Canvas.SetLeft(rect, rectangle.RefPoint.X + (gameController.GameWidth - rect.Width) / 2);
            canvas.Children.Add(rect);
        }

        private void DrawLines()
        {
            foreach (Line line in gameController.LineList)
            {
                canvas.Children.Add(line);
            }
        }

        private void PauseGame()
        {
            if (isCanvasEnabled)
            {
                canvas.IsEnabled = false;
                PauseButtonSign.Kind = MaterialDesignThemes.Wpf.PackIconKind.PlayArrow;
                _timer.Stop();
            }
            else
            {
                canvas.IsEnabled = true;
                PauseButtonSign.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
                _timer.Start();
            }

            isCanvasEnabled = !isCanvasEnabled;
        }

        private void OnRestartButtonClicked()
        {
            RestartGame?.Invoke(this, EventArgs.Empty);
        }

        private void RestartButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnRestartButtonClicked();
        }

        private void PauseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PauseGame();
        }

        private void PopupBox_Opened(object sender, System.Windows.RoutedEventArgs e)
        {
            PopupBoxPanel.Visibility = System.Windows.Visibility.Visible;
            if (isCanvasEnabled)
            {
                PauseGame();
            }
        }

        private void PopupBox_Closed(object sender, System.Windows.RoutedEventArgs e)
        {
            PopupBoxPanel.Visibility = System.Windows.Visibility.Collapsed;
            if (!isCanvasEnabled)
            {
                PauseGame();
            }
        }

        private void PopupBox_IsMouseDirectlyOverChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            return;

        }

        private void ExitButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void SaveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnSaveGameState();
        }

        private void OnSaveGameState()
        {
            SaveGameSnack.IsActive = true;
            SaveGame?.Invoke(this, EventArgs.Empty);
        }
    }
}
