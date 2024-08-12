using Code.Runtime.Infrastructure.Services;

namespace Code.Runtime.UI.Dialogs.ViewModels
{
    public class Match3GameCompleteDialog : BaseDialogViewModel
    {
        private readonly LoadSceneService _sceneService;

        public Match3GameCompleteDialog(LoadSceneService sceneService)
        {
            _sceneService = sceneService;
        }
        
        public override void Dispose() { }

        public void RestartGame() => 
            _sceneService.Load(RuntimeConstants.Scenes.MATCH3_GAME);

        public void GoToMainScene() => 
            _sceneService.Load(RuntimeConstants.Scenes.MAIN_MENU);
    }
}