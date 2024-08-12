using System;
using System.Collections.Generic;
using System.Threading;
using Code.Runtime.UI.Dialogs.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Runtime.UI
{
    public class UIFactory : IUIFactory
    {
        private readonly Dictionary<Type, IUIView> _viewPrefabs = new Dictionary<Type, IUIView>();
        
        public UniTask Load(CancellationToken token)
        {
            IUIView[] screens = Resources.LoadAll<IUIView>(RuntimeConstants.Assets.UI_SCREEN_VIEWS);
            IUIView[] dialogs = Resources.LoadAll<IUIView>(RuntimeConstants.Assets.UI_DIALOG_VIEWS);

            foreach (var screen in screens)
                _viewPrefabs[screen.modelType] = screen;
            
            foreach (var dialog in dialogs)
                _viewPrefabs[dialog.modelType] = dialog;
            
            return UniTask.CompletedTask;
        }

        public BaseUIScreenView<TViewModel> CreateScreen<TViewModel>(RectTransform container) where TViewModel : IUIViewModel 
        {
            if (_viewPrefabs.TryGetValue(typeof(TViewModel), out IUIView prefab))
                return Object.Instantiate((BaseUIScreenView<TViewModel>) prefab, container);
            
            Debug.LogError($"[{nameof(UIFactory)}] Failed to create window \"{nameof(TViewModel)}\". Prefab not found.");
            return null;
        }

        public BaseUIDialogView<TViewModel> CreateDialog<TViewModel>(RectTransform container) where TViewModel : IUIViewModel
        {
            if (_viewPrefabs.TryGetValue(typeof(TViewModel), out IUIView prefab))
                return Object.Instantiate((BaseUIDialogView<TViewModel>) prefab, container);
            
            Debug.LogError($"[{nameof(UIFactory)}] Failed to create window \"{nameof(TViewModel)}\". Prefab not found.");
            return null;
        }
    }
}