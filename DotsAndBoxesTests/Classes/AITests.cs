using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotsAndBoxes.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using DotsAndBoxes.Structures;

namespace DotsAndBoxes.Classes.Tests
{
    [TestClass()]
    public class AITests
    {
        string red = Brushes.Red.ToString();
        string blue = Brushes.Blue.ToString();
        string white = Brushes.White.ToString();

        [TestMethod()]
        public void ChooseLine_ShouldChooseTwoShotLine()
        {
            var lineList = new List<LineStructure>
            {
                new LineStructure() { X1 = 0, Y1 = 0, X2 = 10, Y2 = 0, StrokeColor = red },
                new LineStructure() { X1 = 10, Y1 = 0, X2 = 20, Y2 = 0, StrokeColor = red },
                new LineStructure() { X1 = 0, Y1 = 10, X2 = 10, Y2 = 10, StrokeColor = red },
                new LineStructure() { X1 = 10, Y1 = 10, X2 = 20, Y2 = 10, StrokeColor = blue },
                new LineStructure() { X1 = 0, Y1 = 20, X2 = 10, Y2 = 20, StrokeColor = blue },

                new LineStructure() { X1 = 0, Y1 = 0, X2 = 0, Y2 = 10, StrokeColor = blue },
                new LineStructure() { X1 = 20, Y1 = 0, X2 = 20, Y2 = 10, StrokeColor = blue },
                new LineStructure() { X1 = 0, Y1 = 10, X2 = 0, Y2 = 20, StrokeColor = white },
                new LineStructure() { X1 = 10, Y1 = 10, X2 = 10, Y2 = 20, StrokeColor = blue }
            };
            var expected = new LineStructure() { X1 = 10, Y1 = 0, X2 = 10, Y2 = 10, StrokeColor = white};
            lineList.Add(expected);

            var ai = new AI(10, 10)
            {
                LineList = lineList
            };

            ai.ChooseLine();
            var actual = ai.ChoosedLine;

            Assert.AreEqual(expected, actual, "The chosen line is not a 'Two shot' line!");
        }



        [TestMethod()]
        public void ChooseLine_ShouldChooseOneShotLine()
        {
            var lineList = new List<LineStructure>
            {
                new LineStructure() { X1 = 0, Y1 = 0, X2 = 10, Y2 = 0, StrokeColor = red },
                new LineStructure() { X1 = 10, Y1 = 0, X2 = 20, Y2 = 0, StrokeColor = blue },
                new LineStructure() { X1 = 0, Y1 = 10, X2 = 10, Y2 = 10, StrokeColor = red },
                new LineStructure() { X1 = 10, Y1 = 10, X2 = 20, Y2 = 10, StrokeColor = blue },
                new LineStructure() { X1 = 0, Y1 = 20, X2 = 10, Y2 = 20, StrokeColor = red },

                new LineStructure() { X1 = 0, Y1 = 0, X2 = 0, Y2 = 10, StrokeColor = white },
                new LineStructure() { X1 = 10, Y1 = 0, X2 = 10, Y2 = 10, StrokeColor = white },
                new LineStructure() { X1 = 20, Y1 = 0, X2 = 20, Y2 = 10, StrokeColor = blue },
                new LineStructure() { X1 = 10, Y1 = 10, X2 = 10, Y2 = 20, StrokeColor = blue }
            };
            var expected = new LineStructure() { X1 = 0, Y1 = 10, X2 = 0, Y2 = 20, StrokeColor = white };
            lineList.Add(expected);

            var ai = new AI(10, 10)
            {
                LineList = lineList
            };

            ai.ChooseLine();
            var actual = ai.ChoosedLine;

            Assert.AreEqual(expected, actual, "The chosen line is not the 'One shot' line!");
        }



        [TestMethod()]
        public void ChooseLine_ShouldChooseSecondLine()
        {
            var lineList = new List<LineStructure>
            {
                new LineStructure() { X1 = 0, Y1 = 0, X2 = 10, Y2 = 0, StrokeColor = red },
                new LineStructure() { X1 = 10, Y1 = 0, X2 = 20, Y2 = 0, StrokeColor = white },
                new LineStructure() { X1 = 0, Y1 = 10, X2 = 10, Y2 = 10, StrokeColor = red },
                new LineStructure() { X1 = 10, Y1 = 10, X2 = 20, Y2 = 10, StrokeColor = blue },

                new LineStructure() { X1 = 0, Y1 = 0, X2 = 0, Y2 = 10, StrokeColor = red },
                new LineStructure() { X1 = 10, Y1 = 0, X2 = 10, Y2 = 10, StrokeColor = blue },
                new LineStructure() { X1 = 20, Y1 = 0, X2 = 20, Y2 = 10, StrokeColor = white }
            };
            var expected1 = new LineStructure() { X1 = 0, Y1 = 10, X2 = 0, Y2 = 20, StrokeColor = white };
            var expected2 = new LineStructure() { X1 = 10, Y1 = 10, X2 = 10, Y2 = 20, StrokeColor = white };
            var expected3 = new LineStructure() { X1 = 0, Y1 = 20, X2 = 10, Y2 = 20, StrokeColor = white };
            lineList.Add(expected1);
            lineList.Add(expected2);
            lineList.Add(expected3);

            var ai = new AI(10, 10)
            {
                LineList = lineList
            };

            ai.ChooseLine();
            var actual = ai.ChoosedLine;

            Assert.IsTrue((expected1.Equals(actual) || expected2.Equals(actual) || expected3.Equals(actual)), "The chosen line is not a second line of a rectangle!");
        }

        [TestMethod()]
        public void ChooseLine_ShouldChooseRandom()
        {
            var lineList = new List<LineStructure>
            {
                new LineStructure() { X1 = 0, Y1 = 0, X2 = 10, Y2 = 0, StrokeColor = red },
                new LineStructure() { X1 = 10, Y1 = 0, X2 = 20, Y2 = 0, StrokeColor = blue },
                new LineStructure() { X1 = 0, Y1 = 20, X2 = 10, Y2 = 20, StrokeColor = red },

                new LineStructure() { X1 = 0, Y1 = 0, X2 = 0, Y2 = 10, StrokeColor = blue },
                new LineStructure() { X1 = 20, Y1 = 0, X2 = 20, Y2 = 10, StrokeColor = red },
                new LineStructure() { X1 = 0, Y1 = 10, X2 = 0, Y2 = 20, StrokeColor = red }
            };
            var expected1 = new LineStructure() { X1 = 0, Y1 = 10, X2 = 10, Y2 = 10, StrokeColor = white };
            var expected2 = new LineStructure() { X1 = 10, Y1 = 10, X2 = 20, Y2 = 10, StrokeColor = white };
            var expected3 = new LineStructure() { X1 = 10, Y1 = 0, X2 = 10, Y2 = 10, StrokeColor = white };
            var expected4 = new LineStructure() { X1 = 10, Y1 = 10, X2 = 10, Y2 = 20, StrokeColor = white };

            lineList.Add(expected1);
            lineList.Add(expected2);
            lineList.Add(expected3);
            lineList.Add(expected4);

            var ai = new AI(10, 10)
            {
                LineList = lineList
            };

            ai.ChooseLine();
            var actual = ai.ChoosedLine;

            Assert.IsTrue(expected1.Equals(actual) ||
                          expected2.Equals(actual) ||
                          expected3.Equals(actual) ||
                          expected4.Equals(actual), "The chosen line is not a white line.");
        }
    }
}