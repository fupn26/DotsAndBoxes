﻿using DotsAndBoxes.Classes;
using DotsAndBoxes.CustomEventArgs;
using DotsAndBoxes.Structures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Point = System.Drawing.Point;

namespace DotsAndBoxes.Views
{
    public partial class GameView : UserControl
    {
        public event EventHandler InitScore;

        public event EventHandler RestoreState;

        public event EventHandler RestartGame;

        public event EventHandler SaveGame;

        public event EventHandler<LineEventArgs> LineClicked;

        public double CanvasWidth
        {
            get { return (double)GetValue(CanvasWidthProperty); }
            set { SetValue(CanvasWidthProperty, value); }
        }

        public static readonly DependencyProperty CanvasWidthProperty =
            DependencyProperty.Register("CanvasWidth", typeof(double), typeof(GameView), new PropertyMetadata(0.0));


        public double CanvasHeight
        {
            get { return (double)GetValue(CanvasHeightProperty); }
            set { SetValue(CanvasHeightProperty, value); }
        }

        public static readonly DependencyProperty CanvasHeightProperty =
            DependencyProperty.Register("CanvasHeight", typeof(double), typeof(GameView), new PropertyMetadata(0.0));




        public bool IsClassicView
        {
            get { return (bool)GetValue(IsClassicViewProperty); }
            set { SetValue(IsClassicViewProperty, value); }
        }

        public static readonly DependencyProperty IsClassicViewProperty =
            DependencyProperty.Register("IsClassicView", typeof(bool), typeof(GameView), new PropertyMetadata(true));





        private GameController gameController;

        private bool isCanvasEnabled;

        private DispatcherTimer _timer;

        private Line _lastLine;


        public GameView()
        {
            InitializeComponent();
        }

        public void LoadComponents()
        {
            gameController = new GameController(CanvasHeight, CanvasWidth, 100, 100, IsClassicView);
            gameController.ScoreChanged += GameController_ScoreChanged;
            gameController.RectangleEnclosed += GameController_RectangleEnclosed;
            gameController.RestartDone += GameController_RestartDone;
            gameController.GameEnded += GameController_GameEnded;
            gameController.LineColored += GameController_LineColored;
            InitScore += gameController.Window_InitScore;
            RestoreState += gameController.Window_RestoreState;
            RestartGame += gameController.Windows_RestartGame;
            SaveGame += gameController.Window_SaveGame;
            LineClicked += gameController.Window_LineClicked;

            isCanvasEnabled = true;
            OnRestoreState();
            InitGame();
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private bool CompareLineToLineStruct(Line line, LineStructure lineStructure)
        {
            return line.X1 == lineStructure.X1 && line.X2 == lineStructure.X2 && line.Y1 == lineStructure.Y1 && line.Y2 == lineStructure.Y2; 
        }

        private void GameController_LineColored(object sender, LineEventArgs e)
        {
            foreach(UIElement element in canvas.Children)
            {
                if (element is Line line)
                {
                    if (CompareLineToLineStruct(line, e.Line))
                    {
                        ColorLine(line, e.Line.StrokeColor);
                    }
                }
            }
            
        }

        private void ColorLine(Line line, string brush)
        {
            line.Stroke = (Brush)new BrushConverter().ConvertFromString(brush);
            line.Cursor = Cursors.Arrow;
            line.MouseEnter -= Line_MouseEnter;
            line.MouseLeave -= Line_MouseLeave;
            line.MouseLeftButtonDown -= Line_MouseLeftButtonDown;
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
            UpdateTimerText();
            OnInitScore();
            DrawLines();
            DrawEllipses();
        }

        private void Restart()
        {
            canvas.Children.Clear();
            gameController.TimeElapsed = 0;
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
                Ellipse ellipse = new Ellipse
                {
                    Width = gameController.EllipseSize,
                    Height = gameController.EllipseSize,
                    Fill = Brushes.Black
                };

                Canvas.SetLeft(ellipse, point.X - gameController.EllipseSize / 2);
                Canvas.SetTop(ellipse, point.Y - gameController.EllipseSize / 2);
                canvas.Children.Add(ellipse);

            }

        }

        private void DrawRectangle(RectangleStructure rectangle)
        {
            Rectangle rect = new Rectangle
            {
                Width = rectangle.Width,
                Height = rectangle.Height,
                Fill = (Brush)new BrushConverter().ConvertFromString(rectangle.Fill),
                RadiusX = rectangle.RadiusX,
                RadiusY = rectangle.RadiusY
            };
            Canvas.SetTop(rect, rectangle.RefPoint.Y + (gameController.GameHeight - rect.Height) / 2);
            Canvas.SetLeft(rect, rectangle.RefPoint.X + (gameController.GameWidth - rect.Width) / 2);
            canvas.Children.Add(rect);
        }

        private void DrawLines()
        {
            foreach (LineStructure lineStructure in gameController.LineList)
            {

                Line line = new Line
                {
                    X1 = lineStructure.X1,
                    Y1 = lineStructure.Y1,
                    X2 = lineStructure.X2,
                    Y2 = lineStructure.Y2,
                    Stroke = (Brush)new BrushConverter().ConvertFromString(lineStructure.StrokeColor),
                    StrokeThickness = 8,
                };
                if (line.Stroke.ToString() == Brushes.White.ToString())
                {
                    line.Cursor = Cursors.Hand;
                }
                line.MouseEnter += Line_MouseEnter;
                line.MouseLeave += Line_MouseLeave;
                line.MouseLeftButtonDown += Line_MouseLeftButtonDown;

                canvas.Children.Add(line);
            }
            return;
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

        private void ExitButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void SaveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnSaveGameState();
        }

        private void Line_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Line line)
            {
                _lastLine = line;
                OnLineClicked();
            }
        }

        private void OnLineClicked()
        {
            LineClicked?.Invoke(this, new LineEventArgs(ConvertLineToLineStructure(_lastLine)));
        }

        private LineStructure ConvertLineToLineStructure(Line line)
        {
            var lineStructure = new LineStructure
            {
                X1 = (int)line.X1,
                Y1 = (int)line.Y1,
                X2 = (int)line.X2,
                Y2 = (int)line.Y2
            };

            return lineStructure;
        }

        private void Line_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Line line)
            {
                if (line.Stroke.ToString() == Brushes.Black.ToString())
                {
                    line.Stroke = Brushes.White;
                }
            }
        }


        private void Line_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Line line)
            {
                if (line.Stroke.ToString() ==  Brushes.White.ToString())
                {
                    line.Stroke = Brushes.Black;

                }
            }
        }

        private void OnSaveGameState()
        {
            SaveGameSnack.IsActive = true;
            SaveGame?.Invoke(this, EventArgs.Empty);
        }

        public void MainWindow_NeedToLoadComponents(object sender, EventArgs e)
        {
            LoadComponents();
        }
    }
}