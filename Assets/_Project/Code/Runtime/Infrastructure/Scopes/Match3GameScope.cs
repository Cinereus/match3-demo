using Code.Runtime.Infrastructure.Factories;
using Code.Runtime.Infrastructure.Flows;
using Code.Runtime.Logic.Match3;
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
            builder.Register<Match3Factory>(Lifetime.Singleton);
            builder.Register<Match3Grid>(Lifetime.Singleton);
            builder.RegisterComponent(_spawnPoint);
            builder.RegisterEntryPoint<Match3GameFlow>();
        }
    }
}