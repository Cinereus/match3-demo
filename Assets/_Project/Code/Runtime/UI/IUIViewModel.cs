using System;

namespace Code.Runtime.UI
{
    public interface IUIViewModel : IDisposable { }

    public interface IUIScreenViewModel : IUIViewModel { }

    public interface IUIDialogViewModel : IUIViewModel { }
}