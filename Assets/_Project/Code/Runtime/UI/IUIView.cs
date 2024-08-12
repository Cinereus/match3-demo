using System;
using UnityEngine;

namespace Code.Runtime.UI
{
    public abstract class IUIView : MonoBehaviour, IDisposable
    {
        public void BringToTop() => transform.SetAsLastSibling();
        public abstract Type modelType { get; }
        public abstract void SetupUIManager(UIManager uiManager);
        public abstract void Show();
        public abstract void Show<T>(T param);
        public abstract void Hide();
        public abstract void HideCompletely();
        public abstract void Bind(IUIViewModel model);
        public abstract void Dispose();
    }
}