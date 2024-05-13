using Code.Runtime.Infrastructure.Flows;
using Code.Runtime.Infrastructure.Services;
using VContainer;
using VContainer.Unity;

namespace Code.Runtime.Infrastructure.Scopes
{
    public class BootstrapScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterServices(builder);
            RegisterBootEntryPoint(builder);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<ILoadingService, LoadingService>(Lifetime.Singleton);
        }

        private void RegisterBootEntryPoint(IContainerBuilder builder) => 
            builder.RegisterEntryPoint<BootstrapFlow>();
    }
}
