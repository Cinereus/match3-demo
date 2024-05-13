using Code.Runtime.Infrastructure.Services;
using VContainer.Unity;

namespace Code.Runtime.Infrastructure.Flows
{
    public class BootstrapFlow : IStartable
    {
        private readonly ILoadingService _loadingService;

        public BootstrapFlow(ILoadingService loadingService)
        {
            _loadingService = loadingService;
        }

        public void Start()
        {
        }
    }
}