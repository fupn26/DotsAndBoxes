using DotsAndBoxes.Classes;
using DotsAndBoxes.Structures;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Point = System.Drawing.Point;

namespace DotsAndBoxes
{
    public partial class MainWindow : Window
    {
        private GameController gameController;

        public event EventHandler InitScore;

        public event EventHandler RestoreState;

        public MainWindow()
        {
            InitializeComponent();
            gameController = new GameController(canvas.Height, canvas.Width);
            gameController.ScoreChanged += GameController_ScoreChanged;
            gameController.RectangleEnclosed += GameController_RectangleEnclosed;
            InitScore += gameController.Window_InitScore;
            RestoreState += gameController.Window_RestoreState;
            InitGame();
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


    }
}
