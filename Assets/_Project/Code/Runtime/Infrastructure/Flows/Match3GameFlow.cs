using Code.Runtime.Infrastructure.Factories;
using Code.Runtime.Logic.Match3;
using UnityEngine;
using VContainer.Unity;

namespace Code.Runtime.Infrastructure.Flows
{
    public class Match3GameFlow : IStartable
    {
        private readonly Transform _spawnPoint;
        private readonly Match3Factory _factory;
        private readonly Match3GameLoop _gameLoop;
        private readonly Match3GridState _gridState;
        private readonly Match3GridView _gridView;

        public Match3GameFlow(Transform spawnPoint, Match3Factory factory, Match3GameLoop gameLoop,
            Match3GridState match3State, Match3GridView gridView)
        {
            _spawnPoint = spawnPoint;
            _factory = factory;
            _gameLoop = gameLoop;
            _gridState = match3State;
            _gridView = gridView;
        }

        public async void Start()
        {
            await _factory.Load();
            await _gridState.Load();
            await _gridView.Load(_spawnPoint.position);
            await _gameLoop.Load();
        }
    }
}