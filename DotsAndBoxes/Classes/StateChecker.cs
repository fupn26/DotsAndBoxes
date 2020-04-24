using DotsAndBoxes.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;

namespace DotsAndBoxes.Classes
{
    public class StateChecker
    {
        public int GameWidth { get; protected set; }
        public int GameHeight { get; protected set; }
        protected readonly string BlackBrushCode = Brushes.Black.ToString();
        protected readonly string WhiteBrushCode = Brushes.White.ToString();


        protected bool AreEqualLines(LineStructure line1, LineStructure line2)
        {
            if (line1.X1 == line2.X1 && line1.Y1 == line2.Y1 && line1.X2 == line2.X2 && line1.Y2 == line2.Y2 ||
                line1.X1 == line2.X2 && line1.Y1 == line2.Y2 && line1.X2 == line2.X1 && line1.Y2 == line2.Y1)
            {
                return true;
            }
            return false;
        }

        protected bool BrushCompare(string brushAsString1, string brushAsString2)
        {
            return brushAsString1 == brushAsString2;
        }


        protected LineStructure MoveUp(LineStructure line)
        {
            LineStructure newLine = new LineStructure
            {
                X1 = line.X1,
                X2 = line.X2,
                Y1 = line.Y1 - GameHeight,
                Y2 = line.Y2 - GameHeight
            };
            return newLine;
        }
        protected LineStructure MoveDown(LineStructure line)
        {
            LineStructure newLine = new LineStructure
            {
                X1 = line.X1,
                X2 = line.X2,
                Y1 = line.Y1 + GameHeight,
                Y2 = line.Y2 + GameHeight
            };
            return newLine;
        }

        protected LineStructure MoveRight(LineStructure line)
        {
            LineStructure newLine = new LineStructure
            {
                X1 = line.X1 + GameWidth,
                X2 = line.X2 + GameWidth,
                Y1 = line.Y1,
                Y2 = line.Y2
            };
            return newLine;
        }

        protected LineStructure MoveLeft(LineStructure line)
        {
            LineStructure newLine = new LineStructure
            {
                X1 = line.X1 - GameWidth,
                X2 = line.X2 - GameWidth,
                Y1 = line.Y1,
                Y2 = line.Y2
            };
            return newLine;
        }

        protected bool IsLineColored(LineStructure line)
        {
            if (BrushCompare(line.StrokeColor, Brushes.Black.ToString()) ||
                    BrushCompare(line.StrokeColor, Brushes.White.ToString()))
            {
                return false;
            }
            return true;
        }

        protected Tuple<List<Point>, int> CheckState(LineStructure refLine, List<LineStructure> lineList)
        {
            Tuple<List<Point>, int> result;
            List<Point> pointList = new List<Point>();
            int counter = 0;
            if (IsVerticalLine(refLine))
            {
                foreach (LineStructure element in lineList)
                {
                    if (AreEqualLines(MoveLeft(refLine), element) &&
                        IsLineColored(element))
                    {
                        counter += IsSquare(refLine, element, true, lineList, pointList);
                    }
                    else if (AreEqualLines(MoveRight(refLine), element) &&
                            IsLineColored(element))
                    {
                        counter += IsSquare(refLine, element, true, lineList, pointList);
                    }
                }
            }
            else
            {
                foreach (LineStructure element in lineList)
                {
                    if (AreEqualLines(MoveUp(refLine), element) &&
                        IsLineColored(element))
                    {
                        counter += IsSquare(refLine, element, false, lineList, pointList);
                    }
                    else if (AreEqualLines(MoveDown(refLine), element) &&
                        IsLineColored(element))
                    {
                        counter += IsSquare(refLine, element, false, lineList, pointList);
                    }
                }
            }

            result = new Tuple<List<Point>, int>(pointList, counter);
            return result;
        }

        private bool IsPointsOfLine(LineStructure line, Point p1, Point p2)
        {
            Point lineP1 = new Point(line.X1, line.Y1);
            Point lineP2 = new Point(line.X2, line.Y2);

            if (lineP1.Equals(p1) && lineP2.Equals(p2) ||
                lineP1.Equals(p2) && lineP2.Equals(p1))
            {
                return true;
            }

            return false;
        }

        protected bool IsVerticalLine(LineStructure line)
        {
            if(line.X1 == line.X2)
            {
                return true;
            }
            return false;
        } 

        private int IsSquare(LineStructure line1,
                             LineStructure line2,
                             bool isVertical,
                             List<LineStructure> lineList,
                             List<Point> pointList)
        {
            Point point1 = new Point(line1.X1, line1.Y1);
            Point point2 = new Point(line1.X2, line1.Y2);
            Point point3 = new Point(line2.X1, line2.Y1);
            Point point4 = new Point(line2.X2, line2.Y2);

            int counter = 0;
            if (isVertical)
            {
                foreach (LineStructure element in lineList)
                {
                    if (element.Y1 == element.Y2)
                    {
                        if (IsPointsOfLine(element, point1, point3) &&
                            IsLineColored(element))
                        {
                            counter++;
                        }
                        else if (IsPointsOfLine(element, point2, point4) &&
                                IsLineColored(element))
                        {
                            counter++;
                        }
                    }
                }
            }
            else
            {
                foreach (LineStructure element in lineList)
                {
                    if (element.X1 == element.X2)
                    {
                        if (IsPointsOfLine(element, point1, point3) &&
                            IsLineColored(element))
                        {
                            counter++;
                        }
                        else if (IsPointsOfLine(element, point2, point4) &&
                                IsLineColored(element))
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
                pointList.Add(MinPoint(point1, point2, point3, point4));
                return 1;
            }
        }
        private Point MinPoint(Point p1, Point p2, Point p3, Point p4)
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
