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
        private const int GameWidth = 100;
        private const int GameHeight = 100;
        private const int EllipseSize = 10;

        private readonly int NumberOfRows;
        private readonly int NumberOfColums;

        private List<Point> pointList;
        private List<Line> lineList;

        private int turnId;

        private int[] scores;

        public MainWindow()
        {
            InitializeComponent();
            NumberOfRows = (int)canvas.Height / GameHeight;
            NumberOfColums = (int)canvas.Width / GameWidth;
            pointList = new List<Point>();
            lineList = new List<Line>();
            scores = new int[2];
            turnId = 0;
            InitGame();
        }

        private void InitGame()
        {
            CreateEllipsePositionList();
            //DrawRectangles();
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
                    Point point = new Point(j*GameWidth, i*GameHeight);

                    pointList.Add(point);
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

        private int CheckState(Line line)
        {
            int counter = 0;
            if (line.X1 == line.X2) {
                foreach (Line element in lineList)
                {
                    if (areEqualLines(moveLeft(line), element) && 
                        element.Stroke != Brushes.Black &&
                        element.Stroke != Brushes.White)
                    {
                        counter += isSquare(line, element, true);
                    }
                    else if (areEqualLines(moveRight(line), element) &&
                        element.Stroke != Brushes.Black &&
                        element.Stroke != Brushes.White)
                    {
                        counter += isSquare(line, element, true);
                    }
                }
            }
            else
            {
                foreach (Line element in lineList)
                {
                    if (areEqualLines(moveUp(line), element) &&
                        element.Stroke != Brushes.Black &&
                        element.Stroke != Brushes.White)
                    {
                        counter += isSquare(line, element, false);
                    }
                    else if (areEqualLines(moveDown(line), element) &&
                        element.Stroke != Brushes.Black &&
                        element.Stroke != Brushes.White)
                    {
                        counter += isSquare(line, element, false);
                    }
                }
            }
            return counter;
        }

        private Point minPoint(Point p1, Point p2, Point p3, Point p4)
        {
            Point minp = p1;
            if (p2.X < minp.X || p2.Y < minp.Y)
            {
                minp = p2;
            }
            if (p3.X < minp.X || p3.Y < minp.Y)
            {
                minp = p3;
            }
            if (p4.X < minp.X || p4.Y < minp.Y)
            {
                minp = p4;
            }
            return minp;
        }

        private bool isPointsOfLine (Line line, Point p1, Point p2)
        {
            Point lineP1 = new Point((int)line.X1, (int)line.Y1);
            Point lineP2 = new Point((int)line.X2, (int)line.Y2);

            if (lineP1.Equals(p1) && lineP2.Equals(p2) ||
                lineP1.Equals(p2) && lineP2.Equals(p1))
            {
                return true;
            }

            return false;
        }

        private int isSquare(Line line1, Line line2, bool isVertical)
        {
            Point point1 = new Point((int)line1.X1, (int)line1.Y1);
            Point point2 = new Point((int)line1.X2, (int)line1.Y2);
            Point point3 = new Point((int)line2.X1, (int)line2.Y1);
            Point point4 = new Point((int)line2.X2, (int)line2.Y2);

            int counter = 0;
            if (isVertical)
            {
                foreach (Line element in lineList)
                {
                    if (element.Y1 == element.Y2)
                    {
                        if (isPointsOfLine(element, point1, point3) &&
                            element.Stroke != Brushes.Black &&
                            element.Stroke != Brushes.White)
                        {
                            counter++;
                        }
                        else if (isPointsOfLine(element, point2, point4) &&
                            element.Stroke != Brushes.Black &&
                            element.Stroke != Brushes.White)
                        {
                            counter++;
                        }
                    }
                }
            }
            else
            {
                foreach (Line element in lineList)
                {
                    if (element.X1 == element.X2)
                    {
                        if (isPointsOfLine(element, point1, point3) &&
                            element.Stroke != Brushes.Black &&
                            element.Stroke != Brushes.White)
                        {
                            counter++;
                        }
                        else if (isPointsOfLine(element, point2, point4) &&
                            element.Stroke != Brushes.Black &&
                            element.Stroke != Brushes.White)
                        {
                            counter++;
                        }
                    }
                }
            }

            if (counter != 2)
            {
                return 0;
            }
            else
            {
                Point point = minPoint(point1, point2, point3, point4);
                DrawRectangle(point);
                return 1;
            }
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

        }

        private void DrawRectangle(Point point)
        {
            Rectangle rect = new Rectangle();
            rect.Width = GameWidth * 0.9;
            rect.Height = GameHeight * 0.9;
            if (turnId == 0)
            {
                rect.Fill = Brushes.Blue;
            }
            else
            {
                rect.Fill = Brushes.Red;
            }
            rect.RadiusX = 8;
            rect.RadiusY = 8;
            Canvas.SetTop(rect, point.Y + (GameHeight - rect.Height) / 2);
            Canvas.SetLeft(rect, point.X + (GameWidth - rect.Width) / 2);
            canvas.Children.Add(rect);
        }

        private void DrawLines(Brush brush)
        {
            for (int i = 0; i <= NumberOfRows; ++i)
            {
                for (int j = 0; j < NumberOfColums; ++j)
                {
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
                    line.Cursor = Cursors.Hand;
                    canvas.Children.Add(line);
                    lineList.Add(line);
                }
            }

            for (int i = 0; i < NumberOfRows; ++i)
            {
                for (int j = 0; j <= NumberOfColums; ++j)
                {
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
                    line.Cursor = Cursors.Hand;
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

                    scores[turnId] += CheckState(line);
                    UpdateScore();

                    turnId = 1 - turnId;
                    break;
                }
            }
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
        }
    }
}
