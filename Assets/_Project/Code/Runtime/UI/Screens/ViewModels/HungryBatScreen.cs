using System;
using System.Collections.Generic;
using Code.Runtime.Infrastructure.Factories;
using Code.Runtime.Infrastructure.StaticData;
using Code.Runtime.Match3;
using Code.Runtime.Match3.Services;

namespace Code.Runtime.UI.Screens.ViewModels
{
    public class HungryBatScreen : BaseScreenViewModel
    {
        public int limit => _goalService.limit;
        public int colorTargetBonusUse { get; private set; }

        public event Action<int> onBonusUsed
        {
            add => _game.onBonusUsed += value;
            remove => _game.onBonusUsed -= value;
        }

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

        private readonly Match3Game _game;
        private readonly IGameGoalService _goalService;
        private readonly IMatch3Factory _factory;

        public HungryBatScreen(IMatch3Factory factory, Match3Game game, IGameGoalService goalService,
            Match3LevelConfig levelConfig)
        {
            _factory = factory;
            _game = game;
            _goalService = goalService;
            colorTargetBonusUse = levelConfig.colorTargetBonusUse;
        }

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

        public override void Dispose()
        {
        }
    }
}