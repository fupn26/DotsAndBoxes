using DotsAndBoxes.CustomEventArgs;
using DotsAndBoxes.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DotsAndBoxes.Classes
{
    public class Ai : StateChecker
    {
        public List<LineStructure> LineList { get; set; }
        public LineStructure ChoosedLine { get; private set; }
        private readonly Random _random;

        public event EventHandler<CustomEventArgs<LineStructure>> LineChosen;

        public Ai(int gameHeight, int gameWidth)
        {
            GameHeight = gameHeight;
            GameWidth = gameWidth;
            _random = new Random();
        }

        public void GameController_AITurn(object sender, CustomEventArgs<List<LineStructure>> e)
        {
            LineList = e.Content;
            ChooseLine();
            OnLineChosen();
        }

        private void OnLineChosen()
        {
            LineChosen?.Invoke(this, new CustomEventArgs<LineStructure>(ChoosedLine));
        }

        public void ChooseLine()
        {
            LineStructure twoShot = null;
            LineStructure oneShot = null;
            LineStructure second = null;

            foreach (LineStructure line in LineList)
            {
                if (BrushCompare(line.StrokeColor, WhiteBrushCode))
                {
                    Tuple<List<Point>, int> result = CheckState(line, LineList);
                    if (result.Item2 == 2)
                    {
                        twoShot = line;
                    }
                    else if (result.Item2 == 1)
                    {
                        oneShot = line;
                    }
                }
            }

            foreach (LineStructure line in LineList)
            {
                if (BrushCompare(line.StrokeColor, WhiteBrushCode))
                {
                    if (ChooseSecond(line))
                    {
                        second = line;
                        break;
                    }
                }
            }

            if (twoShot == null && oneShot == null)
            {
                if (second != null)
                {
                    ChoosedLine = second;
                }
                else
                {
                    ChooseRandom();
                }
            }
            else if (twoShot != null)
            {
                ChoosedLine = twoShot;
            }
            else
            {
                ChoosedLine = oneShot;
            }
        }

        private void ChooseRandom()
        {
            int size = LineList.Count;
            do
            {
                ChoosedLine = LineList[_random.Next(size - 1)];

            } while (!BrushCompare(ChoosedLine.StrokeColor, WhiteBrushCode));
        }

        private bool FindLine(LineStructure refLine)
        {
            foreach(LineStructure line in LineList)
            {
                if(AreEqualLines(refLine, line))
                {
                    return true;
                }
            }
            return false;
        }

        private int CheckBrush(LineStructure refLine)
        {
            foreach (LineStructure line in LineList)
            {
                if (AreEqualLines(refLine, line) && !BrushCompare(line.StrokeColor, WhiteBrushCode))
                {
                    return 1;
                }
            }
            return 0;
        }

        private Tuple<List<LineStructure>, List<LineStructure>> 
            CreateNeighbourLineListVerticalCase(LineStructure refLine)
        {
            List<LineStructure> lineListLeft = new List<LineStructure>();
            List<LineStructure> lineListRight = new List<LineStructure>();

            LineStructure leftSide = MoveLeft(refLine);
            LineStructure leftTop = new LineStructure()
            {
                X1 = leftSide.X1,
                Y1 = leftSide.Y1,
                X2 = refLine.X1,
                Y2 = refLine.Y1
            };
            LineStructure leftBottom = new LineStructure()
            {
                X1 = leftSide.X2,
                Y1 = leftSide.Y2,
                X2 = refLine.X2,
                Y2 = refLine.Y2
            };
            LineStructure rightSide = MoveRight(refLine);
            LineStructure rightTop = new LineStructure()
            {
                X1 = refLine.X1,
                Y1 = refLine.Y1,
                X2 = rightSide.X1,
                Y2 = rightSide.Y1
            };
            LineStructure rightBottom = new LineStructure()
            {
                X1 = refLine.X2,
                Y1 = refLine.Y2,
                X2 = rightSide.X2,
                Y2 = rightSide.Y2
            };

            lineListLeft.Add(leftSide);
            lineListLeft.Add(leftTop);
            lineListLeft.Add(leftBottom);
            lineListRight.Add(rightSide);
            lineListRight.Add(rightTop);
            lineListRight.Add(rightBottom);

            return new Tuple<List<LineStructure>, List<LineStructure>>(lineListLeft, lineListRight);

        }

        private Tuple<List<LineStructure>, List<LineStructure>> 
            CreateNeighbourLineListHorizontalCase(LineStructure refLine)
        {
            List<LineStructure> lineListUp = new List<LineStructure>();
            List<LineStructure> lineListDown = new List<LineStructure>();

            LineStructure upperTop = MoveUp(refLine);
            LineStructure upperLeft = new LineStructure()
            {
                X1 = upperTop.X1,
                Y1 = upperTop.Y1,
                X2 = refLine.X1,
                Y2 = refLine.Y1
            };
            LineStructure upperRight = new LineStructure()
            {
                X1 = upperTop.X2,
                Y1 = upperTop.Y2,
                X2 = refLine.X2,
                Y2 = refLine.Y2
            };
            LineStructure lowerBottom = MoveDown(refLine);
            LineStructure lowerLeft = new LineStructure()
            {
                X1 = refLine.X1,
                Y1 = refLine.Y1,
                X2 = lowerBottom.X1,
                Y2 = lowerBottom.Y1
            };
            LineStructure lowerRight = new LineStructure()
            {
                X1 = refLine.X2,
                Y1 = refLine.Y2,
                X2 = lowerBottom.X2,
                Y2 = lowerBottom.Y2
            };

            lineListUp.Add(upperTop);
            lineListUp.Add(upperLeft);
            lineListUp.Add(upperRight);
            lineListDown.Add(lowerBottom);
            lineListDown.Add(lowerLeft);
            lineListDown.Add(lowerRight);

            return new Tuple<List<LineStructure>, List<LineStructure>>(lineListUp, lineListDown);
        }

        private bool ChecNeighbours(LineStructure refLine)
        {
            int counter1 = 0;
            int counter2 = 0;
            Tuple<List<LineStructure>, List<LineStructure>> neighbourList;
            if (IsVerticalLine(refLine))
            {
                neighbourList = CreateNeighbourLineListVerticalCase(refLine);
            }
            else
            {
                neighbourList = CreateNeighbourLineListHorizontalCase(refLine);
            }

            foreach (LineStructure line in neighbourList.Item1)
            {
                if (!FindLine(line))
                {
                    counter1 = 0;
                    break;
                }
                counter1 += CheckBrush(line);

            }

            foreach (LineStructure line in neighbourList.Item2)
            {
                if (!FindLine(line))
                {
                    counter2 = 0;
                    break;
                }
                counter2 += CheckBrush(line);
            }

            if (counter1 > 1 || counter2 > 1)
            {
                return false;
            }
            else if (counter1 == 1 || counter2 == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private bool ChooseSecond(LineStructure line)
        {
            if (ChecNeighbours(line))
            {
                return true;
            }
            return false;
        }
    }
}
