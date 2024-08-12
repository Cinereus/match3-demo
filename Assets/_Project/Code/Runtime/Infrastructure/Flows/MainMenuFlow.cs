using Code.Runtime.UI;
using Code.Runtime.UI.Screens.ViewModels;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Code.Runtime.Infrastructure.Flows
{
    public class MainMenuFlow : IStartable
    {
        private readonly UIManager _uiManager;
        private readonly LoadingCurtain _loadingCurtain;

        public MainMenuFlow(UIManager uiManager, LoadingCurtain loadingCurtain)
        {
            _uiManager = uiManager;
            _loadingCurtain = loadingCurtain;
        }

        public void Start()
        {
            _uiManager.ShowScreen<MainMenuScreen>();
            _loadingCurtain.Hide().Forget();
        }
    }
}