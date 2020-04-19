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
    /// <summary>
    /// Interaction logic for ClassicGameView.xaml
    /// </summary>
    public partial class ClassicGameView : UserControl
    {
        private GameController gameController;

        public event EventHandler InitScore;

        public event EventHandler RestoreState;

        public event EventHandler RestartGame;

        private bool isCanvasEnabled;

        private int _time;

        private DispatcherTimer _timer;

        public ClassicGameView()
        {
            InitializeComponent();
            gameController = new GameController(canvas.Height, canvas.Width, 100, 100, false);
            gameController.ScoreChanged += GameController_ScoreChanged;
            gameController.RectangleEnclosed += GameController_RectangleEnclosed;
            InitScore += gameController.Window_InitScore;
            RestoreState += gameController.Window_RestoreState;
            RestartGame += gameController.Windows_RestartGame;
            isCanvasEnabled = true;
            InitGame();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += timer_Tick;
            _timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            int sec = _time % 60;
            int min = _time / 60;
            if (sec < 10 && min == 0)
            {
                Timer.Text = string.Format("00:0{0}", _time % 60);
            }
            else if (sec > 10 && min < 10)
            {
                Timer.Text = string.Format("0{0}:{1}", _time / 60, _time % 60);
            }
            else if (sec < 10 && min > 10)
            {
                Timer.Text = string.Format("{0}:0{1}", _time / 60, _time % 60);
            }
            else if (sec < 10 && min < 10 )
            {
                Timer.Text = string.Format("0{0}:0{1}", _time / 60, _time % 60);
            }
            else
            {
                Timer.Text = string.Format("{0}:{1}", _time / 60, _time % 60);
            }
            _time++;
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
            Timer.Text = "00:00";
            _time = 0;
            canvas.Children.Clear();
            OnRestoreState();
            OnInitScore();
            DrawLines();
            DrawEllipses();
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

        private void RestartButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            RestartGame?.Invoke(this, EventArgs.Empty);
            InitGame();
        }

        private void PauseButton_Click(object sender, System.Windows.RoutedEventArgs e)
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
    }
}
