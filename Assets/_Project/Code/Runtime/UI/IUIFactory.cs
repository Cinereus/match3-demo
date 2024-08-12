using Code.Runtime.Infrastructure.Services;
using Code.Runtime.UI.Dialogs.Views;
using UnityEngine;

namespace Code.Runtime.UI
{
    public interface IUIFactory : ILoadUnit
    {
        BaseUIScreenView<TViewModel> CreateScreen<TViewModel>(RectTransform container) where TViewModel : IUIViewModel;
        BaseUIDialogView<TViewModel> CreateDialog<TViewModel>(RectTransform container) where TViewModel : IUIViewModel;
    }
}