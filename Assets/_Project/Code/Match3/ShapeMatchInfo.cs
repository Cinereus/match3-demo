namespace Code.Match3
{
    public readonly struct ShapeMatchInfo
    {
        public readonly ShapePos startPoint;
        public readonly int length;
        public readonly bool isHorizontal;

        public ShapeMatchInfo(ShapePos startPoint, int length, bool isHorizontal = false)
        {
            this.startPoint = startPoint;
            this.length = length;
            this.isHorizontal = isHorizontal;
        }
    }
}