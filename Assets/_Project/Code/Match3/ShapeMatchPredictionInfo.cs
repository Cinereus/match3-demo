namespace Code.Match3
{
    public readonly struct ShapeMatchPredictionInfo
    {
        public readonly ShapePos from;
        public readonly ShapePos direction;

        public ShapeMatchPredictionInfo(ShapePos from, ShapePos direction)
        {
            this.from = from;
            this.direction = direction;
        }
    }
}