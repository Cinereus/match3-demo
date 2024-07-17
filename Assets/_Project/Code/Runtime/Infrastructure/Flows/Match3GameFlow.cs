using Code.Runtime.Infrastructure.Factories;
using Code.Runtime.Infrastructure.Services;
using Code.Runtime.Infrastructure.StaticData;
using Code.Runtime.Match3;
using Code.Runtime.Match3.Services;
using UnityEngine;
using VContainer.Unity;

namespace Code.Runtime.Infrastructure.Flows
{
    public class Match3GameFlow : IStartable
    {
        private readonly Camera _camera;
        private readonly Transform _gridContainer;
        private readonly Transform _environmentContainer;
        private readonly Match3Factory _factory;
        private readonly Match3VFXFactory _vfxFactory;
        private readonly GameMoveService _gameMoveService;
        private readonly Match3Game _match3Game;
        private readonly Match3GridView _gridView;
        private readonly LoadingService _loadingService;
        private readonly Match3LevelConfig _levelConfig;

        public Match3GameFlow(LoadingService loadingService, Transform gridContainer, Transform environmentContainer,
            Match3Factory factory, Match3Game match3Game, Match3GridView gridView, GameMoveService gameMoveService,
            Match3LevelConfig levelConfig, Match3VFXFactory vfxFactory)
        {
            _loadingService = loadingService;
            _gridContainer = gridContainer;
            _environmentContainer = environmentContainer;
            _factory = factory;
            _vfxFactory = vfxFactory;
            _match3Game = match3Game;
            _gridView = gridView;
            _gameMoveService = gameMoveService;
            _levelConfig = levelConfig;
        }

        public async void Start()
        {
            await _loadingService.Load(_vfxFactory);
            await _loadingService.Load(_factory, _levelConfig);
            await _loadingService.Load(_gridView, new Match3GridView.Match3GridViewParams(_gridContainer, _levelConfig));
            await _loadingService.Load(_match3Game, _levelConfig);
            await _loadingService.Load(_gameMoveService,  _gridView.positions[0, 0]);
            _factory.CreateEnvironment(_environmentContainer);
        }
    }
}