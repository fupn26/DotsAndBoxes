using System;
using System.Collections.Generic;
using DotsAndBoxes.CustomEventArgs;
using DotsAndBoxes.Structures;

namespace DotsAndBoxes.Classes
{
    public class Ai : StateChecker
    {
        private readonly Random _random;

        public List<LineStructure> LineList { get; set; }
        public LineStructure ChosenLine { get; private set; }

        public Ai(int gameHeight, int gameWidth)
        {
            GameHeight = gameHeight;
            GameWidth = gameWidth;
            _random = new Random();
        }

        public void ChooseLine()
        {
            var twoShot = SearchForTwoShot();
            var oneShot = SearchForOneShot();
            var second = SearchForSecondLine();


            if (twoShot == null && oneShot == null)
            {
                if (second != null)
                    ChosenLine = second;
                else
                    ChooseRandom();
            }
            else if (twoShot != null)
            {
                ChosenLine = twoShot;
            }
            else
            {
                ChosenLine = oneShot;
            }
        }

        public void GameController_AITurn(object sender, CustomEventArgs<List<LineStructure>> e)
        {
            LineList = e.Content;
            ChooseLine();
            OnLineChosen();
        }

        public event EventHandler<CustomEventArgs<LineStructure>> LineChosen;

        private int CheckBrush(LineStructure refLine)
        {
            foreach (var line in LineList)
                if (AreEqualLines(refLine, line) && IsLineColored(line))
                    return 1;
            return 0;
        }

        private bool CheckNeighbours(LineStructure refLine)
        {
            int counter1;
            int counter2;
            Tuple<List<LineStructure>, List<LineStructure>> neighbourList;
            neighbourList = IsVerticalLine(refLine)
                ? CreateNeighbourLineListVerticalCase(refLine)
                : CreateNeighbourLineListHorizontalCase(refLine);

            counter1 = SearchForColoredLines(neighbourList.Item1);
            counter2 = SearchForColoredLines(neighbourList.Item2);

            if (counter1 > 1 || counter2 > 1)
                return false;
            return counter1 == 1 || counter2 == 1;
        }

        private void ChooseRandom()
        {
            var size = LineList.Count;
            do
            {
                ChosenLine = LineList[_random.Next(size - 1)];
            } while (IsLineColored(ChosenLine));
        }


        private bool ChooseSecond(LineStructure line)
        {
            return CheckNeighbours(line);
        }

        private LineStructure CreateLineStructureBasedOnRefs(int x1, int x2, int y1, int y2)
        {
            var line = new LineStructure
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2
            };

            return line;
        }

        private Tuple<List<LineStructure>, List<LineStructure>>
            CreateNeighbourLineListHorizontalCase(LineStructure refLine)
        {
            List<LineStructure> lineListUp;
            List<LineStructure> lineListDown;

            var upperTop = MoveUp(refLine);
            var upperLeft = CreateLineStructureBasedOnRefs(upperTop.X1, refLine.X1, upperTop.Y1, refLine.Y1);
            var upperRight = CreateLineStructureBasedOnRefs(upperTop.X2, refLine.X2, upperTop.Y2, refLine.Y2);
            var lowerBottom = MoveDown(refLine);
            var lowerLeft = CreateLineStructureBasedOnRefs(refLine.X1, lowerBottom.X1, refLine.Y1, lowerBottom.Y1);
            var lowerRight = CreateLineStructureBasedOnRefs(refLine.X2, lowerBottom.X2, refLine.Y2, lowerBottom.Y2);

            lineListUp = new List<LineStructure> {upperTop, upperLeft, upperRight};
            lineListDown = new List<LineStructure> {lowerBottom, lowerLeft, lowerRight};

            return new Tuple<List<LineStructure>, List<LineStructure>>(lineListUp, lineListDown);
        }

        private Tuple<List<LineStructure>, List<LineStructure>>
            CreateNeighbourLineListVerticalCase(LineStructure refLine)
        {
            List<LineStructure> lineListLeft;
            List<LineStructure> lineListRight;

            var leftSide = MoveLeft(refLine);
            var leftTop = CreateLineStructureBasedOnRefs(leftSide.X1, refLine.X1, leftSide.Y1, refLine.Y1);
            var leftBottom = CreateLineStructureBasedOnRefs(leftSide.X2, refLine.X2, leftSide.Y2, refLine.Y2);
            var rightSide = MoveRight(refLine);
            var rightTop = CreateLineStructureBasedOnRefs(refLine.X1, rightSide.X1, refLine.Y1, rightSide.Y1);
            var rightBottom = CreateLineStructureBasedOnRefs(refLine.X2, rightSide.X2, refLine.Y2, rightSide.Y2);

            lineListLeft = new List<LineStructure> {leftSide, leftTop, leftBottom};
            lineListRight = new List<LineStructure> {rightSide, rightTop, rightBottom};

            return new Tuple<List<LineStructure>, List<LineStructure>>(lineListLeft, lineListRight);
        }

        private bool FindLine(LineStructure refLine)
        {
            foreach (var line in LineList)
                if (AreEqualLines(refLine, line))
                    return true;
            return false;
        }

        private void OnLineChosen()
        {
            LineChosen?.Invoke(this, new CustomEventArgs<LineStructure>(ChosenLine));
        }

        private int SearchForColoredLines(List<LineStructure> lines)
        {
            var counter = 0;
            foreach (var line in lines)
            {
                if (!FindLine(line))
                {
                    counter = 0;
                    break;
                }

                counter += CheckBrush(line);
            }

            return counter;
        }

        private LineStructure SearchForOneShot()
        {
            foreach (var line in LineList)
                if (!IsLineColored(line))
                {
                    var result = CheckState(line, LineList);
                    if (result.Item2 == 1)
                        return line;
                }

            return null;
        }

        private LineStructure SearchForSecondLine()
        {
            foreach (var line in LineList)
                if (!IsLineColored(line))
                    if (ChooseSecond(line))
                        return line;

            return null;
        }

        private LineStructure SearchForTwoShot()
        {
            foreach (var line in LineList)
                if (!IsLineColored(line))
                {
                    var result = CheckState(line, LineList);
                    if (result.Item2 == 2)
                        return line;
                }

            return null;
        }
    }
}