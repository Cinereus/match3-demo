using System;

namespace Code.Runtime.UI
{
    public abstract class BaseUIScreenView<TViewModel> : IUIView where TViewModel : IUIViewModel
    {
        public override Type modelType => typeof(TViewModel);
        
        protected TViewModel _model;
        protected UIManager _uiManager;

        public override void SetupUIManager(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
        
        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Show<T>(T param) => Show();

        public override void Hide()
        {
            gameObject.SetActive(false);
            Dispose();
        }

        public override void HideCompletely()
        {
            Hide();
            _uiManager.ClearDialogCache(GetType());
            _uiManager.ClearScreenCache(GetType());
        }

        public override void Bind(IUIViewModel model)
        {
            if (model is TViewModel viewModel)
                _model = viewModel;
        }

        public override void Dispose() { }
    }
}