using System;
using System.Collections.Generic;
using System.Threading;
using Code.Match3;
using Code.Runtime.Infrastructure.StaticData;
using Cysharp.Threading.Tasks;

namespace Code.Runtime.Match3.Services
{
    public class MovesGoalService : IGameGoalService
    {
        public int limit => _movesLeft;
        public List<Match3GoalData> goals => _goals;
        public event Action<Match3GoalData> onProgress;
        public event Action<bool> onGameFinished;
        public event Action<int> onLimitProcessed;

        private readonly Match3Game _game;
        private readonly Match3GridView _view;
        private readonly List<Match3GoalData> _goals = new List<Match3GoalData>();

        private int _movesLeft;
        private bool _isFinished;

        public MovesGoalService(Match3Game game, Match3GridView view)
        {
            _game = game;
            _view = view;
        }

        public UniTask Load(Match3LevelConfig config, CancellationToken token)
        {
            _movesLeft = config.goal.limit.count;
            _goals.Clear();
            _goals.AddRange(config.GetNewGoals());
            _view.onSwap += OnSwapped;
            _view.onShapeDestroyed += OnShapeDestroyed;
            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            _view.onSwap -= OnSwapped;
            _view.onShapeDestroyed -= OnShapeDestroyed;
        }

        private void OnSwapped()
        {
            if (_movesLeft > 0)
                _movesLeft--;

            onLimitProcessed?.Invoke(_movesLeft);

            if (_movesLeft == 0 && _isFinished == false)
            {
                _isFinished = true;
                _game.isMoveEnabled = false;
                onGameFinished?.Invoke(false);
            }
        }

        private void OnShapeDestroyed(ShapeType type)
        {
            int completed = 0;
            for (int i = 0; i < _goals.Count; i++)
            {
                if (_goals[i].shapeType == type && _goals[i].count > 0)
                {
                    _goals[i] = new Match3GoalData(_goals[i].shapeType, _goals[i].count - 1);
                    onProgress?.Invoke(_goals[i]);
                }

                if (_goals[i].count == 0)
                    completed++;
            }


            if (completed == _goals.Count && _isFinished == false)
            {
                _isFinished = true;
                _game.isMoveEnabled = false;
                onGameFinished?.Invoke(true);
            }
        }
    }
}