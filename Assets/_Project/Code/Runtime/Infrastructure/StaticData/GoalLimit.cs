using System;

namespace Code.Runtime.Infrastructure.StaticData
{
    [Serializable]
    public struct GoalLimit
    {
        public LimitType type;
        public int count;
    }
}