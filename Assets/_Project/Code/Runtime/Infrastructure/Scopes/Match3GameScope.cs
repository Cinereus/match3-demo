using Code.Match3.Services;
using Code.Runtime.Infrastructure.Factories;
using Code.Runtime.Infrastructure.Flows;
using Code.Runtime.Infrastructure.Services;
using Code.Runtime.Infrastructure.StaticData;
using Code.Runtime.Match3;
using Code.Runtime.Match3.Services;
using Code.Runtime.UI;
using Code.Runtime.UI.Dialogs.ViewModels;
using Code.Runtime.UI.Screens.ViewModels;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Runtime.Infrastructure.Scopes
{
    public class Match3GameScope : LifetimeScope
    {
        [SerializeField]
        private RectTransform _uiScreensContainer;
        
        [SerializeField]
        private RectTransform _uiDialogsContainer;
        
        [SerializeField]
        private Transform _gridContainer;
        
        [SerializeField]
        private Transform _environmentContainer;

        private Match3LevelConfig _levelConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterLevelConfig(builder);
            RegisterServices(builder);
            RegisterFactories(builder);
            RegisterGame(builder);
            RegisterGameGoal(builder);
            RegisterUI(builder);
            RegisterEntryPoint(builder);
        }

        private void RegisterEntryPoint(IContainerBuilder builder)
        {
            var containers = new Match3TransformContainers(_gridContainer, _environmentContainer);
            builder.RegisterEntryPoint<Match3GameFlow>().WithParameter(containers);
        }

        private void RegisterLevelConfig(IContainerBuilder builder)
        {
            _levelConfig = Parent.Container.Resolve<Match3LevelConfigContainer>().GetTargetConfig();
            builder.RegisterInstance(_levelConfig);
        }

        private void RegisterGameGoal(IContainerBuilder builder)
        {
            switch (_levelConfig.goal.limit.type)
            {
                case LimitType.MOVES:
                    builder.Register<IGameGoalService, MovesGoalService>(Lifetime.Scoped);
                    break;
                case LimitType.TIME:
                    builder.Register<IGameGoalService, TimeGoalService>(Lifetime.Scoped);
                    break;
            }
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<FindMatchService>(Lifetime.Scoped);
            builder.Register<ShapesFallService>(Lifetime.Scoped);
            builder.Register<ShapesDestructProcessService>(Lifetime.Scoped);
            builder.Register<MatchPredictionService>(Lifetime.Scoped);
            builder.Register<ShapeStatesCreationService>(Lifetime.Scoped).WithParameter(_levelConfig.shapesTypesCount);
            builder.Register<BonusCreateProcessService>(Lifetime.Scoped);
            builder.RegisterEntryPoint<InputService>().AsSelf();
            builder.Register<GameMoveService>(Lifetime.Scoped);
        }
        
        private void RegisterGame(IContainerBuilder builder)
        {
            builder.Register<IMatch3GridView, Match3GridView>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<Match3Game>(Lifetime.Scoped);
        }

        private void RegisterUI(IContainerBuilder builder)
        {
            builder.Register<IUIDialogViewModel, Match3GameQuitDialog>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<IUIDialogViewModel, Match3GameCompleteDialog>(Lifetime.Scoped).AsImplementedInterfaces();
            
            switch (_levelConfig.levelType)
            {
                case Match3LevelType.HUNGRY_BAT:
                    builder.Register<IUIScreenViewModel, HungryBatScreen>(Lifetime.Scoped).AsImplementedInterfaces();
                    break;
                case Match3LevelType.BLOCK_CRUSH:
                    builder.Register<IUIScreenViewModel, BlockCrushScreen>(Lifetime.Scoped).AsImplementedInterfaces();
                    break;
            }
            
            var containers = new UITransformContainers(_uiScreensContainer, _uiDialogsContainer);
            builder.Register<UIManager>(Lifetime.Scoped).WithParameter(containers);
        }

        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<Match3AssetProvider>(Lifetime.Scoped);
            builder.Register<Match3Factory>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            
            switch (_levelConfig.levelType)
            {
                case Match3LevelType.HUNGRY_BAT:
                    builder.Register<IMatch3VFXFactory, HungryBatVFXFactory>(Lifetime.Scoped);
                    break;
                case Match3LevelType.BLOCK_CRUSH:
                    builder.Register<IMatch3VFXFactory, BlockCrushVFXFactory>(Lifetime.Scoped);
                    break;
            }
        }
    }

    public readonly struct UITransformContainers
    {
        public readonly RectTransform screensContainer;
        public readonly RectTransform dialogsContainer;

        public UITransformContainers(RectTransform screensContainer, RectTransform dialogsContainer)
        {
            this.screensContainer = screensContainer;
            this.dialogsContainer = dialogsContainer;
        }
    }
    
    public readonly struct Match3TransformContainers
    {
        public readonly Transform gridContainer;
        public readonly Transform environmentContainer;

        public Match3TransformContainers(Transform gridContainer, Transform environmentContainer)
        {
            this.gridContainer = gridContainer;
            this.environmentContainer = environmentContainer;
        }
    }
}