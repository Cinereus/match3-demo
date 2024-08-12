using System;
using System.Collections.Generic;
using System.Threading;
using Code.Match3;
using Code.Runtime.Infrastructure.StaticData;
using Cysharp.Threading.Tasks;
using Timer = Code.Runtime.Infrastructure.Timers.Timer;

namespace Code.Runtime.Match3.Services
{
    public class TimeGoalService : IGameGoalService
    {
        public int limit => _time;
        public List<Match3GoalData> goals => _goals;
        
        public event Action<bool> onGameFinished;
        public event Action<Match3GoalData> onProgress;
        public event Action<int> onLimitProcessed
        {
            add => _timer.onTick += value; 
            remove => _timer.onTick -= value;
        }

        private readonly List<Match3GoalData> _goals = new List<Match3GoalData>();
        private readonly Timer _timer = new Timer();
        private readonly Match3GridView _view;
        private readonly Match3Game _game;
        private int _time;
        private bool _isFinished;

        public TimeGoalService(Match3GridView view, Match3Game game)
        {
            _view = view;
            _game = game;
        }

        public UniTask Load(Match3LevelConfig config, CancellationToken token)
        {
            _time = config.goal.limit.count;
            _timer.Start(_time);
            _timer.onFinished += OnTimeFinished;
            _view.onShapeDestroyed += OnShapeDestroyed;
            _goals.Clear();
            _goals.AddRange(config.GetNewGoals());
            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            _timer.onFinished -= OnTimeFinished;
            _view.onShapeDestroyed -= OnShapeDestroyed;
            _timer.Dispose();
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
            
            if (completed == _goals.Count && !_isFinished)
            {
                CompleteTheGame(true);
                _timer.onFinished -= OnTimeFinished;
                _timer.Stop();
            }
        }

        private void OnTimeFinished() => 
            CompleteTheGame(false);

        private void CompleteTheGame(bool isVictory)
        {
            onGameFinished?.Invoke(isVictory);
            _game.isMoveEnabled = false;
            _isFinished = true;
        }
    }
}