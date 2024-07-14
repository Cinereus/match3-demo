namespace Code.Match3
{
    public readonly struct ShapeInfo
    {
        public readonly ShapeType type;
        public readonly ShapeBonusType bonusType;

        public bool isBonus => bonusType != ShapeBonusType.NONE;
        public bool isDestroyed => type == ShapeType.NONE && bonusType == ShapeBonusType.NONE;

        public bool Equals(ShapeInfo other) => type == other.type && bonusType == other.bonusType;

        public ShapeInfo(ShapeType type = ShapeType.NONE, ShapeBonusType bonusType = ShapeBonusType.NONE)
        {
            this.type = type;
            this.bonusType = bonusType;
        }

        public override string ToString() => $"({type}, {bonusType})";
    }
}