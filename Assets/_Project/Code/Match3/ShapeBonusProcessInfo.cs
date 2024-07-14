using System.Collections.Generic;

namespace Code.Match3
{
    public readonly struct ShapeBonusProcessInfo
    {
        public readonly ShapePos bonusPos;
        public readonly ShapeBonusType bonusType;
        public readonly List<ShapePos> affectedShapes;

        public ShapeBonusProcessInfo(ShapePos bonusPos, ShapeBonusType bonusType, List<ShapePos> affectedShapes)
        {
            this.bonusPos = bonusPos;
            this.bonusType = bonusType;
            this.affectedShapes = affectedShapes;
        }
    }
}