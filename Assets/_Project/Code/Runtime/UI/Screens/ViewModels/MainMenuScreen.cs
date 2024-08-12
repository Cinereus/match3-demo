using Code.Runtime.Infrastructure.Scopes;
using Code.Runtime.Infrastructure.Services;
using Code.Runtime.Infrastructure.StaticData;
using Code.Runtime.Match3;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Runtime.UI.Screens.ViewModels
{
    public class MainMenuScreen : BaseScreenViewModel
    {
        private readonly LoadSceneService _loadSceneService;
        private readonly Match3LevelConfigContainer _configContainer;

        public MainMenuScreen(LoadSceneService loadSceneService, Match3LevelConfigContainer configContainer)
        {
            _loadSceneService = loadSceneService;
            _configContainer = configContainer;
        }

        public override void Dispose(){}

        public void QuitGame() =>
            Application.Quit();

        public void GoToHungryBat()
        {
            _configContainer.targetType = Match3LevelType.HUNGRY_BAT;
            _loadSceneService.Load(RuntimeConstants.Scenes.MATCH3_GAME).Forget();
        }

        public void GoToBlockRush()
        {
            _configContainer.targetType = Match3LevelType.BLOCK_CRUSH;
            _loadSceneService.Load(RuntimeConstants.Scenes.MATCH3_GAME).Forget();
        }
    }
}