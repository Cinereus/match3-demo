namespace Code.Match3
{
    public readonly struct ShapeFallInfo
    {
        public readonly int x;
        public readonly int fromY;
        public readonly int toY;

        public ShapeFallInfo(int x, int fromY, int toY)
        {
            this.x = x;
            this.fromY = fromY;
            this.toY = toY;
        }
    }
}