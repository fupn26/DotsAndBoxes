using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DotsAndBoxes.Classes
{
    class GameController
    {
        public int GameWidth { get; }
        public int GameHeight { get; }
        public int EllipseSize { get; }

        public int TurnId { get; private set; }

        public int[] Scores { get; private set; }

        public int NumberOfRows { get; }
        public int NumberOfColums { get; }

        public event EventHandler ScoreChanged;
        public event EventHandler<RectangleEventArgs> RectangleEnclosed;




        public List<Point> PointList { get; private set; }
        public List<Line> LineList { get; private set; }

        public GameController(double CanvasHeight, double CanvasWidth)
        {
            PointList = new List<Point>();
            LineList = new List<Line>();
            GameWidth = 100;
            GameHeight = 100;
            EllipseSize = 10;
            NumberOfRows = (int)CanvasHeight / GameHeight;
            NumberOfColums = (int)CanvasWidth / GameWidth;
            Scores = new int[2];
            TurnId = 0;
            CreateEllipsePositionList();
            CreateLineList(Brushes.White);
        }

        public void CreateEllipsePositionList()
        {
            for (int i = 0; i <= NumberOfRows; ++i)
            {
                for (int j = 0; j <= NumberOfColums; ++j)
                {
                    Point point = new Point(j * GameWidth, i * GameHeight);

                    PointList.Add(point);
                }
            }
        }

        public void CreateLineList(Brush brush)
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
                    line.MouseEnter += Line_MouseEnter;
                    line.MouseLeave += Line_MouseLeave;
                    line.MouseLeftButtonDown += Line_MouseLeftButtonDown;
                    LineList.Add(line);
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
                    line.MouseEnter += Line_MouseEnter;
                    line.MouseLeave += Line_MouseLeave;
                    line.MouseLeftButtonDown += Line_MouseLeftButtonDown;
                    LineList.Add(line);
                }
            }
        }

        public void Window_InitScore(object sender, EventArgs e)
        {
            OnScoreChanged();
        }

        private void Line_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Line)
            {
                Line line = (Line)sender;
                if (TurnId == 0)
                {
                    line.Stroke = Brushes.Blue;
                }
                else
                {
                    line.Stroke = Brushes.Red;
                }
                CheckState(line);

            }
        }

        private void Line_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Line)
            {
                Line line = (Line)sender;
                if (line.Stroke == Brushes.Black)
                {
                    line.Stroke = Brushes.White;
                }
            }
        }

        private void Line_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Line)
            {
                Line line = (Line)sender;
                if (line.Stroke == Brushes.White)
                {
                    line.Stroke = Brushes.Black;

                }
            }
        }


        private void OnScoreChanged()
        {
            ScoreChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnRectangleEnclosed(Point point)
        {
            RectangleEnclosed?.Invoke(this, new RectangleEventArgs(point));
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

        public void CheckState(Line line)
        {
            int counter = 0;
            if (line.X1 == line.X2)
            {
                foreach (Line element in LineList)
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
                foreach (Line element in LineList)
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
            Scores[TurnId] += counter;
            if (counter == 0)
            {
                TurnId = 1 - TurnId;
                return;
            }
            OnScoreChanged();

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

        private bool isPointsOfLine(Line line, Point p1, Point p2)
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
                foreach (Line element in LineList)
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
                foreach (Line element in LineList)
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
                OnRectangleEnclosed(point);
                return 1;
            }
        }
    }
}
