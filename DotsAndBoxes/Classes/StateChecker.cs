using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DotsAndBoxes.Classes
{
    class StateChecker
    {
        public int GameWidth { get; protected set; }
        public int GameHeight { get; protected set; }


        protected bool areEqualLines(Line line1, Line line2)
        {
            if (line1.X1 == line2.X1 && line1.Y1 == line2.Y1 && line1.X2 == line2.X2 && line1.Y2 == line2.Y2 ||
                line1.X1 == line2.X2 && line1.Y1 == line2.Y2 && line1.X2 == line2.X1 && line1.Y2 == line2.Y1)
            {
                return true;
            }
            return false;
        }

        protected bool BrushCompare(Brush brush1, Brush brush2)
        {
            if (brush1.ToString() == brush2.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
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

        protected Tuple<List<Point>, int> CheckState(Line refLine, List<Line> lineList)
        {
            Tuple<List<Point>, int> result;
            List<Point> pointList = new List<Point>();
            int counter = 0;
            if (refLine.X1 == refLine.X2)
            {
                foreach (Line element in lineList)
                {
                    if (areEqualLines(moveLeft(refLine), element) &&
                        !BrushCompare(element.Stroke, Brushes.Black) &&
                        !BrushCompare(element.Stroke, Brushes.White))
                    {
                        counter += isSquare(refLine, element, true, lineList, pointList);
                    }
                    else if (areEqualLines(moveRight(refLine), element) &&
                        !BrushCompare(element.Stroke, Brushes.Black) &&
                        !BrushCompare(element.Stroke, Brushes.White))
                    {
                        counter += isSquare(refLine, element, true, lineList, pointList);
                    }
                }
            }
            else
            {
                foreach (Line element in lineList)
                {
                    if (areEqualLines(moveUp(refLine), element) &&
                        !BrushCompare(element.Stroke, Brushes.Black) &&
                        !BrushCompare(element.Stroke, Brushes.White))
                    {
                        counter += isSquare(refLine, element, false, lineList, pointList);
                    }
                    else if (areEqualLines(moveDown(refLine), element) &&
                        !BrushCompare(element.Stroke, Brushes.Black) &&
                        !BrushCompare(element.Stroke, Brushes.White))
                    {
                        counter += isSquare(refLine, element, false, lineList, pointList);
                    }
                }
            }

            result = new Tuple<List<Point>, int>(pointList, counter);
            return result;
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

        private int isSquare(Line line1, Line line2, bool isVertical, List<Line> LineList, List<Point> PointList)
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
                            !BrushCompare(element.Stroke, Brushes.Black) &&
                            !BrushCompare(element.Stroke, Brushes.White))
                        {
                            counter++;
                        }
                        else if (isPointsOfLine(element, point2, point4) &&
                            !BrushCompare(element.Stroke, Brushes.Black) &&
                            !BrushCompare(element.Stroke, Brushes.White))
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
                            !BrushCompare(element.Stroke, Brushes.Black) &&
                            !BrushCompare(element.Stroke, Brushes.White))
                        {
                            counter++;
                        }
                        else if (isPointsOfLine(element, point2, point4) &&
                            !BrushCompare(element.Stroke, Brushes.Black) &&
                            !BrushCompare(element.Stroke, Brushes.White))
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
                PointList.Add(minPoint(point1, point2, point3, point4));
                return 1;
            }
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

    }
}
