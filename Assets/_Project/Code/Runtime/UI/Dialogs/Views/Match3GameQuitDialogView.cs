using Code.Runtime.UI.Dialogs.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Runtime.UI.Dialogs.Views
{
    public class Match3GameQuitDialogView : BaseUIDialogView<Match3GameQuitDialog>
    {
        [SerializeField]
        private Button _quitButton;
        
        [SerializeField]
        private Button _continueButton;

        public override void Show()
        {
            _quitButton.onClick.AddListener(OnQuit);
            _continueButton.onClick.AddListener(OnContinue);
            _model.OnShow();
            base.Show();
        }

        public override void Dispose()
        {
            _quitButton.onClick.RemoveAllListeners();
            _continueButton.onClick.RemoveAllListeners();
            base.Dispose();
        }

        public override void Hide()
        {
            _model.OnHide();
            base.Hide();
        }

        private void OnContinue()
        {
            Hide();
        }

        private void OnQuit()
        {
            _model.GoToMainScene();
        }
    }
}