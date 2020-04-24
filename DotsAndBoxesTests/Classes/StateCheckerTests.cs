using System.Collections.Generic;
using System.Windows.Media;
using DotsAndBoxes.Classes;
using DotsAndBoxes.Structures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotsAndBoxesTests.Classes
{
    [TestClass()]
    public class StateCheckerTests
    {
        string _red = Brushes.Red.ToString();
        string _blue = Brushes.Blue.ToString();
        string _white = Brushes.White.ToString();

        [TestMethod()]
        public void CheckStateTest_ShouldReturn2()
        {
            var lineList = new List<LineStructure>
            {
                new LineStructure() {X1 = 0, Y1 = 0, X2 = 10, Y2 = 0, StrokeColor = _red},
                new LineStructure() {X1 = 10, Y1 = 0, X2 = 20, Y2 = 0, StrokeColor = _red},
                new LineStructure() {X1 = 0, Y1 = 10, X2 = 10, Y2 = 10, StrokeColor = _red},
                new LineStructure() {X1 = 10, Y1 = 10, X2 = 20, Y2 = 10, StrokeColor = _blue},
                new LineStructure() {X1 = 0, Y1 = 20, X2 = 10, Y2 = 20, StrokeColor = _blue},

                new LineStructure() {X1 = 0, Y1 = 0, X2 = 0, Y2 = 10, StrokeColor = _blue},
                new LineStructure() {X1 = 20, Y1 = 0, X2 = 20, Y2 = 10, StrokeColor = _blue},
                new LineStructure() {X1 = 0, Y1 = 10, X2 = 0, Y2 = 20, StrokeColor = _white},
                new LineStructure() {X1 = 10, Y1 = 10, X2 = 10, Y2 = 20, StrokeColor = _blue},
            };

            var refLine = new LineStructure() {X1 = 10, Y1 = 0, X2 = 10, Y2 = 10, StrokeColor = _white};
            var stateChecker = new StateChecker()
            {
                GameHeight = 10,
                GameWidth = 10
            };

            int actual = stateChecker.CheckState(refLine, lineList).Item2;
            int expected = 2;

            Assert.IsTrue(actual == expected);
        }

        [TestMethod()]
        public void CheckStateTest_ShouldReturn1()
        {
            var lineList = new List<LineStructure>
            {
                new LineStructure() { X1 = 0, Y1 = 0, X2 = 10, Y2 = 0, StrokeColor = _red },
                new LineStructure() { X1 = 10, Y1 = 0, X2 = 20, Y2 = 0, StrokeColor = _blue },
                new LineStructure() { X1 = 0, Y1 = 10, X2 = 10, Y2 = 10, StrokeColor = _red },
                new LineStructure() { X1 = 10, Y1 = 10, X2 = 20, Y2 = 10, StrokeColor = _blue },
                new LineStructure() { X1 = 0, Y1 = 20, X2 = 10, Y2 = 20, StrokeColor = _red },

                new LineStructure() { X1 = 0, Y1 = 0, X2 = 0, Y2 = 10, StrokeColor = _white },
                new LineStructure() { X1 = 10, Y1 = 0, X2 = 10, Y2 = 10, StrokeColor = _white },
                new LineStructure() { X1 = 20, Y1 = 0, X2 = 20, Y2 = 10, StrokeColor = _blue },
                new LineStructure() { X1 = 10, Y1 = 10, X2 = 10, Y2 = 20, StrokeColor = _blue }
            };

            var refLine = new LineStructure() { X1 = 0, Y1 = 10, X2 = 0, Y2 = 20, StrokeColor = _white };
            var stateChecker = new StateChecker()
            {
                GameHeight = 10,
                GameWidth = 10
            };

            int actual = stateChecker.CheckState(refLine, lineList).Item2;
            int expected = 1;

            Assert.IsTrue(actual == expected);
        }

        [TestMethod()]
        public void CheckStateTest_ShouldReturn0()
        {
            var lineList = new List<LineStructure>
            {
                new LineStructure() { X1 = 0, Y1 = 0, X2 = 10, Y2 = 0, StrokeColor = _red },
                new LineStructure() { X1 = 10, Y1 = 0, X2 = 20, Y2 = 0, StrokeColor = _blue },
                new LineStructure() { X1 = 0, Y1 = 10, X2 = 10, Y2 = 10, StrokeColor = _red },
                new LineStructure() { X1 = 10, Y1 = 10, X2 = 20, Y2 = 10, StrokeColor = _blue },
                new LineStructure() { X1 = 0, Y1 = 20, X2 = 10, Y2 = 20, StrokeColor = _red },

                new LineStructure() { X1 = 0, Y1 = 0, X2 = 0, Y2 = 10, StrokeColor = _white },
                new LineStructure() { X1 = 0, Y1 = 10, X2 = 0, Y2 = 20, StrokeColor = _white },
                new LineStructure() { X1 = 20, Y1 = 0, X2 = 20, Y2 = 10, StrokeColor = _white },
                new LineStructure() { X1 = 10, Y1 = 10, X2 = 10, Y2 = 20, StrokeColor = _blue }
            };

            var refLine = new LineStructure() { X1 = 10, Y1 = 0, X2 = 10, Y2 = 10, StrokeColor = _white };
            var stateChecker = new StateChecker()
            {
                GameHeight = 10,
                GameWidth = 10
            };

            int actual = stateChecker.CheckState(refLine, lineList).Item2;
            int expected = 0;

            Assert.IsTrue(actual == expected);
        }
    }
}