using Code.Runtime.Infrastructure.Scopes;
using Code.Runtime.UI.Screens.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Runtime.UI.Screens.Views
{
    public class MainMenuScreenView : BaseUIScreenView<MainMenuScreen>
    {
        [SerializeField]
        private Button _hungryBatButton;

        [SerializeField]
        private Button _blockCrushButton;

        [SerializeField]
        private Button _quitButton;

        public override void Show()
        {
            _quitButton.onClick.AddListener(_model.QuitGame);
            _hungryBatButton.onClick.AddListener(_model.GoToHungryBat);
            _blockCrushButton.onClick.AddListener(_model.GoToBlockRush);
            base.Show();
        }

        public override void Dispose()
        {
            _quitButton.onClick.RemoveAllListeners();
            _hungryBatButton.onClick.RemoveAllListeners();
            _blockCrushButton.onClick.RemoveAllListeners();
        }
    }
}