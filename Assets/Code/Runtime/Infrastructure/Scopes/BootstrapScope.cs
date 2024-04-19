using Code.Runtime.Infrastructure.Flows;
using VContainer;
using VContainer.Unity;

namespace Code.Runtime.Infrastructure.Scopes
{
    public class BootstrapScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<BootstrapFlow>();
        }
    }
}
