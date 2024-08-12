using System;

namespace Code.Runtime.Infrastructure.StaticData
{
    [Serializable]
    public struct LevelGoalData
    {
        public GoalLimit limit;
        public int goalCount;
        public int minGoalValue;
        public int maxGoalValue;
    }
}