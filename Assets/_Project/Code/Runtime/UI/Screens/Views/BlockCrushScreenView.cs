using Code.Runtime.Match3.Services;
using Code.Runtime.UI.Dialogs.ViewModels;
using Code.Runtime.UI.Screens.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Runtime.UI.Screens.Views
{
    public class BlockCrushScreenView : BaseUIScreenView<BlockCrushScreen>
    {
        [SerializeField]
        private ShapeGoalsCounterView _goalsCounter;
        
        [SerializeField]
        private UICounterView _timeCounter;
        
        [SerializeField]
        private Button _quitButton;
        
        public override void Show()
        {
            _goalsCounter.Initialize(_model.CreateGoalViews());
            _timeCounter.Initialize(_model.limit);
            _model.onGameFinished += OnGameFinished;
            _model.onGoalCountChanged += OnGoalCountChanged;
            _model.onLimitProcessed += OnTimeCountChanged;
            _quitButton.onClick.AddListener(OnQuit);
            base.Show();
        }

        public override void Hide()
        {
            Dispose();
            _goalsCounter.Clear();
            base.Hide();
        }

        public override void Dispose()
        {
            _model.onGameFinished -= OnGameFinished;
            _model.onGoalCountChanged -= OnGoalCountChanged;
            _model.onLimitProcessed -= OnTimeCountChanged;
            _quitButton.onClick.RemoveAllListeners();
        }

        private void OnQuit() => 
            _uiManager.ShowDialog<Match3GameQuitDialog>();

        private void OnGameFinished(bool isVictory) =>
            _uiManager.ShowDialog<Match3GameCompleteDialog, bool>(param: isVictory);
        
        private void OnGoalCountChanged(Match3GoalData data) =>
            _goalsCounter.SetCount(data);

        private void OnTimeCountChanged(int moves) => 
            _timeCounter.SetCount(moves);
    }
}