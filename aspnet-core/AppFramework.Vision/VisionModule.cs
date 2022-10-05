using AppFramework.Vision.Services;
using AppFramework.Vision.ViewModels;
using AppFramework.Vision.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace AppFramework.Vision
{
    /*
     * Halcon机器视觉模块
     * 
     * 包含Halcon中内置的常用视觉匹配算法实现
     * 
     */

    public class VisionModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        { 
            containerRegistry.Register<NccService>(); 
            containerRegistry.Register<ShapeService>();
            containerRegistry.Register<AnisoShapeService>();
            containerRegistry.Register<ScaledShapeService>();

            containerRegistry.RegisterForNavigation<NccView, NccViewModel>();
            containerRegistry.RegisterForNavigation<ShapeView, ShapeViewModel>();
            containerRegistry.RegisterForNavigation<AnisoShapeView, AnisoShapeViewModel>();
            containerRegistry.RegisterForNavigation<ScaledShapeView, ScaledShapeViewModel>();
        }
    }
}
