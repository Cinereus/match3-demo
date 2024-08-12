using System;
using System.Collections.Generic;
using Code.Runtime.Infrastructure.Scopes;
using Code.Runtime.UI.Dialogs.ViewModels;
using Code.Runtime.UI.Screens.ViewModels;
using UnityEngine;

namespace Code.Runtime.UI
{
    public class UIManager : IDisposable
    {
        private readonly IUIFactory _factory;
        private readonly RectTransform _screenContainer;
        private readonly RectTransform _dialogContainer;
        private readonly Dictionary<Type, IUIView> _cachedScreens = new Dictionary<Type, IUIView>();
        private readonly Dictionary<Type, IUIView> _cachedDialogs = new Dictionary<Type, IUIView>();
        private readonly Dictionary<Type, IUIScreenViewModel> _screenModels = new Dictionary<Type, IUIScreenViewModel>();
        private readonly Dictionary<Type, IUIDialogViewModel> _dialogModels = new Dictionary<Type, IUIDialogViewModel>();

        public UIManager(IUIFactory factory, UITransformContainers containers, IEnumerable<IUIScreenViewModel> screens = null, IEnumerable<IUIDialogViewModel> dialogs = null)
        {
            _factory = factory;
            _dialogContainer = containers.dialogsContainer;
            _screenContainer = containers.screensContainer;

            if (screens != null)
            {
                foreach (var screen in screens)
                {
                    _screenModels[screen.GetType()] = screen;
                }    
            }

            if (dialogs != null)
            {
                foreach (var dialog in dialogs)
                {
                    _dialogModels[dialog.GetType()] = dialog;
                }    
            }
        }
        
        public void ShowScreen<TViewModel>(bool cache = true) where TViewModel : BaseScreenViewModel
        {
            Type key = typeof(TViewModel);
            if (_screenModels.TryGetValue(key, out IUIScreenViewModel model) == false)
            {
                Debug.LogError($"[{nameof(UIManager)}] Failed to show \"{nameof(TViewModel)}\". ViewModel not found.");
                return;
            }

            if (_cachedScreens.TryGetValue(key, out IUIView screen))
            {
                if (screen.isActiveAndEnabled)
                {
                    screen.Hide();
                    screen.BringToTop();
                }
                
                screen.Show();
                if (cache == false)
                    ClearScreenCache(key);
                return;
            }
            
            screen = _factory.CreateScreen<TViewModel>(_screenContainer);
            screen.SetupUIManager(this);
            screen.Bind(model);
            screen.Show();
            
            if (cache)
                _cachedScreens[key] = screen;
        }

        public void ShowDialog<TViewModel>(bool cache = true) where TViewModel : BaseDialogViewModel 
        {
            Type key = typeof(TViewModel);
            if (_dialogModels.TryGetValue(key, out IUIDialogViewModel model) == false)
            {
                Debug.LogError($"[{nameof(UIManager)}] Failed to show \"{nameof(TViewModel)}\". ViewModel not found.");
                return;
            }

            if (_cachedDialogs.TryGetValue(key, out IUIView dialog))
            {
                if (dialog.isActiveAndEnabled)
                {
                    dialog.Hide();
                    dialog.BringToTop();
                }
                
                dialog.Show();
                if (cache == false)
                    ClearDialogCache(key);
                return;
            }
            
            dialog = _factory.CreateDialog<TViewModel>(_dialogContainer);
            dialog.SetupUIManager(this);
            dialog.Bind(model);
            dialog.Show();
            
            if (cache)
                _cachedDialogs[key] = dialog;
        }
        
        public void ShowDialog<TViewModel, TParam>(TParam param, bool cache = true) where TViewModel : BaseDialogViewModel
        {
            Type key = typeof(TViewModel);
            if (_dialogModels.TryGetValue(key, out IUIDialogViewModel model) == false)
            {
                Debug.LogError($"[{nameof(UIManager)}] Failed to show \"{nameof(TViewModel)}\". ViewModel not found.");
                return;
            }

            if (_cachedDialogs.TryGetValue(key, out IUIView dialog))
            {
                if (dialog.isActiveAndEnabled)
                {
                    dialog.Hide();
                    dialog.BringToTop();
                }
                
                dialog.Show(param);
                if (cache == false)
                    ClearDialogCache(key);
                return;
            }
            
            dialog = _factory.CreateDialog<TViewModel>(_dialogContainer);
            dialog.SetupUIManager(this);
            dialog.Bind(model);
            dialog.Show(param);
            
            if (cache)
                _cachedDialogs[key] = dialog;
        }
        
        public void HideScreen<TViewModel>(bool clearCache = false) where TViewModel : BaseScreenViewModel
        {
            Type key = typeof(TViewModel);
            if (_cachedScreens.TryGetValue(key, out IUIView screen))
            {
                screen.Hide();
                if (clearCache) 
                    ClearScreenCache(key);
            }
            else
            {
                Debug.LogWarning($"[{nameof(UIManager)}] Failed to hide \"{nameof(TViewModel)}\". Cache not found.");
            }
        }
        
        public void HideDialog<TViewModel>(bool clearCache = false) where TViewModel : BaseDialogViewModel
        {
            Type key = typeof(TViewModel);
            if (_cachedDialogs.TryGetValue(key, out IUIView dialog))
            {
                dialog.Hide();
                if (clearCache)
                    ClearDialogCache(key);
            }
            else
            {
                Debug.LogWarning($"[{nameof(UIManager)}] Failed to hide \"{nameof(TViewModel)}\". Cache not found.");
            }
        }

        public void Dispose()
        {
            foreach (var screen in _cachedScreens.Values)
            { 
                screen.Dispose();
            }
            _cachedScreens.Clear();
            
            foreach (var dialog in _cachedDialogs.Values)
            { 
                dialog.Dispose();
            }
            _cachedDialogs.Clear();
        }

        public void ClearDialogCache(Type key) => 
            _cachedDialogs.Remove(key);

        public void ClearScreenCache(Type key) => 
            _cachedScreens.Remove(key);
    }
}
