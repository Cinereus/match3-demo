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
        private readonly Match3Grid _grid;
        
        public Match3GameFlow(Transform spawnPoint, Match3Factory factory,  Match3Grid grid)
        {
            _spawnPoint = spawnPoint;
            _factory = factory;
            _grid = grid;
        }

        public async void Start()
        {
            await _factory.Load();
            await _grid.Load(_spawnPoint);
        }
    }
}