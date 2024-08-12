using Code.Runtime.UI.Dialogs.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Runtime.UI.Dialogs.Views
{
    public class Match3GameCompleteDialogView : BaseUIDialogView<Match3GameCompleteDialog>
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private Button _quitButton;

        [SerializeField]
        private Button _restartButton;

        public override bool closeBack => false;
        
        private const string VICTORY_TEXT = "YOU WIN!";
        private const string LOSE_TEXT = "YOU LOSE!";

        public override void Show<T>(T param)
        {
            if (param is not bool isVictory) 
                return;

            _text.text = isVictory ? VICTORY_TEXT : LOSE_TEXT;
            _restartButton.onClick.AddListener(OnRestart);
            _quitButton.onClick.AddListener(OnQuit);
            base.Show(param);
        }

        public override void Hide()
        {
            Dispose();
            base.Hide();
        }

        public override void Dispose()
        {
            _text.text = string.Empty;
            _restartButton.onClick.RemoveAllListeners();
            _quitButton.onClick.RemoveAllListeners();
            base.Dispose();
        }

        private void OnRestart()
        {
            _model.RestartGame();
        }

        private void OnQuit()
        { 
            _model.GoToMainScene();
        }
    }
}