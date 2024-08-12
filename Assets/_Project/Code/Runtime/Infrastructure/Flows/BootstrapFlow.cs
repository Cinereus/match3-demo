using System.Threading;
using Code.Runtime.Infrastructure.Services;
using Code.Runtime.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.Device;
using VContainer.Unity;

namespace Code.Runtime.Infrastructure.Flows
{
    public class BootstrapFlow : IAsyncStartable
    {
        private readonly IUIFactory _uiFactory;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly LoadingService _loadingService;
        private readonly LoadSceneService _loadSceneService;

        public BootstrapFlow(IUIFactory uiFactory, LoadingCurtain loadingCurtain, LoadingService loadingService, LoadSceneService loadSceneService)
        {
            _uiFactory = uiFactory;
            _loadingCurtain = loadingCurtain;
            _loadingService = loadingService;
            _loadSceneService = loadSceneService;
        }

        public async UniTask StartAsync(CancellationToken token)
        {
            Application.targetFrameRate = 60;
            await _loadingService.Load(_uiFactory, token);
            await _loadingService.Load(_loadSceneService, RuntimeConstants.Scenes.MAIN_MENU, token);
            await _loadingCurtain.Hide(token);
        }
    }
}