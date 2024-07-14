namespace Code.Match3
{
    public readonly struct ShapeCreateInfo
    {
        public readonly ShapePos pos;
        public readonly ShapeInfo shape;

        public ShapeCreateInfo(ShapePos pos, ShapeInfo shape)
        {
            this.pos = pos;
            this.shape = shape;
        }
    }
}