using System;
using Code.Match3;

namespace Code.Runtime.Match3.Services
{
    [Serializable]
    public struct Match3GoalData
    {
        public ShapeType shapeType;
        public int count;

        public Match3GoalData(ShapeType shapeType, int count)
        {
            this.shapeType = shapeType;
            this.count = count;
        }
    }
}