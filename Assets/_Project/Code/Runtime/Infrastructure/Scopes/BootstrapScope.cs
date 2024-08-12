using System.Collections.Generic;
using Code.Runtime.Infrastructure.Flows;
using Code.Runtime.Infrastructure.Services;
using Code.Runtime.Infrastructure.StaticData;
using Code.Runtime.Match3;
using Code.Runtime.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Runtime.Infrastructure.Scopes
{
    public class BootstrapScope : LifetimeScope
    {
        [SerializeField]
        private List<Match3LevelConfig> _levelConfigs;
        
        [SerializeField]
        private LoadingCurtain _loadingCurtain;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<Match3LevelConfigContainer>(Lifetime.Singleton).WithParameter(_levelConfigs);
            builder.RegisterComponentInNewPrefab(_loadingCurtain, Lifetime.Singleton);
            RegisterServices(builder);
            RegisterBootEntryPoint(builder);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<LoadingService>(Lifetime.Singleton);
            builder.Register<LoadSceneService>(Lifetime.Singleton);
            builder.Register<IUIFactory, UIFactory>(Lifetime.Singleton);
        }

        private void RegisterBootEntryPoint(IContainerBuilder builder) => 
            builder.RegisterEntryPoint<BootstrapFlow>();
    }
}
