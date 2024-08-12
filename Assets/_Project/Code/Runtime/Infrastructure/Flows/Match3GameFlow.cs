using System.Threading;
using Code.Runtime.Infrastructure.Factories;
using Code.Runtime.Infrastructure.Scopes;
using Code.Runtime.Infrastructure.Services;
using Code.Runtime.Infrastructure.StaticData;
using Code.Runtime.Match3;
using Code.Runtime.Match3.Services;
using Code.Runtime.UI;
using Code.Runtime.UI.Screens.ViewModels;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Code.Runtime.Infrastructure.Flows
{
    public class Match3GameFlow : IAsyncStartable
    {
        private readonly UIManager _uiManager;
        private readonly Match3Game _match3Game;
        private readonly Match3GridView _gridView;
        private readonly Match3Factory _factory;
        private readonly IMatch3VFXFactory _vfxFactory;
        private readonly Match3LevelConfig _levelConfig;
        private readonly IGameGoalService _goalService;
        private readonly LoadingService _loadingService;
        private readonly GameMoveService _gameMoveService;
        private readonly Match3AssetProvider _assetProvider;
        private readonly Match3TransformContainers _containers;
        private readonly LoadingCurtain _loadingCurtain;

        public Match3GameFlow(UIManager uiManager, Match3Game match3Game, Match3GridView gridView,
            Match3Factory factory, IMatch3VFXFactory vfxFactory, Match3LevelConfig levelConfig,
            IGameGoalService goalService, LoadingService loadingService, GameMoveService gameMoveService,
            Match3AssetProvider assetProvider, Match3TransformContainers containers, LoadingCurtain loadingCurtain)
        {
            _uiManager = uiManager;
            _match3Game = match3Game;
            _gridView = gridView;
            _factory = factory;
            _vfxFactory = vfxFactory;
            _levelConfig = levelConfig;
            _goalService = goalService;
            _loadingService = loadingService;
            _gameMoveService = gameMoveService;
            _assetProvider = assetProvider;
            _containers = containers;
            _loadingCurtain = loadingCurtain;
        }

        public async UniTask StartAsync(CancellationToken token)
        {
            _containers.gridContainer.position = _levelConfig.gridSpawnPosition;
            var gridViewParams = new Match3GridView.Params(_containers.gridContainer, _levelConfig);
            var match3FactoryParams = new Match3Factory.Params(_levelConfig.gridWidth * _levelConfig.gridHeight,
                _levelConfig.goal.goalCount);
            
            await _loadingService.Load(_assetProvider, _levelConfig, token);
            await _loadingService.Load(_vfxFactory, token);
            await _loadingService.Load(_factory, match3FactoryParams, token);
            await _loadingService.Load(_gridView, gridViewParams, token);
            await _loadingService.Load(_gameMoveService,  _gridView.positions[0, 0], token);
            await _loadingService.Load(_match3Game, _levelConfig, token);
            await _loadingService.Load(_goalService, _levelConfig, token);
            _factory.CreateEnvironment(_containers.environmentContainer);
            ShowUI();
            await _loadingCurtain.Hide(token);
        }

        private void ShowUI()
        {
            switch (_levelConfig.levelType)
            {
                case Match3LevelType.HUNGRY_BAT:
                    _uiManager.ShowScreen<HungryBatScreen>();
                    break;
                case Match3LevelType.BLOCK_CRUSH:
                    _uiManager.ShowScreen<BlockCrushScreen>();
                    break;
            }
        }
    }
}