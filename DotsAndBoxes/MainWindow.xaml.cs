using DotsAndBoxes.Classes;
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

        public MainWindow()
        {
            InitializeComponent();
            gameController = new GameController(canvas.Height, canvas.Width);
            gameController.ScoreChanged += GameController_ScoreChanged;
            gameController.RectangleEnclosed += GameController_RectangleEnclosed;
            InitScore += gameController.Window_InitScore;
            InitGame();
        }

        private void GameController_RectangleEnclosed(object sender, RectangleEventArgs e)
        {
            DrawRectangle(e.RefPoint);
        }

        private void GameController_ScoreChanged(object sender, EventArgs e)
        {
            ScorePlayer1.Text = gameController.Scores[0].ToString();
            ScorePlayer2.Text = gameController.Scores[1].ToString();
        }

        private void InitGame()
        {
            OnInitScore();
            DrawLines();
            DrawEllipses();
//            UpdateScore();
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

        private void DrawRectangle(Point point)
        {
            Rectangle rect = new Rectangle();
            rect.Width = gameController.GameWidth * 0.9;
            rect.Height = gameController.GameHeight * 0.9;
            if (gameController.TurnId == 0)
            {
                rect.Fill = Brushes.Blue;
            }
            else
            {
                rect.Fill = Brushes.Red;
            }
            rect.RadiusX = 8;
            rect.RadiusY = 8;
            Canvas.SetTop(rect, point.Y + (gameController.GameHeight - rect.Height) / 2);
            Canvas.SetLeft(rect, point.X + (gameController.GameWidth - rect.Width) / 2);
            canvas.Children.Add(rect);
        }

        private void DrawLines()
        {
            foreach (Line line in gameController.LineList)
            {
                canvas.Children.Add(line);
            }
        }


        //private void UpdateScore()
        //{
        //    ScorePlayer1.Text = gameController.Scores[0].ToString();
        //    ScorePlayer2.Text = gameController.Scores[1].ToString();
        //}

        //private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    foreach (Line line in gameController.LineList)
        //    {
        //        if (line.IsMouseDirectlyOver && line.Stroke == Brushes.Black)
        //        {
        //            line.StrokeThickness = 4;
        //            if (gameController.TurnId == 1)
        //            {
        //                line.Stroke = Brushes.Red;
        //            }
        //            else
        //            {
        //                line.Stroke = Brushes.Blue;
        //            }

        //            gameController.Scores[gameController.TurnId] += gameController.CheckState(line);
        //            UpdateScore();

        //            //gameController.TurnId = 1 - gameController.TurnId;
        //            break;
        //        }
        //    }
        //}


        //    private void Window_MouseMove(object sender, MouseEventArgs e)
        //    {
        //        foreach (Line line in gameController.LineList)
        //        {
        //            if (line.IsMouseDirectlyOver && line.Stroke == Brushes.White)
        //            {
        //                line.Stroke = Brushes.Black;
        //            }
        //            else
        //            {
        //                if (line.Stroke == Brushes.Black)
        //                    line.Stroke = Brushes.White;
        //            }
        //        }
        //    }
    }
}
