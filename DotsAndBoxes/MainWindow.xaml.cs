using DotsAndBoxes.Data;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int GameWidth = 100;
        private const int GameHeight = 100;
        private const int EllipseSize = 10;

        private readonly int NumberOfRows;
        private readonly int NumberOfColums;

        private List<Point> pointList;
        private List<MyEllipse> ellipseList;
        private List<Line> lineList;

        private int turnId;

        private int[] scores;

        public MainWindow()
        {
            InitializeComponent();
            NumberOfRows = (int)canvas.Height / GameHeight;
            NumberOfColums = (int)canvas.Width / GameWidth;
            ellipseList = new List<MyEllipse>();
            pointList = new List<Point>();
            lineList = new List<Line>();
            scores = new int[2];
            turnId = 0;
            InitGame();
        }

        private void InitGame()
        {
            CreateEllipsePositionList();
            DrawLines(Brushes.White);
            DrawEllipses();
            UpdateScore();
        }


        private void CreateEllipsePositionList()
        {
            for (int i = 0; i <= NumberOfRows; ++i)
            {
                for (int j = 0; j <= NumberOfColums; ++j)
                {
                    //MyEllipse ellipse = new MyEllipse();
                    //ellipse.Shape = new Ellipse();
                    //ellipse.Shape.Height = 10;
                    //ellipse.Shape.Height = 10;
                    //ellipse.Shape.Fill = Brushes.Black;
                    Point point = new Point(j*GameWidth, i*GameHeight);

                    pointList.Add(point);
                    //ellipseList.Add(ellipse);
                }
            }
        }

        private bool areEqualLines(Line line1, Line line2)
        {
            if (line1.X1 == line2.X1 && line1.Y1 == line2.Y1 && line1.X2 == line2.X2 && line1.Y2 == line2.Y2 ||
                line1.X1 == line2.X2 && line1.Y1 == line2.Y2 && line1.X2 == line2.X1 && line1.Y2 == line2.Y1)
            {
                return true;
            }
            return false;
        }

        private Line moveUp(Line line)
        {
            Line newLine = new Line();
            newLine.X1 = line.X1;
            newLine.X2 = line.X2;
            newLine.Y1 = line.Y1 + GameHeight;
            newLine.Y2 = line.Y2 + GameHeight;
            return newLine;
        }
        private Line moveDown(Line line)
        {
            Line newLine = new Line();
            newLine.X1 = line.X1;
            newLine.X2 = line.X2;
            newLine.Y1 = line.Y1 - GameHeight;
            newLine.Y2 = line.Y2 - GameHeight;
            return newLine;
        }

        private Line moveRight(Line line)
        {
            Line newLine = new Line();
            newLine.X1 = line.X1 + GameWidth;
            newLine.X2 = line.X2 + GameWidth;
            newLine.Y1 = line.Y1;
            newLine.Y2 = line.Y2;
            return newLine;
        }

        private Line moveLeft(Line line)
        {
            Line newLine = new Line();
            newLine.X1 = line.X1 - GameWidth;
            newLine.X2 = line.X2 - GameWidth;
            newLine.Y1 = line.Y1;
            newLine.Y2 = line.Y2;
            return newLine;
        }


        private int isSquare(Line line)
        {
            if (line.X1 == line.X2)
            {
                Line searchedLine = moveRight(line);
                foreach (Line element in lineList)
                {
                    if (areEqualLines(element, searchedLine))
                    {
                        element.Stroke = Brushes.Yellow;
                    }
                }
            }
            return 0;
            //Line line1 = new Line();
            //Line line2 = new Line();
            //Line line3 = new Line();

            //Line seged = new Line();
            //if (line.X1 == line.X2)
            //{
            //    seged.X1 = line.X1 + GameWidth;
            //    seged.X2 = line.X2 + GameWidth;
            //    seged.Y1 = line.Y1;
            //    seged.Y2 = line.Y2;
            //    foreach (Line element in lineList)
            //    {
            //        if ((line.X1 + GameWidth == element.X1 && line.X2 + GameWidth == element.X2 && line.Y1 == element.Y1 && line.Y2 == element.Y2)
            //            || (line.X1 - GameWidth == element.X1 && line.X2 - GameWidth == element.X2 && line.Y1 == element.Y1 && line.Y2 == element.Y2))
            //        {
            //            element.Stroke = Brushes.Yellow;
            //            line1 = element;
            //            break;
            //        }
            //    }

            //    //foreach (Line element in lineList)
            //    //{
            //    //    if ((line.X1 == element.X1 && line1.X1 == element.X2) ||
            //    //        (line.X1 == element.X2 && line1.X1 == element.X1))
            //    //    {
            //    //        element.Stroke = Brushes.Yellow;
            //    //        line2 = element;
            //    //        break;
            //    //    }
            //    //}

            //    //foreach (Line element in lineList)
            //    //{
            //    //    if (line2.Y1 + GameHeight == element.Y1 || line2.Y1 - GameHeight == element.Y1)
            //    //    {
            //    //        element.Stroke = Brushes.Yellow;

            //    //        line3 = element;
            //    //        break;
            //    //    }
            //    //}

            //}
            //else
            //{
            //    foreach (Line element in lineList)
            //    {
            //        if (line.Y1 + GameHeight == element.Y1 || line.Y1 - GameHeight == element.Y1)
            //        {
            //            element.Stroke = Brushes.Yellow;
            //            line1 = element;
            //            break;
            //        }
            //    }

            ////    foreach (Line element in lineList)
            ////    {
            ////        if ((line.Y1 == element.Y1 && line1.Y1 == element.Y2) ||
            ////            (line.Y1 == element.Y2 && line1.Y1 == element.Y1))
            ////        {
            ////            element.Stroke = Brushes.Yellow;
            ////            line2 = element;
            ////            break;
            ////        }
            ////    }

            ////    foreach (Line element in lineList)
            ////    {
            ////        if (line2.X1 + GameWidth == element.X1 || line2.X1 - GameWidth == element.X1)
            ////        {
            ////            element.Stroke = Brushes.Yellow;
            ////            line3 = element;
            ////            break;
            ////        }
            ////    }
            //}

            //if(line.Stroke != Brushes.White && line.Stroke != Brushes.Black &&
            //   line1.Stroke != Brushes.White && line1.Stroke != Brushes.Black &&
            //   line2.Stroke != Brushes.White && line2.Stroke != Brushes.Black &&
            //   line3.Stroke != Brushes.White && line3.Stroke != Brushes.Black)
            //{
            //    return true;
            //}

            //return false;
        }

        private void DrawEllipses()
        {
            foreach (Point point in pointList)
            {
                Ellipse ellipse = new Ellipse();
                ellipse.Width = EllipseSize;
                ellipse.Height = EllipseSize;
                ellipse.Fill = Brushes.Black;

                Canvas.SetLeft(ellipse, point.X - EllipseSize / 2);
                Canvas.SetTop(ellipse, point.Y - EllipseSize / 2);
                canvas.Children.Add(ellipse);

            }

            //Line line = new Line();
            //line.X1 = pointList[0].X+10;
            //line.Y1 = pointList[0].Y+5;
            //line.X2 = pointList[1].X;
            //line.Y2 = pointList[1].Y+5;

            //line.Stroke = Brushes.LightCoral;
            //line.StrokeThickness = 4;
            //canvas.Children.Add(line);

        }

        private void RecolorLines(Brush brush)
        {
            foreach (Line line in lineList)
            {
                line.Stroke = brush;
            }
        }

        private void DrawLines(Brush brush)
        {
            for (int i = 0; i <= NumberOfRows; ++i)
            {
                for (int j = 0; j < NumberOfColums; ++j)
                {
                    //MyEllipse ellipse = new MyEllipse();
                    //ellipse.Shape = new Ellipse();
                    //ellipse.Shape.Height = 10;
                    //ellipse.Shape.Height = 10;
                    //ellipse.Shape.Fill = Brushes.Black;
                    int x1 = j * GameWidth;
                    int y1 = i * GameHeight;
                    int x2 = x1 + GameWidth;
                    int y2 = y1;
                    Line line = new Line();
                    line.X1 = x1;
                    line.Y1 = y1;
                    line.X2 = x2;
                    line.Y2 = y2;
                    line.Stroke = brush;
                    line.StrokeThickness = 8;
                    canvas.Children.Add(line);
                    lineList.Add(line);
                }
            }

            for (int i = 0; i < NumberOfRows; ++i)
            {
                for (int j = 0; j <= NumberOfColums; ++j)
                {
                    //MyEllipse ellipse = new MyEllipse();
                    //ellipse.Shape = new Ellipse();
                    //ellipse.Shape.Height = 10;
                    //ellipse.Shape.Height = 10;
                    //ellipse.Shape.Fill = Brushes.Black;
                    int x1 = j * GameWidth;
                    int y1 = i * GameHeight;
                    int x2 = x1;
                    int y2 = y1 + GameHeight;
                    Line line = new Line();
                    line.X1 = x1;
                    line.Y1 = y1;
                    line.X2 = x2;
                    line.Y2 = y2;
                    line.Stroke = brush;
                    line.StrokeThickness = 8;
                    canvas.Children.Add(line);
                    lineList.Add(line);
                }
            }
        }


        private void UpdateScore()
        {
            ScorePlayer1.Text = scores[0].ToString();
            ScorePlayer2.Text = scores[1].ToString();
        }
        //private void RenderState()
        //{
        //    canvas.Children.Clear();

        //    //RenderLines();
        //    //RenderEll
        //}

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            foreach (Line line in lineList)
            {
                if (line.IsMouseDirectlyOver && line.Stroke == Brushes.Black)
                {
                    line.StrokeThickness = 4;
                    if (turnId == 1)
                    {
                        line.Stroke = Brushes.Red;
                    }
                    else
                    {
                        line.Stroke = Brushes.Blue;
                    }

                    scores[turnId] += isSquare(line);
                    UpdateScore();

                    turnId = 1 - turnId;
                    break;
                }
            }
            //ScorePlayer1.Text = canvas.Width.ToString();
            //Console.WriteLine("Left mouse pressed");
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (Line line in lineList)
            {
                if (line.IsMouseDirectlyOver && line.Stroke == Brushes.White)
                {
                    line.Stroke = Brushes.Black;
                }
                else
                {
                    if (line.Stroke == Brushes.Black)
                        line.Stroke = Brushes.White;
                }
            }
            //ScorePlayer2.Text = e.GetPosition(canvas).ToString();
            //Console.WriteLine("MouseMove");
        }
    }
}
