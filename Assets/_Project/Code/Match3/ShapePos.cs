using System;

namespace Code.Match3
{
    public readonly struct ShapePos
    {
        public static ShapePos zero = new(0, 0);
        public static ShapePos one = new(1, 1);
        public readonly int x;
        public readonly int y;

        public static ShapePos operator -(ShapePos s) =>
            new(-s.x, -s.y);

        public static ShapePos operator -(ShapePos a, ShapePos b) =>
            new(a.x - b.x, a.y - b.y);

        public static ShapePos operator +(ShapePos a, ShapePos b) =>
            new(a.x + b.x, a.y + b.y);

        public static ShapePos operator -(ShapePos a, int b) =>
            new(a.x - b, a.y - b);

        public static ShapePos operator +(ShapePos a, int b) =>
            new(a.x + b, a.y + b);

        public static bool operator ==(ShapePos a, ShapePos b) =>
            a.x == b.x && a.y == b.y;

        public static bool operator !=(ShapePos a, ShapePos b) =>
            !(a == b);

        public ShapePos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public bool Equals(ShapePos other) => 
            x == other.x && y == other.y;

        public override bool Equals(object obj) => 
            obj is ShapePos other && Equals(other);

        public override int GetHashCode() => 
            HashCode.Combine(x, y);
    }
}