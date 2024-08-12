using System;
using System.Collections.Generic;
using Code.Runtime.Infrastructure.Services;
using Code.Runtime.Infrastructure.StaticData;

namespace Code.Runtime.Match3.Services
{
    public interface IGameGoalService : ILoadUnit<Match3LevelConfig>, IDisposable
    {
        public int limit { get; }
        public List<Match3GoalData> goals { get; }
        public event Action<int> onLimitProcessed;
        public event Action<Match3GoalData> onProgress;
        public event Action<bool> onGameFinished;
    }
}