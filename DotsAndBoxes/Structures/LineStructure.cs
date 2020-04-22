using System;
using System.Drawing;

namespace DotsAndBoxes.Structures
{
    public class LineStructure
    {
        public int X1 { get; set; }
        public int X2 { get; set; }
        public int Y1 { get; set; }
        public int Y2 { get; set; }
        public string StrokeColor { get; set; }
        public double StrokeThickness { get; set; }

        public static bool operator ==(LineStructure a, LineStructure b)
        {
            if(a is null && b is null)
            {
                return true;
            }
            else if (a is null || b is null)
            {
                return false;
            }
            return (a.X1 == b.X1 &&
                a.X2 == b.X2 && a.Y1 == b.Y1 && a.Y2 == b.Y2) ||
                (a.X1 == b.X2 &&
                a.X2 == b.X1 && a.Y1 == b.Y2 && a.Y2 == b.Y1);

        }

        public static bool operator !=(LineStructure a, LineStructure b)
        {
            return !(a == b);
        }

        public override bool Equals(object other)
        {
            if (!(other is LineStructure toCompareWith))
                return false;
            return (this.X1 == toCompareWith.X1 &&
                this.X2 == toCompareWith.X2 && this.Y1 == toCompareWith.Y1 && this.Y2 == toCompareWith.Y2) ||
                (this.X1 == toCompareWith.X2 &&
                this.X2 == toCompareWith.X1 && this.Y1 == toCompareWith.Y2 && this.Y2 == toCompareWith.Y1);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X1, X2, Y1, Y2);
        }
    }
}
