using Code.Runtime.Infrastructure.Services;
using VContainer.Unity;

namespace Code.Runtime.Infrastructure.Flows
{
    public class BootstrapFlow : IStartable
    {
        private readonly ILoadingService _loadingService;
        private readonly LoadSceneService _loadSceneService;

        public BootstrapFlow(ILoadingService loadingService, LoadSceneService loadSceneService)
        {
            _loadingService = loadingService;
            _loadSceneService = loadSceneService;
        }

        public async void Start()
        {
            await _loadingService.Load(_loadSceneService, RuntimeConstants.Scenes.MATCH3_GAME);
        }
    }
}