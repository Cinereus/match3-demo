using Code.Runtime.Infrastructure.Flows;
using Code.Runtime.UI;
using Code.Runtime.UI.Screens.ViewModels;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Runtime.Infrastructure.Scopes
{
    public class MainMenuScope : LifetimeScope
    {
        [SerializeField]
        private RectTransform _uiScreensContainer;
        
        [SerializeField]
        private RectTransform _uiDialogsContainer;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterUI(builder);
            RegisterEntryPoint(builder);
            base.Configure(builder);
        }

        private static void RegisterEntryPoint(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<MainMenuFlow>().AsSelf();
        }

        private void RegisterUI(IContainerBuilder builder)
        {
            builder.Register<IUIViewModel, MainMenuScreen>(Lifetime.Scoped).AsImplementedInterfaces();
            var containers = new UITransformContainers(_uiScreensContainer, _uiDialogsContainer);
            builder.Register<UIManager>(Lifetime.Scoped).WithParameter(containers);
        }
    }
}