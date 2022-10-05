using AppFramework.Vision.ViewModels;
using Prism.Ioc;

namespace AppFramework.Vision.Views
{
    public static class ViewsExtensions
    {
        public static void AddViews(this IContainerRegistry services)
        {
            services.RegisterForNavigation<NccView, NccViewModel>();
            services.RegisterForNavigation<ShapeView, ShapeViewModel>();
            services.RegisterForNavigation<AnisoShapeView, AnisoShapeViewModel>();
            services.RegisterForNavigation<ScaledShapeView, ScaledShapeViewModel>();
        }
    }
}
