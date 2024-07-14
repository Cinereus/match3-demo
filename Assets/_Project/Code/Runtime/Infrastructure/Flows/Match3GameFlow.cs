using Code.Runtime.Infrastructure.Factories;
using Code.Runtime.Infrastructure.Services;
using Code.Runtime.Match3;
using Code.Runtime.Match3.Services;
using UnityEngine;
using VContainer.Unity;

namespace Code.Runtime.Infrastructure.Flows
{
    public class Match3GameFlow : IStartable
    {
        private readonly Camera _camera;
        private readonly Transform _spawnPoint;
        private readonly Match3Factory _factory;
        private readonly GameMoveService _gameMoveService;
        private readonly Match3Game _match3Game;
        private readonly Match3GridView _gridView;
        private readonly LoadingService _loadingService;

        public Match3GameFlow(LoadingService loadingService, Transform spawnPoint, Match3Factory factory,
            Match3Game match3Game, Match3GridView gridView, GameMoveService gameMoveService)
        {
            _loadingService = loadingService;
            _spawnPoint = spawnPoint;
            _factory = factory;
            _match3Game = match3Game;
            _gridView = gridView;
            _gameMoveService = gameMoveService;
        }

        public async void Start()
        {
            await _loadingService.Load(_factory);
            await _loadingService.Load(_gridView, _spawnPoint.position);
            await _loadingService.Load(_match3Game, Camera.main);
            await _loadingService.Load(_gameMoveService,  _gridView.positions[0, 0]);
        }
    }
}