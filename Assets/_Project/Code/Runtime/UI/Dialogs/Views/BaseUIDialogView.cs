using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code.Runtime.UI.Dialogs.Views
{
    public abstract class BaseUIDialogView<TViewModel> : IUIView where TViewModel : IUIViewModel
    {
        [SerializeField]
        private Image _backImage;
        
        [SerializeField]
        private Button _dialogBack;
        
        [SerializeField]
        private RectTransform _dialogContainer;

        public virtual bool closeBack => true;
        public override Type modelType => typeof(TViewModel);
        
        protected TViewModel _model;
        protected UIManager _uiManager;

        private const float SHOW_DURATION = 0.4f;

        public override void SetupUIManager(UIManager uiManager)
        {
            _uiManager = uiManager;
        }

        public override void Show()
        {
            if (closeBack)
                _dialogBack.onClick.AddListener(Hide);

            Color backColor = _backImage.color;
            float backAlpha = backColor.a; 
            backColor.a = 0;
            _backImage.color = backColor;
            _dialogContainer.localScale = Vector3.zero;
            _backImage.DOFade(backAlpha, SHOW_DURATION);
            _dialogContainer.DOScale(1, SHOW_DURATION);
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
        }

        public override void Bind(IUIViewModel model)
        {
            if (model is TViewModel viewModel)
                _model = viewModel;
        }

        public override void Dispose()
        {
            _dialogBack.onClick.RemoveAllListeners();
        }
    }
}