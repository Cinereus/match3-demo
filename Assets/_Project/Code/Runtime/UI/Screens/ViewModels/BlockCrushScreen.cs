using System;
using System.Collections.Generic;
using Code.Runtime.Infrastructure.Factories;
using Code.Runtime.Match3.Services;

namespace Code.Runtime.UI.Screens.ViewModels
{
    public class BlockCrushScreen : BaseScreenViewModel
    {
        public int limit => _goalService.limit;

        public event Action<Match3GoalData> onGoalCountChanged
        {
            add => _goalService.onProgress += value;
            remove => _goalService.onProgress -= value;
        }

        public event Action<int> onLimitProcessed
        {
            add => _goalService.onLimitProcessed += value;
            remove => _goalService.onLimitProcessed -= value;
        }

        public event Action<bool> onGameFinished
        {
            add => _goalService.onGameFinished += value;
            remove => _goalService.onGameFinished -= value;
        }

        private readonly IMatch3Factory _factory;
        private readonly IGameGoalService _goalService;

        public BlockCrushScreen(IGameGoalService goalService, IMatch3Factory factory)
        {
            _factory = factory;
            _goalService = goalService;
        }

        public override void Dispose(){}

        public List<Match3ShapeGoalView> CreateGoalViews()
        {
            List<Match3GoalData> goals = _goalService.goals;
            List<Match3ShapeGoalView> goalViews = new List<Match3ShapeGoalView>();
            for (int i = 0; i < goals.Count; i++)
            {
                goalViews.Add(_factory.CreateGoalView(goals[i].shapeType, goals[i].count));
            }
            return goalViews;
        }
    }
}