using Code.Runtime.Infrastructure.Services;
using Code.Runtime.Match3;
using Cysharp.Threading.Tasks;

namespace Code.Runtime.UI.Dialogs.ViewModels
{
    public class Match3GameQuitDialog : BaseDialogViewModel
    {
        private readonly Match3Game _game;
        private readonly LoadSceneService _sceneService;

        public Match3GameQuitDialog(LoadSceneService sceneService, Match3Game game)
        {
            _game = game;
            _sceneService = sceneService;
        }
        
        public override void Dispose() { }

        public void OnShow() => 
            _game.isMoveEnabled = false;
        
        public void OnHide() =>
            _game.isMoveEnabled = true;

        public void GoToMainScene() => 
            _sceneService.Load(RuntimeConstants.Scenes.MAIN_MENU).Forget();
    }
}