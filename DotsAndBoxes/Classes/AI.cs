using DotsAndBoxes.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DotsAndBoxes.Classes
{
    class AI : StateChecker
    {
        public List<Line> LineList { get; set; }
        private Line _choosedLine;
        private readonly Random _random;

        public event EventHandler<LineEventArgs> LineChosen;

        public AI(int gameHeight, int gameWidth)
        {
            GameHeight = gameHeight;
            GameWidth = gameWidth;
            _random = new Random();
        }

        public void GameController_AITurn(object sender, ListEventArgs<Line> e)
        {
            LineList = e.RefList;
            ChooseLine();
            OnLineChosen();
        }

        private void OnLineChosen()
        {
            LineChosen?.Invoke(this, new LineEventArgs(_choosedLine));
        }

        private void ChooseLine()
        {
            Line twoShot = null;
            Line oneShot = null;
            foreach (Line line in LineList)
            {
                if (BrushCompare(line.Stroke, Brushes.White))
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

            if (twoShot == null && oneShot == null)
            {
                if (BrushCompare(LineList[1].Stroke, Brushes.White))
                {
                    _choosedLine = LineList[1];
                }
                else
                {
                    ChooseRandom();
                }
            }
            else if (twoShot != null)
            {
                _choosedLine = twoShot;
            }
            else
            {
                _choosedLine = oneShot;
            }
        }

        private void ChooseRandom()
        {
            int size = LineList.Count;
            do
            {
                _choosedLine = LineList[_random.Next(size - 1)];

            } while (!BrushCompare(_choosedLine.Stroke, Brushes.White));
        }
    }
}
