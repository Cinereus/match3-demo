using Code.Match3.Services;
using Code.Runtime.Infrastructure.Factories;
using Code.Runtime.Infrastructure.Flows;
using Code.Runtime.Infrastructure.Services;
using Code.Runtime.Match3;
using Code.Runtime.Match3.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Runtime.Infrastructure.Scopes
{
    public class Match3GameScope : LifetimeScope
    {
        [SerializeField]
        private Transform _spawnPoint;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_spawnPoint);
            builder.Register<FindMatchService>(Lifetime.Singleton);
            builder.Register<ShapesFallService>(Lifetime.Singleton);
            builder.Register<ShapesDestructProcessService>(Lifetime.Singleton);
            builder.Register<MatchPredictionService>(Lifetime.Singleton);
            builder.Register<ShapeStatesCreationService>(Lifetime.Singleton);
            builder.Register<BonusCreateProcessService>(Lifetime.Singleton);
            builder.RegisterEntryPoint<InputService>().AsSelf();
            builder.Register<GameMoveService>(Lifetime.Singleton);
            builder.Register<Match3Factory>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<Match3GridView>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.RegisterEntryPoint<Match3Game>().AsSelf();
            builder.RegisterEntryPoint<Match3GameFlow>();
        }
    }
}